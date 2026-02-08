let dotnet = null;

function emit(type, payload = {}) {
    try {
        if (!dotnet) return;
        dotnet.invokeMethodAsync("OnVoiceEventAsync", {
            type,
            ts: new Date().toISOString(),
            ...payload,
        });
    } catch (e) {
        // ignore
    }
}

export function init(dotnetRef) {
    dotnet = dotnetRef;

    if (!("speechSynthesis" in window)) {
        emit("unsupported", { message: "Web Speech API not supported (speechSynthesis missing)" });
        return;
    }

    emit("init", state());

    // Voices often arrive later on Safari/iOS
    window.speechSynthesis.addEventListener("voiceschanged", () => {
        emit("voiceschanged", { voices: getVoices() });
    });

    // Trigger voice loading
    window.speechSynthesis.getVoices();
}

export function getVoices() {
    if (!("speechSynthesis" in window)) return [];
    return window.speechSynthesis.getVoices().map(v => ({
        name: v.name,
        lang: v.lang,
        default: v.default,
        localService: v.localService
    }));
}

export function state() {
    if (!("speechSynthesis" in window)) return { supported: false };
    const s = window.speechSynthesis;
    return {
        supported: true,
        speaking: !!s.speaking,
        pending: !!s.pending,
        paused: !!s.paused
    };
}

export function cancel() {
    if (!("speechSynthesis" in window)) return;
    window.speechSynthesis.cancel();
    emit("cancel", state());
}

export function prime() {
    if (!("speechSynthesis" in window)) return;
    const s = window.speechSynthesis;

    // Tiny utterance to "unlock" audio on iOS (best effort)
    const u = new SpeechSynthesisUtterance(" ");
    u.volume = 0;

    s.cancel();
    s.speak(u);

    emit("prime", state());
}

export function speak(text, preferredLang, preferredVoiceName) {
    if (!("speechSynthesis" in window)) {
        emit("unsupported", { message: "speechSynthesis missing" });
        return;
    }

    const s = window.speechSynthesis;
    s.cancel();

    const utter = new SpeechSynthesisUtterance(text ?? "");
    const voices = s.getVoices();

    let voice = null;

    if (preferredVoiceName) {
        voice = voices.find(v => v.name === preferredVoiceName) || null;
    }
    if (!voice && preferredLang) {
        voice = voices.find(v => v.lang === preferredLang) || voices.find(v => (v.lang || "").startsWith(preferredLang.split("-")[0])) || null;
    }
    if (!voice) {
        voice = voices.find(v => (v.lang || "").startsWith("de")) || null;
    }

    if (voice) utter.voice = voice;
    utter.lang = (voice && voice.lang) ? voice.lang : (preferredLang || "de-DE");

    utter.onstart = () => emit("start", { state: state(), lang: utter.lang, voice: voice ? voice.name : null });
    utter.onend = () => emit("end", { state: state() });
    utter.onpause = () => emit("pause", { state: state() });
    utter.onresume = () => emit("resume", { state: state() });
    utter.onboundary = (e) => emit("boundary", { name: e.name, charIndex: e.charIndex });
    utter.onerror = (e) => emit("error", {
        error: e.error || "unknown",
        message: e.message || null,
        state: state(),
        lang: utter.lang,
        voice: voice ? voice.name : null
    });

    emit("speak_called", {
        textLength: (text || "").length,
        preferredLang: preferredLang || null,
        preferredVoiceName: preferredVoiceName || null,
        chosenVoice: voice ? { name: voice.name, lang: voice.lang } : null,
        voicesCount: voices.length,
        state: state()
    });

    s.speak(utter);
}
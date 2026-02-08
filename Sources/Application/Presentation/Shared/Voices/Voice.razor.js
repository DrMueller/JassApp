let primed = false;
let lastVoiceInit = null;

export function primeSpeech() {
    if (!("speechSynthesis" in window)) return;
    if (primed) return;
    primed = true;

    const synth = window.speechSynthesis;

    // Force voices to load
    const ensureVoices = () => {
        const v = synth.getVoices();
        if (v && v.length) return true;
        return false;
    };

    // Tiny silent-ish utterance to unlock on iOS (volume 0)
    const u = new SpeechSynthesisUtterance(" ");
    u.volume = 0;
    u.rate = 1;
    u.pitch = 1;

    synth.cancel();
    synth.speak(u);

    // kick voices loading
    if (!ensureVoices()) {
        lastVoiceInit = () => ensureVoices();
        synth.addEventListener("voiceschanged", lastVoiceInit, { once: true });
        synth.getVoices();
    }
}

export function speak(text) {
    if (!text) return;
    if (!("speechSynthesis" in window)) return;

    const synth = window.speechSynthesis;
    synth.cancel();

    const doSpeak = () => {
        const utter = new SpeechSynthesisUtterance(text);

        const voices = synth.getVoices();
        const voice =
            voices.find(v => v.lang === "de-CH") ||
            voices.find(v => v.lang?.startsWith("de")) ||
            null;

        if (voice) utter.voice = voice;
        utter.lang = voice?.lang || "de-DE";
        utter.rate = 1;
        utter.pitch = 1;
        utter.volume = 1;

        synth.speak(utter);
    };

    if (synth.getVoices().length === 0) {
        synth.addEventListener("voiceschanged", () => setTimeout(doSpeak, 0), { once: true });
        synth.getVoices();
        return;
    }

    doSpeak();
}
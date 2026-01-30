export function isSupported() {
    return (
        typeof window !== "undefined" &&
        "speechSynthesis" in window &&
        typeof window.SpeechSynthesisUtterance === "function"
    );
}

// Real-world test: must be triggered by a user gesture (button click).
// Important: speak() is called synchronously before returning the Promise.
export function testSpeak(lang = "de-CH") {
    if (!isSupported()) return Promise.resolve(false);

    return new Promise((resolve) => {
        const u = new SpeechSynthesisUtterance("Test");
        u.lang = lang;
        u.rate = 1.0;
        u.volume = 0.2;

        let done = false;
        const finish = (ok) => {
            if (done) return;
            done = true;
            resolve(ok);
        };

        u.onend = () => finish(true);
        u.onerror = () => finish(false);

        try {
            speechSynthesis.cancel();
            speechSynthesis.speak(u); // must happen inside the click-triggered call path
        } catch {
            finish(false);
            return;
        }

        // Fallback: if Safari never fires events
        setTimeout(() => finish(speechSynthesis.speaking || speechSynthesis.pending), 800);
    });
}

export function speak(text, lang = "de-CH") {
    if (!isSupported()) return false;

    const u = new SpeechSynthesisUtterance(text);
    u.lang = lang;
    u.rate = 1.0;
    u.volume = 1.0;

    speechSynthesis.cancel();
    speechSynthesis.speak(u);
    return true;
}

export function cancel() {
    if ("speechSynthesis" in window) speechSynthesis.cancel();
}
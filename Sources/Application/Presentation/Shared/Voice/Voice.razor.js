let queue = [];
let isSpeaking = false;

export function speakList(items, delayMs = 0, lang = "de-CH") {
    if (!("speechSynthesis" in window)) throw new Error("SpeechSynthesis not supported.");

    queue = items.slice();
    next(delayMs, lang);
}

function next(delayMs, lang) {
    if (queue.length === 0) {
        isSpeaking = false;
        return;
    }

    isSpeaking = true;
    const text = queue.shift();
    const u = new SpeechSynthesisUtterance(text);
    u.lang = lang;
    u.rate = 1.8;

    u.onend = () => {
        setTimeout(() => next(delayMs, lang), delayMs);
    };

    window.speechSynthesis.speak(u);
}

export function cancel() {
    queue = [];
    if ("speechSynthesis" in window) window.speechSynthesis.cancel();
}
let queue = [];
let isSpeaking = false;

let isPrimed = false;
let voicesReadyPromise = null;

function getVoicesReadyPromise() {
    if (voicesReadyPromise) return voicesReadyPromise;

    voicesReadyPromise = new Promise((resolve) => {
        if (!("speechSynthesis" in window)) {
            resolve(false);
            return;
        }

        const synth = window.speechSynthesis;
        const voices = synth.getVoices();
        if (voices && voices.length > 0) {
            resolve(true);
            return;
        }

        const handler = () => {
            synth.removeEventListener("voiceschanged", handler);
            resolve(true);
        };

        synth.addEventListener("voiceschanged", handler);
        // Fallback: Safari sometimes never fires voiceschanged.
        setTimeout(() => {
            try { synth.removeEventListener("voiceschanged", handler); } catch { }
            resolve(true);
        }, 1000);
    });

    return voicesReadyPromise;
}

export async function prime(lang = "de-CH") {
    if (!("speechSynthesis" in window)) return false;
    if (isPrimed) return true;

    await getVoicesReadyPromise();

    const u = new SpeechSynthesisUtterance(" ");
    u.lang = lang;
    u.rate = 1;
    u.volume = 0;

    return await new Promise((resolve) => {
        u.onend = () => {
            isPrimed = true;
            resolve(true);
        };
        u.onerror = () => resolve(false);

        try {
            window.speechSynthesis.cancel();
            window.speechSynthesis.speak(u);
        } catch {
            resolve(false);
        }
    });
}

export async function speak(text, lang = "de-CH") {
    if (!("speechSynthesis" in window)) throw new Error("SpeechSynthesis not supported.");

    await getVoicesReadyPromise();

    const u = new SpeechSynthesisUtterance(text);
    u.lang = lang;
    u.rate = 1.8;

    window.speechSynthesis.speak(u);
}

export function cancel() {
    queue = [];
    if ("speechSynthesis" in window) window.speechSynthesis.cancel();
}

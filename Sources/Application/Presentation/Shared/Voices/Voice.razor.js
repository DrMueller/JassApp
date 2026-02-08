export function speak(text) {
    if (!text) return;

    console.log("Speaking:", text);

    if (!("speechSynthesis" in window)) {
        console.warn("Web Speech API not supported in this browser.");
        return;
    }

    console.log("Canceling");

    window.speechSynthesis.cancel();

    const utter = new SpeechSynthesisUtterance(text);
    utter.lang = "de-CH";
    utter.rate = 1;
    utter.pitch = 1;
    utter.volume = 1;

    console.log("Speaking");
    window.speechSynthesis.speak(utter);
    console.log("Finito");
}


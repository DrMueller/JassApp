export function checkVoiceSupport() {
    const hasSpeech = ("speechSynthesis" in window) && ("SpeechSynthesisUtterance" in window);
    const tabVisible = document.visibilityState === "visible";

    const ua = navigator.userActivation || null;
    const hasBeenActive = ua ? !!ua.hasBeenActive : false;
    const isActive = ua ? !!ua.isActive : false;

    const isUsable = hasSpeech && tabVisible && hasBeenActive;

    return isUsable;
}
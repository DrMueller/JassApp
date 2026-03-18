export function play(audioElement) {
    if (!audioElement) return;
    const p = audioElement.play();
    if (p && typeof p.catch === 'function') {
        p.catch(() => { /* ignored: autoplay / gesture / decode errors surfaced to caller via UI state */ });
    }
}

export function stop(audioElement) {
    if (!audioElement) return;
    audioElement.pause();
    try {
        audioElement.currentTime = 0;
    } catch {
        // Some browsers can throw if media isn't seekable yet.
    }
}

export function isPlaying(audioElement) {
    if (!audioElement) return false;
    return !audioElement.paused && !audioElement.ended;
}

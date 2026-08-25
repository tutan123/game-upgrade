namespace RenderHeads.Media.AVProVideo;

public enum PlaybackState
{
	None = 0,
	Opening = 1,
	Buffering = 2,
	Playing = 3,
	Paused = 4,
	StateMask = 7,
	Seeking = 32
}

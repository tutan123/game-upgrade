namespace RenderHeads.Media.AVProVideo;

public static class OptionsAppleExtensions
{
	public static bool GenerateMipmaps(this MediaPlayer.OptionsApple.Flags flags)
	{
		return (flags & MediaPlayer.OptionsApple.Flags.GenerateMipMaps) == MediaPlayer.OptionsApple.Flags.GenerateMipMaps;
	}

	public static MediaPlayer.OptionsApple.Flags SetGenerateMipMaps(this MediaPlayer.OptionsApple.Flags flags, bool b)
	{
		if (flags.GenerateMipmaps() ^ b)
		{
			flags = (b ? (flags | MediaPlayer.OptionsApple.Flags.GenerateMipMaps) : (flags & ~MediaPlayer.OptionsApple.Flags.GenerateMipMaps));
		}
		return flags;
	}

	public static bool AllowExternalPlayback(this MediaPlayer.OptionsApple.Flags flags)
	{
		return (flags & MediaPlayer.OptionsApple.Flags.AllowExternalPlayback) == MediaPlayer.OptionsApple.Flags.AllowExternalPlayback;
	}

	public static MediaPlayer.OptionsApple.Flags SetAllowExternalPlayback(this MediaPlayer.OptionsApple.Flags flags, bool b)
	{
		if (flags.AllowExternalPlayback() ^ b)
		{
			flags = (b ? (flags | MediaPlayer.OptionsApple.Flags.AllowExternalPlayback) : (flags & ~MediaPlayer.OptionsApple.Flags.AllowExternalPlayback));
		}
		return flags;
	}

	public static bool PlayWithoutBuffering(this MediaPlayer.OptionsApple.Flags flags)
	{
		return (flags & MediaPlayer.OptionsApple.Flags.PlayWithoutBuffering) == MediaPlayer.OptionsApple.Flags.PlayWithoutBuffering;
	}

	public static MediaPlayer.OptionsApple.Flags SetPlayWithoutBuffering(this MediaPlayer.OptionsApple.Flags flags, bool b)
	{
		if (flags.PlayWithoutBuffering() ^ b)
		{
			flags = (b ? (flags | MediaPlayer.OptionsApple.Flags.PlayWithoutBuffering) : (flags & ~MediaPlayer.OptionsApple.Flags.PlayWithoutBuffering));
		}
		return flags;
	}

	public static bool UseSinglePlayerItem(this MediaPlayer.OptionsApple.Flags flags)
	{
		return (flags & MediaPlayer.OptionsApple.Flags.UseSinglePlayerItem) == MediaPlayer.OptionsApple.Flags.UseSinglePlayerItem;
	}

	public static MediaPlayer.OptionsApple.Flags SetUseSinglePlayerItem(this MediaPlayer.OptionsApple.Flags flags, bool b)
	{
		if (flags.UseSinglePlayerItem() ^ b)
		{
			flags = (b ? (flags | MediaPlayer.OptionsApple.Flags.UseSinglePlayerItem) : (flags & ~MediaPlayer.OptionsApple.Flags.UseSinglePlayerItem));
		}
		return flags;
	}

	public static bool ResumePlaybackAfterAudioSessionRouteChange(this MediaPlayer.OptionsApple.Flags flags)
	{
		return (flags & MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange) == MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange;
	}

	public static MediaPlayer.OptionsApple.Flags SetResumePlaybackAfterAudioSessionRouteChange(this MediaPlayer.OptionsApple.Flags flags, bool b)
	{
		if (flags.ResumePlaybackAfterAudioSessionRouteChange() ^ b)
		{
			flags = (b ? (flags | MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange) : (flags & ~MediaPlayer.OptionsApple.Flags.ResumeMediaPlaybackAfterAudioSessionRouteChange));
		}
		return flags;
	}
}

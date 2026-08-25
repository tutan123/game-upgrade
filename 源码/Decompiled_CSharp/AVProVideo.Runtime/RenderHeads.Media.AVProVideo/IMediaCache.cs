namespace RenderHeads.Media.AVProVideo;

public interface IMediaCache
{
	bool IsMediaCachingSupported();

	void AddMediaToCache(string url, string headers = null, MediaCachingOptions options = null);

	void CancelDownloadOfMediaToCache(string url);

	void PauseDownloadOfMediaToCache(string url);

	void ResumeDownloadOfMediaToCache(string url);

	void RemoveMediaFromCache(string url);

	CachedMediaStatus GetCachedMediaStatus(string url, ref float progress);
}

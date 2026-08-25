namespace RenderHeads.Media.AVProVideo;

public interface ITimedMetadata
{
	bool HasNewTimedMetadataItem();

	TimedMetadataItem GetTimedMetadataItem();
}

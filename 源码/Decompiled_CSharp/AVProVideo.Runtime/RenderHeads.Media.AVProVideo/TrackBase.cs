namespace RenderHeads.Media.AVProVideo;

public class TrackBase
{
	public int Uid { get; private set; }

	public TrackType TrackType { get; private set; }

	public string DisplayName { get; private set; }

	public string Name { get; private set; }

	public string Language { get; private set; }

	public bool IsDefault { get; private set; }

	protected TrackBase()
	{
	}

	internal TrackBase(TrackType trackType, int uid, string name, string language, bool isDefault)
	{
		TrackType = trackType;
		Uid = uid;
		Name = name;
		Language = language;
		IsDefault = isDefault;
		DisplayName = CreateDisplayName();
	}

	protected string CreateDisplayName()
	{
		string text = (string.IsNullOrEmpty(Name) ? ("Track " + Uid) : Name);
		if (!string.IsNullOrEmpty(Language))
		{
			text = $"{text} ({Language})";
		}
		return text;
	}
}

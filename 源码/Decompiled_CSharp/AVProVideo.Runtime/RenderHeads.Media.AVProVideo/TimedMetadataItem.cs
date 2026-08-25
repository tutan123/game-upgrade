namespace RenderHeads.Media.AVProVideo;

public class TimedMetadataItem
{
	private double _presentationTime;

	private string _text;

	public double PresentationTime => _presentationTime;

	public string Text => _text;

	internal TimedMetadataItem(double presentationTime, string text)
	{
		_presentationTime = presentationTime;
		_text = text;
	}

	private TimedMetadataItem()
	{
	}
}

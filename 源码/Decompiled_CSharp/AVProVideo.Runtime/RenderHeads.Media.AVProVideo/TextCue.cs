namespace RenderHeads.Media.AVProVideo;

public class TextCue
{
	public string Text { get; private set; }

	private TextCue()
	{
	}

	internal TextCue(string text)
	{
		Text = text;
	}
}

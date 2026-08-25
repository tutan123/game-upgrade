using System;

namespace RenderHeads.Media.AVProVideo;

public interface IMediaPlayer
{
	void OnEnable();

	void Update();

	void EndUpdate();

	void BeginRender();

	void Render();

	IntPtr GetNativePlayerHandle();
}

using System;
using UnityEngine;
using Xlsx;

namespace FrameWork;

[Serializable]
public class FileData
{
	public Xlsx_Language_Key FileName;

	public Xlsx_Language_Key FileInfo;

	public TextAsset chineseTextAsset;

	public TextAsset englishTextAsset;

	public string videoPath;

	private void VideoChange()
	{
	}
}

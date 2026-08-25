using System;
using System.Collections.Generic;
using UnityEngine;
using Xlsx;

namespace FrameWork;

[Serializable]
public class FolderData
{
	public Xlsx_Language_Key FolderName;

	public List<FileData> FileList;

	[SerializeReference]
	public List<FolderData> FolderList = new List<FolderData>();
}

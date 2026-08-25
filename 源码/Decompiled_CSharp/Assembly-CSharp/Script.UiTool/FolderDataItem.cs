using FrameWork;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class FolderDataItem : MonoBehaviour
{
	public TMP_Text nameText;

	public Image icon;

	public RectTransform itemGroup;

	public Sprite folderIcon;

	public Sprite fileIcon;

	public TMP_FontAsset folderFont;

	public TMP_FontAsset fileFont;

	public float folderFontSize = 45f;

	public float fileFontSize = 38f;

	public RectTransform mask;

	public CheckTextWidth checkTextWidth;

	public GameObject maskGo;

	private ScrollRect _scrollView;

	private FolderData _folderData;

	private FileData _fileData;

	private bool _isFile;

	private bool _isOpenFolder;

	private RectTransform _parent;

	private FolderDataItem _parentFolderDataItem;

	public void Init(ScrollRect view, RectTransform parent, FolderDataItem folderDataItem, FolderData folderData, FileData fileData)
	{
		_scrollView = view;
		_parentFolderDataItem = folderDataItem;
		_parent = parent;
		_folderData = folderData;
		_fileData = fileData;
		_isOpenFolder = false;
		_isFile = fileData != null;
		if (folderData != null)
		{
			icon.sprite = folderIcon;
			nameText.text = LanguageMrg.GetText(folderData.FolderName);
		}
		else if (fileData != null)
		{
			icon.sprite = fileIcon;
			if (IsUnLockFile())
			{
				nameText.text = LanguageMrg.GetText(fileData.FileName);
			}
			else
			{
				nameText.text = LanguageMrg.GetText("A3631");
			}
		}
		for (int num = itemGroup.childCount - 1; num >= 0; num--)
		{
			SingletonAsMono<FolderDataMrg>.Instance.Enqueue(itemGroup.GetChild(num).GetComponent<FolderDataItem>());
		}
		if (_isFile)
		{
			nameText.font = fileFont;
			nameText.fontSize = fileFontSize;
			maskGo.SetActiveAsCheck(!IsUnLockFile());
		}
		else
		{
			nameText.font = folderFont;
			nameText.fontSize = folderFontSize;
			maskGo.SetActiveAsCheck(active: false);
		}
		checkTextWidth.UpdateTextSize();
		UpdateLayoutRebuilder();
		SingletonAsMono<FrameWork.Mono>.Instance.Frame(delegate
		{
			Vector3 position = mask.position;
			Vector2 targetLocalLoc = FrameWork.Tool.GetTargetLocalLoc(_scrollView.viewport, position, UiManager.GetCamera());
			float x = _scrollView.viewport.rect.width - Mathf.Abs(targetLocalLoc.x) - mask.anchoredPosition.x;
			mask.sizeDelta = new Vector2(x, mask.sizeDelta.y);
			nameText.GetComponent<TMPMarquee>().RefreshMarquee();
		});
	}

	private bool IsUnLockFile()
	{
		return SingletonAsMono<GameDataMrg>.Instance.IsUnLockGlobalKey(_fileData.videoPath);
	}

	public void Click()
	{
		if (_isFile)
		{
			if (IsUnLockFile())
			{
				UiManager.GetUi<VideoListWindows>().InitInfo(_fileData);
			}
			return;
		}
		if (_isOpenFolder)
		{
			for (int num = itemGroup.childCount - 1; num >= 0; num--)
			{
				SingletonAsMono<FolderDataMrg>.Instance.Enqueue(itemGroup.GetChild(num).GetComponent<FolderDataItem>());
			}
			_isOpenFolder = false;
		}
		else
		{
			for (int i = 0; i < _folderData.FolderList.Count; i++)
			{
				FolderDataItem folderDataItem = SingletonAsMono<FolderDataMrg>.Instance.Dequeue();
				folderDataItem.transform.SetParent(itemGroup);
				folderDataItem.Init(_scrollView, itemGroup, this, _folderData.FolderList[i], null);
				folderDataItem.transform.localScale = Vector3.one;
			}
			for (int j = 0; j < _folderData.FileList.Count; j++)
			{
				FolderDataItem folderDataItem2 = SingletonAsMono<FolderDataMrg>.Instance.Dequeue();
				folderDataItem2.transform.SetParent(itemGroup);
				folderDataItem2.Init(_scrollView, itemGroup, this, null, _folderData.FileList[j]);
				folderDataItem2.transform.localScale = Vector3.one;
			}
			_isOpenFolder = true;
		}
		UpdateLayoutRebuilder();
	}

	public void UpdateLayoutRebuilder()
	{
		LayoutRebuilder.ForceRebuildLayoutImmediate(itemGroup);
		LayoutRebuilder.ForceRebuildLayoutImmediate(base.transform.GetComponent<RectTransform>());
		if (_parentFolderDataItem != null)
		{
			_parentFolderDataItem.UpdateLayoutRebuilder();
		}
		else if ((bool)_parent)
		{
			LayoutRebuilder.ForceRebuildLayoutImmediate(_parent);
		}
	}
}

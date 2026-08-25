using FrameWork;
using UnityEngine;

namespace Script.UiTool;

public class FolderDataMrg : SingletonAsMono<FolderDataMrg>
{
	private ObjectPool<FolderDataItem> _objectPool = new ObjectPool<FolderDataItem>(GetItem);

	private static FolderDataItem GetItem()
	{
		return Object.Instantiate(ABMrg.Load<GameObject>("FolderDataItem")).GetComponent<FolderDataItem>();
	}

	public FolderDataItem Dequeue()
	{
		FolderDataItem folderDataItem = _objectPool.DeQueue();
		folderDataItem.SetActive(active: true);
		return folderDataItem;
	}

	public void Enqueue(FolderDataItem item)
	{
		item.SetActive(active: false);
		item.transform.SetParent(base.transform);
		item.transform.localPosition = new Vector3(0f, 100000f, 0f);
		_objectPool.EnQueue(item);
	}

	public void Clear()
	{
		while (_objectPool.GetSize() > 0)
		{
			FolderDataItem folderDataItem = _objectPool.DeQueue();
			if ((bool)folderDataItem)
			{
				Object.Destroy(folderDataItem.gameObject);
			}
		}
	}
}

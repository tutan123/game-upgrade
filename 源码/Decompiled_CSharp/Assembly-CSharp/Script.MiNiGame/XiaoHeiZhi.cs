using System.Collections.Generic;
using FrameWork;
using UnityEngine;

namespace Script.MiNiGame;

public class XiaoHeiZhi : MonoBehaviour
{
	public float speed = 5f;

	private Item _dianZhanItem;

	private List<Item> _items = new List<Item>();

	private void Update()
	{
		if (!UiManager.IsCanPlay())
		{
			return;
		}
		if (_dianZhanItem == null)
		{
			_items.Clear();
			Transform itemParent = MiNiGameMrg.Instance.itemParent;
			for (int i = 0; i < itemParent.childCount; i++)
			{
				if (itemParent.GetChild(i).TryGetComponent<Item>(out var component) && component.itemType == ItemType.DianZan)
				{
					_items.Add(component);
					break;
				}
			}
			if (_items.Count == 0)
			{
				Object.Destroy(base.gameObject);
				return;
			}
			_ = _items[0];
			for (int j = 0; j < _items.Count; j++)
			{
				_items.Sort((Item a, Item b) => Vector3.Distance(a.transform.position, base.transform.position).CompareTo(Vector3.Distance(b.transform.position, base.transform.position)));
				_dianZhanItem = _items[0];
			}
		}
		else if (_dianZhanItem != null)
		{
			base.transform.Translate((_dianZhanItem.transform.position - base.transform.position).normalized * speed * Time.deltaTime);
			if (_dianZhanItem.transform.position.x > base.transform.position.x)
			{
				base.transform.localScale = new Vector3(-1f, 1f, 1f);
			}
			else
			{
				base.transform.localScale = new Vector3(1f, 1f, 1f);
			}
			if (Vector3.Distance(base.transform.position, _dianZhanItem.transform.position) < 0.1f)
			{
				GameObject obj = Object.Instantiate(ABMrg.Load<GameObject>("DianZanAnim"));
				obj.transform.position = base.transform.position;
				Object.Destroy(obj, 0.2f);
				Object.Destroy(_dianZhanItem.gameObject);
			}
		}
	}
}

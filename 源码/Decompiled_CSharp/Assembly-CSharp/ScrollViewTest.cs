using System.Collections.Generic;
using FrameWork;
using UnityEngine;
using UnityEngine.UI;

public class ScrollViewTest : MonoBehaviour
{
	public string[] img;

	private Dictionary<int, Sprite> dic = new Dictionary<int, Sprite>();

	public ScrollRectTool scrollRectTool;

	private void Start()
	{
		scrollRectTool.Init(img.Length, UpdateItem);
	}

	public async void UpdateItem(int index, GameObject go)
	{
		if (dic.ContainsKey(index))
		{
			go.GetComponent<Image>().sprite = dic[index];
			return;
		}
		RequestTool requestTool = RequestTool.Create(img[index], Methods.Get);
		go.GetComponent<Image>().sprite = null;
		Sprite sprite = await requestTool.SendTaskAsTexture();
		go.GetComponent<Image>().sprite = sprite;
		dic.TryAdd(index, sprite);
	}

	private void Update()
	{
	}
}

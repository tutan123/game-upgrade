using System.Collections.Generic;
using FrameWork;
using FrameWork.Data;
using UnityEngine;

namespace Script.Tool;

public class MapLoadData : MonoBehaviour
{
	public List<Sprite> MeiMei;

	public List<Sprite> GonShiLaoBan;

	public List<Sprite> BaoZhuPo;

	public List<Sprite> LiQinTon;

	public List<Sprite> XiaoFuPo;

	public List<Sprite> JiuBaLaoBanNian;

	public List<Sprite> XiXueGui;

	public List<Sprite> XiJiaoMei;

	public List<Sprite> MiShu;

	public List<Sprite> PenZai;

	public Sprite GetRandomSprite()
	{
		Sprite result = MeiMei[Random.Range(0, MeiMei.Count)];
		List<Sprite> list = new List<Sprite>();
		Dictionary<string, string> data = GameData.GetCurSavedData().Data;
		if (data.GetProperty("MMFavorability", "Property") > 0)
		{
			list.AddRange(MeiMei);
		}
		if (data.GetProperty("LBNFavorability", "Property") > 0)
		{
			list.AddRange(JiuBaLaoBanNian);
		}
		if (data.GetProperty("BZPFavorability", "Property") > 0)
		{
			list.AddRange(BaoZhuPo);
		}
		if (data.GetProperty("LQTFavorability", "Property") > 0)
		{
			list.AddRange(LiQinTon);
		}
		if (data.GetProperty("XFPFavorability", "Property") > 0)
		{
			list.AddRange(XiaoFuPo);
		}
		if (data.GetProperty("WDLYFavorability", "Property") > 0)
		{
			list.AddRange(GonShiLaoBan);
		}
		if (data.GetProperty("WDLYFavorability", "Property") > 0)
		{
			list.AddRange(GonShiLaoBan);
		}
		if (data.GetProperty("XXGFavorability", "Property") > 0)
		{
			list.AddRange(XiXueGui);
		}
		if (data.GetProperty("XJMFavorability", "Property") > 0)
		{
			list.AddRange(XiJiaoMei);
		}
		if (data.GetProperty("MSFavorability", "Property") > 0)
		{
			list.AddRange(MiShu);
		}
		if (data.GetProperty("PZFavorability", "Property") > 0)
		{
			list.AddRange(PenZai);
		}
		if (list.Count > 0)
		{
			result = list[Random.Range(0, list.Count)];
		}
		return result;
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using XNode;

namespace FrameWork;

public static class ExtendClass
{
	public static void SetActive(this MonoBehaviour mono, bool active)
	{
		if (mono.gameObject.activeSelf != active)
		{
			mono.gameObject.SetActive(active);
		}
	}

	public static void SetActive(this Actor mono, bool active)
	{
		if (mono.GetGameObject().activeSelf != active)
		{
			mono.GetGameObject().SetActive(active);
		}
	}

	public static void SetActive(this Transform mono, bool active)
	{
		if (mono.gameObject.activeSelf != active)
		{
			mono.gameObject.SetActive(active);
		}
	}

	public static void SetActiveAsCheck(this GameObject mono, bool active)
	{
		if (mono.activeSelf != active)
		{
			mono.SetActive(active);
		}
	}

	public static void HideChild(this Transform tran, int count)
	{
		for (int i = 0; i < tran.childCount; i++)
		{
			if (i >= count)
			{
				tran.GetChild(i).gameObject.SetActiveAsCheck(active: false);
			}
		}
	}

	public static void Destroy(this Transform tran)
	{
		UnityEngine.Object.Destroy(tran.gameObject);
	}

	public static void Destroy(this MonoBehaviour tran)
	{
		UnityEngine.Object.Destroy(tran.gameObject);
	}

	public static void Destroy(this Actor tran)
	{
		UnityEngine.Object.Destroy(tran.GetGameObject());
	}

	public static void Destroy(this GameObject tran)
	{
		UnityEngine.Object.Destroy(tran);
	}

	public static List<Node> GetOutNode(this NodePort node)
	{
		return (from port in node.GetConnections()
			select port.node).ToList();
	}

	public static int ToInt(this string v)
	{
		return int.Parse(v);
	}

	public static long ToLong(this string v)
	{
		return long.Parse(v);
	}

	public static bool ToBool(this string v)
	{
		return bool.Parse(v);
	}

	public static T ToEnum<T>(this string v) where T : struct, Enum
	{
		return Enum.Parse<T>(v);
	}

	public static float ToFloat(this string v)
	{
		return float.Parse(v, CultureInfo.InvariantCulture);
	}

	public static void DoScale(this RectTransform v, Vector3 nor, Vector3 scale, Action end = null, float time = 0.3f)
	{
		v.localScale = nor;
		TweenerCore<Vector3, Vector3, VectorOptions> tweenerCore = v.DOScale(scale, time);
		tweenerCore.onComplete = (TweenCallback)Delegate.Combine(tweenerCore.onComplete, (TweenCallback)delegate
		{
			end?.Invoke();
		});
	}

	public static void TranFor(this Transform tran, int count, Transform go, Action<int, GameObject> action = null)
	{
		Tool.HideAllChild(tran);
		for (int i = 0; i < count; i++)
		{
			Transform transform = null;
			transform = ((tran.childCount <= i) ? UnityEngine.Object.Instantiate(go, tran) : tran.GetChild(i));
			transform.SetActive(active: true);
			action?.Invoke(i, transform.gameObject);
		}
	}

	public static void TranForAdd1(this Transform tran, int count, Transform go, Action<int, GameObject> action = null)
	{
		for (int i = 0; i < count + 1; i++)
		{
			Transform transform = null;
			transform = ((tran.childCount <= i) ? UnityEngine.Object.Instantiate(go, tran) : tran.GetChild(i));
			transform.SetActive(active: true);
			if (i != count)
			{
				action?.Invoke(i, transform.gameObject);
			}
		}
	}
}

using FrameWork;
using Script.Mrg;
using UnityEngine;

namespace Script.Tool;

public class NoneColl : MonoBehaviour
{
	private void OnTriggerEnter(Collider other)
	{
		if ((bool)other.GetComponent<MapPlayer>())
		{
			UiManager.ShowTips(LanguageMrg.GetText("A1582"));
		}
	}
}

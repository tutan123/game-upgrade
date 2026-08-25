using DG.Tweening;
using Script.Mrg;
using UnityEngine;

namespace FrameWork;

public class Main
{
	[RuntimeInitializeOnLoadMethod]
	public static void Run()
	{
		DOTween.defaultAutoKill = true;
		LoadMrg.LoadAsNotyMapLoad(Scenes.Load);
	}
}

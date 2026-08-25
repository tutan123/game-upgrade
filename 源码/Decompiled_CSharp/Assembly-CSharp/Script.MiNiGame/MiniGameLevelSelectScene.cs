using FrameWork;
using UnityEngine;

namespace Script.MiNiGame;

public class MiniGameLevelSelectScene : MonoBehaviour
{
	private void Start()
	{
		UiManager.OpenUi<LevelSelectWindows>();
	}
}

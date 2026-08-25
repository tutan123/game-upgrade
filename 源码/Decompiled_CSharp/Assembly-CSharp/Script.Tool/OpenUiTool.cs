using FrameWork;
using Script.Mrg;
using UnityEngine;

namespace Script.Tool;

public class OpenUiTool : MonoBehaviour
{
	public void OpenMap()
	{
		UiManager.OpenUi<MainWindows>();
		LoadMrg.UnloadMap("Day");
		LoadMrg.UnloadMap("Night");
	}

	public void OpenSave()
	{
		UiManager.OpenUi<MainWindows>().OpenSave();
	}

	public void OpenPhone()
	{
		UiManager.OpenUi<MainWindows>().OpenMessage();
	}

	public void OpenMniMap()
	{
		SingletonAsMono<GameDataMrg>.Instance.Is2D = true;
		UiManager.GetUi<MainWindows>().ChangeMap();
	}
}

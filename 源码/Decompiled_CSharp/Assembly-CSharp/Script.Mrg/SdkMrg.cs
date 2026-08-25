using FrameWork;
using FrameWork.Data;
using Steamworks;

namespace Script.Mrg;

public static class SdkMrg
{
	private static AppId_t _dlcAppId = new AppId_t(4489760u);

	public static bool IsDownLoadDlc()
	{
		if (!GameData.IsOpen("HighMode"))
		{
			return false;
		}
		return SteamApps.BIsDlcInstalled(AppId_t.Invalid);
	}

	public static bool IsDown4KDlc()
	{
		if (SteamManager.Initialized)
		{
			return IsDownLoadDlc();
		}
		return false;
	}

	public static void UnLockAchievement(string key)
	{
		if (!SteamManager.Initialized)
		{
			return;
		}
		bool pbAchieved = false;
		SteamUserStats.GetAchievement(key, out pbAchieved);
		if (!pbAchieved)
		{
			if (SteamUserStats.SetAchievement(key))
			{
				SteamUserStats.StoreStats();
				MyLog.Log("成就解锁成功: " + key);
			}
			else
			{
				MyLog.LogError("成就设置失败，请检查 API Name 是否正确");
			}
		}
	}
}

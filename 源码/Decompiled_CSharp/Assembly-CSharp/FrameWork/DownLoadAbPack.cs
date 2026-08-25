using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace FrameWork;

public static class DownLoadAbPack
{
	private static ConcurrentQueue<AbPackDate> _abPackDates = new ConcurrentQueue<AbPackDate>();

	public static void AddPackDownTack(List<AbPackDate> abPackDates, Action<float, float, string, string> progress, Action<List<AbPackDate>> end, Action<string> err = null)
	{
		long lenght = 0L;
		foreach (AbPackDate abPackDate in abPackDates)
		{
			lenght += abPackDate.Size;
			_abPackDates.Enqueue(abPackDate);
		}
		DownLoadAsset(lenght, abPackDates, progress, delegate
		{
			progress?.Invoke(1f, 0f, DownLoad.GetFileSize(lenght), DownLoad.GetFileSize(lenght));
			end(abPackDates);
		}, err);
	}

	private static void DownLoadAsset(long lenght, List<AbPackDate> abPackDates, Action<float, float, string, string> progress, Action end, Action<string> err)
	{
		if (_abPackDates.TryDequeue(out var abPackDate))
		{
			DownLoad.DownLoadAsset(Config.DownLoadUrl + Config.GetAbPath() + abPackDate.Name, delegate(float f1, float f2, string s1, string s2)
			{
				long num = 0L;
				abPackDate.CurDownLoadSize = (long)((float)abPackDate.Size * f1);
				foreach (AbPackDate abPackDate2 in abPackDates)
				{
					num += abPackDate2.CurDownLoadSize;
				}
				progress((float)num / (float)lenght, f1, DownLoad.GetFileSize(num), DownLoad.GetFileSize(lenght));
			}, delegate(byte[] bytes, string info)
			{
				abPackDate.PackData = bytes;
				DownLoadAsset(lenght, abPackDates, progress, end, err);
			}, err);
		}
		else
		{
			end();
		}
	}
}

using System;
using System.Collections;
using System.IO;
using System.Net;
using UnityEngine.Networking;

namespace FrameWork;

public class DownLoad
{
	private static int num = 1024;

	public static void DownLoadAsset(string path, Action<float, float, string, string> progress, Action<byte[], string> data, Action<string> err)
	{
		MyLog.Log(path);
		long statrTime = Tool.ConvertDateTimep(DateTime.Now);
		long lenght = 1L;
		RequestTool requestTool = RequestTool.Create(path, Methods.Get);
		requestTool.Send(delegate(byte[] bytes)
		{
			progress?.Invoke(1f, 0f, GetFileSize(lenght), GetFileSize(lenght));
			data?.Invoke(bytes, Path.GetFileName(path));
		}, err);
		requestTool.Progress = (Action<float, int>)Delegate.Combine(requestTool.Progress, (Action<float, int>)delegate(float f, int s)
		{
			lenght = s;
			long num = Tool.ConvertDateTimep(DateTime.Now) - statrTime;
			float fileSize = GetFileSize((float)s * f / (float)num);
			progress?.Invoke(f, fileSize, GetFileSize((float)s * f).ToString() ?? "", GetFileSize(s));
		});
	}

	private static IEnumerator DownLoadAssetIEumerator(string path, Action<float, float, string, string> progress, Action<byte[], string> data, Action<string> err)
	{
		using UnityWebRequest uwr = UnityWebRequest.Get(path);
		uwr.SendWebRequest();
		if (uwr.isHttpError || uwr.isNetworkError)
		{
			MyLog.Log(uwr.error);
			err?.Invoke(uwr.error);
			yield break;
		}
		long statrTime = Tool.ConvertDateTimep(DateTime.Now);
		long? packSize = GetPackSize(path);
		if (!packSize.HasValue)
		{
			err?.Invoke("网络错误获取失败");
			yield break;
		}
		long lenght = packSize.Value;
		string fileSize = GetFileSize(lenght);
		while (!uwr.isDone)
		{
			long num = Tool.ConvertDateTimep(DateTime.Now) - statrTime;
			float downloadProgress = uwr.downloadProgress;
			float fileSize2 = GetFileSize(downloadProgress * (float)lenght / (float)num);
			progress(downloadProgress, (int)fileSize2, GetFileSize((long)(downloadProgress * (float)lenght)), fileSize);
			yield return null;
		}
		if (uwr.isDone)
		{
			DownloadHandler downloadHandler = uwr.downloadHandler;
			data(downloadHandler.data, Path.GetFileName(path));
		}
	}

	public static string GetFileSize(long size)
	{
		if (size < num)
		{
			return size + "B";
		}
		if ((double)size < Math.Pow(num, 2.0))
		{
			return (size / num).ToString("f2") + "K";
		}
		if ((double)size < Math.Pow(num, 3.0))
		{
			return ((double)size / Math.Pow(num, 2.0)).ToString("f2") + "M";
		}
		if ((double)size < Math.Pow(num, 4.0))
		{
			return ((double)size / Math.Pow(num, 3.0)).ToString("f2") + "G";
		}
		return ((double)size / Math.Pow(num, 4.0)).ToString("f2") + "T";
	}

	public static string GetFileSize(int size)
	{
		if (size < num)
		{
			return size + "B";
		}
		if ((double)size < Math.Pow(num, 2.0))
		{
			return (size / num).ToString("f2") + "K";
		}
		if ((double)size < Math.Pow(num, 3.0))
		{
			return ((double)size / Math.Pow(num, 2.0)).ToString("f2") + "M";
		}
		if ((double)size < Math.Pow(num, 4.0))
		{
			return ((double)size / Math.Pow(num, 3.0)).ToString("f2") + "G";
		}
		return ((double)size / Math.Pow(num, 4.0)).ToString("f2") + "T";
	}

	public static float GetFileSize(float size)
	{
		if (size < (float)num)
		{
			return size;
		}
		if ((double)size < Math.Pow(num, 2.0))
		{
			return size / (float)num;
		}
		if ((double)size < Math.Pow(num, 3.0))
		{
			return (float)((double)size / Math.Pow(num, 2.0));
		}
		if ((double)size < Math.Pow(num, 4.0))
		{
			return (float)((double)size / Math.Pow(num, 3.0));
		}
		return (float)((double)size / Math.Pow(num, 4.0));
	}

	public static long? GetPackSize(string path)
	{
		MyLog.Log(path);
		long value = 1L;
		HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create(path);
		httpWebRequest.Method = "HEAD";
		try
		{
			HttpWebResponse httpWebResponse = (HttpWebResponse)httpWebRequest.GetResponse();
			if (httpWebResponse.StatusCode == HttpStatusCode.OK)
			{
				value = httpWebResponse.ContentLength;
				Console.WriteLine(httpWebResponse.ContentLength);
			}
			return value;
		}
		catch (Exception ex)
		{
			MyLog.Log(ex.Message);
			return null;
		}
	}
}

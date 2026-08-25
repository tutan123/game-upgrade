using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEngine;

namespace FrameWork;

public static class VersionDetection
{
	public static void Detection(string abConfigPath, Action<List<AbPackDate>, byte[]> versionInfo, Action<string> err = null)
	{
		DownLoad.DownLoadAsset(Config.DownLoadUrl + abConfigPath + Config.ConfigName, delegate
		{
		}, delegate(byte[] bytes, string s)
		{
			List<AbPackDate> list = new List<AbPackDate>();
			string @string = Encoding.UTF8.GetString(bytes);
			string[] array = @string.Split('|');
			string[] array2 = array.Select((string s1) => s1.Split(' ')[2]).ToArray();
			string[] source = array.Select((string s1) => s1.Split(' ')[0]).ToArray();
			if (File.Exists(Application.streamingAssetsPath + abConfigPath + Config.ConfigName))
			{
				string[] source2 = (from s1 in File.ReadAllText(Application.streamingAssetsPath + abConfigPath + Config.ConfigName, Encoding.UTF8).Split('|')
					select s1.Split(' ')[2]).ToArray();
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				if (File.Exists(Application.persistentDataPath + abConfigPath + Config.ConfigName))
				{
					string[] source3 = File.ReadAllText(Application.persistentDataPath + abConfigPath + Config.ConfigName).Split('|');
					list2 = source3.Select((string s1) => s1.Split(' ')[2]).ToList();
					list3 = source3.Select((string s1) => s1.Split(' ')[0]).ToList();
				}
				for (int i = 0; i < array2.Length; i++)
				{
					if (!source2.Contains(array2[i]) && !list2.Contains(array2[i]))
					{
						string[] array3 = array[i].Split(' ');
						list.Add(new AbPackDate
						{
							Name = array3[0],
							Size = long.Parse(array3[1]),
							Md5 = array3[2]
						});
					}
				}
				for (int j = 0; j < list3.Count; j++)
				{
					if (!source.Contains(list3[j]))
					{
						MyLog.Log("删除旧资源:" + Application.persistentDataPath + abConfigPath + list3[j]);
						File.Delete(Application.persistentDataPath + abConfigPath + list3[j]);
					}
				}
				versionInfo(list, bytes);
			}
			else
			{
				List<string> list4 = new List<string>();
				List<string> list5 = new List<string>();
				if (File.Exists(Application.persistentDataPath + abConfigPath + Config.ConfigName))
				{
					string[] source4 = File.ReadAllText(Application.persistentDataPath + abConfigPath + Config.ConfigName).Split('|');
					list4 = source4.Select((string s1) => s1.Split(' ')[2]).ToList();
					list5 = source4.Select((string s1) => s1.Split(' ')[0]).ToList();
				}
				if (!string.IsNullOrEmpty(@string))
				{
					for (int k = 0; k < array2.Length; k++)
					{
						if (!list4.Contains(array2[k]))
						{
							string[] array4 = array[k].Split(' ');
							list.Add(new AbPackDate
							{
								Name = array4[0],
								Size = long.Parse(array4[1]),
								Md5 = array4[2]
							});
						}
					}
				}
				for (int l = 0; l < list5.Count; l++)
				{
					if (!source.Contains(list5[l]))
					{
						MyLog.Log("删除旧资源:" + Application.persistentDataPath + abConfigPath + list5[l]);
						File.Delete(Application.persistentDataPath + abConfigPath + list5[l]);
					}
				}
				versionInfo(list, bytes);
			}
		}, err);
	}

	public static void Detection(Action<List<AbPackDate>, byte[]> versionInfo, Action<string> err = null)
	{
		Detection(Config.GetAbPath(), versionInfo, err);
	}

	private static bool ListHasValue(List<AbPackDate> da1, string da2)
	{
		foreach (AbPackDate item in da1)
		{
			if (item.Name.Equals(da2))
			{
				return true;
			}
		}
		return false;
	}
}

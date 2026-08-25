using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public class SubtitleUtils
{
	private static double ParseTimeToSeconds(string text)
	{
		double result = 0.0;
		string[] array = text.Split(':', ',');
		if (array.Length == 4)
		{
			int num = int.Parse(array[0]);
			int num2 = int.Parse(array[1]);
			int num3 = int.Parse(array[2]);
			result = (double)int.Parse(array[3]) / 1000.0 + (double)(num3 + (num2 + num * 60) * 60);
		}
		return result;
	}

	public static List<Subtitle> ParseSubtitlesSRT(string data)
	{
		List<Subtitle> list = null;
		if (!string.IsNullOrEmpty(data))
		{
			data = data.Trim();
			string[] array = new Regex("\n\r|\r\n|\n|\r").Split(data);
			if (array.Length >= 3)
			{
				list = new List<Subtitle>(256);
				int num = 0;
				int num2 = 0;
				Subtitle subtitle = null;
				for (int i = 0; i < array.Length; i++)
				{
					switch (num2)
					{
					case 0:
						subtitle = new Subtitle();
						subtitle.index = num;
						break;
					case 1:
					{
						string[] array2 = array[i].Split(new string[1] { " --> " }, StringSplitOptions.RemoveEmptyEntries);
						if (array2.Length == 2)
						{
							subtitle.timeStart = ParseTimeToSeconds(array2[0]);
							subtitle.timeEnd = ParseTimeToSeconds(array2[1]);
							break;
						}
						throw new FormatException("SRT format doesn't appear to be valid");
					}
					default:
						if (!string.IsNullOrEmpty(array[i]))
						{
							if (num2 == 2)
							{
								subtitle.text = array[i];
								break;
							}
							Subtitle subtitle2 = subtitle;
							subtitle2.text = subtitle2.text + "\n" + array[i];
						}
						break;
					}
					if (string.IsNullOrEmpty(array[i]) && num2 > 1)
					{
						list.Add(subtitle);
						num2 = 0;
						num++;
						subtitle = null;
					}
					else
					{
						num2++;
					}
				}
				if (subtitle != null)
				{
					list.Add(subtitle);
					subtitle = null;
				}
			}
			else
			{
				Debug.LogWarning("[AVProVideo] SRT format doesn't appear to be valid");
			}
		}
		return list;
	}
}

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using FrameWork.AchievementTool;
using FrameWork.ChatTool;
using FrameWork.Data;
using Script.Audio;
using Script.Mrg;
using Script.Tool;
using Script.UiTool;
using UnityEngine;
using UnityEngine.Networking;
using XNode;
using Xlsx;

namespace FrameWork;

public static class Tool
{
	private static readonly StringBuilder _audioPath = new StringBuilder(512);

	private static readonly StringBuilder _videoPath = new StringBuilder(512);

	private static readonly StringBuilder _sb = new StringBuilder(512);

	private static readonly MD5 _md5 = MD5.Create();

	private const string Base62Alphabet = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";

	private static string[] _tips = new string[7] { "A1690", "A1691", "A1692", "A1693", "A1694", "A1695", "A1697" };

	private static CollType[] _collTypes = new CollType[27]
	{
		CollType.GonShi,
		CollType.TuShuGuang,
		CollType.FuZhuanDian,
		CollType.BianLiDian,
		CollType.DianDonChe,
		CollType.CaiPiaoDian,
		CollType.YaoDian,
		CollType.GonYuan,
		CollType.HaiXianDaPaiDan,
		CollType.JianShenFan,
		CollType.HuanMenJi,
		CollType.XiaoFanGuan,
		CollType.CheDian,
		CollType.DianBoNv,
		CollType.LiFaDian,
		CollType.AnMo,
		CollType.HaoZai,
		CollType.DaiKuanRen,
		CollType.HeiYiRen,
		CollType.JiaXiZhengZuo,
		CollType.LuBianXiaoGou,
		CollType.LuBianChuDian,
		CollType.JianQian,
		CollType.FuSheBan,
		CollType.YiZhiCaiDan,
		CollType.CheZhen,
		CollType.HaiBinZhanDao
	};

	public static string Key => "kljsdkkdlo4454GG00155sajuklmbkdl";

	public static string GetAbName(string name)
	{
		return GetMd5AsString(name);
	}

	public static object ConversionType(string type, string value)
	{
		switch (type)
		{
		case "int":
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			return int.Parse(value);
		case "string":
			if (string.IsNullOrEmpty(value))
			{
				return "";
			}
			return value;
		case "float":
			if (string.IsNullOrEmpty(value))
			{
				return 0f;
			}
			return float.Parse(value, CultureInfo.InvariantCulture);
		case "long":
			if (string.IsNullOrEmpty(value))
			{
				return 0;
			}
			return long.Parse(value);
		case "double":
			if (string.IsNullOrEmpty(value))
			{
				return 0f;
			}
			return double.Parse(value, CultureInfo.InvariantCulture);
		case "Vector3":
		{
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			string[] array2 = value.Split(',');
			return new Vector3(float.Parse(array2[0], CultureInfo.InvariantCulture), float.Parse(array2[1], CultureInfo.InvariantCulture), float.Parse(array2[2], CultureInfo.InvariantCulture));
		}
		case "Vector2":
		{
			if (string.IsNullOrEmpty(value))
			{
				return null;
			}
			string[] array = value.Split(',');
			return new Vector2(float.Parse(array[0], CultureInfo.InvariantCulture), float.Parse(array[1], CultureInfo.InvariantCulture));
		}
		case "string[]":
			if (string.IsNullOrEmpty(value))
			{
				return new string[0];
			}
			return (from s in value.Split(',')
				select s.Trim()).ToArray();
		case "int[]":
			if (string.IsNullOrEmpty(value))
			{
				return new int[0];
			}
			return (from s in value.Split(',')
				select int.Parse(s, CultureInfo.InvariantCulture)).ToArray();
		case "long[]":
			if (string.IsNullOrEmpty(value))
			{
				return new long[0];
			}
			return (from s in value.Split(',')
				select long.Parse(s, CultureInfo.InvariantCulture)).ToArray();
		case "float[]":
			if (string.IsNullOrEmpty(value))
			{
				return new float[0];
			}
			return (from s in value.Split(',')
				select float.Parse(s, CultureInfo.InvariantCulture)).ToArray();
		default:
			return value;
		}
	}

	public static string GetMd5(string path)
	{
		using FileStream inputStream = new FileStream(path, FileMode.Open);
		byte[] array = new MD5CryptoServiceProvider().ComputeHash(inputStream);
		StringBuilder stringBuilder = new StringBuilder();
		for (int i = 0; i < array.Length; i++)
		{
			stringBuilder.Append(array[i].ToString("x2"));
		}
		return stringBuilder.ToString();
	}

	public static string GetMd5AsString(string key)
	{
		return GetShortIdentity(key);
	}

	public static string GetShortIdentity(string key)
	{
		using MD5 mD = MD5.Create();
		ulong num = (ulong)Math.Abs(BitConverter.ToInt64(mD.ComputeHash(Encoding.UTF8.GetBytes(key)), 0));
		string text = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
		char[] array = new char[11];
		int length = 0;
		while (num != 0)
		{
			array[length++] = text[(int)(num % 62)];
			num /= 62;
		}
		return new string(array, 0, length);
	}

	public static long ConvertDateTimep(DateTime time)
	{
		return (time.ToUniversalTime().Ticks - 621355968000000000L) / 10000000;
	}

	public static byte[] Encrypt(byte[] toEncryptArray, string key)
	{
		byte[] key2 = Convert.FromBase64String(key);
		return new RijndaelManaged
		{
			Key = key2,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		}.CreateEncryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
	}

	public static byte[] Decrypt(byte[] toEncryptArray, string key)
	{
		byte[] key2 = Convert.FromBase64String(key);
		return new RijndaelManaged
		{
			Key = key2,
			Mode = CipherMode.ECB,
			Padding = PaddingMode.PKCS7
		}.CreateDecryptor().TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);
	}

	public static string GetPathMd5(string value)
	{
		string directoryName = Path.GetDirectoryName(value);
		string md5AsString = GetMd5AsString(Path.GetFileName(value));
		List<string> list = (from s in directoryName.Split("\\")
			where !string.IsNullOrEmpty(s)
			select s).Select(GetMd5AsString).ToList();
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			text = text + "/" + list[i];
		}
		return text + "/" + md5AsString;
	}

	public static AudioClip GetAudioClipPath(string videoName)
	{
		_audioPath.Clear();
		_audioPath.Append(Application.streamingAssetsPath);
		_audioPath.Append("/");
		bool num = SdkMrg.IsDown4KDlc();
		if (!num)
		{
			_audioPath.Append(ConvertToEncryptedPath("Video/Nor" + videoName));
		}
		else
		{
			_audioPath.Append(ConvertToEncryptedPath("Video/High" + videoName));
		}
		_audioPath.Append(".JPG");
		if (num && !File.Exists(_audioPath.ToString()))
		{
			_audioPath.Clear();
			_audioPath.Append(Application.streamingAssetsPath);
			_audioPath.Append("/");
			_audioPath.Append(ConvertToEncryptedPath("Video/Nor" + videoName));
			_audioPath.Append(".JPG");
		}
		string text = _audioPath.ToString();
		if (!File.Exists(text))
		{
			return null;
		}
		using UnityWebRequest unityWebRequest = UnityWebRequestMultimedia.GetAudioClip(text, AudioType.MPEG);
		unityWebRequest.SendWebRequest();
		while (!unityWebRequest.isDone)
		{
		}
		if (unityWebRequest.result != UnityWebRequest.Result.Success)
		{
			Debug.LogError("加载失败: " + unityWebRequest.error);
			return null;
		}
		return DownloadHandlerAudioClip.GetContent(unityWebRequest);
	}

	public static string GetAudioClipPathStr(string videoName)
	{
		_audioPath.Clear();
		_audioPath.Append(Application.streamingAssetsPath);
		_audioPath.Append("/");
		bool num = SdkMrg.IsDown4KDlc();
		if (!num)
		{
			_audioPath.Append(ConvertToEncryptedPath("Video/Nor" + videoName));
		}
		else
		{
			_audioPath.Append(ConvertToEncryptedPath("Video/High" + videoName));
		}
		_audioPath.Append(".MP3");
		if (num && !File.Exists(_audioPath.ToString()))
		{
			_audioPath.Clear();
			_audioPath.Append(Application.streamingAssetsPath);
			_audioPath.Append("/");
			_audioPath.Append(ConvertToEncryptedPath("Video/Nor" + videoName));
			_audioPath.Append(".JPG");
		}
		return _audioPath.ToString();
	}

	public static string GetVideoPath(string videoName)
	{
		_videoPath.Clear();
		_videoPath.Append(Application.streamingAssetsPath);
		_videoPath.Append("/");
		bool num = SdkMrg.IsDown4KDlc();
		if (!num)
		{
			_videoPath.Append(ConvertToEncryptedPath("Video/Nor" + videoName));
		}
		else
		{
			_videoPath.Append(ConvertToEncryptedPath("Video/High" + videoName));
		}
		_videoPath.Append(".PNG");
		if (num && !File.Exists(_videoPath.ToString()))
		{
			_videoPath.Clear();
			_videoPath.Append(Application.streamingAssetsPath);
			_videoPath.Append("/");
			_videoPath.Append(ConvertToEncryptedPath("Video/Nor" + videoName));
			_videoPath.Append(".PNG");
		}
		return _videoPath.ToString();
	}

	public static string ConvertToEncryptedPath(string originalPath)
	{
		lock (_sb)
		{
			_sb.Clear();
			ReadOnlySpan<char> readOnlySpan = originalPath.AsSpan();
			int num = 0;
			int length = readOnlySpan.Length;
			bool flag = true;
			for (int i = 0; i <= length; i++)
			{
				if (i != length && readOnlySpan[i] != '/')
				{
					continue;
				}
				ReadOnlySpan<char> readOnlySpan2 = readOnlySpan.Slice(num, i - num);
				num = i + 1;
				if (readOnlySpan2.Length == 0)
				{
					if (i == 0 && !flag)
					{
						_sb.Append('/');
					}
					flag = false;
					continue;
				}
				flag = false;
				string shortIdentity0Gc = GetShortIdentity0Gc(readOnlySpan2.ToString());
				_sb.Append(shortIdentity0Gc);
				if (i != length)
				{
					_sb.Append('/');
				}
			}
			return _sb.ToString();
		}
	}

	public static string GetShortIdentity0Gc(string key)
	{
		if (string.IsNullOrEmpty(key))
		{
			return string.Empty;
		}
		byte[] bytes = Encoding.UTF8.GetBytes(key);
		ulong num = (ulong)Math.Abs(BitConverter.ToInt64(_md5.ComputeHash(bytes), 0));
		Span<char> span = stackalloc char[11];
		int length = 0;
		while (num != 0)
		{
			span[length++] = "0123456789abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ"[(int)(num % 62)];
			num /= 62;
		}
		return new string(span.Slice(0, length));
	}

	public static string Encrypt(string toEncrypt)
	{
		return Encrypt(toEncrypt, Key);
	}

	public static string Decrypt(string toEncrypt)
	{
		return Decrypt(toEncrypt, Key);
	}

	public static string GetFileDecryptName(string fileName, string end = ".Png")
	{
		return Encrypt(fileName) + end;
	}

	public static string Encrypt(string plainText, string secretKey)
	{
		if (string.IsNullOrEmpty(plainText))
		{
			return "";
		}
		if (string.IsNullOrEmpty(secretKey))
		{
			throw new ArgumentException("Key cannot be empty");
		}
		byte[] mD5Hash = GetMD5Hash(secretKey);
		byte[] iV = mD5Hash;
		byte[] bytes;
		using (Aes aes = Aes.Create())
		{
			aes.Key = mD5Hash;
			aes.IV = iV;
			aes.Mode = CipherMode.CBC;
			aes.Padding = PaddingMode.PKCS7;
			ICryptoTransform transform = aes.CreateEncryptor(aes.Key, aes.IV);
			using MemoryStream memoryStream = new MemoryStream();
			using CryptoStream stream = new CryptoStream(memoryStream, transform, CryptoStreamMode.Write);
			using (StreamWriter streamWriter = new StreamWriter(stream))
			{
				streamWriter.Write(plainText);
			}
			bytes = memoryStream.ToArray();
		}
		return BytesToHexString(bytes);
	}

	public static string Decrypt(string encryptedHexString, string secretKey)
	{
		if (string.IsNullOrEmpty(encryptedHexString))
		{
			return "";
		}
		if (string.IsNullOrEmpty(secretKey))
		{
			throw new ArgumentException("Key cannot be empty");
		}
		try
		{
			byte[] buffer = HexStringToBytes(encryptedHexString);
			byte[] mD5Hash = GetMD5Hash(secretKey);
			byte[] iV = mD5Hash;
			string result = null;
			using (Aes aes = Aes.Create())
			{
				aes.Key = mD5Hash;
				aes.IV = iV;
				aes.Mode = CipherMode.CBC;
				aes.Padding = PaddingMode.PKCS7;
				ICryptoTransform transform = aes.CreateDecryptor(aes.Key, aes.IV);
				using MemoryStream stream = new MemoryStream(buffer);
				using CryptoStream stream2 = new CryptoStream(stream, transform, CryptoStreamMode.Read);
				using StreamReader streamReader = new StreamReader(stream2);
				result = streamReader.ReadToEnd();
			}
			return result;
		}
		catch (Exception)
		{
			return "解密失败：Key 错误或密文损坏";
		}
	}

	private static byte[] GetMD5Hash(string input)
	{
		using MD5 mD = MD5.Create();
		return mD.ComputeHash(Encoding.UTF8.GetBytes(input));
	}

	private static string BytesToHexString(byte[] bytes)
	{
		StringBuilder stringBuilder = new StringBuilder();
		foreach (byte b in bytes)
		{
			stringBuilder.Append(b.ToString("X2"));
		}
		return stringBuilder.ToString();
	}

	private static byte[] HexStringToBytes(string hex)
	{
		if (hex.Length % 2 != 0)
		{
			throw new ArgumentException("Hex string length must be even.");
		}
		byte[] array = new byte[hex.Length / 2];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
		}
		return array;
	}

	public static bool InCircle(int x, int y, Vector2Int circleCenter, int r)
	{
		return Mathf.Pow(x - circleCenter.x, 2f) + Mathf.Pow(y - circleCenter.y, 2f) < Mathf.Pow(r, 2f);
	}

	public static bool IsInSquare(int x, int y, int width, int height, int curX, int curY)
	{
		if (curX >= x - width / 2 && curX <= x + width / 2 && curY >= y - height / 2)
		{
			return curY <= y + height / 2;
		}
		return false;
	}

	public static Vector2 GetTargetLocalLoc(RectTransform target, Vector3 pos, Camera camera = null)
	{
		RectTransformUtility.ScreenPointToLocalPointInRectangle(target, pos, camera, out var localPoint);
		return localPoint;
	}

	public static bool IsAndroid()
	{
		return CheckPlatform(RuntimePlatform.Android);
	}

	public static bool IsWindows()
	{
		if (!CheckPlatform(RuntimePlatform.WindowsEditor))
		{
			return CheckPlatform(RuntimePlatform.WindowsPlayer);
		}
		return true;
	}

	public static bool CheckPlatform(RuntimePlatform runtimePlatform)
	{
		return Application.platform == runtimePlatform;
	}

	public static Type ByClassNameGetType(string className)
	{
		return Assembly.GetExecutingAssembly().GetType("FrameWork." + className + "." + className);
	}

	public static void HideAllChild(Transform transform)
	{
		for (int i = 0; i < transform.childCount; i++)
		{
			transform.GetChild(i).SetActive(active: false);
		}
	}

	public static List<ButtonNode> GetAllButtonNodesOutNor(this VideoNode videoNode)
	{
		return videoNode.GetOutputPort("outVideoNode").GetAllButtonNodes();
	}

	public static List<ButtonNode> GetAllButtonNodes(this NodePort node, bool isEditor = false)
	{
		return (from buttonNode in node.GetOutNode().Select(delegate(Node node1)
			{
				if (node1 is VideoIf videoIf)
				{
					return videoIf.GetVideoNode().GetInputValue<ButtonNode>("buttonNode");
				}
				if (node1 is IsHasVideo isHasVideo)
				{
					return isHasVideo.GetVideoNode().GetInputValue<ButtonNode>("buttonNode");
				}
				if (node1 is ShowIf showIf)
				{
					if (isEditor)
					{
						return showIf.GetOutputPort("output").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
					}
					if (showIf.IsSuc())
					{
						return showIf.GetOutputPort("output").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
					}
					return (ButtonNode)null;
				}
				if (node1 is VideoIfPlayerEbriety videoIfPlayerEbriety)
				{
					if (!videoIfPlayerEbriety.IsSuc())
					{
						return videoIfPlayerEbriety.GetOutputPort("outputFail").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
					}
					return videoIfPlayerEbriety.GetOutputPort("outputSuccess").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
				}
				if (node1 is VideoIfTargetEbriety videoIfTargetEbriety)
				{
					if (!videoIfTargetEbriety.IsSuc())
					{
						return videoIfTargetEbriety.GetOutputPort("outputFail").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
					}
					return videoIfTargetEbriety.GetOutputPort("outputSuccess").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
				}
				if (node1 is ColorSurmise colorSurmise)
				{
					if (!colorSurmise.IsSuc())
					{
						return colorSurmise.GetOutputPort("outputFail").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
					}
					return colorSurmise.GetOutputPort("outputSuccess").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
				}
				if (node1 is IsWineList isWineList)
				{
					if (!isWineList.IsSuc())
					{
						return isWineList.GetOutputPort("outputFail").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
					}
					return isWineList.GetOutputPort("outputSuccess").GetAllVideoNodes()[0].GetInputValue<ButtonNode>("buttonNode");
				}
				return (node1 is VideoIfAsType videoIfAsType) ? videoIfAsType.GetVideoNode().GetInputValue<ButtonNode>("buttonNode") : (node1 as VideoNode).GetInputValue<ButtonNode>("buttonNode");
			})
			where buttonNode != null
			select buttonNode).ToList();
	}

	public static List<VideoNode> GetAllVideoNodesOutNor(this VideoNode videoNode)
	{
		return videoNode.GetOutputPort("outVideoNode").GetAllVideoNodes();
	}

	public static List<VideoNode> GetAllVideoNodesOutQueSuc(this VideoNode videoNode)
	{
		return videoNode.GetOutputPort("qteSuc").GetAllVideoNodes();
	}

	public static List<VideoNode> GetAllVideoNodesOutQueLose(this VideoNode videoNode)
	{
		return videoNode.GetOutputPort("qteFail").GetAllVideoNodes();
	}

	public static List<VideoNode> GetAllVideoNodesOutNor(this VideoNode videoNode, string nodeName)
	{
		return videoNode.GetOutputPort(nodeName).GetAllVideoNodes();
	}

	public static List<VideoNode> GetAllVideoNodes(this NodePort node)
	{
		return (from videoNode in node.GetOutNode().Select(delegate(Node node1)
			{
				if (node1 is VideoIf videoIf)
				{
					return videoIf.GetVideoNode();
				}
				if (node1 is IsHasVideo isHasVideo)
				{
					return isHasVideo.GetVideoNode();
				}
				if (node1 is ShowIf showIf)
				{
					if (!showIf.IsSuc())
					{
						return (VideoNode)null;
					}
					return (VideoNode)showIf.GetOutputPort("output").GetOutNode()[0];
				}
				if (node1 is VideoIfPlayerEbriety videoIfPlayerEbriety)
				{
					if (!videoIfPlayerEbriety.IsSuc())
					{
						return videoIfPlayerEbriety.GetOutputPort("outputFail").GetAllVideoNodes()[0];
					}
					return videoIfPlayerEbriety.GetOutputPort("outputSuccess").GetAllVideoNodes()[0];
				}
				if (node1 is VideoIfTargetEbriety videoIfTargetEbriety)
				{
					if (!videoIfTargetEbriety.IsSuc())
					{
						return videoIfTargetEbriety.GetOutputPort("outputFail").GetAllVideoNodes()[0];
					}
					return videoIfTargetEbriety.GetOutputPort("outputSuccess").GetAllVideoNodes()[0];
				}
				if (node1 is ColorSurmise colorSurmise)
				{
					if (!colorSurmise.IsSuc())
					{
						return colorSurmise.GetOutputPort("outputFail").GetAllVideoNodes()[0];
					}
					return colorSurmise.GetOutputPort("outputSuccess").GetAllVideoNodes()[0];
				}
				if (node1 is IsWineList isWineList)
				{
					if (!isWineList.IsSuc())
					{
						return isWineList.GetOutputPort("outputFail").GetAllVideoNodes()[0];
					}
					return isWineList.GetOutputPort("outputSuccess").GetAllVideoNodes()[0];
				}
				return (node1 is VideoIfAsType videoIfAsType) ? videoIfAsType.GetVideoNode() : (node1 as VideoNode);
			})
			where videoNode != null
			select videoNode).ToList();
	}

	public static VideoNode GetEvenFistNode(this VideoGraph videoGraph)
	{
		for (int i = 0; i < videoGraph.nodes.Count; i++)
		{
			if (videoGraph.nodes[i] is ChapterNode)
			{
				return (videoGraph.nodes[i] as ChapterNode).GetOutputPort("chapterNode").GetOutNode()[0] as VideoNode;
			}
		}
		return null;
	}

	public static ChapterNode GetEvenChapterNode(this VideoGraph videoGraph)
	{
		for (int i = 0; i < videoGraph.nodes.Count; i++)
		{
			if (videoGraph.nodes[i] is ChapterNode)
			{
				return videoGraph.nodes[i] as ChapterNode;
			}
		}
		return null;
	}

	public static VideoNode GetOutVideo(this ButtonNode buttonNode)
	{
		return buttonNode.GetOutputPort("button").GetAllVideoNodes()[0];
	}

	public static string GetTime(long time)
	{
		DateTimeOffset unixEpoch = DateTimeOffset.UnixEpoch;
		return unixEpoch.AddMilliseconds(time).LocalDateTime.ToString("yyyy/MM/dd HH:mm:ss");
	}

	public static string GetName(this PropertyTypeValue type)
	{
		switch (type)
		{
		case PropertyTypeValue.Stamina:
			return LanguageMrg.GetText("A48");
		case PropertyTypeValue.Wisdom:
			return LanguageMrg.GetText("A49");
		case PropertyTypeValue.Charm:
			return LanguageMrg.GetText("A50");
		case PropertyTypeValue.Morality:
			return LanguageMrg.GetText("A51");
		case PropertyTypeValue.CapacityForLiquor:
			return LanguageMrg.GetText("A52");
		case PropertyTypeValue.Execution:
			return LanguageMrg.GetText("A53");
		case PropertyTypeValue.Money:
			return LanguageMrg.GetText("A54");
		case PropertyTypeValue.Pressure:
			return LanguageMrg.GetText("A55");
		case PropertyTypeValue.BZPFavorability:
			return LanguageMrg.GetText("A56");
		case PropertyTypeValue.WDLYFavorability:
			return LanguageMrg.GetText("A57");
		case PropertyTypeValue.XFPFavorability:
			return LanguageMrg.GetText("A58");
		case PropertyTypeValue.XXGFavorability:
			return LanguageMrg.GetText("A59");
		case PropertyTypeValue.LQTFavorability:
			return LanguageMrg.GetText("A60");
		case PropertyTypeValue.MMFavorability:
			return LanguageMrg.GetText("A61");
		case PropertyTypeValue.LBNFavorability:
			return LanguageMrg.GetText("A62");
		case PropertyTypeValue.ZXSNFavorability:
			return LanguageMrg.GetText("A63");
		case PropertyTypeValue.DBNFavorability:
			return LanguageMrg.GetText("A64");
		case PropertyTypeValue.LJFavorability:
			return LanguageMrg.GetText("A65");
		case PropertyTypeValue.XJMFavorability:
			return LanguageMrg.GetText("A66");
		case PropertyTypeValue.NAMFavorability:
			return LanguageMrg.GetText("A67");
		case PropertyTypeValue.MSFavorability:
			return LanguageMrg.GetText("A68");
		case PropertyTypeValue.UpperPressureLimit:
			return LanguageMrg.GetText("A69");
		case PropertyTypeValue.MMHeiAn:
			return LanguageMrg.GetText("A76");
		case PropertyTypeValue.MMZiXing:
			return LanguageMrg.GetText("A77");
		case PropertyTypeValue.MMZiLi:
			return LanguageMrg.GetText("A78");
		case PropertyTypeValue.MMXianHui:
			return LanguageMrg.GetText("A79");
		case PropertyTypeValue.PlayerHp:
		case PropertyTypeValue.TmpPlayerHp:
			return LanguageMrg.GetText("A115");
		case PropertyTypeValue.EnemyHp:
			return LanguageMrg.GetText("A116");
		case PropertyTypeValue.EnemyHpMax:
			return LanguageMrg.GetText("A116");
		case PropertyTypeValue.SkillPoint:
			return LanguageMrg.GetText("A117");
		case PropertyTypeValue.PZFavorability:
			return LanguageMrg.GetText("A135");
		case PropertyTypeValue.MMTiLi:
			return LanguageMrg.GetText("A148");
		case PropertyTypeValue.KPI:
			return LanguageMrg.GetText("A395");
		case PropertyTypeValue.BianLiDianGonZuoCiShu:
			return LanguageMrg.GetText("A432");
		case PropertyTypeValue.PenZaiTiNeng:
			return LanguageMrg.GetText("A487");
		case PropertyTypeValue.PenZaiMeiLi:
			return LanguageMrg.GetText("A488");
		case PropertyTypeValue.PenZaiZhiHui:
			return LanguageMrg.GetText("A489");
		case PropertyTypeValue.PenZaiJiuLian:
			return LanguageMrg.GetText("A490");
		case PropertyTypeValue.PenZaiYaLi:
			return LanguageMrg.GetText("A491");
		case PropertyTypeValue.YaLiShangXian:
			return LanguageMrg.GetText("A959");
		case PropertyTypeValue.XinDonLiShangXian:
			return LanguageMrg.GetText("A960");
		case PropertyTypeValue.JiuLianBiLi:
			return LanguageMrg.GetText("A964") + "%";
		case PropertyTypeValue.TargetJiuLian:
			return LanguageMrg.GetText("A965");
		case PropertyTypeValue.ZhaoShiShuLianDu:
			return LanguageMrg.GetText("A984");
		case PropertyTypeValue.TmpTargetJiuLi:
			return LanguageMrg.GetText("A1185");
		case PropertyTypeValue.TmpJiuLian:
			return LanguageMrg.GetText("A1186");
		case PropertyTypeValue.SiWaWangChengJinDu:
			return LanguageMrg.GetText("A1288");
		case PropertyTypeValue.GonShiZhiMinDu:
			return LanguageMrg.GetText("A1289");
		case PropertyTypeValue.GonShiZhiChang:
			return LanguageMrg.GetText("A1290");
		case PropertyTypeValue.JianJin:
			return LanguageMrg.GetText("A1291");
		case PropertyTypeValue.TSGSNFavorability:
			return LanguageMrg.GetText("A1355");
		case PropertyTypeValue.XXSNFavorability:
			return LanguageMrg.GetText("A1356");
		case PropertyTypeValue.WYFavorability:
			return LanguageMrg.GetText("A1357");
		case PropertyTypeValue.LXYavorability:
			return LanguageMrg.GetText("A1358");
		case PropertyTypeValue.JiNengDian:
		case PropertyTypeValue.TmpJiNengDian:
			return LanguageMrg.GetText("A1575");
		case PropertyTypeValue.JinShengGonJi:
			return LanguageMrg.GetText("A1576");
		case PropertyTypeValue.ZhuHeQuan:
			return LanguageMrg.GetText("A1577");
		case PropertyTypeValue.JunTiQuan:
			return LanguageMrg.GetText("A1578");
		case PropertyTypeValue.RuLaiShengZhang:
			return LanguageMrg.GetText("A1579");
		case PropertyTypeValue.XiXueGuiHuiXue:
			return LanguageMrg.GetText("A1580");
		case PropertyTypeValue.YonChun:
			return LanguageMrg.GetText("A1581");
		case PropertyTypeValue.ZhaoZon:
			return LanguageMrg.GetText("A1702");
		case PropertyTypeValue.Money2:
			return LanguageMrg.GetText("A3039");
		case PropertyTypeValue.RuLaiShengZhangShouLian:
			return LanguageMrg.GetText("A5312");
		case PropertyTypeValue.MaLaoShiNuQi:
			return LanguageMrg.GetText("A6074");
		case PropertyTypeValue.XinDonLiHuiFu:
			return LanguageMrg.GetText("A5498");
		case PropertyTypeValue.AoShuGuoZai:
			return LanguageMrg.GetText("A6122");
		default:
			return "";
		}
	}

	public static string GetItemName(this ItemType itemType)
	{
		try
		{
			return LanguageMrg.GetText(Xlsx_Item_Query.XlsxDataAsOneKey.ByKeyGetValue(itemType.ToString()).Name);
		}
		catch (Exception ex)
		{
			MyLog.LogError(ex.Message);
		}
		return "";
	}

	public static string GetPropertyDataStr(this List<PropertyData> list)
	{
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].PropertyType == PropertyType.Property)
			{
				switch (list[i].propertyTypeValue)
				{
				case PropertyTypeValue.Execution:
					text = text + "<sprite name=TiLi>" + (list[i].PropertyValue + 1) + "  ";
					break;
				case PropertyTypeValue.Money:
					text = text + "<sprite name=Qian>" + (list[i].PropertyValue + 1) + "  ";
					break;
				}
			}
		}
		return text;
	}

	public static string GetPropertyDataStrAs3DScene(this List<PropertyData> list)
	{
		string text = "";
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].PropertyType == PropertyType.Property)
			{
				switch (list[i].propertyTypeValue)
				{
				case PropertyTypeValue.Execution:
					text = text + "<sprite name=TiLi_Tips>" + (list[i].PropertyValue + 1) + "  ";
					break;
				case PropertyTypeValue.Money:
					text = text + "<sprite name=Qian_Tips>" + (list[i].PropertyValue + 1) + "  ";
					break;
				}
			}
		}
		return text;
	}

	public static string GetPropertyDataStrAsVideoGroup(this VideoGraph videoGraph, VideoButton videoButton = null, bool isHasFistVideo = false)
	{
		string text = "";
		VideoNode nextVideoNode = videoGraph.GetEvenFistNode().GetNextVideoNode();
		ChapterNode evenChapterNode = videoGraph.GetEvenChapterNode();
		int num = 0;
		int num2 = 0;
		for (int i = 0; i < evenChapterNode.propertyType.Count; i++)
		{
			if (evenChapterNode.propertyType[i].PropertyType == PropertyType.Property && evenChapterNode.propertyType[i].propertyTypeValue == PropertyTypeValue.Execution)
			{
				num -= evenChapterNode.propertyType[i].PropertyValue + 1;
			}
			if (evenChapterNode.propertyType[i].PropertyType == PropertyType.Property && evenChapterNode.propertyType[i].propertyTypeValue == PropertyTypeValue.Money)
			{
				num2 -= evenChapterNode.propertyType[i].PropertyValue + 1;
			}
		}
		if (isHasFistVideo)
		{
			for (int j = 0; j < nextVideoNode.PropertyData.Count; j++)
			{
				if (nextVideoNode.PropertyData[j].PropertyType == PropertyType.Property && nextVideoNode.PropertyData[j].propertyTypeValue == PropertyTypeValue.Execution)
				{
					num += nextVideoNode.PropertyData[j].PropertyValue;
				}
				if (nextVideoNode.PropertyData[j].PropertyValue < 0 && nextVideoNode.PropertyData[j].PropertyType == PropertyType.Property && nextVideoNode.PropertyData[j].propertyTypeValue == PropertyTypeValue.Money)
				{
					num2 += nextVideoNode.PropertyData[j].PropertyValue;
				}
			}
			for (int k = 0; k < nextVideoNode.VideoEndPropertyData.Count; k++)
			{
				if (nextVideoNode.VideoEndPropertyData[k].PropertyType == PropertyType.Property && nextVideoNode.VideoEndPropertyData[k].propertyTypeValue == PropertyTypeValue.Execution)
				{
					num += nextVideoNode.VideoEndPropertyData[k].PropertyValue;
				}
				if (nextVideoNode.VideoEndPropertyData[k].PropertyValue < 0 && nextVideoNode.VideoEndPropertyData[k].PropertyType == PropertyType.Property && nextVideoNode.VideoEndPropertyData[k].propertyTypeValue == PropertyTypeValue.Money)
				{
					num2 += nextVideoNode.VideoEndPropertyData[k].PropertyValue;
				}
			}
		}
		if (videoButton != null && videoButton.GetButtonNode().isShowResetXdl)
		{
			num += SingletonAsMono<GameDataMrg>.Instance.GetProperty("XinDonLiHuiFu", "Property", 0L);
		}
		if (num != 0)
		{
			string arg = ((num > 0) ? "+" : "");
			text = text + $"<sprite name=TiLi>{arg}{num}" + "\n";
		}
		if (num2 != 0)
		{
			string arg2 = ((num2 > 0) ? "+" : "");
			text = text + $"<sprite name=Qian>{arg2}{num2}" + "\n";
		}
		return text;
	}

	public static bool IsSuc(this List<PropertyData> list)
	{
		if (list.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < list.Count; i++)
		{
			PropertyData propertyData = list[i];
			if (propertyData.videoIfType == VideoIfType.Gre)
			{
				if (propertyData.PropertyType == PropertyType.Item)
				{
					if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.itemType.ToString(), "Item") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.itemType.ToString(), "Item", 0L)) <= propertyData.PropertyValue)
					{
						return false;
					}
				}
				else if (propertyData.PropertyType == PropertyType.Property)
				{
					if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.propertyTypeValue.ToString(), "Property") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.propertyTypeValue.ToString(), "Property", 0L)) <= propertyData.PropertyValue)
					{
						return false;
					}
				}
				else if (propertyData.PropertyType == PropertyType.Value)
				{
					if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.TypeName.ToString(), "Value") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.TypeName.ToString(), "Value", 0L)) <= propertyData.PropertyValue)
					{
						return false;
					}
				}
				else if (propertyData.PropertyType == PropertyType.Other && (propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.otherType.ToString(), "Other") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.otherType.ToString(), "Other", 0L)) <= propertyData.PropertyValue)
				{
					return false;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Item)
			{
				if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.itemType.ToString(), "Item") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.itemType.ToString(), "Item", 0L)) >= propertyData.PropertyValue)
				{
					return false;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Property)
			{
				if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.propertyTypeValue.ToString(), "Property") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.propertyTypeValue.ToString(), "Property", 0L)) >= propertyData.PropertyValue)
				{
					return false;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Value)
			{
				if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.TypeName.ToString(), "Value") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.TypeName.ToString(), "Value", 0L)) >= propertyData.PropertyValue)
				{
					return false;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Other && (propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.otherType.ToString(), "Other") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.otherType.ToString(), "Other", 0L)) >= propertyData.PropertyValue)
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsSucOr(this List<PropertyData> list)
	{
		if (list.Count == 0)
		{
			return true;
		}
		for (int i = 0; i < list.Count; i++)
		{
			PropertyData propertyData = list[i];
			if (propertyData.videoIfType == VideoIfType.Gre)
			{
				if (propertyData.PropertyType == PropertyType.Item)
				{
					if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.itemType.ToString(), "Item") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.itemType.ToString(), "Item", 0L)) > propertyData.PropertyValue)
					{
						return true;
					}
				}
				else if (propertyData.PropertyType == PropertyType.Property)
				{
					if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.propertyTypeValue.ToString(), "Property") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.propertyTypeValue.ToString(), "Property", 0L)) > propertyData.PropertyValue)
					{
						return true;
					}
				}
				else if (propertyData.PropertyType == PropertyType.Value)
				{
					if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.TypeName.ToString(), "Value") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.TypeName.ToString(), "Value", 0L)) > propertyData.PropertyValue)
					{
						return true;
					}
				}
				else if (propertyData.PropertyType == PropertyType.Other && (propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.otherType.ToString(), "Other") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.otherType.ToString(), "Other", 0L)) > propertyData.PropertyValue)
				{
					return true;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Item)
			{
				if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.itemType.ToString(), "Item") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.itemType.ToString(), "Item", 0L)) < propertyData.PropertyValue)
				{
					return true;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Property)
			{
				if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.propertyTypeValue.ToString(), "Property") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.propertyTypeValue.ToString(), "Property", 0L)) < propertyData.PropertyValue)
				{
					return true;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Value)
			{
				if ((propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.TypeName.ToString(), "Value") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.TypeName.ToString(), "Value", 0L)) < propertyData.PropertyValue)
				{
					return true;
				}
			}
			else if (propertyData.PropertyType == PropertyType.Other && (propertyData.isRound ? SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty(propertyData.otherType.ToString(), "Value") : SingletonAsMono<GameDataMrg>.Instance.GetProperty(propertyData.otherType.ToString(), "Value", 0L)) < propertyData.PropertyValue)
			{
				return true;
			}
		}
		return false;
	}

	public static List<PropertyTypeValue> GetAllPropertyKey()
	{
		return new List<PropertyTypeValue>
		{
			PropertyTypeValue.Stamina,
			PropertyTypeValue.Wisdom,
			PropertyTypeValue.Charm,
			PropertyTypeValue.Morality,
			PropertyTypeValue.CapacityForLiquor,
			PropertyTypeValue.Execution,
			PropertyTypeValue.Money,
			PropertyTypeValue.Pressure
		};
	}

	public static string GetLanguage(this Xlsx_Language_Type xlsxLanguageType)
	{
		return xlsxLanguageType switch
		{
			Xlsx_Language_Type.Chinese => LanguageMrg.GetText("A107"), 
			Xlsx_Language_Type.English => LanguageMrg.GetText("A108"), 
			_ => "", 
		};
	}

	public static string GetNextLanguage()
	{
		List<string> list = new List<string>();
		Xlsx_Language_Type language = GameData.Language;
		for (Xlsx_Language_Type xlsx_Language_Type = Xlsx_Language_Type.Chinese; xlsx_Language_Type <= Xlsx_Language_Type.English; xlsx_Language_Type++)
		{
			list.Add(xlsx_Language_Type.ToString());
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Equals(language.ToString()))
			{
				if (i >= list.Count - 1)
				{
					return list[0];
				}
				return list[i + 1];
			}
		}
		return "";
	}

	public static string GetLastLanguage()
	{
		List<string> list = new List<string>();
		Xlsx_Language_Type language = GameData.Language;
		for (Xlsx_Language_Type xlsx_Language_Type = Xlsx_Language_Type.Chinese; xlsx_Language_Type <= Xlsx_Language_Type.English; xlsx_Language_Type++)
		{
			list.Add(xlsx_Language_Type.ToString());
		}
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].Equals(language.ToString()))
			{
				if (i == 0)
				{
					return list.Last();
				}
				return list[i - 1];
			}
		}
		return "";
	}

	public static void AddTypeValue(this List<PropertyData> list)
	{
		for (int i = 0; i < list.Count; i++)
		{
			list[i].AddTypeValue();
		}
	}

	public static void AddTypeValue(this PropertyData propertyData)
	{
		switch (propertyData.PropertyType)
		{
		case PropertyType.Value:
			if (propertyData.isRound)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty(propertyData.TypeName, propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			else
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(propertyData.TypeName, propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			break;
		case PropertyType.Item:
			if (propertyData.isRound)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty(propertyData.itemType.ToString(), propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			else
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(propertyData.itemType.ToString(), propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			break;
		case PropertyType.Property:
			if (propertyData.isRound)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty(propertyData.propertyTypeValue.ToString(), propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			else
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(propertyData.propertyTypeValue.ToString(), propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			break;
		case PropertyType.Other:
			if (propertyData.otherType == OtherType.Round)
			{
				SingletonAsMono<GameDataMrg>.Instance.CurRound++;
			}
			else if (propertyData.isRound)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty(propertyData.otherType.ToString(), propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			else
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(propertyData.otherType.ToString(), propertyData.PropertyValue, propertyData.PropertyType.ToString());
			}
			break;
		}
	}

	public static void AddTypeValueAsShowTips(this PropertyData propertyData, bool isCheck = true, bool isSkip = false)
	{
		if (propertyData.PropertyType == PropertyType.Property && propertyData.propertyTypeValue == PropertyTypeValue.TmpPlayerHp)
		{
			int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("DiFanZhengJia", "Property", 0L);
			PropertyData propertyData2 = new PropertyData
			{
				PropertyType = PropertyType.Property,
				propertyTypeValue = PropertyTypeValue.TmpPlayerHp,
				PropertyValue = (int)((float)propertyData.PropertyValue * Mathf.Max(0f, 1f + (float)property / 100f))
			};
			propertyData2.AddTypeValue();
			propertyData2.AddTypeValueAsShowTipsNotAdd(isCheck, isSkip);
		}
		else if (propertyData.PropertyType == PropertyType.Property && propertyData.propertyTypeValue == PropertyTypeValue.EnemyHp)
		{
			int property2 = SingletonAsMono<GameDataMrg>.Instance.GetProperty("ZiJiZhengJia", "Property", 0L);
			PropertyData propertyData3 = new PropertyData
			{
				PropertyType = PropertyType.Property,
				propertyTypeValue = PropertyTypeValue.EnemyHp,
				PropertyValue = (int)((float)propertyData.PropertyValue * Mathf.Max(0f, 1f + (float)property2 / 100f))
			};
			propertyData3.AddTypeValue();
			propertyData3.AddTypeValueAsShowTipsNotAdd(isCheck, isSkip);
		}
		else
		{
			propertyData.AddTypeValue();
			propertyData.AddTypeValueAsShowTipsNotAdd(isCheck, isSkip);
		}
	}

	public static void AddTypeValueAsShowTipsNotAdd(this PropertyData propertyData, bool isCheck = true, bool isSkip = false)
	{
		if (isCheck && (isSkip || !propertyData.isShowTips))
		{
			return;
		}
		string text = ((propertyData.PropertyValue > 0) ? ("+" + propertyData.PropertyValue.ToString().FormatStringNumber()) : (propertyData.PropertyValue.ToString().FormatStringNumber() ?? ""));
		switch (propertyData.PropertyType)
		{
		case PropertyType.Item:
			SingletonAsMono<InfoTipsMrg>.Instance.Add(propertyData.itemType.GetItemName() + text, propertyData.PropertyValue > 0);
			break;
		case PropertyType.Property:
			switch (propertyData.propertyTypeValue)
			{
			case PropertyTypeValue.MMHeiAn:
			case PropertyTypeValue.MMZiXing:
			case PropertyTypeValue.MMZiLi:
			case PropertyTypeValue.MMXianHui:
			case PropertyTypeValue.MMTiLi:
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.meiMeiGanWu);
				SingletonAsMono<InfoTipsMrg>.Instance.Add(LanguageMrg.GetText("A83"));
				break;
			case PropertyTypeValue.PenZaiTiNeng:
			case PropertyTypeValue.PenZaiMeiLi:
			case PropertyTypeValue.PenZaiZhiHui:
			case PropertyTypeValue.PenZaiJiuLian:
			case PropertyTypeValue.PenZaiYaLi:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(LanguageMrg.GetText("A1364"));
				break;
			case PropertyTypeValue.ZhaoShiShuLianDu:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(propertyData.propertyTypeValue.GetName() + text + "%");
				break;
			case PropertyTypeValue.RuLaiShengZhangShouLian:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(LanguageMrg.GetText("A5312"));
				break;
			case PropertyTypeValue.DiFanZhengJia:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(string.Format(LanguageMrg.GetText("A5358"), text), propertyData.PropertyValue > 0, propertyData.propertyTypeValue.ToString());
				break;
			case PropertyTypeValue.ZiJiZhengJia:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(string.Format(LanguageMrg.GetText("A5359"), text), propertyData.PropertyValue > 0, propertyData.propertyTypeValue.ToString());
				break;
			case PropertyTypeValue.AoShuGuoZai:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(propertyData.propertyTypeValue.GetName() + text + "%", propertyData.PropertyValue > 0, propertyData.propertyTypeValue.ToString());
				break;
			default:
				SingletonAsMono<InfoTipsMrg>.Instance.Add(propertyData.propertyTypeValue.GetName() + text, propertyData.PropertyValue > 0, propertyData.propertyTypeValue.ToString());
				break;
			case PropertyTypeValue.TmpPlayerHp:
			case PropertyTypeValue.EnemyHp:
			case PropertyTypeValue.JinShengGonJi:
			case PropertyTypeValue.ZhuHeQuan:
			case PropertyTypeValue.JunTiQuan:
			case PropertyTypeValue.RuLaiShengZhang:
			case PropertyTypeValue.XiXueGuiHuiXue:
			case PropertyTypeValue.YonChun:
			case PropertyTypeValue.PuTon:
				break;
			}
			break;
		}
	}

	public static List<ChatNode> GetAllChat(this MessageData msg)
	{
		ChatGraph chatGraph = ABMrg.Load<ChatGraph>(msg.assetName);
		List<ChatNode> chatNodes = new List<ChatNode>();
		if (chatGraph == null)
		{
			Debug.LogError("注意检查下:短信连线" + msg.assetName + "找不到报错了 这边是检测剔除了这个短信");
			return chatNodes;
		}
		ChatNode chatNode2 = null;
		for (int i = 0; i < chatGraph.nodes.Count; i++)
		{
			if (chatGraph.nodes[i] is StartChat)
			{
				StartChat startChat = chatGraph.nodes[i] as StartChat;
				if (startChat.maxRound == -1 || msg.msgType != 0 || SingletonAsMono<GameDataMrg>.Instance.CurRound < startChat.maxRound)
				{
					chatNode2 = (ChatNode)chatGraph.nodes[i].GetOutputPort("outPut").GetConnections()[0].node;
				}
				break;
			}
		}
		if (chatNode2 != null)
		{
			GetChatNodes(chatNode2);
		}
		return chatNodes;
		void GetChatNodes(ChatNode chatNode)
		{
			chatNodes.Add(chatNode);
			string msgSelectId = SingletonAsMono<GameDataMrg>.Instance.GetMsgSelectId(chatNode.uniqueID);
			if (chatNode.isHasBtn && !string.IsNullOrEmpty(msgSelectId))
			{
				bool flag = false;
				List<ChatNode> list = (from port in chatNode.GetOutputPort("selectBtnNode").GetConnections()
					select (ChatNode)port.node).ToList();
				for (int j = 0; j < list.Count; j++)
				{
					if (list[j].uniqueID == msgSelectId)
					{
						GetChatNodes(list[j]);
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					GetChatNodes(list[0]);
				}
			}
			else if (chatNode.isHasBtn)
			{
				List<NodePort> connections = chatNode.GetOutputPort("selectBtnNode").GetConnections();
				if (connections.Count > 0)
				{
					GetChatNodes((ChatNode)connections[0].node);
				}
			}
			else
			{
				List<NodePort> connections2 = chatNode.GetOutputPort("outPut").GetConnections();
				if (connections2.Count > 0)
				{
					GetChatNodes((ChatNode)connections2[0].node);
				}
			}
		}
	}

	public static bool ShowTutorial(string key, Sprite[] sprites)
	{
		int num = GameData.GetValue("isExTutorial_" + key, "0").ToInt();
		Debug.Log($"检测弹出教程:{key}----ExCount:{num}");
		if (num > 0)
		{
			Debug.Log("尝试打开教程失败");
		}
		else
		{
			Debug.Log("打开教程成功");
			UiManager.OpenUi<TutorialWindows>().Init(sprites);
			GameData.SetString("isExTutorial_" + key, "1");
		}
		return num <= 0;
	}

	public static bool IsCanShow(string key, int maxCount = 1)
	{
		int num = GameData.GetValue("isExTutorial_" + key, "0").ToInt();
		GameData.SetString("isExTutorial_" + key, (num + 1).ToString() ?? "");
		return num < maxCount;
	}

	public static bool IsCanProperty(this PropertyTypeValue propertyType)
	{
		if (propertyType == PropertyTypeValue.Stamina || propertyType == PropertyTypeValue.Wisdom || propertyType == PropertyTypeValue.Charm || propertyType == PropertyTypeValue.Morality || propertyType == PropertyTypeValue.CapacityForLiquor || propertyType == PropertyTypeValue.Execution || propertyType == PropertyTypeValue.Pressure)
		{
			return true;
		}
		return false;
	}

	public static ChatNode GetFistNode(this string assetName)
	{
		ChatGraph chatGraph = ABMrg.Load<ChatGraph>(assetName);
		ChatNode result = null;
		for (int i = 0; i < chatGraph.nodes.Count; i++)
		{
			if (chatGraph.nodes[i] is StartChat)
			{
				result = (ChatNode)chatGraph.nodes[i].GetOutputPort("outPut").GetConnections()[0].node;
				break;
			}
		}
		return result;
	}

	public static ChatNode GetGetOutChatNode(this ChatNode chatNode)
	{
		List<NodePort> connections = chatNode.GetOutputPort("outPut").GetConnections();
		if (connections.Count > 0)
		{
			return (ChatNode)connections[0].node;
		}
		return null;
	}

	public static List<ChatNode> GetAllChatSelect(this ChatNode node)
	{
		return (from port in node.GetOutputPort("selectBtnNode").GetConnections()
			select (ChatNode)port.node).ToList();
	}

	public static string GetProTips(PropertyTypeValue propertyType)
	{
		switch (propertyType)
		{
		case PropertyTypeValue.Charm:
		{
			int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Charm", "Property", 0L);
			if (property > 150)
			{
				return LanguageMrg.GetText("A1192");
			}
			if (property > 100)
			{
				return LanguageMrg.GetText("A1191");
			}
			if (property > 50)
			{
				return LanguageMrg.GetText("A1190");
			}
			if (property >= 0)
			{
				return LanguageMrg.GetText("A1189");
			}
			break;
		}
		case PropertyTypeValue.Wisdom:
		{
			int property2 = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Wisdom", "Property", 0L);
			if (property2 > 200)
			{
				return LanguageMrg.GetText("A1197");
			}
			if (property2 > 150)
			{
				return LanguageMrg.GetText("A1196");
			}
			if (property2 > 100)
			{
				return LanguageMrg.GetText("A1195");
			}
			if (property2 > 50)
			{
				return LanguageMrg.GetText("A1194");
			}
			if (property2 >= 0)
			{
				return LanguageMrg.GetText("A1193");
			}
			break;
		}
		case PropertyTypeValue.Stamina:
		{
			int property4 = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Stamina", "Property", 0L);
			if (property4 > 200)
			{
				return LanguageMrg.GetText("A1202");
			}
			if (property4 > 150)
			{
				return LanguageMrg.GetText("A1201");
			}
			if (property4 > 100)
			{
				return LanguageMrg.GetText("A1200");
			}
			if (property4 > 50)
			{
				return LanguageMrg.GetText("A1199");
			}
			if (property4 >= 0)
			{
				return LanguageMrg.GetText("A1198");
			}
			break;
		}
		case PropertyTypeValue.CapacityForLiquor:
		{
			int property3 = SingletonAsMono<GameDataMrg>.Instance.GetProperty("CapacityForLiquor", "Property", 0L);
			if (property3 > 1000)
			{
				return LanguageMrg.GetText("A1207");
			}
			if (property3 > 300)
			{
				return LanguageMrg.GetText("A1206");
			}
			if (property3 > 100)
			{
				return LanguageMrg.GetText("A1205");
			}
			if (property3 > 50)
			{
				return LanguageMrg.GetText("A1204");
			}
			if (property3 >= 0)
			{
				return LanguageMrg.GetText("A1203");
			}
			break;
		}
		case PropertyTypeValue.Morality:
			if (SingletonAsMono<GameDataMrg>.Instance.GetProperty("Morality", "Property", 0L) > 0)
			{
				return LanguageMrg.GetText("A1232");
			}
			return LanguageMrg.GetText("A1231");
		}
		return "";
	}

	public static void ClickZb(Xlsx_Item xlsxItem)
	{
		ZbType itemType = (ZbType)xlsxItem.ItemType;
		bool flag = false;
		if (!string.IsNullOrEmpty(xlsxItem.VideoGroup))
		{
			try
			{
				VideoNode evenFistNode = ABMrg.Load<VideoGraph>(xlsxItem.VideoGroup).GetEvenFistNode();
				UiManager.OpenUi<VideoListPlayVideoWindows>().Init(evenFistNode);
			}
			catch (Exception ex)
			{
				MyLog.LogWarning(ex.Message);
			}
		}
		if (!string.IsNullOrEmpty(xlsxItem.Img))
		{
			try
			{
				Sprite sprite = ABMrg.Load<Sprite>(xlsxItem.Img);
				UiManager.OpenUi<ImgShowWindows>().Init(sprite);
			}
			catch (Exception ex2)
			{
				MyLog.LogWarning(ex2.Message);
			}
		}
		switch (itemType)
		{
		case ZbType.PeiShi:
		{
			List<string> list = new List<string>();
			if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi)))
			{
				list.Add(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi));
			}
			if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi2)))
			{
				list.Add(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi2));
			}
			if (!string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi3)))
			{
				list.Add(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi3));
			}
			if (list.Contains(xlsxItem.Key))
			{
				return;
			}
			SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.chuanZhuanBei);
			if (string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi)))
			{
				SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(ZbType.PeiShi, xlsxItem.Key);
			}
			else if (string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi2)))
			{
				SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(ZbType.PeiShi2, xlsxItem.Key);
			}
			else if (string.IsNullOrEmpty(SingletonAsMono<GameDataMrg>.Instance.GetCurZbKey(ZbType.PeiShi3)))
			{
				SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(ZbType.PeiShi3, xlsxItem.Key);
			}
			else
			{
				SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(ZbType.PeiShi, xlsxItem.Key);
			}
			break;
		}
		case ZbType.XiaoHaoPin:
		{
			for (int i = 0; i < xlsxItem.AddType.Length; i++)
			{
				string text = xlsxItem.AddType[i];
				int num = xlsxItem.AddValue[i];
				if (text == "HgdLose")
				{
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("BZPFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("WDLYFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("XFPFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("XXGFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("LQTFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("MMFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("LBNFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("ZXSNFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("DBNFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("LJFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("XJMFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("NAMFavorability", num, "Property");
					SingletonAsMono<GameDataMrg>.Instance.AddProperty("MSFavorability", num, "Property");
					SingletonAsMono<InfoTipsMrg>.Instance.Add(LanguageMrg.GetText("A1218") + num, num > 0);
				}
				else if (text == "CapacityForLiquorTTpm")
				{
					SingletonAsMono<GameDataMrg>.Instance.AddRoundProperty("CapacityForLiquor", num, "Property");
				}
				else if (xlsxItem.Key == "GuaGuaLe")
				{
					UiManager.OpenUi<CaiPiaoWindows>();
				}
				else
				{
					if (!Enum.TryParse<PropertyTypeValue>(text, out var result))
					{
						continue;
					}
					if (result == PropertyTypeValue.Pressure && num == -999)
					{
						new PropertyData
						{
							PropertyType = PropertyType.Property,
							propertyTypeValue = result,
							PropertyValue = -SingletonAsMono<GameDataMrg>.Instance.GetProperty(result.ToString(), "Property", 0L)
						}.AddTypeValueAsShowTips(isCheck: false);
					}
					else if (result == PropertyTypeValue.Execution)
					{
						int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("Execution", "Property", 0L);
						if (num < 0 && property < Mathf.Abs(num))
						{
							UiManager.ShowTips(LanguageMrg.GetText("A840"));
							return;
						}
						new PropertyData
						{
							PropertyType = PropertyType.Property,
							propertyTypeValue = result,
							PropertyValue = num
						}.AddTypeValueAsShowTips(isCheck: false);
					}
					else
					{
						new PropertyData
						{
							PropertyType = PropertyType.Property,
							propertyTypeValue = result,
							PropertyValue = num
						}.AddTypeValueAsShowTips(isCheck: false);
					}
				}
			}
			if (xlsxItem.IsInfinite != 1)
			{
				SingletonAsMono<GameDataMrg>.Instance.AddProperty(xlsxItem.Key, -1L, "Item");
				flag = true;
			}
			break;
		}
		default:
			SingletonAsMono<GameDataMrg>.Instance.SetCurZbKey(itemType, xlsxItem.Key);
			break;
		case ZbType.QiTa:
			break;
		}
		if (flag && xlsxItem.AddItemKeys.Length != 0)
		{
			for (int j = 0; j < xlsxItem.AddItemKeys.Length; j++)
			{
				string value = xlsxItem.AddItemKeys[j];
				int propertyValue = xlsxItem.AddItemValue[j];
				if (Enum.TryParse<ItemType>(value, out var result2))
				{
					new PropertyData
					{
						PropertyType = PropertyType.Item,
						itemType = result2,
						PropertyValue = propertyValue
					}.AddTypeValueAsShowTips();
				}
			}
		}
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.UpdateProperty);
		EventManager.DispatchEvent(MessageType.Game, GameMessageType.ZbUpdate);
	}

	public static bool IsHasPropertyType(this List<PropertyData> list, PropertyTypeValue type, out string count)
	{
		for (int i = 0; i < list.Count; i++)
		{
			if (list[i].propertyTypeValue == type)
			{
				count = ((list[i].PropertyValue > 0) ? ("+" + list[i].PropertyValue) : (list[i].PropertyValue.ToString() ?? ""));
				return true;
			}
		}
		count = "0";
		return false;
	}

	public static bool IsHasPropertyType(this Dictionary<string, VideoNode> list, PropertyTypeValue propertyTypeValue, PropertyTypeValue type, out string count)
	{
		count = "0";
		if (!list.ContainsKey(propertyTypeValue.ToString()))
		{
			return false;
		}
		VideoNode videoNode = list[propertyTypeValue.ToString()];
		for (int i = 0; i < videoNode.PropertyData.Count; i++)
		{
			if (videoNode.PropertyData[i].propertyTypeValue == type)
			{
				count = ((videoNode.PropertyData[i].PropertyValue > 0) ? ("+" + videoNode.PropertyData[i].PropertyValue) : (videoNode.PropertyData[i].PropertyValue.ToString() ?? ""));
				return true;
			}
		}
		count = "0";
		return false;
	}

	public static bool IsSucProperty(string[] key, int[] value)
	{
		if (key.Length == 0)
		{
			return false;
		}
		for (int i = 0; i < key.Length; i++)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.GetProperty(key[i], "Property", 0L) < value[i])
			{
				return false;
			}
		}
		return true;
	}

	public static void IsCanTrigger(this CollType collType, out bool canShow, out bool canTrigger, out string msg, out VideoGraph graph)
	{
		canShow = true;
		canTrigger = false;
		msg = "";
		graph = null;
		int t = (int)collType;
		int curRound = SingletonAsMono<GameDataMrg>.Instance.CurRound;
		int num = -1;
		Xlsx_Pos xlsx_Pos = Xlsx_Pos_Query.XlsxDataAsOneKey.ByKeyGetValue(t);
		if (xlsx_Pos != null && xlsx_Pos.MaxRoundCount > 0)
		{
			num = xlsx_Pos.MaxRoundCount;
		}
		int roundProperty = SingletonAsMono<GameDataMrg>.Instance.GetRoundProperty("EventRound_" + t, "Event");
		List<Xlsx_Event> list = (from e in CheckList(Xlsx_Event_Query.data)
			where e.Type == t
			select e).ToList();
		canShow = list.Count > 0;
		if (num != -1 && roundProperty >= num)
		{
			canShow = false;
			return;
		}
		foreach (Xlsx_Event item in list)
		{
			if (item.Round.Length == 0)
			{
				continue;
			}
			if (item.IsNotRound.Contains(curRound))
			{
				canShow = false;
			}
			else if ((item.Round.Length == 1 && curRound == item.Round[0]) || (item.Round.Length == 2 && curRound >= item.Round[0] && curRound <= item.Round[1]))
			{
				canShow = false;
				bool flag = false;
				if (SingletonAsMono<GameDataMrg>.Instance.IsMorning)
				{
					if (item.IsMorning == 1)
					{
						flag = true;
					}
				}
				else if (item.IsMorning != 1)
				{
					flag = true;
				}
				if (item.IsMorning == -1)
				{
					flag = true;
				}
				canShow = flag;
				canTrigger = flag;
				if (!flag)
				{
					continue;
				}
				int propertyAsEvent = SingletonAsMono<GameDataMrg>.Instance.GetPropertyAsEvent("Ex_" + item.Key);
				VideoGraph videoGraph = ABMrg.Load<VideoGraph>(item.VideoAsset);
				ChapterNode evenChapterNode = videoGraph.GetEvenChapterNode();
				if (canShow && !evenChapterNode.showPropertyType.IsSuc())
				{
					canShow = false;
				}
				if (canShow)
				{
					if (item.Count == -1 || propertyAsEvent < item.Count)
					{
						msg = LanguageMrg.GetText(item.ShowText);
						graph = videoGraph;
						break;
					}
					canShow = false;
					canTrigger = false;
				}
				else
				{
					canTrigger = false;
				}
			}
			else
			{
				canShow = false;
			}
		}
	}

	public static int ToInt(this float value)
	{
		return (int)value;
	}

	public static VideoNode GetNextVideoNode(this VideoNode node)
	{
		if (!node.isToNextGroup && node.isNowToNext && node.isPlayerEndPlayerNext)
		{
			List<VideoNode> allVideoNodesOutNor = node.GetAllVideoNodesOutNor("nextVideoNode");
			if (allVideoNodesOutNor.Count > 0)
			{
				return allVideoNodesOutNor[0].GetNextVideoNode();
			}
			return node;
		}
		return node;
	}

	public static string FormatNumber(this double num, int decimalPlaces = 1)
	{
		bool num2 = num < 0.0;
		num = Mathf.Abs((float)num);
		string text = "";
		double num3 = num;
		if (num >= 1000000000000.0)
		{
			text = "万亿";
			num3 = num / 1000000000000.0;
		}
		else if (num >= 100000000.0)
		{
			text = "亿";
			num3 = num / 100000000.0;
		}
		else if (num >= 10000.0)
		{
			text = "万";
			num3 = num / 10000.0;
		}
		return (num2 ? "-" : "") + num3.ToString($"F{decimalPlaces}") + text;
	}

	public static string FormatStringNumber(this string numStr, int decimalPlaces = 1)
	{
		if (!double.TryParse(numStr, out var result))
		{
			Debug.LogWarning("数字格式不正确：" + numStr);
			return numStr;
		}
		bool num = result < 0.0;
		result = Mathf.Abs((float)result);
		string text = "";
		double num2 = result;
		if (result >= 1000000000000.0)
		{
			text = LanguageMrg.GetText("A3064");
			num2 = result / 1000000000000.0;
		}
		else if (result >= 100000000.0)
		{
			text = LanguageMrg.GetText("A3063");
			num2 = result / 100000000.0;
		}
		else if (result >= 10000.0)
		{
			text = LanguageMrg.GetText("A3062");
			num2 = result / 10000.0;
		}
		else
		{
			decimalPlaces = 0;
		}
		return string.Concat(str1: num2.ToString($"F{decimalPlaces}"), str0: num ? "-" : "", str2: text);
	}

	public static string GetLoadTips()
	{
		return LanguageMrg.GetText(_tips[UnityEngine.Random.Range(0, _tips.Length)]);
	}

	public static bool IsLockFolder(FolderData folderData)
	{
		for (int i = 0; i < folderData.FileList.Count; i++)
		{
			if (SingletonAsMono<GameDataMrg>.Instance.IsUnLockGlobalKey(folderData.FileList[i].videoPath))
			{
				return true;
			}
		}
		for (int j = 0; j < folderData.FolderList.Count; j++)
		{
			if (IsLockFolder(folderData.FolderList[j]))
			{
				return true;
			}
		}
		return false;
	}

	public static void SetTask(TaskData taskData)
	{
		TaskType taskState = SingletonAsMono<GameDataMrg>.Instance.GetTaskState(taskData.xlsxTaskKey.ToString());
		TaskType taskType = taskData.taskType;
		if (taskState != taskType)
		{
			SingletonAsMono<GameDataMrg>.Instance.SetTaskData(taskData.xlsxTaskKey.ToString(), taskData.taskType);
			Xlsx_Task xlsx_Task = Xlsx_Task_Query.XlsxDataAsOneKey.ByKeyGetValue(taskData.xlsxTaskKey.ToString().Trim());
			switch (taskType)
			{
			case TaskType.Open:
			{
				string message2 = string.Format(LanguageMrg.GetText("A1313"), LanguageMrg.GetText(xlsx_Task.Title)) + string.Format(LanguageMrg.GetText("A1273"));
				SingletonAsMono<InfoTopTipsMrg>.Instance.Add(message2);
				break;
			}
			case TaskType.Suc:
			{
				string message = string.Format(LanguageMrg.GetText("A1313"), LanguageMrg.GetText(xlsx_Task.Title)) + string.Format(LanguageMrg.GetText("A1274"));
				SingletonAsMono<InfoTopTipsMrg>.Instance.Add(message);
				break;
			}
			case TaskType.Lose:
				ShowTutorial("TaskLos", new Sprite[1] { ABMrg.Load<Sprite>("TaskLos") });
				SingletonAsMono<InfoTipsRightMrg>.Instance.Add(new TaskTipsData
				{
					Text = LanguageMrg.GetText(xlsx_Task.Title),
					Info = string.Format(LanguageMrg.GetText("A1275")),
					AudioClip = AudioDataClip.AudioData.taskFail,
					XlsxTask = xlsx_Task,
					TaskType = TaskType.Lose
				});
				break;
			}
		}
	}

	public static bool IsUnLockAllVideo(this List<VideoUnlockData> videoUnlockDatas)
	{
		for (int i = 0; i < videoUnlockDatas.Count; i++)
		{
			if (!File.Exists(GetVideoPath(videoUnlockDatas[i].unlockVideoPath)))
			{
				return false;
			}
		}
		return true;
	}

	public static bool IsHasAutoEvent(this VideoGraph videoGraph, out VideoGraph autoGroup)
	{
		autoGroup = null;
		if (videoGraph != null)
		{
			List<VideoNode> allVideoNodesOutNor = videoGraph.GetEvenFistNode().GetAllVideoNodesOutNor();
			for (int i = 0; i < allVideoNodesOutNor.Count; i++)
			{
				VideoNode nextVideoNode = allVideoNodesOutNor[i].GetNextVideoNode();
				if (nextVideoNode.isToNextGroup)
				{
					string xlsxEventKey = nextVideoNode.VideoGroupData.xlsxEventKey;
					Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
					if (xlsx_Event != null && SingletonAsMono<GameDataMrg>.Instance.IsCanAutoSave(xlsx_Event.Key) && xlsx_Event.Type == 0 && xlsx_Event.AutoSave == 1)
					{
						autoGroup = nextVideoNode.VideoGroupData;
						return true;
					}
				}
			}
			return false;
		}
		return false;
	}

	public static bool IsHasAutoEventAsNotCheck(this VideoGraph videoGraph, out VideoGraph autoGroup, bool isNot360 = true)
	{
		autoGroup = null;
		if (videoGraph != null)
		{
			List<VideoGraph> btnAutoGroup = videoGraph.GetEvenFistNode().GetBtnAutoGroup(new List<VideoNode>());
			if (btnAutoGroup.Count > 0)
			{
				autoGroup = btnAutoGroup[0];
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool IsHasAutoEventSAsNotCheck(this VideoGraph videoGraph, out VideoGraph[] autoGroup, bool isNot360 = true)
	{
		List<VideoGraph> list = new List<VideoGraph>();
		autoGroup = list.ToArray();
		if (videoGraph != null)
		{
			List<VideoGraph> btnAutoGroup = videoGraph.GetEvenFistNode().GetBtnAutoGroup(new List<VideoNode>(), isNot360);
			autoGroup = btnAutoGroup.ToArray();
			if (btnAutoGroup.Count > 0)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool IsHasAutoEventSAsNotCheckEq(this VideoGraph videoGraph, out VideoGraph[] autoGroup, bool isNot360 = true)
	{
		List<VideoGraph> list = new List<VideoGraph>();
		autoGroup = list.ToArray();
		if (videoGraph != null)
		{
			List<VideoGraph> btnAutoGroup = videoGraph.GetEvenFistNode().GetBtnAutoGroup(new List<VideoNode>(), isNot360);
			autoGroup = btnAutoGroup.ToArray();
			Dictionary<string, VideoGraph> dictionary = new Dictionary<string, VideoGraph>();
			for (int i = 0; i < autoGroup.Length; i++)
			{
				Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(autoGroup[i].xlsxEventKey);
				if (xlsx_Event != null && !dictionary.ContainsKey(xlsx_Event.RoleIcon))
				{
					dictionary.TryAdd(xlsx_Event.RoleIcon, autoGroup[i]);
				}
			}
			autoGroup = dictionary.Values.ToArray();
			if (btnAutoGroup.Count > 0)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static bool IsHasAutoEventSAsNotCheck(this VideoNode videoNode, out VideoGraph[] autoGroup, bool isNot360 = true)
	{
		List<VideoGraph> list = new List<VideoGraph>();
		autoGroup = list.ToArray();
		if (videoNode != null)
		{
			List<VideoGraph> btnAutoGroup = videoNode.GetBtnAutoGroup(new List<VideoNode>());
			list.AddRange(btnAutoGroup);
			autoGroup = list.ToArray();
			if (list.Count > 0)
			{
				return true;
			}
			return false;
		}
		return false;
	}

	public static List<VideoGraph> GetBtnAutoGroup(this VideoNode videoNode, List<VideoNode> list, bool isNot360 = true)
	{
		List<VideoGraph> list2 = new List<VideoGraph>();
		if (!list.Contains(videoNode))
		{
			list.Add(videoNode);
		}
		VideoNode nextVideoNode = videoNode.GetNextVideoNode();
		if (isNot360 && nextVideoNode.is360Video && !nextVideoNode.imgIsNor)
		{
			return list2;
		}
		if (nextVideoNode.isToNextGroup && nextVideoNode.VideoGroupData != null && (!isNot360 || (!nextVideoNode.VideoGroupData.GetEvenFistNode().GetNextVideoNode().is360Video && !nextVideoNode.imgIsNor)))
		{
			string xlsxEventKey = nextVideoNode.VideoGroupData.xlsxEventKey;
			Xlsx_Event xlsx_Event = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey);
			if (xlsx_Event != null && xlsx_Event.Type == 0 && xlsx_Event.AutoSave == 1)
			{
				list2.Add(nextVideoNode.VideoGroupData);
			}
		}
		List<VideoNode> allVideoNodesOutNor = nextVideoNode.GetAllVideoNodesOutNor();
		for (int i = 0; i < allVideoNodesOutNor.Count; i++)
		{
			if (list.Contains(allVideoNodesOutNor[i]))
			{
				continue;
			}
			if (!list.Contains(allVideoNodesOutNor[i]))
			{
				list.Add(allVideoNodesOutNor[i]);
			}
			VideoNode nextVideoNode2 = allVideoNodesOutNor[i].GetNextVideoNode();
			if (!isNot360 && nextVideoNode2.is360Video && !nextVideoNode2.imgIsNor)
			{
				continue;
			}
			if (nextVideoNode2.isToNextGroup)
			{
				string xlsxEventKey2 = nextVideoNode2.VideoGroupData.xlsxEventKey;
				Xlsx_Event xlsx_Event2 = Xlsx_Event_Query.XlsxDataAsOneKey.ByKeyGetValue(xlsxEventKey2);
				if (xlsx_Event2 != null && xlsx_Event2.Type == 0 && xlsx_Event2.AutoSave == 1)
				{
					list2.Add(nextVideoNode2.VideoGroupData);
				}
			}
			else
			{
				list2.AddRange(nextVideoNode2.GetBtnAutoGroup(list));
			}
		}
		return list2;
	}

	public static void SetTagetTop(this Transform self, Transform target)
	{
		int siblingIndex = target.GetSiblingIndex();
		self.SetSiblingIndex(siblingIndex);
	}

	public static int GetProperty(this Dictionary<string, string> dic, string propertyName, string type = "Nor", int defaultValue = 0)
	{
		return dic.GetValueOrDefault("property_" + type + "_" + propertyName, defaultValue.ToString() ?? "").ToInt();
	}

	public static CollType[] GetMapLockPoint()
	{
		return _collTypes;
	}

	public static int ActiveChild(this Transform tran)
	{
		int num = 0;
		for (int i = 0; i < tran.childCount; i++)
		{
			if (tran.GetChild(i).gameObject.activeSelf)
			{
				num++;
			}
		}
		return num;
	}

	public static List<Xlsx_Event> CheckList(List<Xlsx_Event> list)
	{
		return list.Where((Xlsx_Event e) => e.IsDemo == 1).ToList();
	}

	public static void CheckAchievement()
	{
		AchievementGroup achievementGroup = ABMrg.Load<AchievementGroup>("AchievementData");
		if (!(achievementGroup != null))
		{
			return;
		}
		for (int i = 0; i < achievementGroup.nodes.Count; i++)
		{
			if (!(achievementGroup.nodes[i] is AchievementNode))
			{
				continue;
			}
			AchievementNode achievementNode = (AchievementNode)achievementGroup.nodes[i];
			if (!achievementNode.propertyData.IsSuc())
			{
				continue;
			}
			for (int j = 0; j < achievementNode.achievements.Count; j++)
			{
				Xlsx_Achievement xlsx_Achievement = Xlsx_Achievement_Query.XlsxDataAsOneKey.ByKeyGetValue(achievementNode.achievements[j].ToString());
				if (xlsx_Achievement != null)
				{
					SdkMrg.UnLockAchievement(xlsx_Achievement.Api);
				}
			}
		}
	}
}

using System;
using System.Collections;
using System.Text;
using System.Threading.Tasks;
using LitJson;
using UnityEngine;
using UnityEngine.Networking;

namespace FrameWork;

public class Request
{
	private Action<string> _valueString;

	private Action<byte[]> _valueByte;

	public Action<string> err;

	public Action<float, int> Progress;

	private string _url;

	private Methods _httpMethods;

	private object _data;

	public Request(string url, Methods httpMethods)
	{
		_url = url;
		_httpMethods = httpMethods;
	}

	public static Request Create(string url, Methods httpMethods)
	{
		return new Request(url, httpMethods);
	}

	public void SetSendData(object data)
	{
		_data = data;
	}

	public void Send()
	{
		SingletonAsMono<Mono>.Instance.StartCoroutine(RequestHttp());
	}

	public void Send(Action<string> data, Action<string> err = null)
	{
		_valueString = (Action<string>)Delegate.Combine(_valueString, data);
		this.err = (Action<string>)Delegate.Combine(this.err, err);
		SingletonAsMono<Mono>.Instance.StartCoroutine(RequestHttp());
	}

	public void Send(Action<byte[]> data, Action<string> err = null)
	{
		_valueByte = (Action<byte[]>)Delegate.Combine(_valueByte, data);
		this.err = (Action<string>)Delegate.Combine(this.err, err);
		SingletonAsMono<Mono>.Instance.StartCoroutine(RequestHttp());
	}

	public void Send(Action<Sprite> data, Action<string> err = null)
	{
		SingletonAsMono<Mono>.Instance.StartCoroutine(GetTexture());
		IEnumerator GetTexture()
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
			www.SetRequestHeader("Content-Type", "application/json");
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonMapper.ToJson(_data)));
			www.SendWebRequest();
			if (www.isHttpError || www.isNetworkError)
			{
				err?.Invoke(www.error);
				MyLog.LogError(www.error);
				yield break;
			}
			string requestHeader = www.GetRequestHeader("Content-Length");
			int size = 0;
			if (!string.IsNullOrEmpty(requestHeader))
			{
				size = int.Parse(requestHeader);
			}
			while (!www.isDone)
			{
				Progress?.Invoke(www.downloadProgress, size);
				yield return null;
			}
			yield return null;
			if (www.isHttpError || www.isNetworkError)
			{
				err?.Invoke(www.error);
				MyLog.LogError(www.error);
				yield break;
			}
			if (www.isDone)
			{
				Progress?.Invoke(1f, size);
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				data?.Invoke(Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero));
			}
		}
	}

	public async Task<byte[]> SendTaskBytes()
	{
		TaskCompletionSource<byte[]> tcs = new TaskCompletionSource<byte[]>();
		_valueByte = (Action<byte[]>)Delegate.Combine(_valueByte, (Action<byte[]>)delegate(byte[] v)
		{
			tcs.SetResult(v);
		});
		SingletonAsMono<Mono>.Instance.StartCoroutine(RequestHttp());
		return await tcs.Task;
	}

	public async Task<string> SendTaskAsString()
	{
		TaskCompletionSource<string> tcs = new TaskCompletionSource<string>();
		_valueString = (Action<string>)Delegate.Combine(_valueString, (Action<string>)delegate(string v)
		{
			tcs.SetResult(v);
		});
		SingletonAsMono<Mono>.Instance.StartCoroutine(RequestHttp());
		return await tcs.Task;
	}

	public async Task<Sprite> SendTaskAsTexture()
	{
		TaskCompletionSource<Sprite> tcs = new TaskCompletionSource<Sprite>();
		SingletonAsMono<Mono>.Instance.StartCoroutine(GetTexture());
		return await tcs.Task;
		IEnumerator GetTexture()
		{
			using UnityWebRequest www = UnityWebRequestTexture.GetTexture(_url);
			www.SetRequestHeader("Content-Type", "application/json");
			if (_data != null)
			{
				www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonMapper.ToJson(_data)));
			}
			www.SendWebRequest();
			if (www.isHttpError || www.isNetworkError)
			{
				err?.Invoke(www.error);
				MyLog.LogError(www.error);
				yield break;
			}
			string requestHeader = www.GetRequestHeader("Content-Length");
			int size = 0;
			if (!string.IsNullOrEmpty(requestHeader))
			{
				size = int.Parse(requestHeader);
			}
			while (!www.isDone)
			{
				Progress?.Invoke(www.downloadProgress, size);
				yield return null;
			}
			yield return null;
			if (www.isHttpError || www.isNetworkError)
			{
				err?.Invoke(www.error);
				MyLog.LogError(www.error);
				yield break;
			}
			if (www.isDone)
			{
				Progress?.Invoke(1f, size);
				Texture2D texture = ((DownloadHandlerTexture)www.downloadHandler).texture;
				tcs.SetResult(Sprite.Create(texture, new Rect(0f, 0f, texture.width, texture.height), Vector2.zero));
			}
		}
	}

	private IEnumerator RequestHttp()
	{
		using UnityWebRequest www = new UnityWebRequest(_url, _httpMethods.ToString());
		www.SetRequestHeader("Content-Type", "application/json");
		if (_data != null)
		{
			www.uploadHandler = new UploadHandlerRaw(Encoding.UTF8.GetBytes(JsonMapper.ToJson(_data)));
		}
		www.SendWebRequest();
		if (www.isHttpError || www.isNetworkError)
		{
			err?.Invoke(www.error);
			MyLog.LogError(www.error);
			yield break;
		}
		string requestHeader = www.GetRequestHeader("Content-Length");
		int size = 0;
		if (!string.IsNullOrEmpty(requestHeader))
		{
			size = int.Parse(requestHeader);
		}
		while (!www.isDone)
		{
			Progress?.Invoke(www.downloadProgress, size);
			yield return null;
		}
		yield return null;
		if (www.isHttpError || www.isNetworkError)
		{
			err?.Invoke(www.error);
			MyLog.LogError(www.error);
			yield break;
		}
		if (www.isDone)
		{
			Progress?.Invoke(1f, size);
			_valueString?.Invoke(www.downloadHandler.text);
			_valueByte?.Invoke(www.downloadHandler.data);
		}
	}
}

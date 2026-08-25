using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[HelpURL("https://www.renderheads.com/products/avpro-video/")]
[AddComponentMenu("AVPro Video/Resolve To RenderTexture", 330)]
public class ResolveToRenderTexture : MonoBehaviour
{
	[SerializeField]
	private MediaPlayer _mediaPlayer;

	[SerializeField]
	private VideoResolveOptions _options = VideoResolveOptions.Create();

	[SerializeField]
	private VideoRender.ResolveFlags _resolveFlags = VideoRender.ResolveFlags.Mipmaps | VideoRender.ResolveFlags.PackedAlpha | VideoRender.ResolveFlags.StereoLeft | VideoRender.ResolveFlags.ColorspaceSRGB;

	[SerializeField]
	private RenderTexture _externalTexture;

	private Material _materialResolve;

	private bool _isMaterialSetup;

	private bool _isMaterialDirty;

	private bool _isMaterialOES;

	private RenderTexture _internalTexture;

	private int _textureFrameCount = -1;

	private Material _materialBlit;

	private int _srcTexId;

	public MediaPlayer MediaPlayer
	{
		get
		{
			return _mediaPlayer;
		}
		set
		{
			ChangeMediaPlayer(value);
		}
	}

	public VideoResolveOptions VideoResolveOptions
	{
		get
		{
			return _options;
		}
		set
		{
			_options = value;
			_isMaterialDirty = true;
		}
	}

	public RenderTexture ExternalTexture
	{
		get
		{
			return _externalTexture;
		}
		set
		{
			_externalTexture = value;
		}
	}

	public RenderTexture TargetTexture
	{
		get
		{
			if (_externalTexture == null)
			{
				return _internalTexture;
			}
			return _externalTexture;
		}
	}

	public void SetMaterialDirty()
	{
		_isMaterialDirty = true;
	}

	private void ChangeMediaPlayer(MediaPlayer mediaPlayer)
	{
		if (_mediaPlayer != mediaPlayer)
		{
			_mediaPlayer = mediaPlayer;
			_textureFrameCount = -1;
			_isMaterialSetup = false;
			_isMaterialDirty = true;
			Resolve();
		}
	}

	private void Start()
	{
		_isMaterialOES = _mediaPlayer != null && _mediaPlayer.IsUsingAndroidOESPath();
		_materialResolve = VideoRender.CreateResolveMaterial(_isMaterialOES);
		VideoRender.SetupMaterialForMedia(_materialResolve, _mediaPlayer);
		_materialBlit = new Material(Shader.Find("AVProVideo/Internal/Blit"));
		_srcTexId = Shader.PropertyToID("_SrcTex");
	}

	private void LateUpdate()
	{
		Resolve();
	}

	public void Resolve()
	{
		ITextureProducer textureProducer = ((_mediaPlayer != null) ? _mediaPlayer.TextureProducer : null);
		if (textureProducer == null || !textureProducer.GetTexture())
		{
			return;
		}
		bool flag = _mediaPlayer.IsUsingAndroidOESPath();
		if (_isMaterialOES != flag)
		{
			_isMaterialOES = flag;
			_materialResolve = VideoRender.CreateResolveMaterial(flag);
		}
		if (!_isMaterialSetup)
		{
			VideoRender.SetupMaterialForMedia(_materialResolve, _mediaPlayer);
			_isMaterialSetup = true;
			_isMaterialDirty = true;
		}
		if (_isMaterialDirty)
		{
			VideoRender.SetupResolveMaterial(_materialResolve, _options);
			_isMaterialDirty = false;
		}
		int textureFrameCount = textureProducer.GetTextureFrameCount();
		if (textureFrameCount == _textureFrameCount)
		{
			return;
		}
		_internalTexture = VideoRender.ResolveVideoToRenderTexture(_materialResolve, _internalTexture, textureProducer, _resolveFlags);
		_textureFrameCount = textureFrameCount;
		if (!_internalTexture || !_externalTexture)
		{
			return;
		}
		float num = (float)_internalTexture.width / (float)_internalTexture.height;
		float num2 = (float)_externalTexture.width / (float)_externalTexture.height;
		Vector2 zero = Vector2.zero;
		Vector2 value = new Vector2(1f, 1f);
		if (num != num2)
		{
			switch (_options.aspectRatio)
			{
			case VideoResolveOptions.AspectRatio.NoScaling:
				value.x = (float)_externalTexture.width / (float)_internalTexture.width;
				value.y = (float)_externalTexture.height / (float)_internalTexture.height;
				zero.x = (1f - value.x) * 0.5f;
				zero.y = (1f - value.y) * 0.5f;
				break;
			case VideoResolveOptions.AspectRatio.FitVertically:
				value.x = (float)_internalTexture.height / (float)_internalTexture.width * num2;
				zero.x = (1f - value.x) * 0.5f;
				break;
			case VideoResolveOptions.AspectRatio.FitHorizontally:
				value.y = (float)_externalTexture.height / (float)_externalTexture.width * num;
				zero.y = (1f - value.y) * 0.5f;
				break;
			case VideoResolveOptions.AspectRatio.FitInside:
				if (!(num > num2))
				{
					if (!(num < num2))
					{
						break;
					}
					goto case VideoResolveOptions.AspectRatio.FitVertically;
				}
				goto case VideoResolveOptions.AspectRatio.FitHorizontally;
			case VideoResolveOptions.AspectRatio.FitOutside:
				if (num > num2)
				{
					goto case VideoResolveOptions.AspectRatio.FitVertically;
				}
				if (!(num < num2))
				{
					break;
				}
				goto case VideoResolveOptions.AspectRatio.FitHorizontally;
			}
		}
		_materialBlit.SetTexture(_srcTexId, _internalTexture);
		_materialBlit.SetTextureOffset(_srcTexId, zero);
		_materialBlit.SetTextureScale(_srcTexId, value);
		Graphics.Blit(null, _externalTexture, _materialBlit, 0);
	}

	private void OnDisable()
	{
		if ((bool)_internalTexture)
		{
			RenderTexture.ReleaseTemporary(_internalTexture);
			_internalTexture = null;
		}
	}

	private void OnDestroy()
	{
		if ((bool)_materialResolve)
		{
			Object.Destroy(_materialResolve);
			_materialResolve = null;
		}
	}
}

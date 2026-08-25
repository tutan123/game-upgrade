using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo;

[ExecuteInEditMode]
[RequireComponent(typeof(CanvasRenderer))]
[AddComponentMenu("AVPro Video/Display uGUI", 200)]
[HelpURL("http://renderheads.com/products/avpro-video/")]
public class DisplayUGUI : MaskableGraphic
{
	[SerializeField]
	private MediaPlayer _mediaPlayer;

	[Tooltip("Default texture to display when the video texture is preparing")]
	[SerializeField]
	private Texture _defaultTexture;

	[FormerlySerializedAs("m_UVRect")]
	[SerializeField]
	private Rect _uvRect = new Rect(0f, 0f, 1f, 1f);

	[SerializeField]
	private bool _setNativeSize;

	[SerializeField]
	private ScaleMode _scaleMode = ScaleMode.ScaleToFit;

	[SerializeField]
	private bool _noDefaultDisplay = true;

	[SerializeField]
	private bool _displayInEditor = true;

	private int _lastWidth;

	private int _lastHeight;

	private Orientation _lastOrientation;

	private bool _flipY;

	private Texture _lastTexture;

	private static Shader _shaderStereoPacking;

	private static Shader _shaderAlphaPacking;

	private static Shader _shaderAndroidOES;

	private static Shader _shaderAndroidOESAlphaPacking;

	private bool _isUserMaterial = true;

	private Material _material;

	private List<UIVertex> _vertices = new List<UIVertex>(4);

	private static List<int> QuadIndices = new List<int>(new int[6] { 0, 1, 2, 2, 3, 0 });

	private Vector4 _drawingDimensions = Vector4.zero;

	public MediaPlayer Player
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

	public Texture DefaultTexture
	{
		get
		{
			return _defaultTexture;
		}
		set
		{
			if (_defaultTexture != value)
			{
				_defaultTexture = value;
			}
		}
	}

	public Rect UVRect
	{
		get
		{
			return _uvRect;
		}
		set
		{
			_uvRect = value;
		}
	}

	public bool ApplyNativeSize
	{
		get
		{
			return _setNativeSize;
		}
		set
		{
			_setNativeSize = value;
		}
	}

	public ScaleMode ScaleMode
	{
		get
		{
			return _scaleMode;
		}
		set
		{
			_scaleMode = value;
		}
	}

	public bool NoDefaultDisplay
	{
		get
		{
			return _noDefaultDisplay;
		}
		set
		{
			_noDefaultDisplay = value;
		}
	}

	public bool DisplayInEditor
	{
		get
		{
			return _displayInEditor;
		}
		set
		{
			_displayInEditor = value;
		}
	}

	public Vector4 DrawingDimensions => _drawingDimensions;

	public override Texture mainTexture
	{
		get
		{
			Texture result = Texture2D.whiteTexture;
			if (HasValidTexture())
			{
				Texture texture = ((_mediaPlayer.FrameResampler == null || _mediaPlayer.FrameResampler.OutputTexture == null) ? null : _mediaPlayer.FrameResampler.OutputTexture[0]);
				result = (_mediaPlayer.UseResampler ? texture : _mediaPlayer.TextureProducer.GetTexture());
			}
			else if (_noDefaultDisplay)
			{
				result = null;
			}
			else if (_defaultTexture != null)
			{
				result = _defaultTexture;
			}
			return result;
		}
	}

	public MediaPlayer CurrentMediaPlayer
	{
		get
		{
			return _mediaPlayer;
		}
		set
		{
			if (_mediaPlayer != value)
			{
				_mediaPlayer = value;
				SetMaterialDirty();
			}
		}
	}

	public Rect uvRect
	{
		get
		{
			return _uvRect;
		}
		set
		{
			if (!(_uvRect == value))
			{
				_uvRect = value;
				SetVerticesDirty();
			}
		}
	}

	protected override void Awake()
	{
		if (_mediaPlayer != null)
		{
			_mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
		}
		base.Awake();
	}

	private void OnMediaPlayerEvent(MediaPlayer mp, MediaPlayerEvent.EventType et, ErrorCode errorCode)
	{
		switch (et)
		{
		case MediaPlayerEvent.EventType.FirstFrameReady:
			if (_isUserMaterial && null != GetRequiredShader())
			{
				Debug.LogWarning("[AVProVideo] Custom material is being used but the video requires our internal shader for correct rendering.  Consider removing custom shader or modifying it for AVPro Video support.", this);
			}
			LateUpdate();
			break;
		case MediaPlayerEvent.EventType.Closing:
		case MediaPlayerEvent.EventType.ResolutionChanged:
		case MediaPlayerEvent.EventType.PropertiesChanged:
			LateUpdate();
			break;
		}
		LateUpdate();
	}

	private void ChangeMediaPlayer(MediaPlayer player)
	{
		if (_mediaPlayer != player)
		{
			if (_mediaPlayer != null)
			{
				_mediaPlayer.Events.RemoveListener(OnMediaPlayerEvent);
			}
			_mediaPlayer = player;
			if (_mediaPlayer != null)
			{
				_mediaPlayer.Events.AddListener(OnMediaPlayerEvent);
			}
			LateUpdate();
		}
	}

	private static Shader EnsureShader(Shader shader, string name)
	{
		if (shader == null)
		{
			shader = Shader.Find(name);
			if (shader == null)
			{
				Debug.LogWarning("[AVProVideo] Missing shader " + name);
			}
		}
		return shader;
	}

	private static Shader EnsureAlphaPackingShader()
	{
		_shaderAlphaPacking = EnsureShader(_shaderAlphaPacking, "AVProVideo/Internal/UI/Transparent Packed (stereo)");
		return _shaderAlphaPacking;
	}

	private static Shader EnsureStereoPackingShader()
	{
		_shaderStereoPacking = EnsureShader(_shaderStereoPacking, "AVProVideo/Internal/UI/Stereo");
		return _shaderStereoPacking;
	}

	private Shader EnsureAndroidOESShader()
	{
		_shaderAndroidOES = EnsureShader(_shaderAndroidOES, "AVProVideo/Internal/UI/Stereo - AndroidOES");
		return _shaderAndroidOES;
	}

	private static Shader EnsureAndroidOESAlphaPackingShader()
	{
		_shaderAndroidOESAlphaPacking = EnsureShader(_shaderAndroidOESAlphaPacking, "AVProVideo/Internal/UI/Transparent Packed (stereo) - AndroidOES");
		return _shaderAndroidOESAlphaPacking;
	}

	protected override void Start()
	{
		_isUserMaterial = m_Material != null;
		if (_isUserMaterial)
		{
			_material = new Material(material);
			material = _material;
		}
		base.Start();
	}

	protected override void OnDestroy()
	{
		if (_material != null)
		{
			material = null;
			Object.Destroy(_material);
			_material = null;
		}
		ChangeMediaPlayer(null);
		base.OnDestroy();
	}

	private Shader GetRequiredShader()
	{
		Shader shader = null;
		if (shader == null && _mediaPlayer.TextureProducer != null)
		{
			switch (_mediaPlayer.TextureProducer.GetTextureStereoPacking())
			{
			case StereoPacking.TopBottom:
			case StereoPacking.LeftRight:
			case StereoPacking.MultiviewLeftPrimary:
			case StereoPacking.MultiviewRightPrimary:
				shader = EnsureStereoPackingShader();
				break;
			}
			if (_mediaPlayer.TextureProducer.GetTextureTransparency() == TransparencyMode.Transparent)
			{
				shader = EnsureAlphaPackingShader();
			}
			AlphaPacking textureAlphaPacking = _mediaPlayer.TextureProducer.GetTextureAlphaPacking();
			if (textureAlphaPacking != 0 && (uint)(textureAlphaPacking - 1) <= 1u)
			{
				shader = EnsureAlphaPackingShader();
			}
		}
		if (shader == null && _mediaPlayer.Info != null && QualitySettings.activeColorSpace == ColorSpace.Linear && !_mediaPlayer.Info.PlayerSupportsLinearColorSpace())
		{
			shader = EnsureAlphaPackingShader();
		}
		if (shader == null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTextureCount() == 2)
		{
			shader = EnsureAlphaPackingShader();
		}
		if (_mediaPlayer.TextureProducer != null && _mediaPlayer.IsUsingAndroidOESPath())
		{
			shader = EnsureAndroidOESShader();
			if (_mediaPlayer.TextureProducer.GetTextureTransparency() == TransparencyMode.Transparent)
			{
				shader = EnsureAndroidOESAlphaPackingShader();
			}
			AlphaPacking textureAlphaPacking = _mediaPlayer.TextureProducer.GetTextureAlphaPacking();
			if (textureAlphaPacking != 0 && (uint)(textureAlphaPacking - 1) <= 1u)
			{
				shader = EnsureAndroidOESAlphaPackingShader();
			}
		}
		return shader;
	}

	public bool HasValidTexture()
	{
		return true & Application.isPlaying & (_mediaPlayer != null && _mediaPlayer.TextureProducer != null && _mediaPlayer.TextureProducer.GetTexture() != null);
	}

	private void UpdateInternalMaterial()
	{
		if (!(_mediaPlayer != null))
		{
			return;
		}
		Shader shader = null;
		if (_material != null)
		{
			shader = _material.shader;
		}
		Shader requiredShader = GetRequiredShader();
		if (shader != requiredShader)
		{
			if (_material != null)
			{
				material = null;
				Object.Destroy(_material);
				_material = null;
			}
			if (requiredShader != null)
			{
				_material = new Material(requiredShader);
			}
		}
		material = _material;
	}

	private void LateUpdate()
	{
		if (_setNativeSize)
		{
			SetNativeSize();
		}
		if (_lastTexture != mainTexture)
		{
			_lastTexture = mainTexture;
			SetVerticesDirty();
			SetMaterialDirty();
		}
		if (HasValidTexture() && mainTexture != null)
		{
			Orientation orientation = Helper.GetOrientation(_mediaPlayer.Info.GetTextureTransform());
			if (mainTexture.width != _lastWidth || mainTexture.height != _lastHeight || orientation != _lastOrientation)
			{
				_lastWidth = mainTexture.width;
				_lastHeight = mainTexture.height;
				_lastOrientation = orientation;
				SetVerticesDirty();
				SetMaterialDirty();
			}
		}
		if (Application.isPlaying && !_isUserMaterial)
		{
			UpdateInternalMaterial();
		}
		if (material != null && _mediaPlayer != null)
		{
			VideoRender.SetupMaterialForMedia(materialForRendering, _mediaPlayer);
		}
	}

	[ContextMenu("Set Native Size")]
	public override void SetNativeSize()
	{
		Texture texture = mainTexture;
		if (!(texture != null))
		{
			return;
		}
		int num = Mathf.RoundToInt((float)texture.width * uvRect.width);
		int num2 = Mathf.RoundToInt((float)texture.height * uvRect.height);
		if (_mediaPlayer != null && _mediaPlayer.TextureProducer != null)
		{
			if (_mediaPlayer.TextureProducer.GetTextureAlphaPacking() == AlphaPacking.LeftRight || _mediaPlayer.TextureProducer.GetTextureStereoPacking() == StereoPacking.LeftRight)
			{
				num /= 2;
			}
			else if (_mediaPlayer.TextureProducer.GetTextureAlphaPacking() == AlphaPacking.TopBottom || _mediaPlayer.TextureProducer.GetTextureStereoPacking() == StereoPacking.TopBottom)
			{
				num2 /= 2;
			}
		}
		base.rectTransform.anchorMax = base.rectTransform.anchorMin;
		base.rectTransform.sizeDelta = new Vector2(num, num2);
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		vh.Clear();
		_OnFillVBO(_vertices);
		vh.AddUIVertexStream(_vertices, QuadIndices);
	}

	private void _OnFillVBO(List<UIVertex> vbo)
	{
		_flipY = false;
		if (HasValidTexture())
		{
			_flipY = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
		}
		Rect rect = _uvRect;
		Vector4 vector = (_drawingDimensions = GetDrawingDimensions(_scaleMode, ref rect));
		vbo.Clear();
		UIVertex simpleVert = UIVertex.simpleVert;
		simpleVert.color = color;
		simpleVert.position = new Vector2(vector.x, vector.y);
		simpleVert.uv0 = new Vector2(rect.xMin, rect.yMin);
		if (_flipY)
		{
			simpleVert.uv0 = new Vector2(rect.xMin, 1f - rect.yMin);
		}
		vbo.Add(simpleVert);
		simpleVert.position = new Vector2(vector.x, vector.w);
		simpleVert.uv0 = new Vector2(rect.xMin, rect.yMax);
		if (_flipY)
		{
			simpleVert.uv0 = new Vector2(rect.xMin, 1f - rect.yMax);
		}
		vbo.Add(simpleVert);
		simpleVert.position = new Vector2(vector.z, vector.w);
		simpleVert.uv0 = new Vector2(rect.xMax, rect.yMax);
		if (_flipY)
		{
			simpleVert.uv0 = new Vector2(rect.xMax, 1f - rect.yMax);
		}
		vbo.Add(simpleVert);
		simpleVert.position = new Vector2(vector.z, vector.y);
		simpleVert.uv0 = new Vector2(rect.xMax, rect.yMin);
		if (_flipY)
		{
			simpleVert.uv0 = new Vector2(rect.xMax, 1f - rect.yMin);
		}
		vbo.Add(simpleVert);
	}

	private Vector4 GetDrawingDimensions(ScaleMode scaleMode, ref Rect uvRect)
	{
		Vector4 result = Vector4.zero;
		if (mainTexture != null)
		{
			Vector4 zero = Vector4.zero;
			Vector2 vector = new Vector2(mainTexture.width, mainTexture.height);
			if (HasValidTexture())
			{
				float texturePixelAspectRatio = _mediaPlayer.TextureProducer.GetTexturePixelAspectRatio();
				if (texturePixelAspectRatio > 0f)
				{
					if (texturePixelAspectRatio > 1f)
					{
						vector.x *= texturePixelAspectRatio;
					}
					else
					{
						vector.y /= texturePixelAspectRatio;
					}
				}
			}
			if (_mediaPlayer != null && _mediaPlayer.TextureProducer != null)
			{
				if (_mediaPlayer.TextureProducer.GetTextureAlphaPacking() == AlphaPacking.LeftRight || _mediaPlayer.TextureProducer.GetTextureStereoPacking() == StereoPacking.LeftRight)
				{
					vector.x /= 2f;
				}
				else if (_mediaPlayer.TextureProducer.GetTextureAlphaPacking() == AlphaPacking.TopBottom || _mediaPlayer.TextureProducer.GetTextureStereoPacking() == StereoPacking.TopBottom)
				{
					vector.y /= 2f;
				}
			}
			Rect pixelAdjustedRect = GetPixelAdjustedRect();
			int num = Mathf.RoundToInt(vector.x);
			int num2 = Mathf.RoundToInt(vector.y);
			Vector4 vector2 = new Vector4(zero.x / (float)num, zero.y / (float)num2, ((float)num - zero.z) / (float)num, ((float)num2 - zero.w) / (float)num2);
			if (vector.sqrMagnitude > 0f)
			{
				switch (scaleMode)
				{
				case ScaleMode.ScaleToFit:
				{
					float num7 = vector.x / vector.y;
					float num8 = pixelAdjustedRect.width / pixelAdjustedRect.height;
					if (num7 > num8)
					{
						float height = pixelAdjustedRect.height;
						pixelAdjustedRect.height = pixelAdjustedRect.width * (1f / num7);
						pixelAdjustedRect.y += (height - pixelAdjustedRect.height) * base.rectTransform.pivot.y;
					}
					else
					{
						float width = pixelAdjustedRect.width;
						pixelAdjustedRect.width = pixelAdjustedRect.height * num7;
						pixelAdjustedRect.x += (width - pixelAdjustedRect.width) * base.rectTransform.pivot.x;
					}
					break;
				}
				case ScaleMode.ScaleAndCrop:
				{
					float num3 = vector.x / vector.y;
					float num4 = pixelAdjustedRect.width / pixelAdjustedRect.height;
					if (num4 > num3)
					{
						float num5 = num3 / num4;
						uvRect = new Rect(uvRect.xMin, uvRect.yMin * num5 + (1f - num5) * 0.5f, uvRect.width, num5 * uvRect.height);
					}
					else
					{
						float num6 = num4 / num3;
						uvRect = new Rect(uvRect.xMin * num6 + (0.5f - num6 * 0.5f), uvRect.yMin, num6 * uvRect.width, uvRect.height);
					}
					break;
				}
				}
			}
			result = new Vector4(pixelAdjustedRect.x + pixelAdjustedRect.width * vector2.x, pixelAdjustedRect.y + pixelAdjustedRect.height * vector2.y, pixelAdjustedRect.x + pixelAdjustedRect.width * vector2.z, pixelAdjustedRect.y + pixelAdjustedRect.height * vector2.w);
		}
		return result;
	}
}

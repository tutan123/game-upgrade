using UnityEngine;
using UnityEngine.Serialization;

namespace RenderHeads.Media.AVProVideo;

[ExecuteInEditMode]
[HelpURL("https://www.renderheads.com/products/avpro-video/")]
[AddComponentMenu("AVPro Video/Display IMGUI", 200)]
public class DisplayIMGUI : MonoBehaviour
{
	[SerializeField]
	private MediaPlayer _mediaPlayer;

	[SerializeField]
	private ScaleMode _scaleMode = ScaleMode.ScaleToFit;

	[SerializeField]
	private Color _color = Color.white;

	[FormerlySerializedAs("_alphaBlend")]
	[SerializeField]
	private bool _allowTransparency;

	[SerializeField]
	private bool _useDepth;

	[SerializeField]
	private int _depth;

	[Header("Area")]
	[FormerlySerializedAs("_fullScreen")]
	[SerializeField]
	private bool _isAreaFullScreen = true;

	[FormerlySerializedAs("_x")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _areaX;

	[FormerlySerializedAs("_y")]
	[Range(0f, 1f)]
	[SerializeField]
	private float _areaY;

	[Range(0f, 1f)]
	[FormerlySerializedAs("_width")]
	[SerializeField]
	private float _areaWidth = 1f;

	[Range(0f, 1f)]
	[SerializeField]
	[FormerlySerializedAs("_height")]
	private float _areaHeight = 1f;

	[SerializeField]
	[FormerlySerializedAs("_displayInEditor")]
	private bool _showAreaInEditor;

	private static Shader _shaderAlphaPacking;

	private Material _material;

	public MediaPlayer Player
	{
		get
		{
			return _mediaPlayer;
		}
		set
		{
			_mediaPlayer = value;
			Update();
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

	public Color Color
	{
		get
		{
			return _color;
		}
		set
		{
			_color = value;
		}
	}

	public bool AllowTransparency
	{
		get
		{
			return _allowTransparency;
		}
		set
		{
			_allowTransparency = value;
		}
	}

	public bool UseDepth
	{
		get
		{
			return _useDepth;
		}
		set
		{
			_useDepth = value;
		}
	}

	public int Depth
	{
		get
		{
			return _depth;
		}
		set
		{
			_depth = value;
		}
	}

	public bool IsAreaFullScreen
	{
		get
		{
			return _isAreaFullScreen;
		}
		set
		{
			_isAreaFullScreen = value;
		}
	}

	public float AreaX
	{
		get
		{
			return _areaX;
		}
		set
		{
			_areaX = value;
		}
	}

	public float AreaY
	{
		get
		{
			return _areaY;
		}
		set
		{
			_areaY = value;
		}
	}

	public float AreaWidth
	{
		get
		{
			return _areaWidth;
		}
		set
		{
			_areaWidth = value;
		}
	}

	public float AreaHeight
	{
		get
		{
			return _areaHeight;
		}
		set
		{
			_areaHeight = value;
		}
	}

	public bool ShowAreaInEditor
	{
		get
		{
			return _showAreaInEditor;
		}
		set
		{
			_showAreaInEditor = value;
		}
	}

	private void Start()
	{
		if (!_useDepth)
		{
			base.useGUILayout = false;
		}
		if (!_shaderAlphaPacking)
		{
			_shaderAlphaPacking = Shader.Find("AVProVideo/Internal/IMGUI/Texture Transparent");
			if (!_shaderAlphaPacking)
			{
				Debug.LogError("[AVProVideo] Missing shader 'AVProVideo/Internal/IMGUI/Texture Transparent'");
			}
		}
	}

	public void Update()
	{
		if (_mediaPlayer != null)
		{
			SetupMaterial();
		}
	}

	private void OnDestroy()
	{
		if (_material != null)
		{
			Object.Destroy(_material);
			_material = null;
		}
	}

	private Shader GetRequiredShader()
	{
		return _shaderAlphaPacking;
	}

	private void SetupMaterial()
	{
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
				Object.Destroy(_material);
				_material = null;
			}
			if (requiredShader != null)
			{
				_material = new Material(requiredShader);
			}
		}
	}

	private void OnGUI()
	{
		if (_mediaPlayer == null)
		{
			return;
		}
		Texture texture = null;
		_ = _showAreaInEditor;
		texture = VideoRender.GetTexture(_mediaPlayer, 0);
		if (_mediaPlayer.Info != null && !_mediaPlayer.Info.HasVideo())
		{
			texture = null;
		}
		if (!(texture != null) || (!(_color.a > 0f) && _allowTransparency))
		{
			return;
		}
		GUI.depth = _depth;
		GUI.color = _color;
		Rect areaRect = GetAreaRect();
		if (_material != null)
		{
			VideoRender.SetupMaterialForMedia(_material, _mediaPlayer);
			bool flag = false;
			if (QualitySettings.activeColorSpace == ColorSpace.Linear && !GL.sRGBWrite)
			{
				flag = true;
			}
			if (flag)
			{
				GL.sRGBWrite = true;
			}
			VideoRender.DrawTexture(areaRect, texture, _scaleMode, _mediaPlayer.TextureProducer.GetTextureAlphaPacking(), _mediaPlayer.TextureProducer.GetTexturePixelAspectRatio(), _material);
			if (flag)
			{
				GL.sRGBWrite = false;
			}
			return;
		}
		bool flag2 = false;
		if (_mediaPlayer.TextureProducer != null)
		{
			flag2 = _mediaPlayer.TextureProducer.RequiresVerticalFlip();
		}
		if (flag2)
		{
			GUIUtility.ScaleAroundPivot(new Vector2(1f, -1f), new Vector2(0f, areaRect.y + areaRect.height / 2f));
		}
		float texturePixelAspectRatio = _mediaPlayer.TextureProducer.GetTexturePixelAspectRatio();
		if (texturePixelAspectRatio > 0f)
		{
			if (texturePixelAspectRatio > 1f)
			{
				GUIUtility.ScaleAroundPivot(new Vector2(texturePixelAspectRatio, 1f), new Vector2(areaRect.x + areaRect.width / 2f, areaRect.y + areaRect.height / 2f));
			}
			else
			{
				GUIUtility.ScaleAroundPivot(new Vector2(1f, 1f / texturePixelAspectRatio), new Vector2(areaRect.x + areaRect.width / 2f, areaRect.y + areaRect.height / 2f));
			}
		}
		GUI.DrawTexture(areaRect, texture, _scaleMode, _allowTransparency);
	}

	public Rect GetAreaRect()
	{
		return (!_isAreaFullScreen) ? new Rect(_areaX * (float)(Screen.width - 1), _areaY * (float)(Screen.height - 1), _areaWidth * (float)Screen.width, _areaHeight * (float)Screen.height) : new Rect(0f, 0f, Screen.width, Screen.height);
	}
}

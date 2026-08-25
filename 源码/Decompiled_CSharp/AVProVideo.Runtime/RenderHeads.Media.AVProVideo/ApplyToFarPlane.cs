using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Video;

namespace RenderHeads.Media.AVProVideo;

[HelpURL("https://www.renderheads.com/products/avpro-video/")]
[AddComponentMenu("AVPro Video/Apply To Far Plane", 300)]
public sealed class ApplyToFarPlane : ApplyToBase
{
	private enum Eye
	{
		Left,
		Right
	}

	[Tooltip("The color override to apply to the material")]
	[Header("Shader Options")]
	[SerializeField]
	private Color _mainColor = Color.white;

	[SerializeField]
	[Tooltip("The Main Texture that is being written to by the Media Player")]
	private Texture _texture;

	[Tooltip("The Chroma Texture to apply to the material")]
	[SerializeField]
	private Texture _chroma;

	[SerializeField]
	[Tooltip("Alpha of the far plane that is drawn")]
	private float _alpha = 1f;

	[Tooltip("The Camera far plane to draw to, if left empty main cam will be selected")]
	[SerializeField]
	private Camera _camera;

	[Tooltip("The aspect ratio of the video shown, not used when a custom scaling is set")]
	[SerializeField]
	private VideoAspectRatio _aspectRatio = VideoAspectRatio.Stretch;

	[Tooltip("How much to offset the image by")]
	public Vector2 _drawOffset;

	[Tooltip("Will replace the Aspect Ratio with custom scaling for the video, when both values are non-zero")]
	public Vector2 _customScaling;

	private GameObject _renderedObject;

	private bool _changedSkybox;

	[Tooltip("Default texture to display when the video texture is preparing")]
	[SerializeField]
	[Header("Display")]
	private Texture2D _defaultTexture;

	[SerializeField]
	[Tooltip("The Material to use when rendering the video, if not set will use internal \n Note: Material must use the AVProVideo/Background/AVProVideo-ApplyToFarPlane shader")]
	private Material _material;

	[SerializeField]
	private string _texturePropertyName = "_MainTex";

	[SerializeField]
	private Vector2 _offset = Vector2.zero;

	[SerializeField]
	private Vector2 _scale = Vector2.one;

	private Texture _lastTextureApplied;

	private LazyShaderProperty _propTexture;

	private LazyShaderProperty _propTexture_R;

	private Texture _originalTexture;

	private Vector2 _originalScale = Vector2.one;

	private Vector2 _originalOffset = Vector2.zero;

	public Color MainColor
	{
		get
		{
			return _mainColor;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetColor("_Color", value);
			_mainColor = value;
		}
	}

	public Texture Texture
	{
		get
		{
			return _texture;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetTexture("_MainTex", value);
			_texture = value;
		}
	}

	public Texture Chroma
	{
		get
		{
			return _chroma;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetTexture("_ChromaTex", value);
			_chroma = value;
		}
	}

	public float Alpha
	{
		get
		{
			return _alpha;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetFloat("_Alpha", value);
			_alpha = value;
		}
	}

	public Camera Camera
	{
		get
		{
			return _camera;
		}
		set
		{
			_camera = value;
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetFloat("_TargetCamID", value.GetInstanceID());
		}
	}

	public VideoAspectRatio VideoAspectRatio
	{
		get
		{
			return _aspectRatio;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetFloat("_Aspect", (float)value);
			_aspectRatio = value;
		}
	}

	public Vector2 DrawOffset
	{
		get
		{
			return _drawOffset;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetVector("_DrawOffset", value);
			_drawOffset = value;
		}
	}

	public Vector2 CustomScaling
	{
		get
		{
			return _customScaling;
		}
		set
		{
			if (!_material)
			{
				CreateMaterial();
			}
			_material.SetVector("_CustomScale", value);
			_customScaling = value;
		}
	}

	public Texture2D DefaultTexture
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
				_isDirty = true;
			}
		}
	}

	public string TexturePropertyName
	{
		get
		{
			return _texturePropertyName;
		}
		set
		{
			if (_texturePropertyName != value)
			{
				_texturePropertyName = value;
				_propTexture = new LazyShaderProperty(_texturePropertyName);
				_propTexture_R = new LazyShaderProperty(_texturePropertyName + "_R");
				_isDirty = true;
			}
		}
	}

	public Vector2 Offset
	{
		get
		{
			return _offset;
		}
		set
		{
			if (_offset != value)
			{
				_offset = value;
				_isDirty = true;
			}
		}
	}

	public Vector2 Scale
	{
		get
		{
			return _scale;
		}
		set
		{
			if (_scale != value)
			{
				_scale = value;
				_isDirty = true;
			}
		}
	}

	private Vector2 ImageSize => new Vector2(_media.Info.GetVideoWidth(), _media.Info.GetVideoHeight());

	public void Awake()
	{
		if (!_camera)
		{
			_camera = Camera.main;
		}
		if ((bool)_material)
		{
			_material.SetFloat("_TargetCamID", _camera.GetInstanceID());
		}
	}

	protected override void OnDisable()
	{
		if (_changedSkybox && (bool)_camera)
		{
			_camera.clearFlags = CameraClearFlags.Skybox;
		}
		base.OnDisable();
		if ((bool)_renderedObject)
		{
			_renderedObject.SetActive(value: false);
		}
	}

	private void OnDestroy()
	{
		if (Application.isPlaying)
		{
			Object.Destroy(_renderedObject);
		}
		else
		{
			Object.DestroyImmediate(_renderedObject);
		}
		_renderedObject = null;
	}

	public void Update()
	{
		if (_renderedObject != null)
		{
			_renderedObject.transform.position = new Vector3(0f, 0f, _camera.nearClipPlane) + _camera.transform.position + _camera.transform.forward;
			_renderedObject.transform.rotation = _camera.transform.rotation;
		}
	}

	public Mesh CreateQuadMesh()
	{
		int num = 1;
		int num2 = 1;
		Mesh mesh = new Mesh();
		Vector3[] vertices = new Vector3[4]
		{
			new Vector3(0f, 0f, 0f),
			new Vector3(num, 0f, 0f),
			new Vector3(0f, num2, 0f),
			new Vector3(num, num2, 0f)
		};
		mesh.vertices = vertices;
		int[] triangles = new int[6] { 0, 2, 1, 2, 3, 1 };
		mesh.triangles = triangles;
		Vector3[] normals = new Vector3[4]
		{
			-_camera.transform.forward,
			-_camera.transform.forward,
			-_camera.transform.forward,
			-_camera.transform.forward
		};
		mesh.normals = normals;
		Vector2[] uv = new Vector2[4]
		{
			new Vector2(0f, 0f),
			new Vector2(1f, 0f),
			new Vector2(0f, 1f),
			new Vector2(1f, 1f)
		};
		mesh.uv = uv;
		return mesh;
	}

	public void CreateMaterial()
	{
		_material = new Material(Shader.Find("AVProVideo/Background/AVProVideo-ApplyToFarPlane"));
		if ((bool)_renderedObject)
		{
			if (_renderedObject.TryGetComponent<ApplyToFarPlane_CameraApplier>(out var component))
			{
				component.Material = _material;
			}
			else
			{
				_renderedObject.AddComponent<ApplyToFarPlane_CameraApplier>().Material = _material;
			}
		}
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		if (!_material)
		{
			CreateMaterial();
		}
		if (_renderedObject == null)
		{
			_renderedObject = GameObject.Find("AVPro Video Far Plane");
		}
		if ((bool)_renderedObject)
		{
			_renderedObject.SetActive(value: true);
		}
		else
		{
			_renderedObject = new GameObject("AVPro Video Far Plane");
			MeshRenderer meshRenderer = _renderedObject.AddComponent<MeshRenderer>();
			meshRenderer.shadowCastingMode = ShadowCastingMode.Off;
			meshRenderer.receiveShadows = false;
			MeshFilter meshFilter = _renderedObject.AddComponent<MeshFilter>();
			Mesh sharedMesh = CreateQuadMesh();
			meshFilter.sharedMesh = sharedMesh;
			ApplyToFarPlane_CameraApplier applyToFarPlane_CameraApplier = _renderedObject.AddComponent<ApplyToFarPlane_CameraApplier>();
			if ((bool)_camera)
			{
				_material.SetFloat("_TargetCamID", _camera.GetInstanceID());
			}
			applyToFarPlane_CameraApplier.Material = _material;
			meshRenderer.sharedMaterial = _material;
		}
		if (_camera.clearFlags == CameraClearFlags.Skybox)
		{
			Debug.LogWarning("[AVProVideo] Warning: ApplyToFarPlane does not work with the background clear mode set to skybox, automatically changed to color, this will be undone when the object is disabled");
			_changedSkybox = true;
			_camera.clearFlags = CameraClearFlags.Color;
		}
	}

	private void LateUpdate()
	{
		Apply();
	}

	public void UpdateMaterialProperties(int target)
	{
		if (_material == null)
		{
			CreateMaterial();
		}
		switch (target)
		{
		case 0:
			_material.SetColor("_Color", _mainColor);
			break;
		case 3:
			_material.SetTexture("_MainTex", _texture);
			break;
		case 4:
			_material.SetTexture("_ChromaTex", _chroma);
			break;
		case 5:
			_material.SetFloat("_Alpha", _alpha);
			break;
		case 7:
			_material.SetFloat("_Aspect", (float)_aspectRatio);
			break;
		case 8:
			_material.SetVector("_DrawOffset", _drawOffset);
			break;
		case 9:
			_material.SetVector("_CustomScale", _customScaling);
			break;
		case 1:
		case 2:
		case 6:
			break;
		}
	}

	public override void Apply()
	{
		bool flag = false;
		if (_media != null && _media.TextureProducer != null)
		{
			Texture texture = ((_media.FrameResampler == null || _media.FrameResampler.OutputTexture == null) ? null : _media.FrameResampler.OutputTexture[0]);
			Texture texture2 = (_media.UseResampler ? texture : _media.TextureProducer.GetTexture());
			if (texture2 != null)
			{
				if (texture2 != _lastTextureApplied)
				{
					_isDirty = true;
				}
				if (_isDirty)
				{
					bool requiresYFlip = _media.TextureProducer.RequiresVerticalFlip();
					StereoPacking textureStereoPacking = _media.TextureProducer.GetTextureStereoPacking();
					bool flag2 = textureStereoPacking == StereoPacking.MultiviewLeftPrimary || textureStereoPacking == StereoPacking.MultiviewRightPrimary;
					int num = 1;
					if (!_media.UseResampler)
					{
						num = _media.TextureProducer.GetTextureCount();
						if (flag2)
						{
							num /= 2;
						}
					}
					for (int i = 0; i < num; i++)
					{
						Texture texture3 = ((_media.FrameResampler == null || _media.FrameResampler.OutputTexture == null) ? null : _media.FrameResampler.OutputTexture[i]);
						texture2 = (_media.UseResampler ? texture3 : _media.TextureProducer.GetTexture(i));
						if (texture2 != null)
						{
							ApplyMapping(texture2, requiresYFlip, i);
						}
					}
					if (flag2)
					{
						for (int j = 0; j < num; j++)
						{
							texture2 = _media.TextureProducer.GetTexture(num + j);
							if (texture2 != null)
							{
								ApplyMapping(texture2, requiresYFlip, j, Eye.Right);
							}
						}
					}
				}
				flag = true;
			}
		}
		if (!flag)
		{
			if (_defaultTexture != _lastTextureApplied)
			{
				_isDirty = true;
			}
			if (_isDirty)
			{
				ApplyMapping(_defaultTexture, requiresYFlip: false);
			}
		}
	}

	private void ApplyMapping(Texture texture, bool requiresYFlip, int plane = 0, Eye eye = Eye.Left)
	{
		if (_material != null)
		{
			_isDirty = false;
			switch (plane)
			{
			case 0:
			{
				int id = _propTexture.Id;
				if (eye == Eye.Left)
				{
					VideoRender.SetupMaterialForMedia(_material, _media, id, texture, texture == _defaultTexture);
					_lastTextureApplied = texture;
				}
				else
				{
					id = _propTexture_R.Id;
					_material.SetTexture(id, texture);
				}
				if (!(texture != null))
				{
					break;
				}
				if (requiresYFlip)
				{
					if (_material.HasProperty(id))
					{
						_material.SetTextureScale(id, new Vector2(_scale.x, 0f - _scale.y));
						_material.SetTextureOffset(id, Vector2.up + _offset);
					}
				}
				else
				{
					_material.SetTextureScale(id, _scale);
					_material.SetTextureOffset(id, _offset);
				}
				break;
			}
			case 1:
				if (texture != null)
				{
					if (requiresYFlip)
					{
						_material.SetTextureScale(VideoRender.PropChromaTex.Id, new Vector2(_scale.x, 0f - _scale.y));
						_material.SetTextureOffset(VideoRender.PropChromaTex.Id, Vector2.up + _offset);
					}
					else
					{
						_material.SetTextureScale(VideoRender.PropChromaTex.Id, _scale);
						_material.SetTextureOffset(VideoRender.PropChromaTex.Id, _offset);
					}
				}
				break;
			}
		}
		else
		{
			CreateMaterial();
		}
	}

	protected override void SaveProperties()
	{
		if (_material != null)
		{
			if (string.IsNullOrEmpty(_texturePropertyName))
			{
				_originalTexture = _material.mainTexture;
				_originalScale = _material.mainTextureScale;
				_originalOffset = _material.mainTextureOffset;
			}
			else
			{
				_originalTexture = _material.GetTexture(_texturePropertyName);
				_originalScale = _material.GetTextureScale(_texturePropertyName);
				_originalOffset = _material.GetTextureOffset(_texturePropertyName);
			}
		}
		else
		{
			CreateMaterial();
		}
		_propTexture = new LazyShaderProperty(_texturePropertyName);
		_propTexture_R = new LazyShaderProperty(_texturePropertyName + "_R");
	}

	protected override void RestoreProperties()
	{
		if (_material != null)
		{
			if (string.IsNullOrEmpty(_texturePropertyName))
			{
				_material.mainTexture = _originalTexture;
				_material.mainTextureScale = _originalScale;
				_material.mainTextureOffset = _originalOffset;
			}
			else
			{
				_material.SetTexture(_texturePropertyName, _originalTexture);
				_material.SetTextureScale(_texturePropertyName, _originalScale);
				_material.SetTextureOffset(_texturePropertyName, _originalOffset);
			}
		}
		else
		{
			CreateMaterial();
		}
	}
}

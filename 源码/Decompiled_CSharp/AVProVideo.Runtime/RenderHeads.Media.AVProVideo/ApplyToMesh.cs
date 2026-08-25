using UnityEngine;
using UnityEngine.Serialization;

namespace RenderHeads.Media.AVProVideo;

[ExecuteInEditMode]
[HelpURL("https://www.renderheads.com/products/avpro-video/")]
[AddComponentMenu("AVPro Video/Apply To Mesh", 300)]
public sealed class ApplyToMesh : ApplyToBase
{
	private enum Eye
	{
		Left,
		Right
	}

	[Tooltip("Default texture to display when the video texture is preparing")]
	[Header("Display")]
	[Space(8f)]
	[SerializeField]
	private Texture2D _defaultTexture;

	[Space(8f)]
	[FormerlySerializedAs("_mesh")]
	[Header("Renderer Target")]
	[SerializeField]
	private Renderer _renderer;

	[SerializeField]
	private int _materialIndex = -1;

	[SerializeField]
	private string _texturePropertyName = "_MainTex";

	[SerializeField]
	private Vector2 _offset = Vector2.zero;

	[SerializeField]
	private Vector2 _scale = Vector2.one;

	private Texture _lastTextureApplied;

	private LazyShaderProperty _propTexture;

	private LazyShaderProperty _propTexture_R;

	public Texture2D DefaultTexture
	{
		get
		{
			return _defaultTexture;
		}
		set
		{
			ChangeDefaultTexture(value);
		}
	}

	public Renderer MeshRenderer
	{
		get
		{
			return _renderer;
		}
		set
		{
			ChangeRenderer(value);
		}
	}

	public int MaterialIndex
	{
		get
		{
			return _materialIndex;
		}
		set
		{
			_materialIndex = value;
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

	private void ChangeDefaultTexture(Texture2D texture)
	{
		if (_defaultTexture != texture)
		{
			_defaultTexture = texture;
			ForceUpdate();
		}
	}

	private void ChangeRenderer(Renderer renderer)
	{
		if (_renderer != renderer)
		{
			_ = (bool)_renderer;
			_renderer = renderer;
			if ((bool)_renderer)
			{
				ForceUpdate();
			}
		}
	}

	private void LateUpdate()
	{
		Apply();
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
							ApplyMapping(texture2, _media.TextureProducer.RequiresVerticalFlip(), i, Eye.Left, _materialIndex);
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
				ApplyMapping(_defaultTexture, requiresYFlip: false, 0, Eye.Left, _materialIndex);
			}
		}
	}

	private void ApplyMapping(Texture texture, bool requiresYFlip, int plane, Eye eye = Eye.Left, int materialIndex = -1)
	{
		if (!(_renderer != null))
		{
			return;
		}
		_isDirty = false;
		Material[] array = ((Application.isEditor && !Application.isPlaying) ? _renderer.sharedMaterials : _renderer.materials);
		if (array == null)
		{
			return;
		}
		for (int i = 0; i < array.Length; i++)
		{
			if (_materialIndex >= 0 && i != _materialIndex)
			{
				continue;
			}
			Material material = array[i];
			if (!(material != null))
			{
				continue;
			}
			if (base.StereoRedGreenTint)
			{
				material.EnableKeyword("STEREO_DEBUG");
			}
			else
			{
				material.DisableKeyword("STEREO_DEBUG");
			}
			switch (plane)
			{
			case 0:
			{
				int id = _propTexture.Id;
				if (eye == Eye.Left)
				{
					VideoRender.SetupMaterialForMedia(material, _media, _propTexture.Id, texture, texture == _defaultTexture);
					_lastTextureApplied = texture;
				}
				else
				{
					id = _propTexture_R.Id;
					material.SetTexture(id, texture);
				}
				if (texture != null)
				{
					if (requiresYFlip)
					{
						material.SetTextureScale(_propTexture.Id, new Vector2(_scale.x, 0f - _scale.y));
						material.SetTextureOffset(_propTexture.Id, Vector2.up + _offset);
					}
					else
					{
						material.SetTextureScale(_propTexture.Id, _scale);
						material.SetTextureOffset(_propTexture.Id, _offset);
					}
				}
				break;
			}
			case 1:
				if (texture != null)
				{
					if (requiresYFlip)
					{
						material.SetTextureScale(VideoRender.PropChromaTex.Id, new Vector2(_scale.x, 0f - _scale.y));
						material.SetTextureOffset(VideoRender.PropChromaTex.Id, Vector2.up + _offset);
					}
					else
					{
						material.SetTextureScale(VideoRender.PropChromaTex.Id, _scale);
						material.SetTextureOffset(VideoRender.PropChromaTex.Id, _offset);
					}
				}
				break;
			}
		}
	}

	protected override void OnEnable()
	{
		if (_renderer == null)
		{
			_renderer = GetComponent<MeshRenderer>();
			if (_renderer == null)
			{
				Debug.LogWarning("[AVProVideo] No MeshRenderer set or found in gameobject");
			}
		}
		_propTexture = new LazyShaderProperty(_texturePropertyName);
		ForceUpdate();
	}

	protected override void OnDisable()
	{
		ApplyMapping(_defaultTexture, requiresYFlip: false, 0, Eye.Left, _materialIndex);
	}

	protected override void SaveProperties()
	{
		_propTexture = new LazyShaderProperty(_texturePropertyName);
		_propTexture_R = new LazyShaderProperty(_texturePropertyName + "_R");
	}
}

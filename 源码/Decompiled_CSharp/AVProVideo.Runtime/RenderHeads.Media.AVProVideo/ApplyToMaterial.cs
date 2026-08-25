using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

[AddComponentMenu("AVPro Video/Apply To Material", 300)]
[ExecuteInEditMode]
[HelpURL("https://www.renderheads.com/products/avpro-video/")]
public sealed class ApplyToMaterial : ApplyToBase
{
	private enum Eye
	{
		Left,
		Right
	}

	[Header("Display")]
	[Tooltip("Default texture to display when the video texture is preparing")]
	[SerializeField]
	private Texture2D _defaultTexture;

	[SerializeField]
	[Header("Material Target")]
	[Space(8f)]
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

	public Material Material
	{
		get
		{
			return _material;
		}
		set
		{
			if (_material != value)
			{
				_material = value;
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
		if (!(_material != null))
		{
			return;
		}
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
			if (texture != null)
			{
				if (requiresYFlip)
				{
					_material.SetTextureScale(id, new Vector2(_scale.x, 0f - _scale.y));
					_material.SetTextureOffset(id, Vector2.up + _offset);
				}
				else
				{
					_material.SetTextureScale(id, _scale);
					_material.SetTextureOffset(id, _offset);
				}
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
	}
}

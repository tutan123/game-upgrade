using UnityEngine;

namespace RenderHeads.Media.AVProVideo;

public struct LazyShaderProperty
{
	private string _name;

	private int _id;

	public string Name => _name;

	public int Id
	{
		get
		{
			if (_id == 0)
			{
				_id = Shader.PropertyToID(_name);
			}
			return _id;
		}
	}

	public LazyShaderProperty(string name)
	{
		_name = name;
		_id = 0;
	}
}

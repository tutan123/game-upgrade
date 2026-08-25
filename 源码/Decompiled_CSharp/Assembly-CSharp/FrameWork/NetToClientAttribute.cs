using System;
using UnityEngine;

namespace FrameWork;

[AttributeUsage(AttributeTargets.Method)]
public class NetToClientAttribute : Attribute
{
	public NetToClientAttribute()
	{
		Debug.Log("aaaaa");
	}
}

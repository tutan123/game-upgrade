using UnityEngine;
using UnityEngine.UI;

namespace RenderHeads.Media.AVProVideo.Demos.UI;

[ExecuteInEditMode]
public class HorizontalSegmentsPrimitive : Graphic
{
	private float[] _segments = new float[2];

	public float[] Segments
	{
		get
		{
			return _segments;
		}
		set
		{
			SetSegments(value);
		}
	}

	private void SetSegments(float[] segments)
	{
		if (segments != null && segments.Length > 1)
		{
			_segments = segments;
		}
		else
		{
			_segments = new float[2];
		}
		SetVerticesDirty();
	}

	protected override void OnPopulateMesh(VertexHelper vh)
	{
		Vector2 zero = Vector2.zero;
		Vector2 zero2 = Vector2.zero;
		zero.x = 0f;
		zero.y = 0f;
		zero2.x = 1f;
		zero2.y = 1f;
		zero.x -= base.rectTransform.pivot.x;
		zero.y -= base.rectTransform.pivot.y;
		zero2.x -= base.rectTransform.pivot.x;
		zero2.y -= base.rectTransform.pivot.y;
		zero.x *= base.rectTransform.rect.width;
		zero.y *= base.rectTransform.rect.height;
		zero2.x *= base.rectTransform.rect.width;
		zero2.y *= base.rectTransform.rect.height;
		vh.Clear();
		int num = _segments.Length / 2;
		UIVertex simpleVert = UIVertex.simpleVert;
		int num2 = 0;
		for (int i = 0; i < num; i++)
		{
			float x = _segments[i * 2] * (zero2.x - zero.x) + zero.x;
			float x2 = _segments[i * 2 + 1] * (zero2.x - zero.x) + zero.x;
			simpleVert.position = new Vector2(x, zero.y);
			simpleVert.color = color;
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector2(x, zero2.y);
			simpleVert.color = color;
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector2(x2, zero2.y);
			simpleVert.color = color;
			vh.AddVert(simpleVert);
			simpleVert.position = new Vector2(x2, zero.y);
			simpleVert.color = color;
			vh.AddVert(simpleVert);
			vh.AddTriangle(num2, 1 + num2, 2 + num2);
			vh.AddTriangle(2 + num2, 3 + num2, num2);
			num2 += 4;
		}
	}
}

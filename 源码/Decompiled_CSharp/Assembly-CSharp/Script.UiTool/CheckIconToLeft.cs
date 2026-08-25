using UnityEngine;

namespace Script.UiTool;

[ExecuteAlways]
public class CheckIconToLeft : MonoBehaviour
{
	public PosType posType;

	public Vector3 offset;

	public RectTransform rectTransform;

	private void Update()
	{
		if (!(rectTransform == null))
		{
			Vector3 localPosition = rectTransform.localPosition;
			float num = 0f;
			num = ((posType != 0) ? (localPosition.x + rectTransform.rect.width * rectTransform.pivot.x) : (localPosition.x - rectTransform.rect.width * rectTransform.pivot.x));
			base.transform.localPosition = new Vector3(num, localPosition.y, localPosition.z) + offset;
		}
	}
}

using UnityEngine;

public class AE_Door : MonoBehaviour
{
	private bool trig;

	private bool open;

	public float smooth = 2f;

	public float DoorOpenAngle = 87f;

	private Quaternion defaultRot;

	private Quaternion openRot;

	private bool isKeyPressed;

	[Header("GUI Settings")]
	public string openMessage = "Open E";

	public string closeMessage = "Close E";

	public Font messageFont;

	public int fontSize = 24;

	public Color fontColor = Color.white;

	public Vector2 messagePosition = new Vector2(0.5f, 0.5f);

	private string doorMessage = "";

	private void Start()
	{
		defaultRot = base.transform.rotation;
		openRot = Quaternion.Euler(base.transform.eulerAngles.x, base.transform.eulerAngles.y + DoorOpenAngle, base.transform.eulerAngles.z);
		isKeyPressed = false;
	}

	private void Update()
	{
		if (open)
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, openRot, Time.deltaTime * smooth);
		}
		else
		{
			base.transform.rotation = Quaternion.Slerp(base.transform.rotation, defaultRot, Time.deltaTime * smooth);
		}
		if (Input.GetKeyDown(KeyCode.E) && trig && !isKeyPressed)
		{
			open = !open;
			isKeyPressed = true;
		}
		if (Input.GetKeyUp(KeyCode.E))
		{
			isKeyPressed = false;
		}
		if (trig)
		{
			doorMessage = (open ? closeMessage : openMessage);
		}
		else
		{
			doorMessage = "";
		}
	}

	private void OnGUI()
	{
		if (!string.IsNullOrEmpty(doorMessage))
		{
			GUIStyle gUIStyle = new GUIStyle(GUI.skin.label);
			gUIStyle.alignment = TextAnchor.MiddleCenter;
			gUIStyle.fontSize = fontSize;
			gUIStyle.normal.textColor = fontColor;
			if (messageFont != null)
			{
				gUIStyle.font = messageFont;
			}
			float num = Screen.width;
			float num2 = Screen.height;
			Vector2 vector = gUIStyle.CalcSize(new GUIContent(doorMessage));
			float x = num * messagePosition.x - vector.x / 2f;
			float y = num2 * messagePosition.y - vector.y / 2f;
			GUI.Label(new Rect(x, y, vector.x, vector.y), doorMessage, gUIStyle);
		}
	}

	private void OnTriggerEnter(Collider coll)
	{
		if (coll.tag == "Player")
		{
			doorMessage = (open ? closeMessage : openMessage);
			trig = true;
		}
	}

	private void OnTriggerExit(Collider coll)
	{
		if (coll.tag == "Player")
		{
			doorMessage = "";
			trig = false;
		}
	}
}

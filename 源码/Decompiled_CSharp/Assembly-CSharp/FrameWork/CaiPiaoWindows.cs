using System.Linq;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Normal, true)]
[ActorInfo("", "CaiPiaoWindows")]
public class CaiPiaoWindows : UiActor
{
	[Header("刮刮乐设置")]
	private int brushSize = 30;

	private Texture2D eraseTexture;

	public float completeRatio = 0.3f;

	private int totalPixels;

	private int lastCheckFrame;

	private bool isCompleted;

	private int _coin;

	public RectTransform RectTransformCount;

	public CanvasRenderer CanvasRendererCount;

	public TextMeshProUGUI TextMeshProUGUICount;

	public AddScripts AddScriptsCount;

	public RectTransform RectTransformMask;

	public CanvasRenderer CanvasRendererMask;

	public Image ImageMask;

	public AddScripts AddScriptsMask;

	public RectTransform RectTransformClose;

	public CanvasRenderer CanvasRendererClose;

	public Image ImageClose;

	public BT BTClose;

	public AddScripts AddScriptsClose;

	public RectTransform RectTransformImg;

	public CanvasRenderer CanvasRendererImg;

	public Image ImageImg;

	public AddScripts AddScriptsImg;

	public override void Awake()
	{
		base.Awake();
		RectTransformCount = GetGameObject().transform.Find("View/Bg/Count/").GetComponent<RectTransform>();
		CanvasRendererCount = GetGameObject().transform.Find("View/Bg/Count/").GetComponent<CanvasRenderer>();
		TextMeshProUGUICount = GetGameObject().transform.Find("View/Bg/Count/").GetComponent<TextMeshProUGUI>();
		AddScriptsCount = GetGameObject().transform.Find("View/Bg/Count/").GetComponent<AddScripts>();
		RectTransformMask = GetGameObject().transform.Find("View/Bg/Mask/").GetComponent<RectTransform>();
		CanvasRendererMask = GetGameObject().transform.Find("View/Bg/Mask/").GetComponent<CanvasRenderer>();
		ImageMask = GetGameObject().transform.Find("View/Bg/Mask/").GetComponent<Image>();
		AddScriptsMask = GetGameObject().transform.Find("View/Bg/Mask/").GetComponent<AddScripts>();
		RectTransformClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<RectTransform>();
		CanvasRendererClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<CanvasRenderer>();
		ImageClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<Image>();
		BTClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<BT>();
		AddScriptsClose = GetGameObject().transform.Find("View/Bg/Close/").GetComponent<AddScripts>();
		RectTransformImg = GetGameObject().transform.Find("View/Bg/Img/").GetComponent<RectTransform>();
		CanvasRendererImg = GetGameObject().transform.Find("View/Bg/Img/").GetComponent<CanvasRenderer>();
		ImageImg = GetGameObject().transform.Find("View/Bg/Img/").GetComponent<Image>();
		AddScriptsImg = GetGameObject().transform.Find("View/Bg/Img/").GetComponent<AddScripts>();
	}

	public CaiPiaoWindows(Transform trans)
		: base(trans)
	{
	}

	public CaiPiaoWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTClose.onClick.AddListener(CloseUi);
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		Init();
		RandomReward();
		BTClose.gameObject.SetActive(value: false);
	}

	private void Init()
	{
		isCompleted = false;
		if (ImageMask == null || ImageImg == null || ImageImg.sprite == null)
		{
			MyLog.LogError("请在Inspector赋值 ImageMask / ImageImg 且带Sprite!");
			return;
		}
		int width = ImageImg.sprite.texture.width;
		int height = ImageImg.sprite.texture.height;
		eraseTexture = new Texture2D(width, height);
		eraseTexture.filterMode = FilterMode.Bilinear;
		eraseTexture.wrapMode = TextureWrapMode.Clamp;
		Color[] pixels = ImageImg.sprite.texture.GetPixels();
		eraseTexture.SetPixels(pixels);
		eraseTexture.Apply();
		Material material = new Material(Shader.Find("UI/Unlit/Transparent") ?? Shader.Find("UI/Default"));
		ImageMask.material = material;
		UpdateSprite();
		totalPixels = width * height;
	}

	private void RandomReward()
	{
		int[] array = new int[4] { 10000, 5000, 3000, 100 };
		int[] array2 = new int[4] { 0, 10, 50, 1000000 };
		int maxExclusive = array.Sum();
		int num = Random.Range(0, maxExclusive);
		int num2 = 0;
		for (int i = 0; i < array.Length; i++)
		{
			num2 += array[i];
			if (num < num2)
			{
				_coin = array2[i];
				break;
			}
		}
		if (_coin == 0)
		{
			TextMeshProUGUICount.text = LanguageMrg.GetText("A1402");
		}
		else
		{
			TextMeshProUGUICount.text = _coin.ToString();
		}
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (!isCompleted)
		{
			if (Input.GetMouseButton(0))
			{
				Erase(Input.mousePosition);
			}
			if (Time.frameCount % 30 == 0 && lastCheckFrame != Time.frameCount)
			{
				lastCheckFrame = Time.frameCount;
				CheckEraseRatio();
			}
		}
	}

	private void Erase(Vector2 screenPos)
	{
		if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(ImageMask.rectTransform, screenPos, UiManager.GetCamera(), out var localPoint))
		{
			return;
		}
		float num = (localPoint.x + ImageMask.rectTransform.rect.width / 2f) / ImageMask.rectTransform.rect.width;
		float num2 = (localPoint.y + ImageMask.rectTransform.rect.height / 2f) / ImageMask.rectTransform.rect.height;
		Texture2D texture = ImageMask.sprite.texture;
		int num3 = Mathf.Clamp((int)(num * (float)texture.width), 0, texture.width - 1);
		int num4 = Mathf.Clamp((int)(num2 * (float)texture.height), 0, texture.height - 1);
		for (int i = -brushSize; i <= brushSize; i++)
		{
			for (int j = -brushSize; j <= brushSize; j++)
			{
				if (i * i + j * j <= brushSize * brushSize)
				{
					int num5 = num3 + i;
					int num6 = num4 + j;
					if (num5 >= 0 && num5 < texture.width && num6 >= 0 && num6 < texture.height)
					{
						texture.SetPixel(num5, num6, Color.clear);
					}
				}
			}
		}
		texture.Apply();
	}

	private void CheckEraseRatio()
	{
		Color[] pixels = eraseTexture.GetPixels();
		int num = 0;
		Color[] array = pixels;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].a < 0.1f)
			{
				num++;
			}
		}
		if ((float)num / (float)totalPixels >= completeRatio)
		{
			isCompleted = true;
			OnEraseComplete();
			EraseAll();
		}
	}

	private void EraseAll()
	{
		Color[] array = new Color[eraseTexture.width * eraseTexture.height];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = Color.clear;
		}
		eraseTexture.SetPixels(array);
		eraseTexture.Apply();
		UpdateSprite();
	}

	private void UpdateSprite()
	{
		ImageMask.sprite = Sprite.Create(eraseTexture, new Rect(0f, 0f, eraseTexture.width, eraseTexture.height), new Vector2(0.5f, 0.5f));
	}

	private void OnEraseComplete()
	{
		BTClose.gameObject.SetActive(value: true);
		if (_coin == 0)
		{
			SingletonAsMono<InfoTipsMrg>.Instance.Add(LanguageMrg.GetText("A1354"));
			return;
		}
		new PropertyData
		{
			PropertyType = PropertyType.Property,
			propertyTypeValue = PropertyTypeValue.Money,
			PropertyValue = _coin
		}.AddTypeValueAsShowTips();
	}
}

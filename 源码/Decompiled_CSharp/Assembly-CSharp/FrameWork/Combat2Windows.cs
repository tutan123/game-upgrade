using System.Collections.Generic;
using DG.Tweening;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Background, true)]
[ActorInfo("", "Combat2Windows")]
public class Combat2Windows : UiActor
{
	private Queue<string> _playerLoseData = new Queue<string>();

	private Tweener _playerTweener;

	private Tweener _enemyTweener;

	public RectTransform RectTransformPlayerHp;

	public Slider SliderPlayerHp;

	public AddScripts AddScriptsPlayerHp;

	public RectTransform RectTransformPlayerHpText;

	public CanvasRenderer CanvasRendererPlayerHpText;

	public TextMeshProUGUI TextMeshProUGUIPlayerHpText;

	public AddScripts AddScriptsPlayerHpText;

	public RectTransform RectTransformPlayerHpJianGroup;

	public AddScripts AddScriptsPlayerHpJianGroup;

	public override void Awake()
	{
		base.Awake();
		RectTransformPlayerHp = GetGameObject().transform.Find("PlayerHp/").GetComponent<RectTransform>();
		SliderPlayerHp = GetGameObject().transform.Find("PlayerHp/").GetComponent<Slider>();
		AddScriptsPlayerHp = GetGameObject().transform.Find("PlayerHp/").GetComponent<AddScripts>();
		RectTransformPlayerHpText = GetGameObject().transform.Find("PlayerHp/PlayerHpText/").GetComponent<RectTransform>();
		CanvasRendererPlayerHpText = GetGameObject().transform.Find("PlayerHp/PlayerHpText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIPlayerHpText = GetGameObject().transform.Find("PlayerHp/PlayerHpText/").GetComponent<TextMeshProUGUI>();
		AddScriptsPlayerHpText = GetGameObject().transform.Find("PlayerHp/PlayerHpText/").GetComponent<AddScripts>();
		RectTransformPlayerHpJianGroup = GetGameObject().transform.Find("PlayerHpJianGroup/").GetComponent<RectTransform>();
		AddScriptsPlayerHpJianGroup = GetGameObject().transform.Find("PlayerHpJianGroup/").GetComponent<AddScripts>();
	}

	public Combat2Windows(Transform trans)
		: base(trans)
	{
	}

	public Combat2Windows()
	{
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		Tool.HideAllChild(RectTransformPlayerHpJianGroup);
	}

	public override void OnEnable()
	{
		base.OnEnable();
		UpdateHp(null);
		EventManager.AddListener(MessageType.Game, GameMessageType.UpdateProperty, UpdateHp);
	}

	public override void OnDisable()
	{
		base.OnDisable();
		EventManager.RemoveListener(MessageType.Game, GameMessageType.UpdateProperty, UpdateHp);
	}

	protected override void Pause()
	{
	}

	protected override void Play()
	{
	}

	private void UpdateHp(List<object> objects)
	{
		float num = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp);
		SliderPlayerHp.value = num / (float)SingletonAsMono<GameDataMrg>.Instance.PlayerHp;
		TextMeshProUGUIPlayerHpText.text = num.ToString("F0");
		if (objects != null && objects.Count > 2 && objects[2] is long && objects[1].ToString() == "TmpPlayerHp")
		{
			long num2 = (long)objects[2];
			_playerLoseData.Enqueue(num2.ToString() ?? "");
		}
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		UpdatePlayerLoseHp();
	}

	private void UpdatePlayerLoseHp()
	{
		if (_playerLoseData.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < RectTransformPlayerHpJianGroup.childCount; i++)
		{
			if (!RectTransformPlayerHpJianGroup.GetChild(i).gameObject.activeSelf)
			{
				TMP_Text tips = RectTransformPlayerHpJianGroup.GetChild(i).GetComponent<TMP_Text>();
				tips.SetActive(active: true);
				tips.text = _playerLoseData.Dequeue();
				Timer.DelayCall(1.5f, delegate
				{
					tips.SetActive(active: false);
				});
				break;
			}
		}
	}
}

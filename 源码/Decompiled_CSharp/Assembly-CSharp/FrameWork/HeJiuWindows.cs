using System.Collections.Generic;
using DG.Tweening;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "HeJiuWindows")]
[UiMode(Mode.Background, true)]
public class HeJiuWindows : UiActor
{
	private Tweener selfTweener;

	private Tweener targetTweener;

	public RectTransform RectTransformSelfZuijIu;

	public CanvasRenderer CanvasRendererSelfZuijIu;

	public TextMeshProUGUI TextMeshProUGUISelfZuijIu;

	public AddScripts AddScriptsSelfZuijIu;

	public RectTransform RectTransformSelfSlider;

	public Slider SliderSelfSlider;

	public AddScripts AddScriptsSelfSlider;

	public RectTransform RectTransformTargetSlider;

	public Slider SliderTargetSlider;

	public AddScripts AddScriptsTargetSlider;

	public override void Awake()
	{
		base.Awake();
		RectTransformSelfZuijIu = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfZuijIu/").GetComponent<RectTransform>();
		CanvasRendererSelfZuijIu = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfZuijIu/").GetComponent<CanvasRenderer>();
		TextMeshProUGUISelfZuijIu = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfZuijIu/").GetComponent<TextMeshProUGUI>();
		AddScriptsSelfZuijIu = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfZuijIu/").GetComponent<AddScripts>();
		RectTransformSelfSlider = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfSlider/").GetComponent<RectTransform>();
		SliderSelfSlider = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfSlider/").GetComponent<Slider>();
		AddScriptsSelfSlider = GetGameObject().transform.Find("Bg/Self/Image (1)/SelfSlider/").GetComponent<AddScripts>();
		RectTransformTargetSlider = GetGameObject().transform.Find("Bg/Target/Image (1)/TargetSlider/").GetComponent<RectTransform>();
		SliderTargetSlider = GetGameObject().transform.Find("Bg/Target/Image (1)/TargetSlider/").GetComponent<Slider>();
		AddScriptsTargetSlider = GetGameObject().transform.Find("Bg/Target/Image (1)/TargetSlider/").GetComponent<AddScripts>();
	}

	public HeJiuWindows(Transform trans)
		: base(trans)
	{
	}

	public HeJiuWindows()
	{
	}

	public override void Open(object[] objects)
	{
		base.Open(objects);
		Tool.ShowTutorial("HeJiu", new Sprite[1] { ABMrg.Load<Sprite>("HeJiu") });
		int tmpPlayerJiuLian = SingletonAsMono<GameDataMrg>.Instance.TmpPlayerJiuLian;
		TextMeshProUGUISelfZuijIu.text = tmpPlayerJiuLian + "/" + SingletonAsMono<GameDataMrg>.Instance.PlayerJiuLian;
		SliderSelfSlider.value = (float)tmpPlayerJiuLian / (float)SingletonAsMono<GameDataMrg>.Instance.PlayerJiuLian;
		SliderTargetSlider.value = (float)SingletonAsMono<GameDataMrg>.Instance.TmpTargetJiuLian / (float)SingletonAsMono<GameDataMrg>.Instance.TargetJiuLian;
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
		int tmpPlayerJiuLian = SingletonAsMono<GameDataMrg>.Instance.TmpPlayerJiuLian;
		TextMeshProUGUISelfZuijIu.text = tmpPlayerJiuLian + "/" + SingletonAsMono<GameDataMrg>.Instance.PlayerJiuLian;
		SliderSelfSlider.value = (float)tmpPlayerJiuLian / (float)SingletonAsMono<GameDataMrg>.Instance.PlayerJiuLian;
		SliderTargetSlider.value = (float)SingletonAsMono<GameDataMrg>.Instance.TmpTargetJiuLian / (float)SingletonAsMono<GameDataMrg>.Instance.TargetJiuLian;
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}

using System.Collections.Generic;
using DG.Tweening;
using Script.Audio;
using Script.Mrg;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[UiMode(Mode.Background, true)]
[ActorInfo("", "BaoKeMengWindows")]
public class BaoKeMengWindows : UiActor
{
	private AudioSource _audioSource;

	private Tweener _playerTweener;

	private Tweener _enemyTweener;

	public RectTransform RectTransformSelfHp;

	public CanvasRenderer CanvasRendererSelfHp;

	public Image ImageSelfHp;

	public AddScripts AddScriptsSelfHp;

	public RectTransform RectTransformTargetHp;

	public CanvasRenderer CanvasRendererTargetHp;

	public Image ImageTargetHp;

	public AddScripts AddScriptsTargetHp;

	public override void Awake()
	{
		base.Awake();
		RectTransformSelfHp = GetGameObject().transform.Find("ViewCombat/SelfHp/Ag/SelfHp/").GetComponent<RectTransform>();
		CanvasRendererSelfHp = GetGameObject().transform.Find("ViewCombat/SelfHp/Ag/SelfHp/").GetComponent<CanvasRenderer>();
		ImageSelfHp = GetGameObject().transform.Find("ViewCombat/SelfHp/Ag/SelfHp/").GetComponent<Image>();
		AddScriptsSelfHp = GetGameObject().transform.Find("ViewCombat/SelfHp/Ag/SelfHp/").GetComponent<AddScripts>();
		RectTransformTargetHp = GetGameObject().transform.Find("ViewCombat/TargetHp/Ag/TargetHp/").GetComponent<RectTransform>();
		CanvasRendererTargetHp = GetGameObject().transform.Find("ViewCombat/TargetHp/Ag/TargetHp/").GetComponent<CanvasRenderer>();
		ImageTargetHp = GetGameObject().transform.Find("ViewCombat/TargetHp/Ag/TargetHp/").GetComponent<Image>();
		AddScriptsTargetHp = GetGameObject().transform.Find("ViewCombat/TargetHp/Ag/TargetHp/").GetComponent<AddScripts>();
	}

	public BaoKeMengWindows(Transform trans)
		: base(trans)
	{
	}

	public BaoKeMengWindows()
	{
	}

	public override void OnEnable()
	{
		base.OnEnable();
		_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.baoKeMenZhanDou, loop: true);
		EventManager.AddListener(MessageType.Game, GameMessageType.UpdateProperty, UpdateHp);
		InitHp();
	}

	public override void OnDisable()
	{
		base.OnDisable();
		SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
		EventManager.RemoveListener(MessageType.Game, GameMessageType.UpdateProperty, UpdateHp);
	}

	protected override void Pause()
	{
	}

	protected override void Play()
	{
	}

	private void InitHp()
	{
		float num = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp);
		ImageSelfHp.fillAmount = num / (float)SingletonAsMono<GameDataMrg>.Instance.PlayerHp;
		float num2 = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.EnemyHp);
		ImageTargetHp.fillAmount = num2 / (float)SingletonAsMono<GameDataMrg>.Instance.EnemyHpMax;
	}

	private void UpdateHp(List<object> objects)
	{
		_playerTweener?.Kill();
		_enemyTweener.Kill();
		float num = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp);
		_playerTweener = ImageSelfHp.DOFillAmount(num / (float)SingletonAsMono<GameDataMrg>.Instance.PlayerHp, 0.2f);
		float num2 = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.EnemyHp);
		_enemyTweener = ImageTargetHp.DOFillAmount(num2 / (float)SingletonAsMono<GameDataMrg>.Instance.EnemyHpMax, 0.2f);
	}

	public override void Open(object[] objects)
	{
	}

	public override void CloseUi()
	{
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			RemoveUi(GetIndex());
		}
	}
}

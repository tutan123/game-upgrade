using System.Collections.Generic;
using DG.Tweening;
using Script.Audio;
using Script.Mrg;
using Script.Scene;
using Script.Tool;
using Script.UiTool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "CombatWindows")]
[UiMode(Mode.Background, true)]
public class CombatWindows : UiActor
{
	private AudioSource _audioSource;

	private Dictionary<string, VideoNode> _videoNodes = new Dictionary<string, VideoNode>();

	private Dictionary<string, ButtonNode> _buttonNodes = new Dictionary<string, ButtonNode>();

	private Queue<string> _enemyLoseData = new Queue<string>();

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

	public RectTransform RectTransformSkillPoint;

	public HorizontalLayoutGroup HorizontalLayoutGroupSkillPoint;

	public AddScripts AddScriptsSkillPoint;

	public RectTransform RectTransformEnemyHp;

	public Slider SliderEnemyHp;

	public AddScripts AddScriptsEnemyHp;

	public RectTransform RectTransformEnemyHpText;

	public CanvasRenderer CanvasRendererEnemyHpText;

	public TextMeshProUGUI TextMeshProUGUIEnemyHpText;

	public AddScripts AddScriptsEnemyHpText;

	public RectTransform RectTransformEnemName;

	public CanvasRenderer CanvasRendererEnemName;

	public TextMeshProUGUI TextMeshProUGUIEnemName;

	public AddScripts AddScriptsEnemName;

	public RectTransform RectTransformBtnGroup;

	public Animator AnimatorBtnGroup;

	public CanvasGroup CanvasGroupBtnGroup;

	public AddScripts AddScriptsBtnGroup;

	public RectTransform RectTransformPuTon;

	public CanvasRenderer CanvasRendererPuTon;

	public TextMeshProUGUI TextMeshProUGUIPuTon;

	public BT BTPuTon;

	public AddScripts AddScriptsPuTon;

	public Shank ShankPuTon;

	public CheckTextWidth CheckTextWidthPuTon;

	public RectTransform RectTransformPuTonPoint;

	public CanvasRenderer CanvasRendererPuTonPoint;

	public TextMeshProUGUI TextMeshProUGUIPuTonPoint;

	public AddScripts AddScriptsPuTonPoint;

	public RectTransform RectTransformJinSheng;

	public CanvasRenderer CanvasRendererJinSheng;

	public TextMeshProUGUI TextMeshProUGUIJinSheng;

	public BT BTJinSheng;

	public AddScripts AddScriptsJinSheng;

	public Shank ShankJinSheng;

	public CheckTextWidth CheckTextWidthJinSheng;

	public RectTransform RectTransformJinShengPoint;

	public CanvasRenderer CanvasRendererJinShengPoint;

	public TextMeshProUGUI TextMeshProUGUIJinShengPoint;

	public AddScripts AddScriptsJinShengPoint;

	public RectTransform RectTransformZhuHe;

	public CanvasRenderer CanvasRendererZhuHe;

	public TextMeshProUGUI TextMeshProUGUIZhuHe;

	public BT BTZhuHe;

	public AddScripts AddScriptsZhuHe;

	public Shank ShankZhuHe;

	public CheckTextWidth CheckTextWidthZhuHe;

	public RectTransform RectTransformZhuHePoint;

	public CanvasRenderer CanvasRendererZhuHePoint;

	public TextMeshProUGUI TextMeshProUGUIZhuHePoint;

	public AddScripts AddScriptsZhuHePoint;

	public RectTransform RectTransformJunTi;

	public CanvasRenderer CanvasRendererJunTi;

	public TextMeshProUGUI TextMeshProUGUIJunTi;

	public BT BTJunTi;

	public AddScripts AddScriptsJunTi;

	public Shank ShankJunTi;

	public CheckTextWidth CheckTextWidthJunTi;

	public RectTransform RectTransformJunTiPoint;

	public CanvasRenderer CanvasRendererJunTiPoint;

	public TextMeshProUGUI TextMeshProUGUIJunTiPoint;

	public AddScripts AddScriptsJunTiPoint;

	public RectTransform RectTransformRuLaiShengZhang;

	public CanvasRenderer CanvasRendererRuLaiShengZhang;

	public TextMeshProUGUI TextMeshProUGUIRuLaiShengZhang;

	public BT BTRuLaiShengZhang;

	public AddScripts AddScriptsRuLaiShengZhang;

	public Shank ShankRuLaiShengZhang;

	public CheckTextWidth CheckTextWidthRuLaiShengZhang;

	public RectTransform RectTransformRuLaiShengZhangPoint;

	public CanvasRenderer CanvasRendererRuLaiShengZhangPoint;

	public TextMeshProUGUI TextMeshProUGUIRuLaiShengZhangPoint;

	public AddScripts AddScriptsRuLaiShengZhangPoint;

	public RectTransform RectTransformXiXueGuiHuiFu;

	public CanvasRenderer CanvasRendererXiXueGuiHuiFu;

	public TextMeshProUGUI TextMeshProUGUIXiXueGuiHuiFu;

	public BT BTXiXueGuiHuiFu;

	public AddScripts AddScriptsXiXueGuiHuiFu;

	public Shank ShankXiXueGuiHuiFu;

	public CheckTextWidth CheckTextWidthXiXueGuiHuiFu;

	public RectTransform RectTransformXiXueGuiHuiFuPoint;

	public CanvasRenderer CanvasRendererXiXueGuiHuiFuPoint;

	public TextMeshProUGUI TextMeshProUGUIXiXueGuiHuiFuPoint;

	public AddScripts AddScriptsXiXueGuiHuiFuPoint;

	public RectTransform RectTransformYonCun;

	public CanvasRenderer CanvasRendererYonCun;

	public TextMeshProUGUI TextMeshProUGUIYonCun;

	public BT BTYonCun;

	public AddScripts AddScriptsYonCun;

	public Shank ShankYonCun;

	public CheckTextWidth CheckTextWidthYonCun;

	public RectTransform RectTransformYonCunPoint;

	public CanvasRenderer CanvasRendererYonCunPoint;

	public TextMeshProUGUI TextMeshProUGUIYonCunPoint;

	public AddScripts AddScriptsYonCunPoint;

	public RectTransform RectTransformEnemyHpJianGroup;

	public AddScripts AddScriptsEnemyHpJianGroup;

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
		RectTransformSkillPoint = GetGameObject().transform.Find("PlayerHp/SkillPoint/").GetComponent<RectTransform>();
		HorizontalLayoutGroupSkillPoint = GetGameObject().transform.Find("PlayerHp/SkillPoint/").GetComponent<HorizontalLayoutGroup>();
		AddScriptsSkillPoint = GetGameObject().transform.Find("PlayerHp/SkillPoint/").GetComponent<AddScripts>();
		RectTransformEnemyHp = GetGameObject().transform.Find("EnemyHp/").GetComponent<RectTransform>();
		SliderEnemyHp = GetGameObject().transform.Find("EnemyHp/").GetComponent<Slider>();
		AddScriptsEnemyHp = GetGameObject().transform.Find("EnemyHp/").GetComponent<AddScripts>();
		RectTransformEnemyHpText = GetGameObject().transform.Find("EnemyHp/EnemyHpText/").GetComponent<RectTransform>();
		CanvasRendererEnemyHpText = GetGameObject().transform.Find("EnemyHp/EnemyHpText/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIEnemyHpText = GetGameObject().transform.Find("EnemyHp/EnemyHpText/").GetComponent<TextMeshProUGUI>();
		AddScriptsEnemyHpText = GetGameObject().transform.Find("EnemyHp/EnemyHpText/").GetComponent<AddScripts>();
		RectTransformEnemName = GetGameObject().transform.Find("EnemyHp/EnemName/").GetComponent<RectTransform>();
		CanvasRendererEnemName = GetGameObject().transform.Find("EnemyHp/EnemName/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIEnemName = GetGameObject().transform.Find("EnemyHp/EnemName/").GetComponent<TextMeshProUGUI>();
		AddScriptsEnemName = GetGameObject().transform.Find("EnemyHp/EnemName/").GetComponent<AddScripts>();
		RectTransformBtnGroup = GetGameObject().transform.Find("BtnGroup/").GetComponent<RectTransform>();
		AnimatorBtnGroup = GetGameObject().transform.Find("BtnGroup/").GetComponent<Animator>();
		CanvasGroupBtnGroup = GetGameObject().transform.Find("BtnGroup/").GetComponent<CanvasGroup>();
		AddScriptsBtnGroup = GetGameObject().transform.Find("BtnGroup/").GetComponent<AddScripts>();
		RectTransformPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<RectTransform>();
		CanvasRendererPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<TextMeshProUGUI>();
		BTPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<BT>();
		AddScriptsPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<AddScripts>();
		ShankPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<Shank>();
		CheckTextWidthPuTon = GetGameObject().transform.Find("BtnGroup/PuTon/").GetComponent<CheckTextWidth>();
		RectTransformPuTonPoint = GetGameObject().transform.Find("BtnGroup/PuTon/PuTonPoint/").GetComponent<RectTransform>();
		CanvasRendererPuTonPoint = GetGameObject().transform.Find("BtnGroup/PuTon/PuTonPoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIPuTonPoint = GetGameObject().transform.Find("BtnGroup/PuTon/PuTonPoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsPuTonPoint = GetGameObject().transform.Find("BtnGroup/PuTon/PuTonPoint/").GetComponent<AddScripts>();
		RectTransformJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<RectTransform>();
		CanvasRendererJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<TextMeshProUGUI>();
		BTJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<BT>();
		AddScriptsJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<AddScripts>();
		ShankJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<Shank>();
		CheckTextWidthJinSheng = GetGameObject().transform.Find("BtnGroup/JinSheng/").GetComponent<CheckTextWidth>();
		RectTransformJinShengPoint = GetGameObject().transform.Find("BtnGroup/JinSheng/JinShengPoint/").GetComponent<RectTransform>();
		CanvasRendererJinShengPoint = GetGameObject().transform.Find("BtnGroup/JinSheng/JinShengPoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIJinShengPoint = GetGameObject().transform.Find("BtnGroup/JinSheng/JinShengPoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsJinShengPoint = GetGameObject().transform.Find("BtnGroup/JinSheng/JinShengPoint/").GetComponent<AddScripts>();
		RectTransformZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<RectTransform>();
		CanvasRendererZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<TextMeshProUGUI>();
		BTZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<BT>();
		AddScriptsZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<AddScripts>();
		ShankZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<Shank>();
		CheckTextWidthZhuHe = GetGameObject().transform.Find("BtnGroup/ZhuHe/").GetComponent<CheckTextWidth>();
		RectTransformZhuHePoint = GetGameObject().transform.Find("BtnGroup/ZhuHe/ZhuHePoint/").GetComponent<RectTransform>();
		CanvasRendererZhuHePoint = GetGameObject().transform.Find("BtnGroup/ZhuHe/ZhuHePoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIZhuHePoint = GetGameObject().transform.Find("BtnGroup/ZhuHe/ZhuHePoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsZhuHePoint = GetGameObject().transform.Find("BtnGroup/ZhuHe/ZhuHePoint/").GetComponent<AddScripts>();
		RectTransformJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<RectTransform>();
		CanvasRendererJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<TextMeshProUGUI>();
		BTJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<BT>();
		AddScriptsJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<AddScripts>();
		ShankJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<Shank>();
		CheckTextWidthJunTi = GetGameObject().transform.Find("BtnGroup/JunTi/").GetComponent<CheckTextWidth>();
		RectTransformJunTiPoint = GetGameObject().transform.Find("BtnGroup/JunTi/JunTiPoint/").GetComponent<RectTransform>();
		CanvasRendererJunTiPoint = GetGameObject().transform.Find("BtnGroup/JunTi/JunTiPoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIJunTiPoint = GetGameObject().transform.Find("BtnGroup/JunTi/JunTiPoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsJunTiPoint = GetGameObject().transform.Find("BtnGroup/JunTi/JunTiPoint/").GetComponent<AddScripts>();
		RectTransformRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<RectTransform>();
		CanvasRendererRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<TextMeshProUGUI>();
		BTRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<BT>();
		AddScriptsRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<AddScripts>();
		ShankRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<Shank>();
		CheckTextWidthRuLaiShengZhang = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/").GetComponent<CheckTextWidth>();
		RectTransformRuLaiShengZhangPoint = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/RuLaiShengZhangPoint/").GetComponent<RectTransform>();
		CanvasRendererRuLaiShengZhangPoint = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/RuLaiShengZhangPoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIRuLaiShengZhangPoint = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/RuLaiShengZhangPoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsRuLaiShengZhangPoint = GetGameObject().transform.Find("BtnGroup/RuLaiShengZhang/RuLaiShengZhangPoint/").GetComponent<AddScripts>();
		RectTransformXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<RectTransform>();
		CanvasRendererXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<TextMeshProUGUI>();
		BTXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<BT>();
		AddScriptsXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<AddScripts>();
		ShankXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<Shank>();
		CheckTextWidthXiXueGuiHuiFu = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/").GetComponent<CheckTextWidth>();
		RectTransformXiXueGuiHuiFuPoint = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/XiXueGuiHuiFuPoint/").GetComponent<RectTransform>();
		CanvasRendererXiXueGuiHuiFuPoint = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/XiXueGuiHuiFuPoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIXiXueGuiHuiFuPoint = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/XiXueGuiHuiFuPoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsXiXueGuiHuiFuPoint = GetGameObject().transform.Find("BtnGroup/XiXueGuiHuiFu/XiXueGuiHuiFuPoint/").GetComponent<AddScripts>();
		RectTransformYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<RectTransform>();
		CanvasRendererYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<TextMeshProUGUI>();
		BTYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<BT>();
		AddScriptsYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<AddScripts>();
		ShankYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<Shank>();
		CheckTextWidthYonCun = GetGameObject().transform.Find("BtnGroup/YonCun/").GetComponent<CheckTextWidth>();
		RectTransformYonCunPoint = GetGameObject().transform.Find("BtnGroup/YonCun/YonCunPoint/").GetComponent<RectTransform>();
		CanvasRendererYonCunPoint = GetGameObject().transform.Find("BtnGroup/YonCun/YonCunPoint/").GetComponent<CanvasRenderer>();
		TextMeshProUGUIYonCunPoint = GetGameObject().transform.Find("BtnGroup/YonCun/YonCunPoint/").GetComponent<TextMeshProUGUI>();
		AddScriptsYonCunPoint = GetGameObject().transform.Find("BtnGroup/YonCun/YonCunPoint/").GetComponent<AddScripts>();
		RectTransformEnemyHpJianGroup = GetGameObject().transform.Find("EnemyHpJianGroup/").GetComponent<RectTransform>();
		AddScriptsEnemyHpJianGroup = GetGameObject().transform.Find("EnemyHpJianGroup/").GetComponent<AddScripts>();
		RectTransformPlayerHpJianGroup = GetGameObject().transform.Find("PlayerHpJianGroup/").GetComponent<RectTransform>();
		AddScriptsPlayerHpJianGroup = GetGameObject().transform.Find("PlayerHpJianGroup/").GetComponent<AddScripts>();
	}

	public CombatWindows(Transform trans)
		: base(trans)
	{
	}

	public CombatWindows()
	{
	}

	public override void Start()
	{
		base.Start();
		BTPuTon.onClick.AddListener(delegate
		{
			OnClickBtn(BTPuTon);
		});
		BTJinSheng.onClick.AddListener(delegate
		{
			OnClickBtn(BTJinSheng);
		});
		BTZhuHe.onClick.AddListener(delegate
		{
			OnClickBtn(BTZhuHe);
		});
		BTJunTi.onClick.AddListener(delegate
		{
			OnClickBtn(BTJunTi);
		});
		BTRuLaiShengZhang.onClick.AddListener(delegate
		{
			OnClickBtn(BTRuLaiShengZhang);
		});
		BTXiXueGuiHuiFu.onClick.AddListener(delegate
		{
			OnClickBtn(BTXiXueGuiHuiFu);
		});
		BTYonCun.onClick.AddListener(delegate
		{
			OnClickBtn(BTYonCun);
		});
	}

	protected override void Pause()
	{
	}

	protected override void Play()
	{
	}

	public void OnClickBtn(BT bt)
	{
		VideoNode videoNode = null;
		ButtonNode buttonNode = null;
		if (bt == BTPuTon)
		{
			videoNode = _videoNodes["PuTon"];
			buttonNode = _buttonNodes["PuTon"];
		}
		else if (bt == BTJinSheng)
		{
			videoNode = _videoNodes["JinShengGonJi"];
			buttonNode = _buttonNodes["JinShengGonJi"];
		}
		else if (bt == BTZhuHe)
		{
			videoNode = _videoNodes["ZhuHeQuan"];
			buttonNode = _buttonNodes["ZhuHeQuan"];
		}
		else if (bt == BTJunTi)
		{
			videoNode = _videoNodes["JunTiQuan"];
			buttonNode = _buttonNodes["JunTiQuan"];
		}
		else if (bt == BTRuLaiShengZhang)
		{
			videoNode = _videoNodes["RuLaiShengZhang"];
			buttonNode = _buttonNodes["RuLaiShengZhang"];
		}
		else if (bt == BTXiXueGuiHuiFu)
		{
			videoNode = _videoNodes["XiXueGuiHuiXue"];
			buttonNode = _buttonNodes["XiXueGuiHuiXue"];
		}
		else if (bt == BTYonCun)
		{
			videoNode = _videoNodes["YonChun"];
			buttonNode = _buttonNodes["YonChun"];
		}
		if (!(videoNode != null))
		{
			return;
		}
		if (!buttonNode.usePropertyDatas.IsSuc())
		{
			if (buttonNode.usePropertyName != 0)
			{
				UiManager.ShowTips(LanguageMrg.GetText(buttonNode.usePropertyName));
			}
			else
			{
				UiManager.ShowTips(LanguageMrg.GetText("A1211"));
			}
		}
		else
		{
			RectTransformBtnGroup.SetActive(active: false);
			GameScene.Instance.PlayVideo(videoNode);
		}
	}

	private void InitPoint()
	{
		TextMeshProUGUIPuTonPoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.PuTon, PropertyTypeValue.TmpJiNengDian, out var count));
		TextMeshProUGUIPuTonPoint.text = "<sprite name=ZJD>" + count;
		TextMeshProUGUIJinShengPoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.JinShengGonJi, PropertyTypeValue.TmpJiNengDian, out var count2));
		TextMeshProUGUIJinShengPoint.text = "<sprite name=ZJD>" + count2;
		TextMeshProUGUIZhuHePoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.ZhuHeQuan, PropertyTypeValue.TmpJiNengDian, out var count3));
		TextMeshProUGUIZhuHePoint.text = "<sprite name=ZJD>" + count3;
		TextMeshProUGUIJunTiPoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.JunTiQuan, PropertyTypeValue.TmpJiNengDian, out var count4));
		TextMeshProUGUIJunTiPoint.text = "<sprite name=ZJD>" + count4;
		TextMeshProUGUIRuLaiShengZhangPoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.RuLaiShengZhang, PropertyTypeValue.TmpJiNengDian, out var count5));
		TextMeshProUGUIRuLaiShengZhangPoint.text = "<sprite name=ZJD>" + count5;
		TextMeshProUGUIXiXueGuiHuiFuPoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.XiXueGuiHuiXue, PropertyTypeValue.TmpJiNengDian, out var count6));
		TextMeshProUGUIXiXueGuiHuiFuPoint.text = "<sprite name=ZJD>" + count6;
		TextMeshProUGUIYonCunPoint.SetActive(_videoNodes.IsHasPropertyType(PropertyTypeValue.YonChun, PropertyTypeValue.TmpJiNengDian, out var count7));
		TextMeshProUGUIYonCunPoint.text = "<sprite name=ZJD>" + count7;
	}

	private void InitSkillPoint()
	{
		int property = SingletonAsMono<GameDataMrg>.Instance.GetProperty("TmpJiNengDian", "Property", 0L);
		Debug.Log(property);
		RectTransformSkillPoint.TranFor(property, RectTransformSkillPoint.GetChild(0), delegate
		{
		});
	}

	public void Init(Dictionary<string, VideoNode> videoNodes, Dictionary<string, ButtonNode> buttonNodes)
	{
		_buttonNodes = buttonNodes;
		_videoNodes = videoNodes;
		BTPuTon.SetActive(videoNodes.ContainsKey("PuTon"));
		BTJinSheng.SetActive(videoNodes.ContainsKey("JinShengGonJi"));
		BTZhuHe.SetActive(videoNodes.ContainsKey("ZhuHeQuan") && SingletonAsMono<GameDataMrg>.Instance.GetProperty("ZhuHeQuan", "Property", 0L) > 0);
		BTJunTi.SetActive(videoNodes.ContainsKey("JunTiQuan") && SingletonAsMono<GameDataMrg>.Instance.GetProperty("JunTiQuan", "Property", 0L) > 0);
		BTRuLaiShengZhang.SetActive(videoNodes.ContainsKey("RuLaiShengZhang") && SingletonAsMono<GameDataMrg>.Instance.GetProperty("RuLaiShengZhang", "Property", 0L) > 0);
		BTXiXueGuiHuiFu.SetActive(videoNodes.ContainsKey("XiXueGuiHuiXue") && SingletonAsMono<GameDataMrg>.Instance.GetProperty("XiXueGuiHuiXue", "Property", 0L) > 0);
		BTYonCun.SetActive(videoNodes.ContainsKey("YonChun") && SingletonAsMono<GameDataMrg>.Instance.GetProperty("YonChun", "Property", 0L) > 0);
		RectTransformBtnGroup.SetActive(active: true);
		InitPoint();
		InitSkillPoint();
		SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.skillShow);
	}

	private void UpdateEnemyLoseHp()
	{
		if (_enemyLoseData.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < _videoNodes.Count; i++)
		{
			if (!RectTransformEnemyHpJianGroup.GetChild(i).gameObject.activeSelf)
			{
				TMP_Text tips = RectTransformEnemyHpJianGroup.GetChild(i).GetComponent<TMP_Text>();
				tips.SetActive(active: true);
				tips.text = _enemyLoseData.Dequeue();
				Timer.DelayCall(1.5f, delegate
				{
					tips.SetActive(active: false);
				});
				break;
			}
		}
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

	public override void Open(object[] objects)
	{
		if (_audioSource != null)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
		_audioSource = SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.combot, loop: true);
		RectTransformBtnGroup.SetActive(active: false);
		if ((bool)GameScene.Instance.videoItem && GameScene.Instance.videoItem.GetNode().isInitHp)
		{
			TextMeshProUGUIEnemName.text = LanguageMrg.GetText(GameScene.Instance.videoItem.GetNode().bossName);
		}
		Tool.HideAllChild(RectTransformEnemyHpJianGroup);
		Tool.HideAllChild(RectTransformPlayerHpJianGroup);
		_enemyLoseData.Clear();
		_playerLoseData.Clear();
	}

	public override void OnClose()
	{
		base.OnClose();
		SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
		_audioSource = null;
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
		if (_audioSource != null)
		{
			SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
			_audioSource = null;
		}
	}

	private void UpdateHp(List<object> objects)
	{
		float num = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.TmpPlayerHp);
		SliderPlayerHp.value = num / (float)SingletonAsMono<GameDataMrg>.Instance.PlayerHp;
		TextMeshProUGUIPlayerHpText.text = num.ToString("F0");
		float num2 = Mathf.Max(0, SingletonAsMono<GameDataMrg>.Instance.EnemyHp);
		SliderEnemyHp.value = num2 / (float)SingletonAsMono<GameDataMrg>.Instance.EnemyHpMax;
		TextMeshProUGUIEnemyHpText.text = num2.ToString("F0");
		if (objects != null && objects.Count > 2 && objects[2] is long)
		{
			string text = objects[1].ToString();
			long num3 = (long)objects[2];
			if (num3 < 0 && text == "EnemyHp")
			{
				_enemyLoseData.Enqueue(num3.ToString() ?? "");
			}
			if (text == "TmpPlayerHp")
			{
				_playerLoseData.Enqueue(num3.ToString() ?? "");
			}
		}
		InitSkillPoint();
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		UpdateEnemyLoseHp();
		UpdatePlayerLoseHp();
	}

	protected override void RemoveUi(List<object> parma)
	{
		if (parma[1] as string == "load")
		{
			if (_audioSource != null)
			{
				SingletonAsMono<AudioMrg>.Instance.Enqueue(_audioSource);
				_audioSource = null;
			}
			RemoveUi(GetIndex());
		}
	}
}

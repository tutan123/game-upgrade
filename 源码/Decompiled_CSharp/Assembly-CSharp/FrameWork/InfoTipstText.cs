using System;
using System.Collections.Generic;
using Script.Audio;
using Script.Mrg;
using Script.Tool;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FrameWork;

[ActorInfo("", "InfoTipstText")]
[UiMode(Mode.Popup, false)]
public class InfoTipstText : UiActor
{
	private Queue<TipsData> _queue = new Queue<TipsData>();

	public RectTransform RectTransformInfoTipstText;

	public AddScripts AddScriptsInfoTipstText;

	public RectTransform RectTransformView;

	public CanvasRenderer CanvasRendererView;

	public Image ImageView;

	public VerticalLayoutGroup VerticalLayoutGroupView;

	public AddScripts AddScriptsView;

	public override void Awake()
	{
		base.Awake();
		RectTransformInfoTipstText = GetGameObject().transform.GetComponent<RectTransform>();
		AddScriptsInfoTipstText = GetGameObject().transform.GetComponent<AddScripts>();
		RectTransformView = GetGameObject().transform.Find("View/").GetComponent<RectTransform>();
		CanvasRendererView = GetGameObject().transform.Find("View/").GetComponent<CanvasRenderer>();
		ImageView = GetGameObject().transform.Find("View/").GetComponent<Image>();
		VerticalLayoutGroupView = GetGameObject().transform.Find("View/").GetComponent<VerticalLayoutGroup>();
		AddScriptsView = GetGameObject().transform.Find("View/").GetComponent<AddScripts>();
	}

	public InfoTipstText(Transform trans)
		: base(trans)
	{
	}

	public InfoTipstText()
	{
	}

	public override void Start()
	{
		base.Start();
		Tool.HideAllChild(RectTransformView);
	}

	public void AddTips(string message, bool isAdd = false, string type = "nor")
	{
		_queue.Enqueue(new TipsData
		{
			Type = type,
			Text = message,
			IsAdd = isAdd
		});
	}

	public override void Update(float deltaTime)
	{
		base.Update(deltaTime);
		if (_queue.Count <= 0)
		{
			return;
		}
		for (int i = 0; i < RectTransformView.childCount; i++)
		{
			if (!RectTransformView.GetChild(i).gameObject.activeSelf)
			{
				ShowTips(RectTransformView.GetChild(i), _queue.Dequeue());
				break;
			}
		}
	}

	protected override void RemoveUi(List<object> parma)
	{
	}

	private void ShowTips(Transform tips, TipsData data)
	{
		tips.SetActive(active: true);
		Tool.HideAllChild(tips.GetChild(0).GetChild(0));
		tips.GetChild(1).GetChild(0).GetComponent<TMP_Text>()
			.text = data.Text;
		tips.GetChild(0).GetChild(0).GetComponent<UpDatePos>()
			.Init(data.Text);
		Transform transform = null;
		if (Enum.TryParse<PropertyTypeValue>(data.Type, out var result))
		{
			switch (result)
			{
			case PropertyTypeValue.Stamina:
				if (data.IsAdd)
				{
					transform = tips.GetChild(0).GetChild(0).Find("TiLiJia");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJia);
				}
				else
				{
					transform = tips.GetChild(0).GetChild(0).Find("TiLiJian");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJian);
				}
				break;
			case PropertyTypeValue.Wisdom:
				if (data.IsAdd)
				{
					transform = tips.GetChild(0).GetChild(0).Find("ZhiLiJia");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJia);
				}
				else
				{
					transform = tips.GetChild(0).GetChild(0).Find("ZhiLiJian");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJian);
				}
				break;
			case PropertyTypeValue.Charm:
				if (data.IsAdd)
				{
					transform = tips.GetChild(0).GetChild(0).Find("MeiLiJia");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJia);
				}
				else
				{
					transform = tips.GetChild(0).GetChild(0).Find("MeiLiJian");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJian);
				}
				break;
			case PropertyTypeValue.Morality:
				if (data.IsAdd)
				{
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJia);
				}
				else
				{
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJian);
				}
				transform = tips.GetChild(0).GetChild(0).Find("DaoDe");
				break;
			case PropertyTypeValue.CapacityForLiquor:
				if (data.IsAdd)
				{
					transform = tips.GetChild(0).GetChild(0).Find("JiuLianJia");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJia);
				}
				else
				{
					transform = tips.GetChild(0).GetChild(0).Find("JiuLianJian");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJian);
				}
				break;
			case PropertyTypeValue.Money:
				if (data.IsAdd)
				{
					transform = tips.GetChild(0).GetChild(0).Find("QianJia");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.shuXinBianHua);
				}
				else
				{
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.qian);
					transform = tips.GetChild(0).GetChild(0).Find("QianJian");
				}
				break;
			case PropertyTypeValue.Pressure:
				if (data.IsAdd)
				{
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJian);
					transform = tips.GetChild(0).GetChild(0).Find("YaLiJia");
				}
				else
				{
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.zhuYaoJia);
					transform = tips.GetChild(0).GetChild(0).Find("YaLiJia");
				}
				break;
			case PropertyTypeValue.BZPFavorability:
			case PropertyTypeValue.WDLYFavorability:
			case PropertyTypeValue.XFPFavorability:
			case PropertyTypeValue.XXGFavorability:
			case PropertyTypeValue.LQTFavorability:
			case PropertyTypeValue.MMFavorability:
			case PropertyTypeValue.LBNFavorability:
			case PropertyTypeValue.ZXSNFavorability:
			case PropertyTypeValue.DBNFavorability:
			case PropertyTypeValue.LJFavorability:
			case PropertyTypeValue.XJMFavorability:
			case PropertyTypeValue.NAMFavorability:
			case PropertyTypeValue.MSFavorability:
			case PropertyTypeValue.PZFavorability:
			case PropertyTypeValue.TSGSNFavorability:
			case PropertyTypeValue.XXSNFavorability:
			case PropertyTypeValue.WYFavorability:
			case PropertyTypeValue.LXYavorability:
				if (data.IsAdd)
				{
					transform = tips.GetChild(0).GetChild(0).Find("HaoGanJia");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.haoGanDuJia);
				}
				else
				{
					transform = tips.GetChild(0).GetChild(0).Find("HaoGanJian");
					SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.haoGanDuJian);
				}
				break;
			case PropertyTypeValue.Execution:
				transform = ((!data.IsAdd) ? tips.GetChild(0).GetChild(0).Find("XinDonLiJia") : tips.GetChild(0).GetChild(0).Find("XinDonLiShangXianJia"));
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.shuXinBianHua);
				break;
			default:
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.shuXinBianHua);
				break;
			}
		}
		else
		{
			SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.shuXinBianHua);
		}
		tips.GetChild(0).SetActive(transform != null);
		if (transform != null)
		{
			transform.SetActive(active: true);
		}
		Timer.DelayCall(3f, delegate
		{
			tips.SetActive(active: false);
		});
	}
}

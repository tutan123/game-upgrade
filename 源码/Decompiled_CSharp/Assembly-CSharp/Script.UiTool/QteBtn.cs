using System;
using System.Collections.Generic;
using DG.Tweening;
using FrameWork;
using Script.Audio;
using Script.Mrg;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Script.UiTool;

public class QteBtn : MonoBehaviour
{
	public QteType qteType;

	public Image ag;

	public TMP_Text keyText;

	public Image target;

	public List<Sprite> imgs;

	private Action _action;

	private Action _lose;

	private float _showTime;

	private float _curTime;

	private bool _isInit;

	private QteData _data;

	private Tweener _moveTweener;

	private KeyCode _keyCode;

	private int _clickCount;

	public void Init(QteData qteData, Action suc = null, Action lose = null)
	{
		_clickCount = 0;
		_data = qteData;
		qteType = qteData.qteType;
		_showTime = _data.qteEndTime - _data.qteStartTime;
		_curTime = 0f;
		_action = suc;
		_isInit = true;
		_lose = lose;
		_action = (Action)Delegate.Combine(_action, (Action)delegate
		{
			try
			{
				UnityEngine.Object.Instantiate(ABMrg.Load<GameObject>(qteType.ToString() + "Suc"), base.transform.parent).transform.localPosition = base.transform.localPosition;
			}
			catch (Exception)
			{
				MyLog.Log("预制体不存在");
			}
			if (qteType != 0 && qteType != QteType.NoneFailGo)
			{
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.qteSuc);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		});
		_lose = (Action)Delegate.Combine(_lose, (Action)delegate
		{
			try
			{
				UnityEngine.Object.Instantiate(ABMrg.Load<GameObject>(qteType.ToString() + "Fail"), base.transform.parent).transform.localPosition = base.transform.localPosition;
			}
			catch (Exception)
			{
				MyLog.Log("预制体不存在");
			}
			if (qteType != 0 && qteType != QteType.NoneFailGo)
			{
				SingletonAsMono<AudioMrg>.Instance.Play(AudioDataClip.AudioData.qteFail);
			}
			UnityEngine.Object.Destroy(base.gameObject);
		});
		if (qteData.qteType == QteType.SlideGo)
		{
			_moveTweener = ag.transform.DOLocalMoveX(350f, _data.qteEndTime - _data.qteStartTime).SetEase(Ease.Linear);
			Tweener moveTweener = _moveTweener;
			moveTweener.onComplete = (TweenCallback)Delegate.Combine(moveTweener.onComplete, (TweenCallback)delegate
			{
				lose?.Invoke();
			});
		}
		if (qteData.qteType == QteType.KeyGo)
		{
			List<KeyCode> list = new List<KeyCode>
			{
				KeyCode.UpArrow,
				KeyCode.DownArrow,
				KeyCode.LeftArrow,
				KeyCode.RightArrow
			};
			int index = UnityEngine.Random.Range(0, list.Count);
			KeyCode keyCode = list[index];
			_keyCode = keyCode;
			keyText.text = _keyCode.ToString();
			target.sprite = imgs[index];
		}
	}

	private void Update()
	{
		if (_isInit)
		{
			switch (qteType)
			{
			case QteType.CondensationGo:
				UpdateCondensation();
				break;
			case QteType.KeyGo:
				UpdateKeyGo();
				break;
			case QteType.SlideGo:
				UpdateSlideGo();
				break;
			case QteType.NoneGo:
				CheckNone();
				break;
			case QteType.ClickGo:
				UpdateClickGo();
				break;
			case QteType.NoneFailGo:
				CheckNoneFail();
				break;
			}
		}
	}

	private void UpdateClickGo()
	{
		_curTime += Time.deltaTime;
		ag.fillAmount = 1f - _curTime / _showTime;
		keyText.text = _clickCount + "/" + _data.clickCount;
		if (_curTime >= _showTime)
		{
			_isInit = false;
			_lose?.Invoke();
		}
		CheckClickSpace();
	}

	private void CheckClickSpace()
	{
		if (qteType == QteType.ClickGo && Input.GetKeyDown(KeyCode.Space))
		{
			CheckClick();
		}
	}

	private void CheckClick()
	{
		_clickCount++;
		if (_clickCount >= _data.clickCount)
		{
			_data.isSucEx = true;
			_action?.Invoke();
		}
	}

	private void UpdateKeyGo()
	{
		_curTime += Time.deltaTime;
		ag.fillAmount = 1f - _curTime / _showTime;
		if (_curTime >= _showTime)
		{
			_isInit = false;
			_lose?.Invoke();
		}
		CheckKey();
	}

	private void CheckKey()
	{
		if (Input.GetKeyDown(_keyCode))
		{
			_isInit = false;
			_action?.Invoke();
		}
		else if ((_keyCode == KeyCode.LeftArrow && Input.GetKey(KeyCode.A)) || (_keyCode == KeyCode.UpArrow && Input.GetKey(KeyCode.W)) || (_keyCode == KeyCode.RightArrow && Input.GetKey(KeyCode.D)) || (_keyCode == KeyCode.DownArrow && Input.GetKey(KeyCode.S)))
		{
			_isInit = false;
			_action?.Invoke();
		}
	}

	private void CheckNoneFail()
	{
		_curTime += Time.deltaTime;
		if (_curTime >= _showTime)
		{
			_isInit = false;
			_lose?.Invoke();
		}
	}

	private void CheckNone()
	{
		_curTime += Time.deltaTime;
		if (_curTime >= _showTime)
		{
			_isInit = false;
			_action?.Invoke();
		}
	}

	private void UpdateSlideGo()
	{
		_curTime += Time.deltaTime;
		if (_curTime >= _showTime)
		{
			_isInit = false;
			_lose?.Invoke();
		}
		ClickCheckSlide();
	}

	private void UpdateCondensation()
	{
		_curTime += Time.deltaTime;
		ag.fillAmount = 1f - _curTime / _showTime;
		if (_curTime >= _showTime)
		{
			_isInit = false;
			_lose?.Invoke();
		}
	}

	private void ClickCheckSlide()
	{
		if (Input.touchCount > 0 || Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
		{
			_isInit = false;
			_moveTweener?.Kill();
			float x = target.rectTransform.anchoredPosition.x;
			float num = target.rectTransform.anchoredPosition.x + target.rectTransform.sizeDelta.x;
			if (ag.rectTransform.anchoredPosition.x > x && ag.rectTransform.anchoredPosition.x < num)
			{
				_action?.Invoke();
			}
			else
			{
				_lose?.Invoke();
			}
		}
	}

	public void OnClick()
	{
		if (qteType == QteType.ClickGo)
		{
			CheckClick();
			return;
		}
		_data.isSucEx = true;
		_action?.Invoke();
	}
}

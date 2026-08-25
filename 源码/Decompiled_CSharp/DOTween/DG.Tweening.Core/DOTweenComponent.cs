using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DG.Tweening.Core;

[AddComponentMenu("")]
public class DOTweenComponent : MonoBehaviour, IDOTweenInit
{
	[CompilerGenerated]
	private sealed class _003CWaitForCompletion_003Ed__17 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Tween t;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitForCompletion_003Ed__17(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (t.active && !t.isComplete)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitForElapsedLoops_003Ed__20 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Tween t;

		public int elapsedLoops;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitForElapsedLoops_003Ed__20(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (t.active && t.completedLoops < elapsedLoops)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitForKill_003Ed__19 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Tween t;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitForKill_003Ed__19(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (t.active)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitForPosition_003Ed__21 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Tween t;

		public float position;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitForPosition_003Ed__21(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (t.active && t.position * (float)(t.completedLoops + 1) < position)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitForRewind_003Ed__18 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Tween t;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitForRewind_003Ed__18(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (t.active && (!t.playedOnce || t.position * (float)(t.completedLoops + 1) > 0f))
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	[CompilerGenerated]
	private sealed class _003CWaitForStart_003Ed__22 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public Tween t;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return _003C_003E2__current;
			}
		}

		[DebuggerHidden]
		public _003CWaitForStart_003Ed__22(int _003C_003E1__state)
		{
			this._003C_003E1__state = _003C_003E1__state;
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
			_003C_003E1__state = -2;
		}

		private bool MoveNext()
		{
			switch (_003C_003E1__state)
			{
			default:
				return false;
			case 0:
				_003C_003E1__state = -1;
				break;
			case 1:
				_003C_003E1__state = -1;
				break;
			}
			if (t.active && !t.playedOnce)
			{
				_003C_003E2__current = null;
				_003C_003E1__state = 1;
				return true;
			}
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
			throw new NotSupportedException();
		}
	}

	public int inspectorUpdater;

	private float _unscaledTime;

	private float _unscaledDeltaTime;

	private bool _paused;

	private float _pausedTime;

	private bool _isQuitting;

	private bool _duplicateToDestroy;

	private void Awake()
	{
		if (DOTween.instance == null)
		{
			DOTween.instance = this;
			inspectorUpdater = 0;
			_unscaledTime = Time.realtimeSinceStartup;
			Type looseScriptType = DOTweenUtils.GetLooseScriptType("DG.Tweening.DOTweenModuleUtils");
			if ((object)looseScriptType == null)
			{
				Debugger.LogError("Couldn't load Modules system");
			}
			else
			{
				looseScriptType.GetMethod("Init", BindingFlags.Static | BindingFlags.Public).Invoke(null, null);
			}
		}
		else
		{
			if (Debugger.logPriority >= 1)
			{
				Debugger.LogWarning("Duplicate DOTweenComponent instance found in scene: destroying it");
			}
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Start()
	{
		if (DOTween.instance != this)
		{
			_duplicateToDestroy = true;
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		_unscaledDeltaTime = Time.realtimeSinceStartup - _unscaledTime;
		if (DOTween.useSmoothDeltaTime && _unscaledDeltaTime > DOTween.maxSmoothUnscaledTime)
		{
			_unscaledDeltaTime = DOTween.maxSmoothUnscaledTime;
		}
		if (TweenManager.hasActiveDefaultTweens)
		{
			TweenManager.Update(UpdateType.Normal, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, _unscaledDeltaTime * DOTween.unscaledTimeScale * DOTween.timeScale);
		}
		_unscaledTime = Time.realtimeSinceStartup;
		if (!TweenManager.isUnityEditor)
		{
			return;
		}
		inspectorUpdater++;
		if (DOTween.showUnityEditorReport && TweenManager.hasActiveTweens)
		{
			if (TweenManager.totActiveTweeners > DOTween.maxActiveTweenersReached)
			{
				DOTween.maxActiveTweenersReached = TweenManager.totActiveTweeners;
			}
			if (TweenManager.totActiveSequences > DOTween.maxActiveSequencesReached)
			{
				DOTween.maxActiveSequencesReached = TweenManager.totActiveSequences;
			}
		}
	}

	private void LateUpdate()
	{
		if (TweenManager.hasActiveLateTweens)
		{
			TweenManager.Update(UpdateType.Late, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, _unscaledDeltaTime * DOTween.unscaledTimeScale * DOTween.timeScale);
		}
	}

	private void FixedUpdate()
	{
		if (TweenManager.hasActiveFixedTweens && Time.timeScale > 0f)
		{
			TweenManager.Update(UpdateType.Fixed, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) * DOTween.timeScale, (DOTween.useSmoothDeltaTime ? Time.smoothDeltaTime : Time.deltaTime) / Time.timeScale * DOTween.unscaledTimeScale * DOTween.timeScale);
		}
	}

	private void OnDrawGizmos()
	{
		if (!DOTween.drawGizmos || !TweenManager.isUnityEditor)
		{
			return;
		}
		int count = DOTween.GizmosDelegates.Count;
		if (count != 0)
		{
			for (int i = 0; i < count; i++)
			{
				DOTween.GizmosDelegates[i]();
			}
		}
	}

	private void OnDestroy()
	{
		if (_duplicateToDestroy)
		{
			return;
		}
		if (DOTween.showUnityEditorReport)
		{
			Debugger.LogReport("Max overall simultaneous active Tweeners/Sequences: " + DOTween.maxActiveTweenersReached + "/" + DOTween.maxActiveSequencesReached);
		}
		if (DOTween.useSafeMode)
		{
			int totErrors = DOTween.safeModeReport.GetTotErrors();
			if (totErrors > 0)
			{
				string text = $"DOTween's safe mode captured {totErrors} errors. This is usually ok (it's what safe mode is there for) but if your game is encountering issues you should set Log Behaviour to Default in DOTween Utility Panel in order to get detailed warnings when an error is captured (consider that these errors are always on the user side).";
				if (DOTween.safeModeReport.totMissingTargetOrFieldErrors > 0)
				{
					text = text + "\n- " + DOTween.safeModeReport.totMissingTargetOrFieldErrors + " missing target or field errors";
				}
				if (DOTween.safeModeReport.totStartupErrors > 0)
				{
					text = text + "\n- " + DOTween.safeModeReport.totStartupErrors + " startup errors";
				}
				if (DOTween.safeModeReport.totCallbackErrors > 0)
				{
					text = text + "\n- " + DOTween.safeModeReport.totCallbackErrors + " errors inside callbacks (these might be important)";
				}
				if (DOTween.safeModeReport.totUnsetErrors > 0)
				{
					text = text + "\n- " + DOTween.safeModeReport.totUnsetErrors + " undetermined errors (these might be important)";
				}
				Debugger.LogSafeModeReport(text);
			}
		}
		if (DOTween.instance == this)
		{
			DOTween.instance = null;
		}
		DOTween.Clear(destroy: true, _isQuitting);
	}

	public void OnApplicationPause(bool pauseStatus)
	{
		if (pauseStatus)
		{
			_paused = true;
			_pausedTime = Time.realtimeSinceStartup;
		}
		else if (_paused)
		{
			_paused = false;
			_unscaledTime += Time.realtimeSinceStartup - _pausedTime;
		}
	}

	private void OnApplicationQuit()
	{
		_isQuitting = true;
		DOTween.isQuitting = true;
	}

	public IDOTweenInit SetCapacity(int tweenersCapacity, int sequencesCapacity)
	{
		TweenManager.SetCapacities(tweenersCapacity, sequencesCapacity);
		return this;
	}

	internal IEnumerator WaitForCompletion(Tween t)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaitForCompletion_003Ed__17(0)
		{
			t = t
		};
	}

	internal IEnumerator WaitForRewind(Tween t)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaitForRewind_003Ed__18(0)
		{
			t = t
		};
	}

	internal IEnumerator WaitForKill(Tween t)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaitForKill_003Ed__19(0)
		{
			t = t
		};
	}

	internal IEnumerator WaitForElapsedLoops(Tween t, int elapsedLoops)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaitForElapsedLoops_003Ed__20(0)
		{
			t = t,
			elapsedLoops = elapsedLoops
		};
	}

	internal IEnumerator WaitForPosition(Tween t, float position)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaitForPosition_003Ed__21(0)
		{
			t = t,
			position = position
		};
	}

	internal IEnumerator WaitForStart(Tween t)
	{
		//yield-return decompiler failed: Unexpected instruction in Iterator.Dispose()
		return new _003CWaitForStart_003Ed__22(0)
		{
			t = t
		};
	}

	internal static void Create()
	{
		if (!(DOTween.instance != null))
		{
			GameObject obj = new GameObject("[DOTween]");
			UnityEngine.Object.DontDestroyOnLoad(obj);
			DOTween.instance = obj.AddComponent<DOTweenComponent>();
		}
	}

	internal static void DestroyInstance()
	{
		if (DOTween.instance != null)
		{
			UnityEngine.Object.Destroy(DOTween.instance.gameObject);
		}
		DOTween.instance = null;
	}
}

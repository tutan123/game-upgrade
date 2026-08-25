using UnityEngine;

internal class Tiled_Texture_Animation : MonoBehaviour
{
	public int _uvTieX = 1;

	public int _uvTieY = 1;

	public float _fps = 10f;

	public float mWaitBeforeStart;

	public int mLoopStartFrame = 3;

	private float mStartWait;

	private float iX;

	private float iY = 1f;

	private int mMaxFrames;

	private int mFrameCntr;

	private Vector2 _size;

	private Renderer _myRenderer;

	private int mLastCntr = -1;

	private void Start()
	{
		mStartWait = mWaitBeforeStart;
		_size = new Vector2(1f / (float)_uvTieX, 1f / (float)_uvTieY);
		_myRenderer = GetComponent<Renderer>();
		if (_myRenderer == null)
		{
			base.enabled = false;
		}
		_myRenderer.material.SetTextureScale("_MainTex", _size);
		Vector2 value = new Vector2(0f, 1f - _size.y);
		_myRenderer.material.SetTextureOffset("_MainTex", value);
		mMaxFrames = _uvTieX * _uvTieY;
		if (mLoopStartFrame >= mMaxFrames)
		{
			mLoopStartFrame = 0;
		}
		mFrameCntr = 0;
	}

	private void Update()
	{
		if (mStartWait > 0f)
		{
			mStartWait -= Time.deltaTime;
			if (!(mStartWait < 0f))
			{
				return;
			}
			mStartWait = 0f;
		}
		int num = (int)(Time.timeSinceLevelLoad * _fps) % (_uvTieX * _uvTieY);
		if (num != mLastCntr)
		{
			iX = mFrameCntr % _uvTieX;
			iY = (mFrameCntr / _uvTieX + 1) % _uvTieY;
			Vector2 value = new Vector2(iX * _size.x, 1f - _size.y * iY);
			_myRenderer.material.SetTextureOffset("_MainTex", value);
			mFrameCntr++;
			if (mFrameCntr == mMaxFrames)
			{
				iX = mLoopStartFrame % _uvTieX;
				iY = (mLoopStartFrame / _uvTieX + 1) % _uvTieY;
				mFrameCntr = mLoopStartFrame;
			}
			mLastCntr = num;
		}
	}
}

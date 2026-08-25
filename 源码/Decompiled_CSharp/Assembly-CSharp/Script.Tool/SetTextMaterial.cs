using TMPro;
using UnityEngine;

namespace Script.Tool;

public class SetTextMaterial : MonoBehaviour
{
	public TMP_Text text;

	public Material normalMaterial;

	public Material selectedMaterial;

	private void Start()
	{
		text.fontMaterial = normalMaterial;
	}

	public void Enter()
	{
		text.fontMaterial = selectedMaterial;
	}

	public void Exit()
	{
		text.fontMaterial = normalMaterial;
	}
}

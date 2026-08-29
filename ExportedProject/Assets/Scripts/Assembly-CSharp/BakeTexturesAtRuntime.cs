using UnityEngine;

public class BakeTexturesAtRuntime : MonoBehaviour
{
	public GameObject target;

	private float elapsedTime;

	private MB3_TextureBaker.CreateAtlasesCoroutineResult result;

	private void OnGUI()
	{
	}

	private void OnBuiltAtlasesSuccess()
	{
	}
}

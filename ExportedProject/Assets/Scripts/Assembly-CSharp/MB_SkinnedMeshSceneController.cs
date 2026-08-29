using UnityEngine;

public class MB_SkinnedMeshSceneController : MonoBehaviour
{
	public GameObject swordPrefab;

	public GameObject hatPrefab;

	public GameObject glassesPrefab;

	public GameObject workerPrefab;

	public GameObject targetCharacter;

	public MB3_MeshBaker skinnedMeshBaker;

	private GameObject swordInstance;

	private GameObject glassesInstance;

	private GameObject hatInstance;

	private void Start()
	{
	}

	private void OnGUI()
	{
	}

	public Transform SearchHierarchyForBone(Transform current, string name)
	{
		return null;
	}
}

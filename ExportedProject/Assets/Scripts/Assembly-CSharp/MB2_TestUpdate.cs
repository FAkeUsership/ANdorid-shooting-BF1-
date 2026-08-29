using UnityEngine;

public class MB2_TestUpdate : MonoBehaviour
{
	public MB3_MeshBaker meshbaker;

	public MB3_MultiMeshBaker multiMeshBaker;

	public GameObject[] objsToMove;

	public GameObject objWithChangingUVs;

	private Vector2[] uvs;

	private Mesh m;

	private void Start()
	{
	}

	private void LateUpdate()
	{
	}
}

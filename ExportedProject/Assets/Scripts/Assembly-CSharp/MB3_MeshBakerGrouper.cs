using DigitalOpus.MB.Core;
using UnityEngine;

public class MB3_MeshBakerGrouper : MonoBehaviour
{
	public enum ClusterType
	{
		none = 0,
		grid = 1,
		pie = 2,
		agglomerative = 3
	}

	public MB3_MeshBakerGrouperCore grouper;

	public ClusterType clusterType;

	public GrouperData data;

	[HideInInspector]
	public Bounds sourceObjectBounds;

	private void OnDrawGizmosSelected()
	{
	}

	public MB3_MeshBakerGrouperCore CreateGrouper(ClusterType t, GrouperData data)
	{
		return null;
	}
}

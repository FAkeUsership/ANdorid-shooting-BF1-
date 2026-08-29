using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public class MB3_MeshBakerGrouperCluster : MB3_MeshBakerGrouperCore
	{
		public MB3_AgglomerativeClustering cluster;

		private float _lastMaxDistBetweenClusters;

		public float _ObjsExtents;

		public float _minDistBetweenClusters;

		private List<MB3_AgglomerativeClustering.ClusterNode> _clustersToDraw;

		private float[] _radii;

		public MB3_MeshBakerGrouperCluster(GrouperData data, List<GameObject> gos)
		{
		}

		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			return null;
		}

		public void BuildClusters(List<GameObject> gos, ProgressUpdateCancelableDelegate progFunc)
		{
		}

		private void _BuildListOfClustersToDraw(ProgressUpdateCancelableDelegate progFunc, out float smallest, out float largest)
		{
			smallest = default;
			largest = default;
		}

		public override void DrawGizmos(Bounds sceneObjectBounds)
		{
		}
	}
}

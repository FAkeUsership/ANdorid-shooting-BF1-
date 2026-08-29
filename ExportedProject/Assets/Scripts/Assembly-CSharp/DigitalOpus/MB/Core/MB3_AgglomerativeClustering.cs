using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public class MB3_AgglomerativeClustering
	{
		[Serializable]
		public class ClusterNode
		{
			public item_s leaf;

			public ClusterNode cha;

			public ClusterNode chb;

			public int height;

			public float distToMergedCentroid;

			public Vector3 centroid;

			public int[] leafs;

			public int idx;

			public bool isUnclustered;

			public ClusterNode(item_s ii, int index)
			{
			}

			public ClusterNode(ClusterNode a, ClusterNode b, int index, int h, float dist, ClusterNode[] clusters)
			{
			}
		}

		[Serializable]
		public class item_s
		{
			public GameObject go;

			public Vector3 coord;
		}

		public class ClusterDistance
		{
			public ClusterNode a;

			public ClusterNode b;

			public ClusterDistance(ClusterNode aa, ClusterNode bb)
			{
			}
		}

		public List<item_s> items;

		public ClusterNode[] clusters;

		public bool wasCanceled;

		private const int MAX_PRIORITY_Q_SIZE = 2048;

		private float euclidean_distance(Vector3 a, Vector3 b)
		{
			return 0f;
		}

		public bool agglomerate(ProgressUpdateCancelableDelegate progFunc)
		{
			return false;
		}

		private float _RefillPriorityQWithSome(PriorityQueue<float, ClusterDistance> pq, List<ClusterNode> unclustered, ClusterNode[] clusters, ProgressUpdateCancelableDelegate progFunc)
		{
			return 0f;
		}

		public int TestRun(List<GameObject> gos)
		{
			return 0;
		}

		public static void Main()
		{
		}

		public static T NthSmallestElement<T>(List<T> array, int n) where T : IComparable<T>
		{
			return default;
		}

		private static List<T> QuickSelectSmallest<T>(List<T> input, int n) where T : IComparable<T>
		{
			return null;
		}

		private static int QuickSelectPartition<T>(List<T> array, int startIndex, int endIndex, int pivotIndex) where T : IComparable<T>
		{
			return 0;
		}

		private static void Swap<T>(List<T> array, int index1, int index2)
		{
		}
	}
}

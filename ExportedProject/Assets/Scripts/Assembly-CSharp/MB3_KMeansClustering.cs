using System.Collections.Generic;
using UnityEngine;

public class MB3_KMeansClustering
{
	private class DataPoint
	{
		public Vector3 center;

		public GameObject gameObject;

		public int Cluster;

		public DataPoint(GameObject go)
		{
		}
	}

	private List<DataPoint> _normalizedDataToCluster;

	private Vector3[] _clusters;

	private int _numberOfClusters;

	public MB3_KMeansClustering(List<GameObject> gos, int numClusters)
	{
	}

	private void InitializeCentroids()
	{
	}

	private bool UpdateDataPointMeans(bool force)
	{
		return false;
	}

	private bool AnyAreEmpty(List<DataPoint> data)
	{
		return false;
	}

	private bool UpdateClusterMembership()
	{
		return false;
	}

	private float ElucidanDistance(DataPoint dataPoint, Vector3 mean)
	{
		return 0f;
	}

	private int MinIndex(float[] distances)
	{
		return 0;
	}

	public List<Renderer> GetCluster(int idx, out Vector3 mean, out float size)
	{
		mean = default;
		size = default;
		return null;
	}

	public void Cluster()
	{
	}
}

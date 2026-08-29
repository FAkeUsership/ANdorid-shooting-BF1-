using System;
using UnityEngine;

namespace Cinemachine
{
	public abstract class CinemachinePathBase : MonoBehaviour
	{
		[Serializable]
		[DocumentationSorting(18.1f, DocumentationSortingAttribute.Level.UserRef)]
		public class Appearance
		{
			[Tooltip("The color of the path itself when it is active in the editor")]
			public Color pathColor;

			[Tooltip("The color of the path itself when it is inactive in the editor")]
			public Color inactivePathColor;

			[Range(0f, 10f)]
			[Tooltip("The width of the railroad-tracks that are drawn to represent the path")]
			public float width;
		}

		public enum PositionUnits
		{
			PathUnits = 0,
			Distance = 1
		}

		[Tooltip("Path samples per waypoint.  This is used for calculating path distances.")]
		[Range(1f, 100f)]
		public int m_Resolution;

		[Tooltip("The settings that control how the path will appear in the editor scene view.")]
		public Appearance m_Appearance;

		private float[] m_DistanceToPos;

		private float[] m_PosToDistance;

		private int m_CachedSampleSteps;

		private float m_PathLength;

		private float m_cachedPosStepSize;

		private float m_cachedDistanceStepSize;

		public abstract float MinPos { get; }

		public abstract float MaxPos { get; }

		public abstract bool Looped { get; }

		public abstract int DistanceCacheSampleStepsPerSegment { get; }

		public float PathLength => 0f;

		public virtual float NormalizePos(float pos)
		{
			return 0f;
		}

		public abstract Vector3 EvaluatePosition(float pos);

		public abstract Vector3 EvaluateTangent(float pos);

		public abstract Quaternion EvaluateOrientation(float pos);

		public virtual float FindClosestPoint(Vector3 p, int startSegment, int searchRadius, int stepsPerSegment)
		{
			return 0f;
		}

		public float MinUnit(PositionUnits units)
		{
			return 0f;
		}

		public float MaxUnit(PositionUnits units)
		{
			return 0f;
		}

		public virtual float NormalizeUnit(float pos, PositionUnits units)
		{
			return 0f;
		}

		public Vector3 EvaluatePositionAtUnit(float pos, PositionUnits units)
		{
			return default;
		}

		public Vector3 EvaluateTangentAtUnit(float pos, PositionUnits units)
		{
			return default;
		}

		public Quaternion EvaluateOrientationAtUnit(float pos, PositionUnits units)
		{
			return default;
		}

		public virtual void InvalidateDistanceCache()
		{
		}

		public bool DistanceCacheIsValid()
		{
			return false;
		}

		public float NormalizePathDistance(float distance)
		{
			return 0f;
		}

		public float GetPathPositionFromDistance(float distance)
		{
			return 0f;
		}

		public float GetPathDistanceFromPosition(float pos)
		{
			return 0f;
		}

		private void ResamplePath(int stepsPerSegment)
		{
		}
	}
}

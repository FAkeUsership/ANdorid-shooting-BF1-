using System;
using UnityEngine;

namespace Cinemachine
{
	[AddComponentMenu("Cinemachine/CinemachineSmoothPath")]
	[DocumentationSorting(18.5f, DocumentationSortingAttribute.Level.UserRef)]
	[SaveDuringPlay]
	public class CinemachineSmoothPath : CinemachinePathBase
	{
		[Serializable]
		[DocumentationSorting(18.7f, DocumentationSortingAttribute.Level.UserRef)]
		public struct Waypoint
		{
			[Tooltip("Position in path-local space")]
			public Vector3 position;

			[Tooltip("Defines the roll of the path at this waypoint.  The other orientation axes are inferred from the tangent and world up.")]
			public float roll;

			internal Vector4 AsVector4 => default;

			internal static Waypoint FromVector4(Vector4 v)
			{
				return default;
			}
		}

		[Tooltip("If checked, then the path ends are joined to form a continuous loop.")]
		public bool m_Looped;

		[Tooltip("The waypoints that define the path.  They will be interpolated using a bezier curve.")]
		public Waypoint[] m_Waypoints;

		private Waypoint[] m_ControlPoints1;

		private Waypoint[] m_ControlPoints2;

		private bool m_IsLoopedCache;

		public override float MinPos => 0f;

		public override float MaxPos => 0f;

		public override bool Looped => false;

		public override int DistanceCacheSampleStepsPerSegment => 0;

		private void OnValidate()
		{
		}

		public override void InvalidateDistanceCache()
		{
		}

		private void UpdateControlPoints()
		{
		}

		private float GetBoundingIndices(float pos, out int indexA, out int indexB)
		{
			indexA = default;
			indexB = default;
			return 0f;
		}

		public override Vector3 EvaluatePosition(float pos)
		{
			return default;
		}

		public override Vector3 EvaluateTangent(float pos)
		{
			return default;
		}

		public override Quaternion EvaluateOrientation(float pos)
		{
			return default;
		}
	}
}

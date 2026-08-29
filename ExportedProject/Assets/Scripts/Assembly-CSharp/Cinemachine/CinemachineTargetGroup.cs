using System;
using UnityEngine;

namespace Cinemachine
{
	[ExecuteInEditMode]
	[SaveDuringPlay]
	[AddComponentMenu("Cinemachine/CinemachineTargetGroup")]
	[DocumentationSorting(19f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineTargetGroup : MonoBehaviour
	{
		[Serializable]
		[DocumentationSorting(19.1f, DocumentationSortingAttribute.Level.UserRef)]
		public struct Target
		{
			[Tooltip("The target objects.  This object's position and orientation will contribute to the group's average position and orientation, in accordance with its weight")]
			public Transform target;

			[Tooltip("How much weight to give the target when averaging.  Cannot be negative")]
			public float weight;

			[Tooltip("The radius of the target, used for calculating the bounding box.  Cannot be negative")]
			public float radius;
		}

		[DocumentationSorting(19.2f, DocumentationSortingAttribute.Level.UserRef)]
		public enum PositionMode
		{
			GroupCenter = 0,
			GroupAverage = 1
		}

		[DocumentationSorting(19.3f, DocumentationSortingAttribute.Level.UserRef)]
		public enum RotationMode
		{
			Manual = 0,
			GroupAverage = 1
		}

		public enum UpdateMethod
		{
			Update = 0,
			FixedUpdate = 1,
			LateUpdate = 2
		}

		[Tooltip("How the group's position is calculated.  Select GroupCenter for the center of the bounding box, and GroupAverage for a weighted average of the positions of the members.")]
		public PositionMode m_PositionMode;

		[Tooltip("How the group's rotation is calculated.  Select Manual to use the value in the group's transform, and GroupAverage for a weighted average of the orientations of the members.")]
		public RotationMode m_RotationMode;

		[Tooltip("When to update the group's transform based on the position of the group members")]
		public UpdateMethod m_UpdateMethod;

		[NoSaveDuringPlay]
		[Tooltip("The target objects, together with their weights and radii, that will contribute to the group's average position, orientation, and size.")]
		public Target[] m_Targets;

		private float m_lastRadius;

		public Bounds BoundingBox => default;

		public bool IsEmpty => false;

		public Bounds GetViewSpaceBoundingBox(Matrix4x4 mView)
		{
			return default;
		}

		private Vector3 CalculateAveragePosition(out float averageWeight)
		{
			averageWeight = default;
			return default;
		}

		private Quaternion CalculateAverageOrientation()
		{
			return default;
		}

		private void OnValidate()
		{
		}

		private void FixedUpdate()
		{
		}

		private void Update()
		{
		}

		private void LateUpdate()
		{
		}

		private void UpdateTransform()
		{
		}
	}
}

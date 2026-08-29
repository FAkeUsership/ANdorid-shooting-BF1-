using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[AddComponentMenu(null)]
	[DocumentationSorting(6f, DocumentationSortingAttribute.Level.UserRef)]
	[SaveDuringPlay]
	[RequireComponent(typeof(CinemachinePipeline))]
	public class CinemachineOrbitalTransposer : CinemachineTransposer
	{
		[Serializable]
		[DocumentationSorting(6.2f, DocumentationSortingAttribute.Level.UserRef)]
		public struct Heading
		{
			[DocumentationSorting(6.21f, DocumentationSortingAttribute.Level.UserRef)]
			public enum HeadingDefinition
			{
				PositionDelta = 0,
				Velocity = 1,
				TargetForward = 2,
				WorldForward = 3
			}

			[Tooltip("How 'forward' is defined.  The camera will be placed by default behind the target.  PositionDelta will consider 'forward' to be the direction in which the target is moving.")]
			public HeadingDefinition m_HeadingDefinition;

			[Range(0f, 10f)]
			[Tooltip("Size of the velocity sampling window for target heading filter.  This filters out irregularities in the target's movement.  Used only if deriving heading from target's movement (PositionDelta or Velocity)")]
			public int m_VelocityFilterStrength;

			[Tooltip("Where the camera is placed when the X-axis value is zero.  This is a rotation in degrees around the Y axis.  When this value is 0, the camera will be placed behind the target.  Nonzero offsets will rotate the zero position around the target.")]
			[Range(-180f, 180f)]
			public float m_HeadingBias;

			public Heading(HeadingDefinition def, int filterStrength, float bias)
			{
				m_HeadingDefinition = HeadingDefinition.PositionDelta;
				m_VelocityFilterStrength = 0;
				m_HeadingBias = 0f;
			}
		}

		[Serializable]
		[DocumentationSorting(6.5f, DocumentationSortingAttribute.Level.UserRef)]
		public struct Recentering
		{
			[Tooltip("If checked, will enable automatic recentering of the camera based on the heading definition. If unchecked, recenting is disabled.")]
			public bool m_enabled;

			[Tooltip("If no input has been detected, the camera will wait this long in seconds before moving its heading to the zero position.")]
			public float m_RecenterWaitTime;

			[Tooltip("Maximum angular speed of recentering.  Will accelerate into and decelerate out of this.")]
			public float m_RecenteringTime;

			[SerializeField]
			[HideInInspector]
			[FormerlySerializedAs("m_HeadingDefinition")]
			private int m_LegacyHeadingDefinition;

			[HideInInspector]
			[FormerlySerializedAs("m_VelocityFilterStrength")]
			[SerializeField]
			private int m_LegacyVelocityFilterStrength;

			public Recentering(bool enabled, float recenterWaitTime, float recenteringSpeed)
			{
				m_enabled = false;
				m_RecenterWaitTime = 0f;
				m_RecenteringTime = 0f;
				m_LegacyHeadingDefinition = 0;
				m_LegacyVelocityFilterStrength = 0;
			}

			public void Validate()
			{
			}

			internal bool LegacyUpgrade(ref Heading.HeadingDefinition heading, ref int velocityFilter)
			{
				return false;
			}
		}

		internal delegate float UpdateHeadingDelegate(CinemachineOrbitalTransposer orbital, float deltaTime, Vector3 up);

		private class HeadingTracker
		{
			private struct Item
			{
				public Vector3 velocity;

				public float weight;

				public float time;
			}

			private Item[] mHistory;

			private int mTop;

			private int mBottom;

			private int mCount;

			private Vector3 mHeadingSum;

			private float mWeightSum;

			private float mWeightTime;

			private Vector3 mLastGoodHeading;

			private static float mDecayExponent;

			public int FilterSize => 0;

			public HeadingTracker(int filterSize)
			{
			}

			private void ClearHistory()
			{
			}

			private static float Decay(float time)
			{
				return 0f;
			}

			public void Add(Vector3 velocity)
			{
			}

			private void PopBottom()
			{
			}

			public void DecayHistory()
			{
			}

			public Vector3 GetReliableHeading()
			{
				return default;
			}
		}

		[Space]
		[Tooltip("The definition of Forward.  Camera will follow behind.")]
		public Heading m_Heading;

		[Tooltip("Automatic heading recentering.  The settings here defines how the camera will reposition itself in the absence of player input.")]
		public Recentering m_RecenterToTargetHeading;

		[Tooltip("Heading Control.  The settings here control the behaviour of the camera in response to the player's input.")]
		public AxisState m_XAxis;

		[FormerlySerializedAs("m_Radius")]
		[HideInInspector]
		[SerializeField]
		private float m_LegacyRadius;

		[FormerlySerializedAs("m_HeightOffset")]
		[SerializeField]
		[HideInInspector]
		private float m_LegacyHeightOffset;

		[SerializeField]
		[FormerlySerializedAs("m_HeadingBias")]
		[HideInInspector]
		private float m_LegacyHeadingBias;

		[NoSaveDuringPlay]
		[HideInInspector]
		public bool m_HeadingIsSlave;

		internal UpdateHeadingDelegate HeadingUpdater;

		private float mLastHeadingAxisInputTime;

		private float mHeadingRecenteringVelocity;

		private Vector3 mLastTargetPosition;

		private HeadingTracker mHeadingTracker;

		private Rigidbody mTargetRigidBody;

		private Quaternion mHeadingPrevFrame;

		private Vector3 mOffsetPrevFrame;

		private Transform PreviousTarget { get; set; }

		protected override void OnValidate()
		{
		}

		public float UpdateHeading(float deltaTime, Vector3 up, ref AxisState axis)
		{
			return 0f;
		}

		private void OnEnable()
		{
		}

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}

		public override void OnPositionDragged(Vector3 delta)
		{
		}

		private static string GetFullName(GameObject current)
		{
			return null;
		}

		private float GetTargetHeading(float currentHeading, Quaternion targetOrientation, float deltaTime)
		{
			return 0f;
		}
	}
}

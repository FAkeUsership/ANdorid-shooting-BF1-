using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[AddComponentMenu(null)]
	[SaveDuringPlay]
	[DocumentationSorting(15f, DocumentationSortingAttribute.Level.UserRef)]
	[ExecuteInEditMode]
	public class CinemachineCollider : CinemachineExtension
	{
		public enum ResolutionStrategy
		{
			PullCameraForward = 0,
			PreserveCameraHeight = 1,
			PreserveCameraDistance = 2
		}

		private class VcamExtraState
		{
			public Vector3 m_previousDisplacement;

			public float colliderDisplacement;

			public bool targetObscured;

			public List<Vector3> debugResolutionPath;

			public void AddPointToDebugPath(Vector3 p)
			{
			}
		}

		[Tooltip("The Unity layer mask against which the collider will raycast")]
		[Header("Obstacle Detection")]
		public LayerMask m_CollideAgainst;

		[Tooltip("Obstacles with this tag will be ignored.  It is a good idea to set this field to the target's tag")]
		[TagField]
		public string m_IgnoreTag;

		[Tooltip("Obstacles closer to the target than this will be ignored")]
		public float m_MinimumDistanceFromTarget;

		[Space]
		[Tooltip("When enabled, will attempt to resolve situations where the line of sight to the target is blocked by an obstacle")]
		[FormerlySerializedAs("m_PreserveLineOfSight")]
		public bool m_AvoidObstacles;

		[FormerlySerializedAs("m_LineOfSightFeelerDistance")]
		[Tooltip("The maximum raycast distance when checking if the line of sight to this camera's target is clear.  If the setting is 0 or less, the current actual distance to target will be used.")]
		public float m_DistanceLimit;

		[Tooltip("Camera will try to maintain this distance from any obstacle.  Try to keep this value small.  Increase it if you are seeing inside obstacles due to a large FOV on the camera.")]
		public float m_CameraRadius;

		[Tooltip("The way in which the Collider will attempt to preserve sight of the target.")]
		public ResolutionStrategy m_Strategy;

		[Tooltip("Upper limit on how many obstacle hits to process.  Higher numbers may impact performance.  In most environments, 4 is enough.")]
		[Range(1f, 10f)]
		public int m_MaximumEffort;

		[FormerlySerializedAs("m_Smoothing")]
		[Range(0f, 10f)]
		[Tooltip("The gradualness of collision resolution.  Higher numbers will move the camera more gradually away from obstructions.")]
		public float m_Damping;

		[Header("Shot Evaluation")]
		[Tooltip("If greater than zero, a higher score will be given to shots when the target is closer to this distance.  Set this to zero to disable this feature.")]
		public float m_OptimalTargetDistance;

		private const float PrecisionSlush = 0.001f;

		private RaycastHit[] m_CornerBuffer;

		private const float AngleThreshold = 0.1f;

		private Collider[] mColliderBuffer;

		private SphereCollider mCameraCollider;

		private GameObject mCameraColliderGameObject;

		public List<List<Vector3>> DebugPaths => null;

		public bool IsTargetObscured(ICinemachineCamera vcam)
		{
			return false;
		}

		public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
		{
			return false;
		}

		private void OnValidate()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
		}

		private Vector3 PreserveLignOfSight(ref CameraState state, ref VcamExtraState extra)
		{
			return default;
		}

		private bool RaycastIgnoreTag(Ray ray, out RaycastHit hitInfo, float rayLength)
		{
			hitInfo = default;
			return false;
		}

		private Vector3 PushCameraBack(Vector3 currentPos, Vector3 pushDir, RaycastHit obstacle, Vector3 lookAtPos, Plane startPlane, float targetDistance, int iterations, ref VcamExtraState extra)
		{
			return default;
		}

		private bool GetWalkingDirection(Vector3 pos, Vector3 pushDir, RaycastHit obstacle, ref Vector3 outDir)
		{
			return false;
		}

		private float GetPushBackDistance(Ray ray, Plane startPlane, float targetDistance, Vector3 lookAtPos)
		{
			return 0f;
		}

		private float ClampRayToBounds(Ray ray, float distance, Bounds bounds)
		{
			return 0f;
		}

		private Vector3 RespectCameraRadius(Vector3 cameraPos, Vector3 lookAtPos)
		{
			return default;
		}

		private void CleanupCameraCollider()
		{
		}

		private bool CheckForTargetObstructions(CameraState state)
		{
			return false;
		}
	}
}

using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(5f, DocumentationSortingAttribute.Level.UserRef)]
	[SaveDuringPlay]
	[AddComponentMenu(null)]
	[RequireComponent(typeof(CinemachinePipeline))]
	public class CinemachineTransposer : CinemachineComponentBase
	{
		[DocumentationSorting(5.01f, DocumentationSortingAttribute.Level.UserRef)]
		public enum BindingMode
		{
			LockToTargetOnAssign = 0,
			LockToTargetWithWorldUp = 1,
			LockToTargetNoRoll = 2,
			LockToTarget = 3,
			WorldSpace = 4,
			SimpleFollowWithWorldUp = 5
		}

		[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
		public BindingMode m_BindingMode;

		[Tooltip("The distance vector that the transposer will attempt to maintain from the Follow target")]
		public Vector3 m_FollowOffset;

		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the X-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's x-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_XDamping;

		[Tooltip("How aggressively the camera tries to maintain the offset in the Y-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's y-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		[Range(0f, 20f)]
		public float m_YDamping;

		[Tooltip("How aggressively the camera tries to maintain the offset in the Z-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's z-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		[Range(0f, 20f)]
		public float m_ZDamping;

		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to track the target rotation's X angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		public float m_PitchDamping;

		[Tooltip("How aggressively the camera tries to track the target rotation's Y angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		[Range(0f, 20f)]
		public float m_YawDamping;

		[Tooltip("How aggressively the camera tries to track the target rotation's Z angle.  Small numbers are more responsive.  Larger numbers give a more heavy slowly responding camera.")]
		[Range(0f, 20f)]
		public float m_RollDamping;

		private Vector3 m_PreviousTargetPosition;

		private Quaternion m_PreviousReferenceOrientation;

		private Quaternion m_targetOrientationOnAssign;

		private Transform m_previousTarget;

		protected Vector3 EffectiveOffset => default;

		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		protected Vector3 Damping => default;

		protected Vector3 AngularDamping => default;

		protected virtual void OnValidate()
		{
		}

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}

		public override void OnPositionDragged(Vector3 delta)
		{
		}

		protected void InitPrevFrameStateInfo(ref CameraState curState, float deltaTime)
		{
		}

		protected void TrackTarget(float deltaTime, Vector3 up, Vector3 desiredCameraOffset, out Vector3 outTargetPosition, out Quaternion outTargetOrient)
		{
			outTargetPosition = default;
			outTargetOrient = default;
		}

		public Vector3 GeTargetCameraPosition(Vector3 worldUp)
		{
			return default;
		}

		public Quaternion GetReferenceOrientation(Vector3 worldUp)
		{
			return default;
		}

		private static Quaternion Uppify(Quaternion q, Vector3 up)
		{
			return default;
		}
	}
}

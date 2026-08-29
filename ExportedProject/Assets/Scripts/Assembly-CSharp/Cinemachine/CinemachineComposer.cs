using System;
using Cinemachine.Utility;
using UnityEngine;

namespace Cinemachine
{
	[SaveDuringPlay]
	[RequireComponent(typeof(CinemachinePipeline))]
	[AddComponentMenu(null)]
	[ExecuteInEditMode]
	[DocumentationSorting(3f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineComposer : CinemachineComponentBase
	{
		[HideInInspector]
		[NoSaveDuringPlay]
		public Action OnGUICallback;

		[Tooltip("Target offset from the target object's center in target-local space. Use this to fine-tune the tracking target position when the desired area is not the tracked object's center.")]
		public Vector3 m_TrackedObjectOffset;

		[Range(0f, 1f)]
		[Tooltip("This setting will instruct the composer to adjust its target offset based on the motion of the target.  The composer will look at a point where it estimates the target will be this many seconds into the future.  Note that this setting is sensitive to noisy animation, and can amplify the noise, resulting in undesirable camera jitter.  If the camera jitters unacceptably when the target is in motion, turn down this setting, or animate the target more smoothly.")]
		public float m_LookaheadTime;

		[Tooltip("Controls the smoothness of the lookahead algorithm.  Larger values smooth out jittery predictions and also increase prediction lag")]
		[Range(3f, 30f)]
		public float m_LookaheadSmoothing;

		[Space]
		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to follow the target in the screen-horizontal direction. Small numbers are more responsive, rapidly orienting the camera to keep the target in the dead zone. Larger numbers give a more heavy slowly responding camera. Using different vertical and horizontal settings can yield a wide range of camera behaviors.")]
		public float m_HorizontalDamping;

		[Tooltip("How aggressively the camera tries to follow the target in the screen-vertical direction. Small numbers are more responsive, rapidly orienting the camera to keep the target in the dead zone. Larger numbers give a more heavy slowly responding camera. Using different vertical and horizontal settings can yield a wide range of camera behaviors.")]
		[Range(0f, 20f)]
		public float m_VerticalDamping;

		[Range(0f, 1f)]
		[Tooltip("Horizontal screen position for target. The camera will rotate to position the tracked object here.")]
		[Space]
		public float m_ScreenX;

		[Tooltip("Vertical screen position for target, The camera will rotate to position the tracked object here.")]
		[Range(0f, 1f)]
		public float m_ScreenY;

		[Range(0f, 1f)]
		[Tooltip("Camera will not rotate horizontally if the target is within this range of the position.")]
		public float m_DeadZoneWidth;

		[Tooltip("Camera will not rotate vertically if the target is within this range of the position.")]
		[Range(0f, 1f)]
		public float m_DeadZoneHeight;

		[Tooltip("When target is within this region, camera will gradually rotate horizontally to re-align towards the desired position, depending on the damping speed.")]
		[Range(0f, 2f)]
		public float m_SoftZoneWidth;

		[Tooltip("When target is within this region, camera will gradually rotate vertically to re-align towards the desired position, depending on the damping speed.")]
		[Range(0f, 2f)]
		public float m_SoftZoneHeight;

		[Tooltip("A non-zero bias will move the target position horizontally away from the center of the soft zone.")]
		[Range(-0.5f, 0.5f)]
		public float m_BiasX;

		[Tooltip("A non-zero bias will move the target position vertically away from the center of the soft zone.")]
		[Range(-0.5f, 0.5f)]
		public float m_BiasY;

		private Vector3 m_CameraPosPrevFrame;

		private Vector3 m_LookAtPrevFrame;

		private Vector2 m_ScreenOffsetPrevFrame;

		private Quaternion m_CameraOrientationPrevFrame;

		private PositionPredictor m_Predictor;

		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public Vector3 TrackedPoint { get; private set; }

		public Rect SoftGuideRect
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public Rect HardGuideRect
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		protected virtual Vector3 GetLookAtPointAndSetTrackedPoint(Vector3 lookAt)
		{
			return default;
		}

		public override void PrePipelineMutateCameraState(ref CameraState curState)
		{
		}

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}

		private Rect ScreenToFOV(Rect rScreen, float fov, float fovH, float aspect)
		{
			return default;
		}

		private bool RotateToScreenBounds(ref CameraState state, Rect screenRect, ref Quaternion rigOrientation, float fov, float fovH, float deltaTime)
		{
			return false;
		}

		private bool ClampVerticalBounds(ref Rect r, Vector3 dir, Vector3 up, float fov)
		{
			return false;
		}
	}
}

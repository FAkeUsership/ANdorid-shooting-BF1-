using System;
using Cinemachine.Utility;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[ExecuteInEditMode]
	[AddComponentMenu(null)]
	[RequireComponent(typeof(CinemachinePipeline))]
	[SaveDuringPlay]
	[DocumentationSorting(5.5f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineFramingTransposer : CinemachineComponentBase
	{
		[DocumentationSorting(4.01f, DocumentationSortingAttribute.Level.UserRef)]
		public enum FramingMode
		{
			Horizontal = 0,
			Vertical = 1,
			HorizontalAndVertical = 2,
			None = 3
		}

		public enum AdjustmentMode
		{
			ZoomOnly = 0,
			DollyOnly = 1,
			DollyThenZoom = 2
		}

		[HideInInspector]
		[NoSaveDuringPlay]
		public Action OnGUICallback;

		[Range(0f, 1f)]
		[Tooltip("This setting will instruct the composer to adjust its target offset based on the motion of the target.  The composer will look at a point where it estimates the target will be this many seconds into the future.  Note that this setting is sensitive to noisy animation, and can amplify the noise, resulting in undesirable camera jitter.  If the camera jitters unacceptably when the target is in motion, turn down this setting, or animate the target more smoothly.")]
		public float m_LookaheadTime;

		[Tooltip("Controls the smoothness of the lookahead algorithm.  Larger values smooth out jittery predictions and also increase prediction lag")]
		[Range(3f, 30f)]
		public float m_LookaheadSmoothing;

		[Tooltip("How aggressively the camera tries to maintain the offset in the X-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's x-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		[Range(0f, 20f)]
		public float m_XDamping;

		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Y-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's y-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_YDamping;

		[Range(0f, 20f)]
		[Tooltip("How aggressively the camera tries to maintain the offset in the Z-axis.  Small numbers are more responsive, rapidly translating the camera to keep the target's z-axis offset.  Larger numbers give a more heavy slowly responding camera. Using different settings per axis can yield a wide range of camera behaviors.")]
		public float m_ZDamping;

		[Space]
		[Range(0f, 1f)]
		[Tooltip("Horizontal screen position for target. The camera will move to position the tracked object here.")]
		public float m_ScreenX;

		[Tooltip("Vertical screen position for target, The camera will move to position the tracked object here.")]
		[Range(0f, 1f)]
		public float m_ScreenY;

		[Tooltip("The distance along the camera axis that will be maintained from the Follow target")]
		public float m_CameraDistance;

		[Tooltip("Camera will not move horizontally if the target is within this range of the position.")]
		[Space]
		[Range(0f, 1f)]
		public float m_DeadZoneWidth;

		[Range(0f, 1f)]
		[Tooltip("Camera will not move vertically if the target is within this range of the position.")]
		public float m_DeadZoneHeight;

		[Tooltip("The camera will not move along its z-axis if the Follow target is within this distance of the specified camera distance")]
		[FormerlySerializedAs("m_DistanceDeadZoneSize")]
		public float m_DeadZoneDepth;

		[Space]
		[Tooltip("If checked, then then soft zone will be unlimited in size.")]
		public bool m_UnlimitedSoftZone;

		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually move horizontally to re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneWidth;

		[Range(0f, 2f)]
		[Tooltip("When target is within this region, camera will gradually move vertically to re-align towards the desired position, depending on the damping speed.")]
		public float m_SoftZoneHeight;

		[Tooltip("A non-zero bias will move the target position horizontally away from the center of the soft zone.")]
		[Range(-0.5f, 0.5f)]
		public float m_BiasX;

		[Range(-0.5f, 0.5f)]
		[Tooltip("A non-zero bias will move the target position vertically away from the center of the soft zone.")]
		public float m_BiasY;

		[FormerlySerializedAs("m_FramingMode")]
		[Tooltip("What screen dimensions to consider when framing.  Can be Horizontal, Vertical, or both")]
		[Space]
		public FramingMode m_GroupFramingMode;

		[Tooltip("How to adjust the camera to get the desired framing.  You can zoom, dolly in/out, or do both.")]
		public AdjustmentMode m_AdjustmentMode;

		[Tooltip("The bounding box of the targets should occupy this amount of the screen space.  1 means fill the whole screen.  0.5 means fill half the screen, etc.")]
		public float m_GroupFramingSize;

		[Tooltip("The maximum distance toward the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyIn;

		[Tooltip("The maximum distance away the target that this behaviour is allowed to move the camera.")]
		public float m_MaxDollyOut;

		[Tooltip("Set this to limit how close to the target the camera can get.")]
		public float m_MinimumDistance;

		[Tooltip("Set this to limit how far from the target the camera can get.")]
		public float m_MaximumDistance;

		[Tooltip("If adjusting FOV, will not set the FOV lower than this.")]
		[Range(1f, 179f)]
		public float m_MinimumFOV;

		[Range(1f, 179f)]
		[Tooltip("If adjusting FOV, will not set the FOV higher than this.")]
		public float m_MaximumFOV;

		[Tooltip("If adjusting Orthographic Size, will not set it lower than this.")]
		public float m_MinimumOrthoSize;

		[Tooltip("If adjusting Orthographic Size, will not set it higher than this.")]
		public float m_MaximumOrthoSize;

		private const float kMinimumCameraDistance = 0.01f;

		private Vector3 m_PreviousCameraPosition;

		private PositionPredictor m_Predictor;

		private float m_prevTargetHeight;

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

		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public Vector3 TrackedPoint { get; private set; }

		public Bounds m_LastBounds { get; private set; }

		public Matrix4x4 m_lastBoundsMatrix { get; private set; }

		public CinemachineTargetGroup TargetGroup => null;

		private void OnValidate()
		{
		}

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}

		private Rect ScreenToOrtho(Rect rScreen, float orthoSize, float aspect)
		{
			return default;
		}

		private Vector3 OrthoOffsetToScreenBounds(Vector3 targetPos2D, Rect screenRect)
		{
			return default;
		}

		private float AdjustCameraDepthAndLensForGroupFraming(CinemachineTargetGroup group, float targetZ, ref CameraState curState, float deltaTime)
		{
			return 0f;
		}

		private float GetTargetHeight(Bounds b)
		{
			return 0f;
		}
	}
}

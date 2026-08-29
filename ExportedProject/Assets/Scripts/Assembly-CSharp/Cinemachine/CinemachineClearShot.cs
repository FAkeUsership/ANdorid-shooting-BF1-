using UnityEngine;

namespace Cinemachine
{
	[ExecuteInEditMode]
	[AddComponentMenu("Cinemachine/CinemachineClearShot")]
	[DocumentationSorting(12f, DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	public class CinemachineClearShot : CinemachineVirtualCameraBase
	{
		private struct Pair
		{
			public int a;

			public float b;
		}

		[NoSaveDuringPlay]
		[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all children specify targets of their own.")]
		public Transform m_LookAt;

		[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all children specify targets of their own.")]
		[NoSaveDuringPlay]
		public Transform m_Follow;

		[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
		[NoSaveDuringPlay]
		public bool m_ShowDebugText;

		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		public CinemachineVirtualCameraBase[] m_ChildCameras;

		[Tooltip("Wait this many seconds before activating a new child camera")]
		public float m_ActivateAfter;

		[Tooltip("An active camera must be active for at least this many seconds")]
		public float m_MinDuration;

		[Tooltip("If checked, camera choice will be randomized if multiple cameras are equally desirable.  Otherwise, child list order and child camera priority will be used.")]
		public bool m_RandomizeChoice;

		[Tooltip("The blend which is used if you don't explicitly define a blend between two Virtual Cameras")]
		[CinemachineBlendDefinitionProperty]
		public CinemachineBlendDefinition m_DefaultBlend;

		[HideInInspector]
		public CinemachineBlenderSettings m_CustomBlends;

		private CameraState m_State;

		private float mActivationTime;

		private float mPendingActivationTime;

		private ICinemachineCamera mPendingCamera;

		private CinemachineBlend mActiveBlend;

		private bool mRandomizeNow;

		private CinemachineVirtualCameraBase[] m_RandomizedChilden;

		public override string Description => null;

		public ICinemachineCamera LiveChild { get; set; }

		public override CameraState State => default;

		public override ICinemachineCamera LiveChildOrSelf => null;

		public override Transform LookAt
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public override Transform Follow
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public bool IsBlending => false;

		public CinemachineVirtualCameraBase[] ChildCameras => null;

		public override bool IsLiveChild(ICinemachineCamera vcam)
		{
			return false;
		}

		public override void RemovePostPipelineStageHook(OnPostPipelineStageDelegate d)
		{
		}

		public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}

		protected override void OnEnable()
		{
		}

		public void OnTransformChildrenChanged()
		{
		}

		private void InvalidateListOfChildren()
		{
		}

		public void ResetRandomization()
		{
		}

		private void UpdateListOfChildren()
		{
		}

		private ICinemachineCamera ChooseCurrentCamera(Vector3 worldUp, float deltaTime)
		{
			return null;
		}

		private CinemachineVirtualCameraBase[] Randomize(CinemachineVirtualCameraBase[] src)
		{
			return null;
		}

		private AnimationCurve LookupBlendCurve(ICinemachineCamera fromKey, ICinemachineCamera toKey, out float duration)
		{
			duration = default;
			return null;
		}

		private CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, AnimationCurve blendCurve, float duration, CinemachineBlend activeBlend, float deltaTime)
		{
			return null;
		}

		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
		}
	}
}

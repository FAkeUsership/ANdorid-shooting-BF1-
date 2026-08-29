using System;
using UnityEngine;

namespace Cinemachine
{
	[SaveDuringPlay]
	public abstract class CinemachineVirtualCameraBase : MonoBehaviour, ICinemachineCamera
	{
		public delegate void OnPostPipelineStageDelegate(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState newState, float deltaTime);

		[HideInInspector]
		[NoSaveDuringPlay]
		public Action CinemachineGUIDebuggerCallback;

		[NoSaveDuringPlay]
		[SerializeField]
		[HideInInspector]
		public string[] m_ExcludedPropertiesInInspector;

		[HideInInspector]
		[NoSaveDuringPlay]
		[SerializeField]
		public CinemachineCore.Stage[] m_LockStageInInspector;

		private int m_ValidatingStreamVersion;

		private bool m_OnValidateCalled;

		[NoSaveDuringPlay]
		[SerializeField]
		[HideInInspector]
		private int m_StreamingVersion;

		[Tooltip("The priority will determine which camera becomes active based on the state of other cameras and this camera.  Higher numbers have greater priority.")]
		[NoSaveDuringPlay]
		public int m_Priority;

		protected OnPostPipelineStageDelegate OnPostPipelineStage;

		private bool m_previousStateIsValid;

		private Transform m_previousLookAtTarget;

		private Transform m_previousFollowTarget;

		private bool mSlaveStatusUpdated;

		private CinemachineVirtualCameraBase m_parentVcam;

		private int m_QueuePriority;

		public int ValidatingStreamVersion
		{
			get
			{
				return 0;
			}
			private set
			{
			}
		}

		public string Name => null;

		public virtual string Description => null;

		public int Priority
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public GameObject VirtualCameraGameObject => null;

		public abstract CameraState State { get; }

		public virtual ICinemachineCamera LiveChildOrSelf => null;

		public ICinemachineCamera ParentCamera => null;

		public abstract Transform LookAt { get; set; }

		public abstract Transform Follow { get; set; }

		public bool PreviousStateIsValid
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual void AddPostPipelineStageHook(OnPostPipelineStageDelegate d)
		{
		}

		public virtual void RemovePostPipelineStageHook(OnPostPipelineStageDelegate d)
		{
		}

		protected void InvokePostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState newState, float deltaTime)
		{
		}

		public virtual bool IsLiveChild(ICinemachineCamera vcam)
		{
			return false;
		}

		public abstract void UpdateCameraState(Vector3 worldUp, float deltaTime);

		public virtual void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
		}

		protected virtual void Start()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		protected virtual void OnValidate()
		{
		}

		protected virtual void OnEnable()
		{
		}

		protected virtual void OnDisable()
		{
		}

		protected virtual void Update()
		{
		}

		protected virtual void OnTransformParentChanged()
		{
		}

		private void UpdateSlaveStatus()
		{
		}

		protected Transform ResolveLookAt(Transform localLookAt)
		{
			return null;
		}

		protected Transform ResolveFollow(Transform localFollow)
		{
			return null;
		}

		private void UpdateVcamPoolStatus()
		{
		}

		public void MoveToTopOfPrioritySubqueue()
		{
		}
	}
}

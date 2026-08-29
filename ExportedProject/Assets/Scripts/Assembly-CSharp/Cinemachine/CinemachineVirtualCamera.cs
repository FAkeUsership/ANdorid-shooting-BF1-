using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[ExecuteInEditMode]
	[DocumentationSorting(1f, DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[AddComponentMenu("Cinemachine/CinemachineVirtualCamera")]
	public class CinemachineVirtualCamera : CinemachineVirtualCameraBase
	{
		public delegate Transform CreatePipelineDelegate(CinemachineVirtualCamera vcam, string name, CinemachineComponentBase[] copyFrom);

		public delegate void DestroyPipelineDelegate(GameObject pipeline);

		[Tooltip("The object that the camera wants to look at (the Aim target).  If this is null, then the vcam's Transform orientation will define the camera's orientation.")]
		[NoSaveDuringPlay]
		public Transform m_LookAt;

		[NoSaveDuringPlay]
		[Tooltip("The object that the camera wants to move with (the Body target).  If this is null, then the vcam's Transform position will define the camera's position.")]
		public Transform m_Follow;

		[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active.")]
		[FormerlySerializedAs("m_LensAttributes")]
		[LensSettingsProperty]
		public LensSettings m_Lens;

		public const string PipelineName = "cm";

		public static CreatePipelineDelegate CreatePipelineOverride;

		public static DestroyPipelineDelegate DestroyPipelineOverride;

		private CameraState m_State;

		private CinemachineComponentBase[] m_ComponentPipeline;

		[SerializeField]
		[HideInInspector]
		private Transform m_ComponentOwner;

		public override CameraState State => default;

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

		public bool UserIsDragging { get; set; }

		public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}

		protected override void OnEnable()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void OnValidate()
		{
		}

		private void OnTransformChildrenChanged()
		{
		}

		private void Reset()
		{
		}

		private void DestroyPipeline()
		{
		}

		private Transform CreatePipeline(CinemachineVirtualCamera copyFrom)
		{
			return null;
		}

		public void InvalidateComponentPipeline()
		{
		}

		public Transform GetComponentOwner()
		{
			return null;
		}

		public CinemachineComponentBase[] GetComponentPipeline()
		{
			return null;
		}

		public CinemachineComponentBase GetCinemachineComponent(CinemachineCore.Stage stage)
		{
			return null;
		}

		public T GetCinemachineComponent<T>() where T : CinemachineComponentBase
		{
			return null;
		}

		public T AddCinemachineComponent<T>() where T : CinemachineComponentBase
		{
			return null;
		}

		public void DestroyCinemachineComponent<T>() where T : CinemachineComponentBase
		{
		}

		public void OnPositionDragged(Vector3 delta)
		{
		}

		private void UpdateComponentPipeline()
		{
		}

		private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
		{
			return default;
		}

		private CinemachineCore.Stage AdvancePipelineStage(ref CameraState state, float deltaTime, CinemachineCore.Stage curStage, int maxStage)
		{
			return CinemachineCore.Stage.Body;
		}

		private CameraState PullStateFromVirtualCamera(Vector3 worldUp)
		{
			return default;
		}

		internal void SetStateRawPosition(Vector3 pos)
		{
		}
	}
}

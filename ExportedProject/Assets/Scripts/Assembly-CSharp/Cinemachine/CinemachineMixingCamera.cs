using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(20f, DocumentationSortingAttribute.Level.UserRef)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Cinemachine/CinemachineMixingCamera")]
	public class CinemachineMixingCamera : CinemachineVirtualCameraBase
	{
		public const int MaxCameras = 8;

		[Tooltip("The weight of the first tracked camera")]
		public float m_Weight0;

		[Tooltip("The weight of the second tracked camera")]
		public float m_Weight1;

		[Tooltip("The weight of the third tracked camera")]
		public float m_Weight2;

		[Tooltip("The weight of the fourth tracked camera")]
		public float m_Weight3;

		[Tooltip("The weight of the fifth tracked camera")]
		public float m_Weight4;

		[Tooltip("The weight of the sixth tracked camera")]
		public float m_Weight5;

		[Tooltip("The weight of the seventh tracked camera")]
		public float m_Weight6;

		[Tooltip("The weight of the eighth tracked camera")]
		public float m_Weight7;

		private CameraState m_State;

		private CinemachineVirtualCameraBase[] m_ChildCameras;

		private Dictionary<CinemachineVirtualCameraBase, int> m_indexMap;

		private ICinemachineCamera LiveChild { get; set; }

		public override CameraState State => default;

		public override Transform LookAt { get; set; }

		public override Transform Follow { get; set; }

		public override ICinemachineCamera LiveChildOrSelf => null;

		public CinemachineVirtualCameraBase[] ChildCameras => null;

		public float GetWeight(int index)
		{
			return 0f;
		}

		public void SetWeight(int index, float w)
		{
		}

		public float GetWeight(CinemachineVirtualCameraBase vcam)
		{
			return 0f;
		}

		public void SetWeight(CinemachineVirtualCameraBase vcam, float w)
		{
		}

		public override void RemovePostPipelineStageHook(OnPostPipelineStageDelegate d)
		{
		}

		protected override void OnEnable()
		{
		}

		public void OnTransformChildrenChanged()
		{
		}

		protected override void OnValidate()
		{
		}

		public override bool IsLiveChild(ICinemachineCamera vcam)
		{
			return false;
		}

		protected void InvalidateListOfChildren()
		{
		}

		protected void ValidateListOfChildren()
		{
		}

		public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}
	}
}

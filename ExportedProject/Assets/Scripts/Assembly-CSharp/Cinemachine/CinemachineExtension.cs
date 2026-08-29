using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(23f, DocumentationSortingAttribute.Level.API)]
	public abstract class CinemachineExtension : MonoBehaviour
	{
		protected const float Epsilon = 0.0001f;

		private CinemachineVirtualCameraBase m_vcamOwner;

		private Dictionary<ICinemachineCamera, object> mExtraState;

		public CinemachineVirtualCameraBase VirtualCamera => null;

		protected virtual void Awake()
		{
		}

		protected virtual void OnDestroy()
		{
		}

		private void ConnectToVcam()
		{
		}

		protected abstract void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime);

		protected T GetExtraState<T>(ICinemachineCamera vcam) where T : class, new()
		{
			return null;
		}

		protected List<T> GetAllExtraStates<T>() where T : class, new()
		{
			return null;
		}
	}
}

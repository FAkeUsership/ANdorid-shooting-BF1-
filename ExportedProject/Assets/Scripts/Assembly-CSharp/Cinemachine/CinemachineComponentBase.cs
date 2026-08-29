using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(24f, DocumentationSortingAttribute.Level.API)]
	public abstract class CinemachineComponentBase : MonoBehaviour
	{
		protected const float Epsilon = 0.0001f;

		private CinemachineVirtualCameraBase m_vcamOwner;

		public CinemachineVirtualCameraBase VirtualCamera => null;

		public Transform FollowTarget => null;

		public Transform LookAtTarget => null;

		public CameraState VcamState => default;

		public abstract bool IsValid { get; }

		public abstract CinemachineCore.Stage Stage { get; }

		public virtual void PrePipelineMutateCameraState(ref CameraState state)
		{
		}

		public abstract void MutateCameraState(ref CameraState curState, float deltaTime);

		public virtual void OnPositionDragged(Vector3 delta)
		{
		}
	}
}

using UnityEngine;

namespace Cinemachine
{
	internal class BlendSourceVirtualCamera : ICinemachineCamera
	{
		public CinemachineBlend Blend { get; private set; }

		public string Name => null;

		public string Description => null;

		public int Priority { get; set; }

		public Transform LookAt { get; set; }

		public Transform Follow { get; set; }

		public CameraState State { get; private set; }

		public GameObject VirtualCameraGameObject => null;

		public ICinemachineCamera LiveChildOrSelf => null;

		public ICinemachineCamera ParentCamera => null;

		public BlendSourceVirtualCamera(CinemachineBlend blend, float deltaTime)
		{
		}

		public bool IsLiveChild(ICinemachineCamera vcam)
		{
			return false;
		}

		public CameraState CalculateNewState(float deltaTime)
		{
			return default;
		}

		public void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}

		public void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
		}
	}
}

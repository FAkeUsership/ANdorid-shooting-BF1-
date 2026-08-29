using UnityEngine;

namespace Cinemachine
{
	public class CinemachineBlend
	{
		public ICinemachineCamera CamA { get; set; }

		public ICinemachineCamera CamB { get; set; }

		public AnimationCurve BlendCurve { get; set; }

		public float TimeInBlend { get; set; }

		public float BlendWeight => 0f;

		public bool IsValid => false;

		public float Duration { get; set; }

		public bool IsComplete => false;

		public string Description => null;

		public CameraState State => default;

		public bool Uses(ICinemachineCamera cam)
		{
			return false;
		}

		public CinemachineBlend(ICinemachineCamera a, ICinemachineCamera b, AnimationCurve curve, float duration, float t)
		{
		}

		public void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}
	}
}

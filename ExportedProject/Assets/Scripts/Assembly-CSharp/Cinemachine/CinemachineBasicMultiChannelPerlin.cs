using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[DocumentationSorting(8f, DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu(null)]
	[RequireComponent(typeof(CinemachinePipeline))]
	[SaveDuringPlay]
	public class CinemachineBasicMultiChannelPerlin : CinemachineComponentBase
	{
		[HideInInspector]
		[Tooltip("The asset containing the Noise Profile.  Define the frequencies and amplitudes there to make a characteristic noise profile.  Make your own or just use one of the many presets.")]
		[FormerlySerializedAs("m_Definition")]
		public NoiseSettings m_NoiseProfile;

		[Tooltip("Gain to apply to the amplitudes defined in the NoiseSettings asset.  1 is normal.  Setting this to 0 completely mutes the noise.")]
		public float m_AmplitudeGain;

		[Tooltip("Scale factor to apply to the frequencies defined in the NoiseSettings asset.  1 is normal.  Larger magnitudes will make the noise shake more rapidly.")]
		public float m_FrequencyGain;

		private bool mInitialized;

		private float mNoiseTime;

		private Vector3 mNoiseOffsets;

		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}

		private void Initialize()
		{
		}

		private static Vector3 GetCombinedFilterResults(NoiseSettings.TransformNoiseParams[] noiseParams, float time, Vector3 noiseOffsets)
		{
			return default;
		}
	}
}

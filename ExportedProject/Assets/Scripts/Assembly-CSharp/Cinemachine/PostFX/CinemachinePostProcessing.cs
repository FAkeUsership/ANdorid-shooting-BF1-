using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

namespace Cinemachine.PostFX
{
	[DocumentationSorting(101f, DocumentationSortingAttribute.Level.UserRef)]
	[ExecuteInEditMode]
	[AddComponentMenu(null)]
	[SaveDuringPlay]
	public class CinemachinePostProcessing : CinemachineExtension
	{
		[Tooltip("If checked, then the Focus Distance will be set to the distance between the camera and the LookAt target.  Requires DepthOfField effect in the Profile")]
		public bool m_FocusTracksTarget;

		[Tooltip("Offset from target distance, to be used with Focus Tracks Target.  Offsets the sharpest point away from the LookAt target.")]
		public float m_FocusOffset;

		[Tooltip("This Post-Processing profile will be applied whenever this virtual camera is live")]
		public PostProcessProfile m_Profile;

		private bool mCachedProfileIsInvalid;

		private PostProcessProfile mProfileCopy;

		private static string sVolumeOwnerName;

		private static List<PostProcessVolume> sVolumes;

		public PostProcessProfile Profile => null;

		public bool IsValid => false;

		public void InvalidateCachedProfile()
		{
		}

		private void CreateProfileCopy()
		{
		}

		private void DestroyProfileCopy()
		{
		}

		protected override void OnDestroy()
		{
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
		}

		private static void OnCameraCut(CinemachineBrain brain)
		{
		}

		private static void ApplyPostFX(CinemachineBrain brain)
		{
		}

		private static List<PostProcessVolume> GetDynamicBrainVolumes(CinemachineBrain brain, PostProcessLayer ppLayer, int minVolumes)
		{
			return null;
		}

		[RuntimeInitializeOnLoadMethod]
		public static void InitializeModule()
		{
		}

		private static void StaticPostFXHandler(CinemachineBrain brain)
		{
		}
	}
}

using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Cinemachine
{
	[DocumentationSorting(11f, DocumentationSortingAttribute.Level.UserRef)]
	[DisallowMultipleComponent]
	[AddComponentMenu("Cinemachine/CinemachineFreeLook")]
	[ExecuteInEditMode]
	public class CinemachineFreeLook : CinemachineVirtualCameraBase
	{
		[Serializable]
		public struct Orbit
		{
			public float m_Height;

			public float m_Radius;

			public Orbit(float h, float r)
			{
				m_Height = 0f;
				m_Radius = 0f;
			}
		}

		public delegate CinemachineVirtualCamera CreateRigDelegate(CinemachineFreeLook vcam, string name, CinemachineVirtualCamera copyFrom);

		public delegate void DestroyRigDelegate(GameObject rig);

		[Tooltip("Object for the camera children to look at (the aim target).")]
		[NoSaveDuringPlay]
		public Transform m_LookAt;

		[NoSaveDuringPlay]
		[Tooltip("Object for the camera children wants to move with (the body target).")]
		public Transform m_Follow;

		[FormerlySerializedAs("m_UseCommonLensSetting")]
		[Tooltip("If enabled, this lens setting will apply to all three child rigs, otherwise the child rig lens settings will be used")]
		public bool m_CommonLens;

		[Tooltip("Specifies the lens properties of this Virtual Camera.  This generally mirrors the Unity Camera's lens settings, and will be used to drive the Unity camera when the vcam is active")]
		[FormerlySerializedAs("m_LensAttributes")]
		[LensSettingsProperty]
		public LensSettings m_Lens;

		[Tooltip("The Vertical axis.  Value is 0..1.  Chooses how to blend the child rigs")]
		[Header("Axis Control")]
		public AxisState m_YAxis;

		[Tooltip("The Horizontal axis.  Value is 0..359.  This is passed on to the rigs' OrbitalTransposer component")]
		public AxisState m_XAxis;

		[Tooltip("The definition of Forward.  Camera will follow behind.")]
		public CinemachineOrbitalTransposer.Heading m_Heading;

		[Tooltip("Controls how automatic recentering of the X axis is accomplished")]
		public CinemachineOrbitalTransposer.Recentering m_RecenterToTargetHeading;

		[Header("Orbits")]
		[Tooltip("The coordinate space to use when interpreting the offset from the target.  This is also used to set the camera's Up vector, which will be maintained when aiming the camera.")]
		public CinemachineTransposer.BindingMode m_BindingMode;

		[FormerlySerializedAs("m_SplineTension")]
		[Tooltip("Controls how taut is the line that connects the rigs' orbits, which determines final placement on the Y axis")]
		[Range(0f, 1f)]
		public float m_SplineCurvature;

		[Tooltip("The radius and height of the three orbiting rigs.")]
		public Orbit[] m_Orbits;

		[HideInInspector]
		[FormerlySerializedAs("m_HeadingBias")]
		[SerializeField]
		private float m_LegacyHeadingBias;

		private bool mUseLegacyRigDefinitions;

		private bool mIsDestroyed;

		private CameraState m_State;

		[SerializeField]
		[HideInInspector]
		[NoSaveDuringPlay]
		private CinemachineVirtualCamera[] m_Rigs;

		private CinemachineOrbitalTransposer[] mOrbitals;

		private CinemachineBlend mBlendA;

		private CinemachineBlend mBlendB;

		public static CreateRigDelegate CreateRigOverride;

		public static DestroyRigDelegate DestroyRigOverride;

		private Orbit[] m_CachedOrbits;

		private float m_CachedTension;

		private Vector4[] m_CachedKnots;

		private Vector4[] m_CachedCtrl1;

		private Vector4[] m_CachedCtrl2;

		public static string[] RigNames => null;

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

		public override ICinemachineCamera LiveChildOrSelf => null;

		protected override void OnValidate()
		{
		}

		public CinemachineVirtualCamera GetRig(int i)
		{
			return null;
		}

		protected override void OnEnable()
		{
		}

		protected override void OnDestroy()
		{
		}

		private void OnTransformChildrenChanged()
		{
		}

		private void Reset()
		{
		}

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

		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
		{
		}

		private void InvalidateRigCache()
		{
		}

		private void DestroyRigs()
		{
		}

		private CinemachineVirtualCamera[] CreateRigs(CinemachineVirtualCamera[] copyFrom)
		{
			return null;
		}

		private void UpdateRigCache()
		{
		}

		private int LocateExistingRigs(string[] rigNames, bool forceOrbital)
		{
			return 0;
		}

		private void PushSettingsToRigs()
		{
		}

		private CameraState CalculateNewState(Vector3 worldUp, float deltaTime)
		{
			return default;
		}

		private CameraState PullStateFromVirtualCamera(Vector3 worldUp)
		{
			return default;
		}

		public Vector3 GetLocalPositionForCameraFromInput(float t)
		{
			return default;
		}

		private void UpdateCachedSpline()
		{
		}
	}
}

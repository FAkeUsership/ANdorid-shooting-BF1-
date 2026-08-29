using System;
using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(13f, DocumentationSortingAttribute.Level.UserRef)]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[AddComponentMenu("Cinemachine/CinemachineStateDrivenCamera")]
	public class CinemachineStateDrivenCamera : CinemachineVirtualCameraBase
	{
		[Serializable]
		public struct Instruction
		{
			[Tooltip("The full hash of the animation state")]
			public int m_FullHash;

			[Tooltip("The virtual camera to activate whrn the animation state becomes active")]
			public CinemachineVirtualCameraBase m_VirtualCamera;

			[Tooltip("How long to wait (in seconds) before activating the virtual camera. This filters out very short state durations")]
			public float m_ActivateAfter;

			[Tooltip("The minimum length of time (in seconds) to keep a virtual camera active")]
			public float m_MinDuration;
		}

		[Serializable]
		[DocumentationSorting(13.2f, DocumentationSortingAttribute.Level.Undoc)]
		public struct ParentHash
		{
			public int m_Hash;

			public int m_ParentHash;

			public ParentHash(int h, int p)
			{
				m_Hash = 0;
				m_ParentHash = 0;
			}
		}

		[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		public Transform m_LookAt;

		[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		public Transform m_Follow;

		[Tooltip("The state machine whose state changes will drive this camera's choice of active child")]
		[Space]
		public Animator m_AnimatedTarget;

		[Tooltip("Which layer in the target state machine to observe")]
		public int m_LayerIndex;

		[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
		public bool m_ShowDebugText;

		[Tooltip("Force all child cameras to be enabled.  This is useful if animating them in Timeline, but consumes extra resources")]
		public bool m_EnableAllChildCameras;

		[NoSaveDuringPlay]
		[SerializeField]
		[HideInInspector]
		public CinemachineVirtualCameraBase[] m_ChildCameras;

		[Tooltip("The set of instructions associating virtual cameras with states.  These instructions are used to choose the live child at any given moment")]
		public Instruction[] m_Instructions;

		[Tooltip("The blend which is used if you don't explicitly define a blend between two Virtual Camera children")]
		[CinemachineBlendDefinitionProperty]
		public CinemachineBlendDefinition m_DefaultBlend;

		[Tooltip("This is the asset which contains custom settings for specific child blends")]
		public CinemachineBlenderSettings m_CustomBlends;

		[HideInInspector]
		[SerializeField]
		public ParentHash[] m_ParentHash;

		private CameraState m_State;

		private float mActivationTime;

		private Instruction mActiveInstruction;

		private float mPendingActivationTime;

		private Instruction mPendingInstruction;

		private CinemachineBlend mActiveBlend;

		private Dictionary<int, int> mInstructionDictionary;

		private Dictionary<int, int> mStateParentLookup;

		private List<AnimatorClipInfo> m_clipInfoList;

		public override string Description => null;

		public ICinemachineCamera LiveChild { get; set; }

		public override ICinemachineCamera LiveChildOrSelf => null;

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

		public CinemachineVirtualCameraBase[] ChildCameras => null;

		public bool IsBlending => false;

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

		protected override void OnEnable()
		{
		}

		public void OnTransformChildrenChanged()
		{
		}

		public static string CreateFakeHashName(int parentHash, string stateName)
		{
			return null;
		}

		private void InvalidateListOfChildren()
		{
		}

		private void UpdateListOfChildren()
		{
		}

		public void ValidateInstructions()
		{
		}

		private CinemachineVirtualCameraBase ChooseCurrentCamera(float deltaTime)
		{
			return null;
		}

		private int GetClipHash(int hash, List<AnimatorClipInfo> clips)
		{
			return 0;
		}

		private AnimationCurve LookupBlendCurve(ICinemachineCamera fromKey, ICinemachineCamera toKey, out float duration)
		{
			duration = default;
			return null;
		}

		private CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, AnimationCurve blendCurve, float duration, CinemachineBlend activeBlend, float deltaTime)
		{
			return null;
		}
	}
}

using System;
using UnityEngine;

namespace Cinemachine
{
	[AddComponentMenu("Cinemachine/CinemachineBlendListCamera")]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[DocumentationSorting(13f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineBlendListCamera : CinemachineVirtualCameraBase
	{
		[Serializable]
		public struct Instruction
		{
			[Tooltip("The virtual camera to activate when this instruction becomes active")]
			public CinemachineVirtualCameraBase m_VirtualCamera;

			[Tooltip("How long to wait (in seconds) before activating the next virtual camera in the list (if any)")]
			public float m_Hold;

			[Tooltip("How to blend to the next virtual camera in the list (if any)")]
			[CinemachineBlendDefinitionProperty]
			public CinemachineBlendDefinition m_Blend;
		}

		[Tooltip("Default object for the camera children to look at (the aim target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		[NoSaveDuringPlay]
		public Transform m_LookAt;

		[NoSaveDuringPlay]
		[Tooltip("Default object for the camera children wants to move with (the body target), if not specified in a child camera.  May be empty if all of the children define targets of their own.")]
		public Transform m_Follow;

		[Tooltip("When enabled, the current child camera and blend will be indicated in the game window, for debugging")]
		public bool m_ShowDebugText;

		[Tooltip("Force all child cameras to be enabled.  This is useful if animating them in Timeline, but consumes extra resources")]
		public bool m_EnableAllChildCameras;

		[NoSaveDuringPlay]
		[HideInInspector]
		[SerializeField]
		public CinemachineVirtualCameraBase[] m_ChildCameras;

		[Tooltip("The set of instructions for enabling child cameras.")]
		public Instruction[] m_Instructions;

		private CameraState m_State;

		private float mActivationTime;

		private int mCurrentInstruction;

		private CinemachineBlend mActiveBlend;

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

		public override void OnTransitionFromCamera(ICinemachineCamera fromCam, Vector3 worldUp, float deltaTime)
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

		private void InvalidateListOfChildren()
		{
		}

		private void UpdateListOfChildren()
		{
		}

		public void ValidateInstructions()
		{
		}

		private void AdvanceCurrentInstruction()
		{
		}

		private CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, AnimationCurve blendCurve, float duration, CinemachineBlend activeBlend, float deltaTime)
		{
			return null;
		}
	}
}

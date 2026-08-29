using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

namespace Cinemachine
{
	[DocumentationSorting(0f, DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu("Cinemachine/CinemachineBrain")]
	[DisallowMultipleComponent]
	[ExecuteInEditMode]
	[SaveDuringPlay]
	public class CinemachineBrain : MonoBehaviour
	{
		[DocumentationSorting(0.1f, DocumentationSortingAttribute.Level.UserRef)]
		public enum UpdateMethod
		{
			FixedUpdate = 0,
			LateUpdate = 1,
			SmartUpdate = 2
		}

		[Serializable]
		public class BrainEvent : UnityEvent<CinemachineBrain>
		{
		}

		[Serializable]
		public class VcamEvent : UnityEvent<ICinemachineCamera>
		{
		}

		private class OverrideStackFrame
		{
			public int id;

			public ICinemachineCamera camera;

			public CinemachineBlend blend;

			public float deltaTime;

			public float timeOfOverride;

			public bool Active => false;

			public bool Expired => false;
		}

		[CompilerGenerated]
		private sealed class _003CAfterPhysics_003Ed__44 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CinemachineBrain _003C_003E4__this;

			object IEnumerator<object>.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			object IEnumerator.Current
			{
				[DebuggerHidden]
				get
				{
					return null;
				}
			}

			[DebuggerHidden]
			public _003CAfterPhysics_003Ed__44(int _003C_003E1__state)
			{
			}

			[DebuggerHidden]
			void IDisposable.Dispose()
			{
			}

			private bool MoveNext()
			{
				return false;
			}

			bool IEnumerator.MoveNext()
			{
				//ILSpy generated this explicit interface implementation from .override directive in MoveNext
				return this.MoveNext();
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		[Tooltip("When enabled, the current camera and blend will be indicated in the game window, for debugging")]
		public bool m_ShowDebugText;

		[Tooltip("When enabled, the camera's frustum will be shown at all times in the scene view")]
		public bool m_ShowCameraFrustum;

		[Tooltip("When enabled, the cameras will always respond in real-time to user input and damping, even if the game is running in slow motion")]
		public bool m_IgnoreTimeScale;

		[Tooltip("If set, this object's Y axis will define the worldspace Up vector for all the virtual cameras.  This is useful for instance in top-down game environments.  If not set, Up is worldspace Y.  Setting this appropriately is important, because Virtual Cameras don't like looking straight up or straight down.")]
		public Transform m_WorldUpOverride;

		[Tooltip("Use FixedUpdate if all your targets are animated during FixedUpdate (e.g. RigidBodies), LateUpdate if all your targets are animated during the normal Update loop, and SmartUpdate if you want Cinemachine to do the appropriate thing on a per-target basis.  SmartUpdate is the recommended setting")]
		public UpdateMethod m_UpdateMethod;

		[CinemachineBlendDefinitionProperty]
		[Tooltip("The blend that is used in cases where you haven't explicitly defined a blend between two Virtual Cameras")]
		public CinemachineBlendDefinition m_DefaultBlend;

		[Tooltip("This is the asset that contains custom settings for blends between specific virtual cameras in your scene")]
		public CinemachineBlenderSettings m_CustomBlends;

		private Camera m_OutputCamera;

		[Tooltip("This event will fire whenever a virtual camera goes live and there is no blend")]
		public BrainEvent m_CameraCutEvent;

		[Tooltip("This event will fire whenever a virtual camera goes live.  If a blend is involved, then the event will fire on the first frame of the blend.")]
		public VcamEvent m_CameraActivatedEvent;

		internal static BrainEvent sPostProcessingHandler;

		private ICinemachineCamera mActiveCameraPreviousFrame;

		private ICinemachineCamera mOutgoingCameraPreviousFrame;

		private CinemachineBlend mActiveBlend;

		private bool mPreviousFrameWasOverride;

		private List<OverrideStackFrame> mOverrideStack;

		private int mNextOverrideId;

		private OverrideStackFrame mOverrideBlendFromNothing;

		private WaitForFixedUpdate mWaitForFixedUpdate;

		private static int msCurrentFrame;

		private static int msFirstBrainObjectId;

		private static int msSubframes;

		public Camera OutputCamera => null;

		internal Component PostProcessingComponent { get; set; }

		public static ICinemachineCamera SoloCamera { get; set; }

		public Vector3 DefaultWorldUp => default;

		public bool IsBlending => false;

		public CinemachineBlend ActiveBlend => null;

		public ICinemachineCamera ActiveVirtualCamera => null;

		public CameraState CurrentCameraState { get; private set; }

		public static Color GetSoloGUIColor()
		{
			return default;
		}

		private OverrideStackFrame GetOverrideFrame(int id)
		{
			return null;
		}

		private OverrideStackFrame GetNextActiveFrame(int overrideId)
		{
			return null;
		}

		private OverrideStackFrame GetActiveOverride()
		{
			return null;
		}

		internal int SetCameraOverride(int overrideId, ICinemachineCamera camA, ICinemachineCamera camB, float weightB, float deltaTime)
		{
			return 0;
		}

		internal void ReleaseCameraOverride(int overrideId)
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void Start()
		{
		}

		[IteratorStateMachine(typeof(_003CAfterPhysics_003Ed__44))]
		private IEnumerator AfterPhysics()
		{
			return null;
		}

		private void LateUpdate()
		{
		}

		private float GetEffectiveDeltaTime(bool fixedDelta)
		{
			return 0f;
		}

		private void UpdateVirtualCameras(CinemachineCore.UpdateFilter updateFilter, float deltaTime)
		{
		}

		private void ProcessActiveCamera(float deltaTime)
		{
		}

		public bool IsLive(ICinemachineCamera vcam)
		{
			return false;
		}

		private bool IsLiveItself(ICinemachineCamera vcam)
		{
			return false;
		}

		private ICinemachineCamera TopCameraFromPriorityQueue()
		{
			return null;
		}

		private AnimationCurve LookupBlendCurve(ICinemachineCamera fromKey, ICinemachineCamera toKey, out float duration)
		{
			duration = default;
			return null;
		}

		private CinemachineBlend CreateBlend(ICinemachineCamera camA, ICinemachineCamera camB, AnimationCurve blendCurve, float duration, CinemachineBlend activeBlend)
		{
			return null;
		}

		private void PushStateToUnityCamera(CameraState state, ICinemachineCamera vcam)
		{
		}

		private void AddSubframe()
		{
		}

		internal static int GetSubframeCount()
		{
			return 0;
		}
	}
}

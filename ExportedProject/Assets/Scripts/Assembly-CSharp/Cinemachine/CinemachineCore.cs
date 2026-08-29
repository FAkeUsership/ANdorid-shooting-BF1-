using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	public sealed class CinemachineCore
	{
		public enum Stage
		{
			Body = 0,
			Aim = 1,
			Noise = 2
		}

		public delegate float AxisInputDelegate(string axisName);

		private struct UpdateStatus
		{
			private const int kWindowSize = 30;

			public int lastUpdateFrame;

			public int lastUpdateSubframe;

			public int windowStart;

			public int numWindowLateUpdateMoves;

			public int numWindowFixedUpdateMoves;

			public int numWindows;

			public UpdateFilter preferredUpdate;

			public Matrix4x4 targetPos;

			public UpdateStatus(int currentFrame)
			{
				lastUpdateFrame = 0;
				lastUpdateSubframe = 0;
				windowStart = 0;
				numWindowLateUpdateMoves = 0;
				numWindowFixedUpdateMoves = 0;
				numWindows = 0;
				preferredUpdate = UpdateFilter.Fixed;
				targetPos = default;
			}

			public UpdateFilter ChoosePreferredUpdate(int currentFrame, Matrix4x4 pos, UpdateFilter updateFilter)
			{
				return UpdateFilter.Fixed;
			}
		}

		public enum UpdateFilter
		{
			Fixed = 0,
			ForcedFixed = 1,
			Late = 2,
			ForcedLate = 3
		}

		public static readonly int kStreamingVersion;

		public static readonly string kVersionString;

		private static CinemachineCore sInstance;

		public static bool sShowHiddenObjects;

		public static AxisInputDelegate GetInputAxis;

		private List<CinemachineBrain> mActiveBrains;

		private List<ICinemachineCamera> mActiveCameras;

		private List<List<ICinemachineCamera>> mChildCameras;

		private Dictionary<ICinemachineCamera, UpdateStatus> mUpdateStatus;

		public static CinemachineCore Instance => null;

		public int BrainCount => 0;

		public int VirtualCameraCount => 0;

		internal UpdateFilter CurrentUpdateFilter { get; set; }

		public CinemachineBrain GetActiveBrain(int index)
		{
			return null;
		}

		internal void AddActiveBrain(CinemachineBrain brain)
		{
		}

		internal void RemoveActiveBrain(CinemachineBrain brain)
		{
		}

		public ICinemachineCamera GetVirtualCamera(int index)
		{
			return null;
		}

		internal void AddActiveCamera(ICinemachineCamera vcam)
		{
		}

		internal void RemoveActiveCamera(ICinemachineCamera vcam)
		{
		}

		internal void AddChildCamera(ICinemachineCamera vcam)
		{
		}

		internal void RemoveChildCamera(ICinemachineCamera vcam)
		{
		}

		internal void UpdateAllActiveVirtualCameras(Vector3 worldUp, float deltaTime)
		{
		}

		internal bool UpdateVirtualCamera(ICinemachineCamera vcam, Vector3 worldUp, float deltaTime)
		{
			return false;
		}

		private static bool GetTargetPosition(ICinemachineCamera vcam, out Matrix4x4 targetPos)
		{
			targetPos = default;
			return false;
		}

		public UpdateFilter GetVcamUpdateStatus(ICinemachineCamera vcam)
		{
			return UpdateFilter.Fixed;
		}

		public bool IsLive(ICinemachineCamera vcam)
		{
			return false;
		}

		public void GenerateCameraActivationEvent(ICinemachineCamera vcam)
		{
		}

		public void GenerateCameraCutEvent(ICinemachineCamera vcam)
		{
		}

		public CinemachineBrain FindPotentialTargetBrain(ICinemachineCamera vcam)
		{
			return null;
		}
	}
}

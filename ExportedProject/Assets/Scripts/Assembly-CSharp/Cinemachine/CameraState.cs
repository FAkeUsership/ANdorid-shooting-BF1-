using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	public struct CameraState
	{
		public struct CustomBlendable
		{
			public Object m_Custom;

			public float m_Weight;

			public CustomBlendable(Object custom, float weight)
			{
				m_Custom = null;
				m_Weight = 0f;
			}
		}

		public static Vector3 kNoPoint;

		private CustomBlendable mCustom0;

		private CustomBlendable mCustom1;

		private CustomBlendable mCustom2;

		private CustomBlendable mCustom3;

		private List<CustomBlendable> m_CustomOverflow;

		public LensSettings Lens { get; set; }

		public Vector3 ReferenceUp { get; set; }

		public Vector3 ReferenceLookAt { get; set; }

		public bool HasLookAt => false;

		public Vector3 RawPosition { get; set; }

		public Quaternion RawOrientation { get; set; }

		internal Vector3 PositionDampingBypass { get; set; }

		public float ShotQuality { get; set; }

		public Vector3 PositionCorrection { get; set; }

		public Quaternion OrientationCorrection { get; set; }

		public Vector3 CorrectedPosition => default;

		public Quaternion CorrectedOrientation => default;

		public Vector3 FinalPosition => default;

		public Quaternion FinalOrientation => default;

		public static CameraState Default => default;

		public int NumCustomBlendables { get; private set; }

		public CustomBlendable GetCustomBlendable(int index)
		{
			return default;
		}

		private int FindCustomBlendable(Object custom)
		{
			return 0;
		}

		public void AddCustomBlendable(CustomBlendable b)
		{
		}

		public static CameraState Lerp(CameraState stateA, CameraState stateB, float t)
		{
			return default;
		}

		private float InterpolateFOV(float fovA, float fovB, float dA, float dB, float t)
		{
			return 0f;
		}
	}
}

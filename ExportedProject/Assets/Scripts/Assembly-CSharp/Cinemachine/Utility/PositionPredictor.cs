using UnityEngine;

namespace Cinemachine.Utility
{
	internal class PositionPredictor
	{
		private Vector3 m_Position;

		private const float kSmoothingDefault = 10f;

		private float mSmoothing;

		private GaussianWindow1D_Vector3 m_Velocity;

		private GaussianWindow1D_Vector3 m_Accel;

		public float Smoothing
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public bool IsEmpty => false;

		public void Reset()
		{
		}

		public void AddPosition(Vector3 pos)
		{
		}

		public Vector3 PredictPosition(float lookaheadTime)
		{
			return default;
		}
	}
}

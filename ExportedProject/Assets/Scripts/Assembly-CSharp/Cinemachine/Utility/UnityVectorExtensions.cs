using UnityEngine;

namespace Cinemachine.Utility
{
	public static class UnityVectorExtensions
	{
		public const float Epsilon = 0.0001f;

		public static float ClosestPointOnSegment(this Vector3 p, Vector3 s0, Vector3 s1)
		{
			return 0f;
		}

		public static float ClosestPointOnSegment(this Vector2 p, Vector2 s0, Vector2 s1)
		{
			return 0f;
		}

		public static Vector3 ProjectOntoPlane(this Vector3 vector, Vector3 planeNormal)
		{
			return default;
		}

		public static bool AlmostZero(this Vector3 v)
		{
			return false;
		}

		public static float SignedAngle(Vector3 from, Vector3 to, Vector3 refNormal)
		{
			return 0f;
		}

		public static Vector3 SlerpWithReferenceUp(Vector3 vA, Vector3 vB, float t, Vector3 up)
		{
			return default;
		}
	}
}

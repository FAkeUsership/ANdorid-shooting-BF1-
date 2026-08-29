using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MB3_UVTransformUtility
	{
		public static void Test()
		{
		}

		public static float TransformX(DRect r, double x)
		{
			return 0f;
		}

		public static DRect CombineTransforms(ref DRect r1, ref DRect r2)
		{
			return default;
		}

		public static Rect CombineTransforms(ref Rect r1, ref Rect r2)
		{
			return default;
		}

		public static DRect InverseTransform(ref DRect t)
		{
			return default;
		}

		public static DRect GetShiftTransformToFitBinA(ref DRect A, ref DRect B)
		{
			return default;
		}

		public static DRect GetEncapsulatingRectShifted(ref DRect uvRect1, ref DRect willBeIn)
		{
			return default;
		}

		public static DRect GetEncapsulatingRect(ref DRect uvRect1, ref DRect uvRect2)
		{
			return default;
		}

		public static bool RectContainsShifted(ref DRect bucket, ref DRect tryFit)
		{
			return false;
		}

		public static bool RectContainsShifted(ref Rect bucket, ref Rect tryFit)
		{
			return false;
		}

		public static bool RectContains(ref DRect bigRect, ref DRect smallToTestIfFits)
		{
			return false;
		}

		public static bool RectContains(ref Rect bigRect, ref Rect smallToTestIfFits)
		{
			return false;
		}

		public static Vector2 TransformPoint(ref DRect r, Vector2 p)
		{
			return default;
		}

		public static DVector2 TransformPoint(ref DRect r, DVector2 p)
		{
			return default;
		}
	}
}

using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public struct DRect
	{
		public double x;

		public double y;

		public double width;

		public double height;

		public DVector2 minD => default;

		public DVector2 maxD => default;

		public Vector2 min => default;

		public Vector2 max => default;

		public Vector2 size => default;

		public DVector2 center => default;

		public DRect(Rect r)
		{
			x = 0.0;
			y = 0.0;
			width = 0.0;
			height = 0.0;
		}

		public DRect(Vector2 o, Vector2 s)
		{
			x = 0.0;
			y = 0.0;
			width = 0.0;
			height = 0.0;
		}

		public DRect(DRect r)
		{
			x = 0.0;
			y = 0.0;
			width = 0.0;
			height = 0.0;
		}

		public DRect(float xx, float yy, float w, float h)
		{
			x = 0.0;
			y = 0.0;
			width = 0.0;
			height = 0.0;
		}

		public DRect(double xx, double yy, double w, double h)
		{
			x = 0.0;
			y = 0.0;
			width = 0.0;
			height = 0.0;
		}

		public Rect GetRect()
		{
			return default;
		}

		public override bool Equals(object obj)
		{
			return false;
		}

		public static bool operator ==(DRect a, DRect b)
		{
			return false;
		}

		public static bool operator !=(DRect a, DRect b)
		{
			return false;
		}

		public override string ToString()
		{
			return null;
		}

		public void Expand(float amt)
		{
		}

		public bool Encloses(DRect smallToTestIfFits)
		{
			return false;
		}

		public override int GetHashCode()
		{
			return 0;
		}
	}
}

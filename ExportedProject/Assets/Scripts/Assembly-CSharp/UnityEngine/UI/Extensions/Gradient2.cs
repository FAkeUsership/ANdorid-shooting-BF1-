namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Effects/Extensions/Gradient2")]
	public class Gradient2 : BaseMeshEffect
	{
		public enum Type
		{
			Horizontal = 0,
			Vertical = 1,
			Radial = 2,
			Diamond = 3
		}

		public enum Blend
		{
			Override = 0,
			Add = 1,
			Multiply = 2
		}

		[SerializeField]
		private Type _gradientType;

		[SerializeField]
		private Blend _blendMode;

		[SerializeField]
		[Range(-1f, 1f)]
		private float _offset;

		[SerializeField]
		private UnityEngine.Gradient _effectGradient;

		public Blend BlendMode
		{
			get
			{
				return Blend.Override;
			}
			set
			{
			}
		}

		public UnityEngine.Gradient EffectGradient
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Type GradientType
		{
			get
			{
				return Type.Horizontal;
			}
			set
			{
			}
		}

		public float Offset
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public override void ModifyMesh(VertexHelper helper)
		{
		}

		private Color BlendColor(Color colorA, Color colorB)
		{
			return default;
		}
	}
}

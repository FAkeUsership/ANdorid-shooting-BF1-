namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(Text), typeof(RectTransform))]
	[AddComponentMenu("UI/Effects/Extensions/Cylinder Text")]
	public class CylinderText : BaseMeshEffect
	{
		public float radius;

		private RectTransform rectTrans;

		protected override void Awake()
		{
		}

		protected override void OnEnable()
		{
		}

		public override void ModifyMesh(VertexHelper vh)
		{
		}
	}
}

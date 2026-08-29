namespace UnityEngine.UI.Extensions.Examples
{
	[RequireComponent(typeof(UILineRenderer))]
	public class LineRendererOrbit : MonoBehaviour
	{
		private UILineRenderer lr;

		private Circle circle;

		public GameObject OrbitGO;

		private RectTransform orbitGOrt;

		private float orbitTime;

		[SerializeField]
		private float _xAxis;

		[SerializeField]
		private float _yAxis;

		[SerializeField]
		private int _steps;

		public float xAxis
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public float yAxis
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public int Steps
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		private void Awake()
		{
		}

		private void Update()
		{
		}

		private void GenerateOrbit()
		{
		}

		private void OnValidate()
		{
		}
	}
}

using UnityEngine.EventSystems;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("Event/VR Input Module")]
	public class VRInputModule : BaseInputModule
	{
		public static GameObject targetObject;

		private static VRInputModule _singleton;

		private int counter;

		private static bool mouseClicked;

		public static Vector3 cursorPosition;

		protected override void Awake()
		{
		}

		public override void Process()
		{
		}

		public static void PointerSubmit(GameObject obj)
		{
		}

		public static void PointerExit(GameObject obj)
		{
		}

		public static void PointerEnter(GameObject obj)
		{
		}
	}
}

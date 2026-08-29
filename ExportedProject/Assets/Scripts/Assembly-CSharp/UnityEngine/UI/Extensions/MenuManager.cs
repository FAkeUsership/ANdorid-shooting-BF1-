using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	[AddComponentMenu("UI/Extensions/Menu Manager")]
	[DisallowMultipleComponent]
	public class MenuManager : MonoBehaviour
	{
		public Menu[] MenuScreens;

		public int StartScreen;

		private Stack<Menu> menuStack;

		public static MenuManager Instance { get; set; }

		private void Awake()
		{
		}

		private void OnDestroy()
		{
		}

		public void CreateInstance<T>() where T : Menu
		{
		}

		public void CreateInstance(string MenuName)
		{
		}

		public void OpenMenu(Menu instance)
		{
		}

		private GameObject GetPrefab(string PrefabName)
		{
			return null;
		}

		private T GetPrefab<T>() where T : Menu
		{
			return null;
		}

		public void CloseMenu(Menu menu)
		{
		}

		public void CloseTopMenu()
		{
		}

		private void Update()
		{
		}
	}
}

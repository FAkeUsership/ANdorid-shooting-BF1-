using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(InputField))]
	[AddComponentMenu("UI/Extensions/Input Field Submit")]
	public class InputFieldEnterSubmit : MonoBehaviour
	{
		[Serializable]
		public class EnterSubmitEvent : UnityEvent<string>
		{
		}

		public EnterSubmitEvent EnterSubmit;

		private InputField _input;

		private void Awake()
		{
		}

		public void OnEndEdit(string txt)
		{
		}
	}
}

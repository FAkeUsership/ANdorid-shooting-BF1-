using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

public class MB_DynamicAddDeleteExample : MonoBehaviour
{
	[CompilerGenerated]
	private sealed class _003ClargeNumber_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public MB_DynamicAddDeleteExample _003C_003E4__this;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003ClargeNumber_003Ed__6(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	public GameObject prefab;

	private List<GameObject> objsInCombined;

	private MB3_MeshBaker mbd;

	private GameObject[] objs;

	private float GaussianValue()
	{
		return 0f;
	}

	private void Start()
	{
	}

	[IteratorStateMachine(typeof(_003ClargeNumber_003Ed__6))]
	private IEnumerator largeNumber()
	{
		return null;
	}

	private void OnGUI()
	{
	}
}

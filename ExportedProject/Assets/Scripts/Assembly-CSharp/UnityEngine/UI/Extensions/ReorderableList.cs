using System;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[DisallowMultipleComponent]
	[AddComponentMenu("UI/Extensions/Re-orderable list")]
	[RequireComponent(typeof(RectTransform))]
	public class ReorderableList : MonoBehaviour
	{
		[Serializable]
		public struct ReorderableListEventStruct
		{
			public GameObject DroppedObject;

			public int FromIndex;

			public ReorderableList FromList;

			public bool IsAClone;

			public GameObject SourceObject;

			public int ToIndex;

			public ReorderableList ToList;

			public void Cancel()
			{
			}
		}

		[Serializable]
		public class ReorderableListHandler : UnityEvent<ReorderableListEventStruct>
		{
		}

		[Tooltip("Child container with re-orderable items in a layout group")]
		public LayoutGroup ContentLayout;

		[Tooltip("Parent area to draw the dragged element on top of containers. Defaults to the root Canvas")]
		public RectTransform DraggableArea;

		[Tooltip("Can items be dragged from the container?")]
		public bool IsDraggable;

		[Tooltip("Should the draggable components be removed or cloned?")]
		public bool CloneDraggedObject;

		[Tooltip("Can new draggable items be dropped in to the container?")]
		public bool IsDropable;

		[Header("UI Re-orderable Events")]
		public ReorderableListHandler OnElementDropped;

		public ReorderableListHandler OnElementGrabbed;

		public ReorderableListHandler OnElementRemoved;

		public ReorderableListHandler OnElementAdded;

		private RectTransform _content;

		private ReorderableListContent _listContent;

		public RectTransform Content => null;

		private Canvas GetCanvas()
		{
			return null;
		}

		private void Awake()
		{
		}

		public void TestReOrderableListTarget(ReorderableListEventStruct item)
		{
		}
	}
}

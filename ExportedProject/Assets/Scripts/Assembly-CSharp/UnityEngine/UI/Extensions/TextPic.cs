using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace UnityEngine.UI.Extensions
{
	[ExecuteInEditMode]
	[AddComponentMenu("UI/Extensions/TextPic")]
	public class TextPic : Text, IPointerClickHandler, IEventSystemHandler, IPointerExitHandler, IPointerEnterHandler, ISelectHandler
	{
		[Serializable]
		public struct IconName
		{
			public string name;

			public Sprite sprite;

			public Vector2 offset;

			public Vector2 scale;
		}

		[Serializable]
		public class HrefClickEvent : UnityEvent<string>
		{
		}

		private class HrefInfo
		{
			public int startIndex;

			public int endIndex;

			public string name;

			public readonly List<Rect> boxes;
		}

		private readonly List<Image> m_ImagesPool;

		private readonly List<GameObject> culled_ImagesPool;

		private bool clearImages;

		private Object thisLock;

		private readonly List<int> m_ImagesVertexIndex;

		private static readonly Regex s_Regex;

		private string fixedString;

		[SerializeField]
		[Tooltip("Allow click events to be received by parents, (default) blocks")]
		private bool m_ClickParents;

		private string m_OutputText;

		public IconName[] inspectorIconList;

		[Tooltip("Global scaling factor for all images")]
		public float ImageScalingFactor;

		public string hyperlinkColor;

		[SerializeField]
		public Vector2 imageOffset;

		private Button button;

		private Selectable highlightselectable;

		private List<Vector2> positions;

		private string previousText;

		public bool isCreating_m_HrefInfos;

		private readonly List<HrefInfo> m_HrefInfos;

		private static readonly StringBuilder s_TextBuilder;

		private static readonly Regex s_HrefRegex;

		[SerializeField]
		private HrefClickEvent m_OnHrefClick;

		public bool AllowClickParents
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public HrefClickEvent onHrefClick
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public override void SetVerticesDirty()
		{
		}

		private new void Start()
		{
		}

		protected void UpdateQuadImage()
		{
		}

		protected override void OnPopulateMesh(VertexHelper toFill)
		{
		}

		protected string GetOutputText()
		{
			return null;
		}

		public void OnPointerClick(PointerEventData eventData)
		{
		}

		public void OnPointerEnter(PointerEventData eventData)
		{
		}

		public void OnPointerExit(PointerEventData eventData)
		{
		}

		public void OnSelect(BaseEventData eventData)
		{
		}

		private void Update()
		{
		}

		private void Reset_m_HrefInfos()
		{
		}
	}
}

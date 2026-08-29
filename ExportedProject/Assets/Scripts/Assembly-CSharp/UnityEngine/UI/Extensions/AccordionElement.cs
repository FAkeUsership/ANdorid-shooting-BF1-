using System;
using UnityEngine.UI.Extensions.Tweens;

namespace UnityEngine.UI.Extensions
{
	[RequireComponent(typeof(RectTransform), typeof(LayoutElement))]
	[AddComponentMenu("UI/Extensions/Accordion/Accordion Element")]
	public class AccordionElement : Toggle
	{
		[SerializeField]
		private float m_MinHeight;

		private Accordion m_Accordion;

		private RectTransform m_RectTransform;

		private LayoutElement m_LayoutElement;

		[NonSerialized]
		private readonly TweenRunner<FloatTween> m_FloatTweenRunner;

		protected AccordionElement()
		{
		}

		protected override void Awake()
		{
		}

		public void OnValueChanged(bool state)
		{
		}

		protected float GetExpandedHeight()
		{
			return 0f;
		}

		protected void StartTween(float startFloat, float targetFloat)
		{
		}

		protected void SetHeight(float height)
		{
		}
	}
}

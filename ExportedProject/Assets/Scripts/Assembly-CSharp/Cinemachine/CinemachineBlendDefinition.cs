using System;
using UnityEngine;

namespace Cinemachine
{
	[Serializable]
	[DocumentationSorting(10.2f, DocumentationSortingAttribute.Level.UserRef)]
	public struct CinemachineBlendDefinition
	{
		[DocumentationSorting(10.21f, DocumentationSortingAttribute.Level.UserRef)]
		public enum Style
		{
			Cut = 0,
			EaseInOut = 1,
			EaseIn = 2,
			EaseOut = 3,
			HardIn = 4,
			HardOut = 5,
			Linear = 6
		}

		[Tooltip("Shape of the blend curve")]
		public Style m_Style;

		[Tooltip("Duration of the blend, in seconds")]
		public float m_Time;

		public AnimationCurve BlendCurve => null;

		public CinemachineBlendDefinition(Style style, float time)
		{
			m_Style = Style.Cut;
			m_Time = 0f;
		}
	}
}

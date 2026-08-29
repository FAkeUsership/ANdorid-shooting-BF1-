using System;
using UnityEngine;

namespace Cinemachine
{
	[Serializable]
	[DocumentationSorting(10f, DocumentationSortingAttribute.Level.UserRef)]
	public sealed class CinemachineBlenderSettings : ScriptableObject
	{
		[Serializable]
		[DocumentationSorting(10.1f, DocumentationSortingAttribute.Level.UserRef)]
		public struct CustomBlend
		{
			[Tooltip("When blending from this camera")]
			public string m_From;

			[Tooltip("When blending to this camera")]
			public string m_To;

			[Tooltip("Blend curve definition")]
			public CinemachineBlendDefinition m_Blend;
		}

		[Tooltip("The array containing explicitly defined blends between two Virtual Cameras")]
		public CustomBlend[] m_CustomBlends;

		public const string kBlendFromAnyCameraLabel = "**ANY CAMERA**";

		public AnimationCurve GetBlendCurveForVirtualCameras(string fromCameraName, string toCameraName, AnimationCurve defaultCurve)
		{
			return null;
		}
	}
}

using UnityEngine;

namespace Cinemachine
{
	[SaveDuringPlay]
	[RequireComponent(typeof(CinemachinePipeline))]
	[AddComponentMenu(null)]
	[DocumentationSorting(23f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachinePOV : CinemachineComponentBase
	{
		[Tooltip("The Vertical axis.  Value is -90..90. Controls the vertical orientation")]
		public AxisState m_VerticalAxis;

		[Tooltip("The Horizontal axis.  Value is -180..180.  Controls the horizontal orientation")]
		public AxisState m_HorizontalAxis;

		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		private void OnValidate()
		{
		}

		private void OnEnable()
		{
		}

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}
	}
}

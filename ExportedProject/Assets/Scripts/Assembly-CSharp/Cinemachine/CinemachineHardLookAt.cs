using UnityEngine;

namespace Cinemachine
{
	[DocumentationSorting(23f, DocumentationSortingAttribute.Level.UserRef)]
	[AddComponentMenu(null)]
	[RequireComponent(typeof(CinemachinePipeline))]
	[SaveDuringPlay]
	public class CinemachineHardLookAt : CinemachineComponentBase
	{
		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}
	}
}

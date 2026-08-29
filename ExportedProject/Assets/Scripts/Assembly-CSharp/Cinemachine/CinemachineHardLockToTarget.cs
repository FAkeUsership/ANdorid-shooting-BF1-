using UnityEngine;

namespace Cinemachine
{
	[AddComponentMenu(null)]
	[RequireComponent(typeof(CinemachinePipeline))]
	[SaveDuringPlay]
	[DocumentationSorting(23f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineHardLockToTarget : CinemachineComponentBase
	{
		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}
	}
}

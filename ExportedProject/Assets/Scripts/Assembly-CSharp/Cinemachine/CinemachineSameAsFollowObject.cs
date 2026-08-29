using UnityEngine;

namespace Cinemachine
{
	[SaveDuringPlay]
	[RequireComponent(typeof(CinemachinePipeline))]
	[AddComponentMenu(null)]
	[DocumentationSorting(27f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineSameAsFollowObject : CinemachineComponentBase
	{
		public override bool IsValid => false;

		public override CinemachineCore.Stage Stage => CinemachineCore.Stage.Body;

		public override void MutateCameraState(ref CameraState curState, float deltaTime)
		{
		}
	}
}

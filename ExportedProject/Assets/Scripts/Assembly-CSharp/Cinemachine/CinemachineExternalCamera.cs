using UnityEngine;

namespace Cinemachine
{
	[AddComponentMenu("Cinemachine/CinemachineExternalCamera")]
	[ExecuteInEditMode]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Camera))]
	[DocumentationSorting(14f, DocumentationSortingAttribute.Level.UserRef)]
	public class CinemachineExternalCamera : CinemachineVirtualCameraBase
	{
		[Tooltip("The object that the camera is looking at.  Setting this will improve the quality of the blends to and from this camera")]
		[NoSaveDuringPlay]
		public Transform m_LookAt;

		private Camera m_Camera;

		private CameraState m_State;

		public override CameraState State => default;

		public override Transform LookAt
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public override Transform Follow { get; set; }

		public override void UpdateCameraState(Vector3 worldUp, float deltaTime)
		{
		}
	}
}

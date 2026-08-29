using System.Collections.Generic;
using UnityEngine;

namespace Cinemachine
{
	[ExecuteInEditMode]
	[AddComponentMenu(null)]
	[DocumentationSorting(22f, DocumentationSortingAttribute.Level.UserRef)]
	[SaveDuringPlay]
	public class CinemachineConfiner : CinemachineExtension
	{
		public enum Mode
		{
			Confine2D = 0,
			Confine3D = 1
		}

		private class VcamExtraState
		{
			public Vector3 m_previousDisplacement;

			public float confinerDisplacement;
		}

		[Tooltip("The confiner can operate using a 2D bounding shape or a 3D bounding volume")]
		public Mode m_ConfineMode;

		[Tooltip("The volume within which the camera is to be contained")]
		public Collider m_BoundingVolume;

		[Tooltip("The 2D shape within which the camera is to be contained")]
		public Collider2D m_BoundingShape2D;

		[Tooltip("If camera is orthographic, screen edges will be confined to the volume.  If not checked, then only the camera center will be confined")]
		public bool m_ConfineScreenEdges;

		[Range(0f, 10f)]
		[Tooltip("How gradually to return the camera to the bounding volume if it goes beyond the borders.  Higher numbers are more gradual.")]
		public float m_Damping;

		private List<List<Vector2>> m_pathCache;

		public bool IsValid => false;

		public bool CameraWasDisplaced(CinemachineVirtualCameraBase vcam)
		{
			return false;
		}

		private void OnValidate()
		{
		}

		protected override void PostPipelineStageCallback(CinemachineVirtualCameraBase vcam, CinemachineCore.Stage stage, ref CameraState state, float deltaTime)
		{
		}

		public void InvalidatePathCache()
		{
		}

		private bool ValidatePathCache()
		{
			return false;
		}

		private Vector3 ConfinePoint(Vector3 camPos)
		{
			return default;
		}

		private Vector3 ConfineScreenEdges(CinemachineVirtualCameraBase vcam, ref CameraState state)
		{
			return default;
		}
	}
}

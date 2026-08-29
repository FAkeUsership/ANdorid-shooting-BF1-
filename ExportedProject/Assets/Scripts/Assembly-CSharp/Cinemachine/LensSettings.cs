using System;
using UnityEngine;

namespace Cinemachine
{
	[Serializable]
	[DocumentationSorting(2f, DocumentationSortingAttribute.Level.UserRef)]
	public struct LensSettings
	{
		public static LensSettings Default;

		[Tooltip("This is the camera view in vertical degrees. For cinematic people, a 50mm lens on a super-35mm sensor would equal a 19.6 degree FOV")]
		[Range(1f, 179f)]
		public float FieldOfView;

		[Tooltip("When using an orthographic camera, this defines the half-height, in world coordinates, of the camera view.")]
		public float OrthographicSize;

		[Tooltip("This defines the near region in the renderable range of the camera frustum. Raising this value will stop the game from drawing things near the camera, which can sometimes come in handy.  Larger values will also increase your shadow resolution.")]
		public float NearClipPlane;

		[Tooltip("This defines the far region of the renderable range of the camera frustum. Typically you want to set this value as low as possible without cutting off desired distant objects")]
		public float FarClipPlane;

		[Tooltip("Camera Z roll, or tilt, in degrees.")]
		[Range(-180f, 180f)]
		public float Dutch;

		internal bool Orthographic { get; set; }

		internal float Aspect { get; set; }

		public static LensSettings FromCamera(Camera fromCamera)
		{
			return default;
		}

		public LensSettings(float fov, float orthographicSize, float nearClip, float farClip, float dutch, bool ortho, float aspect)
		{
			FieldOfView = 0f;
			OrthographicSize = 0f;
			NearClipPlane = 0f;
			FarClipPlane = 0f;
			Dutch = 0f;
			Orthographic = false;
			Aspect = 0f;
		}

		public static LensSettings Lerp(LensSettings lensA, LensSettings lensB, float t)
		{
			return default;
		}

		public void Validate()
		{
		}
	}
}

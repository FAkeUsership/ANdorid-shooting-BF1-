using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MBVersionConcrete : MBVersionInterface
	{
		private Vector2 _HALF_UV;

		public string version()
		{
			return null;
		}

		public int GetMajorVersion()
		{
			return 0;
		}

		public int GetMinorVersion()
		{
			return 0;
		}

		public bool GetActive(GameObject go)
		{
			return false;
		}

		public void SetActive(GameObject go, bool isActive)
		{
		}

		public void SetActiveRecursively(GameObject go, bool isActive)
		{
		}

		public UnityEngine.Object[] FindSceneObjectsOfType(Type t)
		{
			return null;
		}

		public void OptimizeMesh(Mesh m)
		{
		}

		public bool IsRunningAndMeshNotReadWriteable(Mesh m)
		{
			return false;
		}

		public Vector2[] GetMeshUV1s(Mesh m, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		public Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		public void MeshClear(Mesh m, bool t)
		{
		}

		public void MeshAssignUV3(Mesh m, Vector2[] uv3s)
		{
		}

		public void MeshAssignUV4(Mesh m, Vector2[] uv4s)
		{
		}

		public Vector4 GetLightmapTilingOffset(Renderer r)
		{
			return default;
		}

		public Transform[] GetBones(Renderer r)
		{
			return null;
		}

		public int GetBlendShapeFrameCount(Mesh m, int shapeIndex)
		{
			return 0;
		}

		public float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex)
		{
			return 0f;
		}

		public void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
		}

		public void ClearBlendShapes(Mesh m)
		{
		}

		public void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
		}

		public int MaxMeshVertexCount()
		{
			return 0;
		}

		public void SetMeshIndexFormatAndClearMesh(Mesh m, int numVerts, bool vertices, bool justClearTriangles)
		{
		}
	}
}

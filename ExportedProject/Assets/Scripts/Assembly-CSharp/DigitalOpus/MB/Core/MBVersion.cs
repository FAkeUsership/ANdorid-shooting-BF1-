using System;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MBVersion
	{
		private static MBVersionInterface _MBVersion;

		private static MBVersionInterface _CreateMBVersionConcrete()
		{
			return null;
		}

		public static string version()
		{
			return null;
		}

		public static int GetMajorVersion()
		{
			return 0;
		}

		public static int GetMinorVersion()
		{
			return 0;
		}

		public static bool GetActive(GameObject go)
		{
			return false;
		}

		public static void SetActive(GameObject go, bool isActive)
		{
		}

		public static void SetActiveRecursively(GameObject go, bool isActive)
		{
		}

		public static UnityEngine.Object[] FindSceneObjectsOfType(Type t)
		{
			return null;
		}

		public static bool IsRunningAndMeshNotReadWriteable(Mesh m)
		{
			return false;
		}

		public static Vector2[] GetMeshUV3orUV4(Mesh m, bool get3, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		public static void MeshClear(Mesh m, bool t)
		{
		}

		public static void MeshAssignUV3(Mesh m, Vector2[] uv3s)
		{
		}

		public static void MeshAssignUV4(Mesh m, Vector2[] uv4s)
		{
		}

		public static Vector4 GetLightmapTilingOffset(Renderer r)
		{
			return default;
		}

		public static Transform[] GetBones(Renderer r)
		{
			return null;
		}

		public static void OptimizeMesh(Mesh m)
		{
		}

		public static int GetBlendShapeFrameCount(Mesh m, int shapeIndex)
		{
			return 0;
		}

		public static float GetBlendShapeFrameWeight(Mesh m, int shapeIndex, int frameIndex)
		{
			return 0f;
		}

		public static void GetBlendShapeFrameVertices(Mesh m, int shapeIndex, int frameIndex, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
		}

		public static void ClearBlendShapes(Mesh m)
		{
		}

		public static void AddBlendShapeFrame(Mesh m, string nm, float wt, Vector3[] vs, Vector3[] ns, Vector3[] ts)
		{
		}

		public static int MaxMeshVertexCount()
		{
			return 0;
		}

		public static void SetMeshIndexFormatAndClearMesh(Mesh m, int numVerts, bool vertices, bool justClearTriangles)
		{
		}
	}
}

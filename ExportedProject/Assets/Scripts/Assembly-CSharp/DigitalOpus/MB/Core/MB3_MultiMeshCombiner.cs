using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public class MB3_MultiMeshCombiner : MB3_MeshCombiner
	{
		[Serializable]
		public class CombinedMesh
		{
			public MB3_MeshCombinerSingle combinedMesh;

			public int extraSpace;

			public int numVertsInListToDelete;

			public int numVertsInListToAdd;

			public List<GameObject> gosToAdd;

			public List<int> gosToDelete;

			public List<GameObject> gosToUpdate;

			public bool isDirty;

			public CombinedMesh(int maxNumVertsInMesh, GameObject resultSceneObject, MB2_LogLevel ll)
			{
			}

			public bool isEmpty()
			{
				return false;
			}
		}

		private static GameObject[] empty;

		private static int[] emptyIDs;

		public Dictionary<int, CombinedMesh> obj2MeshCombinerMap;

		[SerializeField]
		public List<CombinedMesh> meshCombiners;

		[SerializeField]
		private int _maxVertsInMesh;

		public override MB2_LogLevel LOG_LEVEL
		{
			get
			{
				return MB2_LogLevel.none;
			}
			set
			{
			}
		}

		public override MB2_ValidationLevel validationLevel
		{
			get
			{
				return MB2_ValidationLevel.none;
			}
			set
			{
			}
		}

		public int maxVertsInMesh
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public override int GetNumObjectsInCombined()
		{
			return 0;
		}

		public override int GetNumVerticesFor(GameObject go)
		{
			return 0;
		}

		public override int GetNumVerticesFor(int gameObjectID)
		{
			return 0;
		}

		public override List<GameObject> GetObjectsInCombined()
		{
			return null;
		}

		public override int GetLightmapIndex()
		{
			return 0;
		}

		public override bool CombinedMeshContains(GameObject go)
		{
			return false;
		}

		private bool _validateTextureBakeResults()
		{
			return false;
		}

		public override void Apply(GenerateUV2Delegate uv2GenerationMethod)
		{
		}

		public override void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapesFlag = false, GenerateUV2Delegate uv2GenerationMethod = null)
		{
		}

		public override void UpdateSkinnedMeshApproximateBounds()
		{
		}

		public override void UpdateSkinnedMeshApproximateBoundsFromBones()
		{
		}

		public override void UpdateSkinnedMeshApproximateBoundsFromBounds()
		{
		}

		public override void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV2 = false, bool updateUV3 = false, bool updateUV4 = false, bool updateColors = false, bool updateSkinningInfo = false)
		{
		}

		public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true)
		{
			return false;
		}

		public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource = true)
		{
			return false;
		}

		private bool _validate(GameObject[] gos, int[] deleteGOinstanceIDs)
		{
			return false;
		}

		private void _distributeAmongBakers(GameObject[] gos, int[] deleteGOinstanceIDs)
		{
		}

		private bool _bakeStep1(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource)
		{
			return false;
		}

		public override Dictionary<MBBlendShapeKey, MBBlendShapeValue> BuildSourceBlendShapeToCombinedIndexMap()
		{
			return null;
		}

		public override void ClearBuffers()
		{
		}

		public override void ClearMesh()
		{
		}

		public override void DestroyMesh()
		{
		}

		public override void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods)
		{
		}

		private void _setMBValues(MB3_MeshCombinerSingle targ)
		{
		}

		public override List<Material> GetMaterialsOnTargetRenderer()
		{
			return null;
		}

		public override void CheckIntegrity()
		{
		}
	}
}

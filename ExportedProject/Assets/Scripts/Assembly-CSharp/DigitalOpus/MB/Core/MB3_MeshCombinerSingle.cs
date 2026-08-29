using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public class MB3_MeshCombinerSingle : MB3_MeshCombiner
	{
		[Serializable]
		public class SerializableIntArray
		{
			public int[] data;

			public SerializableIntArray()
			{
			}

			public SerializableIntArray(int len)
			{
			}
		}

		[Serializable]
		public class MB_DynamicGameObject : IComparable<MB_DynamicGameObject>
		{
			public int instanceID;

			public string name;

			public int vertIdx;

			public int blendShapeIdx;

			public int numVerts;

			public int numBlendShapes;

			public int[] indexesOfBonesUsed;

			public int lightmapIndex;

			public Vector4 lightmapTilingOffset;

			public Vector3 meshSize;

			public bool show;

			public bool invertTriangles;

			public int[] submeshTriIdxs;

			public int[] submeshNumTris;

			public int[] targetSubmeshIdxs;

			public Rect[] uvRects;

			public Rect[] encapsulatingRect;

			public Rect[] sourceMaterialTiling;

			public Rect[] obUVRects;

			public bool _beingDeleted;

			public int _triangleIdxAdjustment;

			[NonSerialized]
			public SerializableIntArray[] _tmpSubmeshTris;

			[NonSerialized]
			public Transform[] _tmpCachedBones;

			[NonSerialized]
			public Matrix4x4[] _tmpCachedBindposes;

			[NonSerialized]
			public BoneWeight[] _tmpCachedBoneWeights;

			[NonSerialized]
			public int[] _tmpIndexesOfSourceBonesUsed;

			public int CompareTo(MB_DynamicGameObject b)
			{
				return 0;
			}
		}

		public class MeshChannels
		{
			public Vector3[] vertices;

			public Vector3[] normals;

			public Vector4[] tangents;

			public Vector2[] uv0raw;

			public Vector2[] uv0modified;

			public Vector2[] uv2;

			public Vector2[] uv3;

			public Vector2[] uv4;

			public Color[] colors;

			public BoneWeight[] boneWeights;

			public Matrix4x4[] bindPoses;

			public int[] triangles;

			public MBBlendShape[] blendShapes;
		}

		[Serializable]
		public class MBBlendShapeFrame
		{
			public float frameWeight;

			public Vector3[] vertices;

			public Vector3[] normals;

			public Vector3[] tangents;
		}

		[Serializable]
		public class MBBlendShape
		{
			public int gameObjectID;

			public string name;

			public int indexInSource;

			public MBBlendShapeFrame[] frames;
		}

		public class MeshChannelsCache
		{
			private MB3_MeshCombinerSingle mc;

			protected Dictionary<int, MeshChannels> meshID2MeshChannels;

			private Vector2 _HALF_UV;

			public MeshChannelsCache(MB3_MeshCombinerSingle mcs)
			{
			}

			internal Vector3[] GetVertices(Mesh m)
			{
				return null;
			}

			internal Vector3[] GetNormals(Mesh m)
			{
				return null;
			}

			internal Vector4[] GetTangents(Mesh m)
			{
				return null;
			}

			internal Vector2[] GetUv0Raw(Mesh m)
			{
				return null;
			}

			internal Vector2[] GetUv0Modified(Mesh m)
			{
				return null;
			}

			internal Vector2[] GetUv2(Mesh m)
			{
				return null;
			}

			internal Vector2[] GetUv3(Mesh m)
			{
				return null;
			}

			internal Vector2[] GetUv4(Mesh m)
			{
				return null;
			}

			internal Color[] GetColors(Mesh m)
			{
				return null;
			}

			internal Matrix4x4[] GetBindposes(Renderer r)
			{
				return null;
			}

			internal BoneWeight[] GetBoneWeights(Renderer r, int numVertsInMeshBeingAdded)
			{
				return null;
			}

			internal int[] GetTriangles(Mesh m)
			{
				return null;
			}

			internal MBBlendShape[] GetBlendShapes(Mesh m, int gameObjectID)
			{
				return null;
			}

			private Color[] _getMeshColors(Mesh m)
			{
				return null;
			}

			private Vector3[] _getMeshNormals(Mesh m)
			{
				return null;
			}

			private Vector4[] _getMeshTangents(Mesh m)
			{
				return null;
			}

			private Vector2[] _getMeshUVs(Mesh m)
			{
				return null;
			}

			private Vector2[] _getMeshUV2s(Mesh m)
			{
				return null;
			}

			public static Matrix4x4[] _getBindPoses(Renderer r)
			{
				return null;
			}

			public static BoneWeight[] _getBoneWeights(Renderer r, int numVertsInMeshBeingAdded)
			{
				return null;
			}

			private void _generateTangents(int[] triangles, Vector3[] verts, Vector2[] uvs, Vector3[] normals, Vector4[] outTangents)
			{
			}
		}

		public struct BoneAndBindpose
		{
			public Transform bone;

			public Matrix4x4 bindPose;

			public BoneAndBindpose(Transform t, Matrix4x4 bp)
			{
				bone = null;
				bindPose = default;
			}

			public override bool Equals(object obj)
			{
				return false;
			}

			public override int GetHashCode()
			{
				return 0;
			}
		}

		[SerializeField]
		protected List<GameObject> objectsInCombinedMesh;

		[SerializeField]
		private int lightmapIndex;

		[SerializeField]
		private List<MB_DynamicGameObject> mbDynamicObjectsInCombinedMesh;

		private Dictionary<int, MB_DynamicGameObject> _instance2combined_map;

		[SerializeField]
		private Vector3[] verts;

		[SerializeField]
		private Vector3[] normals;

		[SerializeField]
		private Vector4[] tangents;

		[SerializeField]
		private Vector2[] uvs;

		[SerializeField]
		private Vector2[] uv2s;

		[SerializeField]
		private Vector2[] uv3s;

		[SerializeField]
		private Vector2[] uv4s;

		[SerializeField]
		private Color[] colors;

		[SerializeField]
		private Matrix4x4[] bindPoses;

		[SerializeField]
		private Transform[] bones;

		[SerializeField]
		internal MBBlendShape[] blendShapes;

		[SerializeField]
		internal MBBlendShape[] blendShapesInCombined;

		[SerializeField]
		private SerializableIntArray[] submeshTris;

		[SerializeField]
		private Mesh _mesh;

		private BoneWeight[] boneWeights;

		private GameObject[] empty;

		private int[] emptyIDs;

		public override MB2_TextureBakeResults textureBakeResults
		{
			set
			{
			}
		}

		public override MB_RenderType renderType
		{
			set
			{
			}
		}

		public override GameObject resultSceneObject
		{
			set
			{
			}
		}

		private MB_DynamicGameObject instance2Combined_MapGet(int gameObjectID)
		{
			return null;
		}

		private void instance2Combined_MapAdd(int gameObjectID, MB_DynamicGameObject dgo)
		{
		}

		private void instance2Combined_MapRemove(int gameObjectID)
		{
		}

		private bool instance2Combined_MapTryGetValue(int gameObjectID, out MB_DynamicGameObject dgo)
		{
			dgo = null;
			return false;
		}

		private int instance2Combined_MapCount()
		{
			return 0;
		}

		private void instance2Combined_MapClear()
		{
		}

		private bool instance2Combined_MapContainsKey(int gameObjectID)
		{
			return false;
		}

		public override int GetNumObjectsInCombined()
		{
			return 0;
		}

		public override List<GameObject> GetObjectsInCombined()
		{
			return null;
		}

		public Mesh GetMesh()
		{
			return null;
		}

		public Transform[] GetBones()
		{
			return null;
		}

		public override int GetLightmapIndex()
		{
			return 0;
		}

		public override int GetNumVerticesFor(GameObject go)
		{
			return 0;
		}

		public override int GetNumVerticesFor(int instanceID)
		{
			return 0;
		}

		public override Dictionary<MBBlendShapeKey, MBBlendShapeValue> BuildSourceBlendShapeToCombinedIndexMap()
		{
			return null;
		}

		private void _initialize(int numResultMats)
		{
		}

		private bool _collectMaterialTriangles(Mesh m, MB_DynamicGameObject dgo, Material[] sharedMaterials, OrderedDictionary sourceMats2submeshIdx_map)
		{
			return false;
		}

		private bool _collectOutOfBoundsUVRects2(Mesh m, MB_DynamicGameObject dgo, Material[] sharedMaterials, OrderedDictionary sourceMats2submeshIdx_map, Dictionary<int, MB_Utility.MeshAnalysisResult[]> meshAnalysisResults, MeshChannelsCache meshChannelCache)
		{
			return false;
		}

		private bool _validateTextureBakeResults()
		{
			return false;
		}

		private bool _validateMeshFlags()
		{
			return false;
		}

		private bool _showHide(GameObject[] goToShow, GameObject[] goToHide)
		{
			return false;
		}

		private bool _addToCombined(GameObject[] goToAdd, int[] goToDelete, bool disableRendererInSource)
		{
			return false;
		}

		private void _copyAndAdjustUVsFromMesh(MB_DynamicGameObject dgo, Mesh mesh, int vertsIdx, MeshChannelsCache meshChannelsCache)
		{
		}

		private void _copyAndAdjustUV2FromMesh(MB_DynamicGameObject dgo, Mesh mesh, int vertsIdx, MeshChannelsCache meshChannelsCache)
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

		private int _getNumBones(Renderer r)
		{
			return 0;
		}

		private Transform[] _getBones(Renderer r)
		{
			return null;
		}

		public override void Apply(GenerateUV2Delegate uv2GenerationMethod)
		{
		}

		public virtual void ApplyShowHide()
		{
		}

		public override void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapesFlag = false, GenerateUV2Delegate uv2GenerationMethod = null)
		{
		}

		private int _numNonZeroLengthSubmeshTris(SerializableIntArray[] subTris)
		{
			return 0;
		}

		private void _updateMaterialsOnTargetRenderer(SerializableIntArray[] subTris, int numNonZeroLengthSubmeshTris)
		{
		}

		public SerializableIntArray[] GetSubmeshTrisWithShowHideApplied()
		{
			return null;
		}

		public override void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV2 = false, bool updateUV3 = false, bool updateUV4 = false, bool updateColors = false, bool updateSkinningInfo = false)
		{
		}

		private void _updateGameObjects(GameObject[] gos, bool recalcBounds, bool updateVertices, bool updateNormals, bool updateTangents, bool updateUV, bool updateUV2, bool updateUV3, bool updateUV4, bool updateColors, bool updateSkinningInfo)
		{
		}

		private void _updateGameObject(GameObject go, bool updateVertices, bool updateNormals, bool updateTangents, bool updateUV, bool updateUV2, bool updateUV3, bool updateUV4, bool updateColors, bool updateSkinningInfo, MeshChannelsCache meshChannelCache)
		{
		}

		public bool ShowHideGameObjects(GameObject[] toShow, GameObject[] toHide)
		{
			return false;
		}

		public override bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true)
		{
			return false;
		}

		public override bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource)
		{
			return false;
		}

		public override bool CombinedMeshContains(GameObject go)
		{
			return false;
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

		public bool ValidateTargRendererAndMeshAndResultSceneObj()
		{
			return false;
		}

		internal static Renderer BuildSceneHierarchPreBake(MB3_MeshCombinerSingle mom, GameObject root, Mesh m, bool createNewChild = false, GameObject[] objsToBeAdded = null)
		{
			return null;
		}

		public static void BuildPrefabHierarchy(MB3_MeshCombinerSingle mom, GameObject instantiatedPrefabRoot, Mesh m, bool createNewChild = false, GameObject[] objsToBeAdded = null)
		{
		}

		private static void _ConfigureSceneHierarch(MB3_MeshCombinerSingle mom, GameObject root, MeshRenderer mr, MeshFilter mf, SkinnedMeshRenderer smr, Mesh m, GameObject[] objsToBeAdded = null)
		{
		}

		public void BuildSceneMeshObject(GameObject[] gos = null, bool createNewChild = false)
		{
		}

		private bool IsMirrored(Matrix4x4 tm)
		{
			return false;
		}

		public override void CheckIntegrity()
		{
		}

		private void _ZeroArray(Vector3[] arr, int idx, int length)
		{
		}

		private List<MB_DynamicGameObject>[] _buildBoneIdx2dgoMap()
		{
			return null;
		}

		private void _CollectBonesToAddForDGO(MB_DynamicGameObject dgo, Dictionary<Transform, int> bone2idx, HashSet<int> boneIdxsToDelete, HashSet<BoneAndBindpose> bonesToAdd, Renderer r, MeshChannelsCache meshChannelCache)
		{
		}

		private void _CopyBonesWeAreKeepingToNewBonesArrayAndAdjustBWIndexes(HashSet<int> boneIdxsToDeleteHS, HashSet<BoneAndBindpose> bonesToAdd, Transform[] nbones, Matrix4x4[] nbindPoses, BoneWeight[] nboneWeights, int totalDeleteVerts)
		{
		}

		private void _AddBonesToNewBonesArrayAndAdjustBWIndexes(MB_DynamicGameObject dgo, Renderer r, int vertsIdx, Transform[] nbones, BoneWeight[] nboneWeights, MeshChannelsCache meshChannelCache)
		{
		}

		private void _copyUV2unchangedToSeparateRects()
		{
		}

		public override List<Material> GetMaterialsOnTargetRenderer()
		{
			return null;
		}
	}
}

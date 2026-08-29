using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public abstract class MB3_MeshCombiner
	{
		public delegate void GenerateUV2Delegate(Mesh m, float hardAngle, float packMargin);

		public class MBBlendShapeKey
		{
			public int gameObjecID;

			public int blendShapeIndexInSrc;

			public MBBlendShapeKey(int srcSkinnedMeshRenderGameObjectID, int blendShapeIndexInSource)
			{
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

		public class MBBlendShapeValue
		{
			public GameObject combinedMeshGameObject;

			public int blendShapeIndex;
		}

		[SerializeField]
		protected MB2_LogLevel _LOG_LEVEL;

		[SerializeField]
		protected MB2_ValidationLevel _validationLevel;

		[SerializeField]
		protected string _name;

		[SerializeField]
		protected MB2_TextureBakeResults _textureBakeResults;

		[SerializeField]
		protected GameObject _resultSceneObject;

		[SerializeField]
		protected Renderer _targetRenderer;

		[SerializeField]
		protected MB_RenderType _renderType;

		[SerializeField]
		protected MB2_OutputOptions _outputOption;

		[SerializeField]
		protected MB2_LightmapOptions _lightmapOption;

		[SerializeField]
		protected bool _doNorm;

		[SerializeField]
		protected bool _doTan;

		[SerializeField]
		protected bool _doCol;

		[SerializeField]
		protected bool _doUV;

		[SerializeField]
		protected bool _doUV3;

		[SerializeField]
		protected bool _doUV4;

		[SerializeField]
		protected bool _doBlendShapes;

		[SerializeField]
		protected bool _recenterVertsToBoundsCenter;

		[SerializeField]
		public bool _optimizeAfterBake;

		[SerializeField]
		public float uv2UnwrappingParamsHardAngle;

		[SerializeField]
		public float uv2UnwrappingParamsPackMargin;

		protected bool _usingTemporaryTextureBakeResult;

		public static bool EVAL_VERSION => false;

		public virtual MB2_LogLevel LOG_LEVEL
		{
			get
			{
				return MB2_LogLevel.none;
			}
			set
			{
			}
		}

		public virtual MB2_ValidationLevel validationLevel
		{
			get
			{
				return MB2_ValidationLevel.none;
			}
			set
			{
			}
		}

		public string name
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual MB2_TextureBakeResults textureBakeResults
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual GameObject resultSceneObject
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual Renderer targetRenderer
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public virtual MB_RenderType renderType
		{
			get
			{
				return MB_RenderType.meshRenderer;
			}
			set
			{
			}
		}

		public virtual MB2_OutputOptions outputOption
		{
			get
			{
				return MB2_OutputOptions.bakeIntoSceneObject;
			}
			set
			{
			}
		}

		public virtual MB2_LightmapOptions lightmapOption
		{
			get
			{
				return MB2_LightmapOptions.preserve_current_lightmapping;
			}
			set
			{
			}
		}

		public virtual bool doNorm
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doTan
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doCol
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doUV
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doUV1
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doUV3
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doUV4
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doBlendShapes
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool recenterVertsToBoundsCenter
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool optimizeAfterBake
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public virtual bool doUV2()
		{
			return false;
		}

		public abstract int GetLightmapIndex();

		public abstract void ClearBuffers();

		public abstract void ClearMesh();

		public abstract void DestroyMesh();

		public abstract void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods);

		public abstract List<GameObject> GetObjectsInCombined();

		public abstract int GetNumObjectsInCombined();

		public abstract int GetNumVerticesFor(GameObject go);

		public abstract int GetNumVerticesFor(int instanceID);

		public abstract Dictionary<MBBlendShapeKey, MBBlendShapeValue> BuildSourceBlendShapeToCombinedIndexMap();

		public virtual void Apply()
		{
		}

		public abstract void Apply(GenerateUV2Delegate uv2GenerationMethod);

		public abstract void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapeFlag = false, GenerateUV2Delegate uv2GenerationMethod = null);

		public abstract void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV2 = false, bool updateUV3 = false, bool updateUV4 = false, bool updateColors = false, bool updateSkinningInfo = false);

		public abstract bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true);

		public abstract bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource);

		public abstract bool CombinedMeshContains(GameObject go);

		public abstract void UpdateSkinnedMeshApproximateBounds();

		public abstract void UpdateSkinnedMeshApproximateBoundsFromBones();

		public abstract void CheckIntegrity();

		public abstract void UpdateSkinnedMeshApproximateBoundsFromBounds();

		public static void UpdateSkinnedMeshApproximateBoundsFromBonesStatic(Transform[] bs, SkinnedMeshRenderer smr)
		{
		}

		public static void UpdateSkinnedMeshApproximateBoundsFromBoundsStatic(List<GameObject> objectsInCombined, SkinnedMeshRenderer smr)
		{
		}

		protected virtual bool _CreateTemporaryTextrueBakeResult(GameObject[] gos, List<Material> matsOnTargetRenderer)
		{
			return false;
		}

		public abstract List<Material> GetMaterialsOnTargetRenderer();
	}
}

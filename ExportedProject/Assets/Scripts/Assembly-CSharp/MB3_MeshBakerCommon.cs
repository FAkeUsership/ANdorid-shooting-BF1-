using System.Collections.Generic;
using DigitalOpus.MB.Core;
using UnityEngine;

public abstract class MB3_MeshBakerCommon : MB3_MeshBakerRoot
{
	public List<GameObject> objsToMesh;

	public bool useObjsToMeshFromTexBaker;

	public bool clearBuffersAfterBake;

	public string bakeAssetsInPlaceFolderPath;

	[HideInInspector]
	public GameObject resultPrefab;

	public abstract MB3_MeshCombiner meshCombiner { get; }

	public override MB2_TextureBakeResults textureBakeResults
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public override List<GameObject> GetObjectsToCombine()
	{
		return null;
	}

	public void EnableDisableSourceObjectRenderers(bool show)
	{
	}

	public virtual void ClearMesh()
	{
	}

	public virtual void DestroyMesh()
	{
	}

	public virtual void DestroyMeshEditor(MB2_EditorMethodsInterface editorMethods)
	{
	}

	public virtual int GetNumObjectsInCombined()
	{
		return 0;
	}

	public virtual int GetNumVerticesFor(GameObject go)
	{
		return 0;
	}

	public MB3_TextureBaker GetTextureBaker()
	{
		return null;
	}

	public abstract bool AddDeleteGameObjects(GameObject[] gos, GameObject[] deleteGOs, bool disableRendererInSource = true);

	public abstract bool AddDeleteGameObjectsByID(GameObject[] gos, int[] deleteGOinstanceIDs, bool disableRendererInSource = true);

	public virtual void Apply(MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
	{
	}

	public virtual void Apply(bool triangles, bool vertices, bool normals, bool tangents, bool uvs, bool uv2, bool uv3, bool uv4, bool colors, bool bones = false, bool blendShapesFlag = false, MB3_MeshCombiner.GenerateUV2Delegate uv2GenerationMethod = null)
	{
	}

	public virtual bool CombinedMeshContains(GameObject go)
	{
		return false;
	}

	public virtual void UpdateGameObjects(GameObject[] gos, bool recalcBounds = true, bool updateVertices = true, bool updateNormals = true, bool updateTangents = true, bool updateUV = false, bool updateUV1 = false, bool updateUV2 = false, bool updateColors = false, bool updateSkinningInfo = false)
	{
	}

	public virtual void UpdateSkinnedMeshApproximateBounds()
	{
	}

	public virtual void UpdateSkinnedMeshApproximateBoundsFromBones()
	{
	}

	public virtual void UpdateSkinnedMeshApproximateBoundsFromBounds()
	{
	}

	protected virtual bool _ValidateForUpdateSkinnedMeshBounds()
	{
		return false;
	}
}

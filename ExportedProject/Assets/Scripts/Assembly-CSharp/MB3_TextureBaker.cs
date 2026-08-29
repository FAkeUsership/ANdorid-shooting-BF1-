using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using DigitalOpus.MB.Core;
using UnityEngine;

public class MB3_TextureBaker : MB3_MeshBakerRoot
{
	public delegate void OnCombinedTexturesCoroutineSuccess();

	public delegate void OnCombinedTexturesCoroutineFail();

	public class CreateAtlasesCoroutineResult
	{
		public bool success;

		public bool isFinished;
	}

	[CompilerGenerated]
	private sealed class _003CCreateAtlasesCoroutine_003Ed__78 : IEnumerator<object>, IEnumerator, IDisposable
	{
		private int _003C_003E1__state;

		private object _003C_003E2__current;

		public CreateAtlasesCoroutineResult coroutineResult;

		public MB3_TextureBaker _003C_003E4__this;

		public float maxTimePerFrame;

		public bool saveAtlasesAsAssets;

		public ProgressUpdateDelegate progressInfo;

		public MB2_EditorMethodsInterface editorMethods;

		private MB3_TextureCombiner _003Ccombiner_003E5__2;

		private int _003Ci_003E5__3;

		private MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult _003CcoroutineResult2_003E5__4;

		object IEnumerator<object>.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		object IEnumerator.Current
		{
			[DebuggerHidden]
			get
			{
				return null;
			}
		}

		[DebuggerHidden]
		public _003CCreateAtlasesCoroutine_003Ed__78(int _003C_003E1__state)
		{
		}

		[DebuggerHidden]
		void IDisposable.Dispose()
		{
		}

		private bool MoveNext()
		{
			return false;
		}

		bool IEnumerator.MoveNext()
		{
			//ILSpy generated this explicit interface implementation from .override directive in MoveNext
			return this.MoveNext();
		}

		[DebuggerHidden]
		void IEnumerator.Reset()
		{
		}
	}

	public MB2_LogLevel LOG_LEVEL;

	[SerializeField]
	protected MB2_TextureBakeResults _textureBakeResults;

	[SerializeField]
	protected int _atlasPadding;

	[SerializeField]
	protected int _maxAtlasSize;

	[SerializeField]
	protected bool _resizePowerOfTwoTextures;

	[SerializeField]
	protected bool _fixOutOfBoundsUVs;

	[SerializeField]
	protected int _maxTilingBakeSize;

	[SerializeField]
	protected MB2_PackingAlgorithmEnum _packingAlgorithm;

	[SerializeField]
	protected bool _meshBakerTexturePackerForcePowerOfTwo;

	[SerializeField]
	protected List<ShaderTextureProperty> _customShaderProperties;

	[SerializeField]
	protected List<string> _customShaderPropNames_Depricated;

	[SerializeField]
	protected bool _doMultiMaterial;

	[SerializeField]
	protected bool _doMultiMaterialSplitAtlasesIfTooBig;

	[SerializeField]
	protected bool _doMultiMaterialSplitAtlasesIfOBUVs;

	[SerializeField]
	protected Material _resultMaterial;

	[SerializeField]
	protected bool _considerNonTextureProperties;

	[SerializeField]
	protected bool _doSuggestTreatment;

	private CreateAtlasesCoroutineResult _coroutineResult;

	public MB_MultiMaterial[] resultMaterials;

	public List<GameObject> objsToMesh;

	public OnCombinedTexturesCoroutineSuccess onBuiltAtlasesSuccess;

	public OnCombinedTexturesCoroutineFail onBuiltAtlasesFail;

	public MB_AtlasesAndRects[] OnCombinedTexturesCoroutineAtlasesAndRects;

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

	public virtual int atlasPadding
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public virtual int maxAtlasSize
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public virtual bool resizePowerOfTwoTextures
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool fixOutOfBoundsUVs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual int maxTilingBakeSize
	{
		get
		{
			return 0;
		}
		set
		{
		}
	}

	public virtual MB2_PackingAlgorithmEnum packingAlgorithm
	{
		get
		{
			return MB2_PackingAlgorithmEnum.UnitysPackTextures;
		}
		set
		{
		}
	}

	public bool meshBakerTexturePackerForcePowerOfTwo
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual List<ShaderTextureProperty> customShaderProperties
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual List<string> customShaderPropNames
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public virtual bool doMultiMaterial
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool doMultiMaterialSplitAtlasesIfTooBig
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual bool doMultiMaterialSplitAtlasesIfOBUVs
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public virtual Material resultMaterial
	{
		get
		{
			return null;
		}
		set
		{
		}
	}

	public bool considerNonTextureProperties
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public bool doSuggestTreatment
	{
		get
		{
			return false;
		}
		set
		{
		}
	}

	public CreateAtlasesCoroutineResult CoroutineResult => null;

	public override List<GameObject> GetObjectsToCombine()
	{
		return null;
	}

	public MB_AtlasesAndRects[] CreateAtlases()
	{
		return null;
	}

	[IteratorStateMachine(typeof(_003CCreateAtlasesCoroutine_003Ed__78))]
	public IEnumerator CreateAtlasesCoroutine(ProgressUpdateDelegate progressInfo, CreateAtlasesCoroutineResult coroutineResult, bool saveAtlasesAsAssets = false, MB2_EditorMethodsInterface editorMethods = null, float maxTimePerFrame = 0.01f)
	{
		return null;
	}

	public MB_AtlasesAndRects[] CreateAtlases(ProgressUpdateDelegate progressInfo, bool saveAtlasesAsAssets = false, MB2_EditorMethodsInterface editorMethods = null)
	{
		return null;
	}

	private void unpackMat2RectMap(MB2_TextureBakeResults tbr)
	{
	}

	public MB3_TextureCombiner CreateAndConfigureTextureCombiner()
	{
		return null;
	}

	public static void ConfigureNewMaterialToMatchOld(Material newMat, Material original)
	{
	}

	private string PrintSet(HashSet<Material> s)
	{
		return null;
	}

	private bool _ValidateResultMaterials()
	{
		return false;
	}
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public class MB3_TextureCombiner
	{
		public class CombineTexturesIntoAtlasesCoroutineResult
		{
			public bool success;

			public bool isFinished;
		}

		[CompilerGenerated]
		private sealed class _003CCombineTexturesIntoAtlasesCoroutine_003Ed__52 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public CombineTexturesIntoAtlasesCoroutineResult coroutineResult;

			public float maxTimePerFrame;

			public MB3_TextureCombiner _003C_003E4__this;

			public ProgressUpdateDelegate progressInfo;

			public MB_AtlasesAndRects resultAtlasesAndRects;

			public Material resultMaterial;

			public List<GameObject> objsToMesh;

			public List<Material> allowedMaterialsFilter;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public List<AtlasPackingResult> packingResults;

			public bool onlyPackRects;

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
			public _003CCombineTexturesIntoAtlasesCoroutine_003Ed__52(int _003C_003E1__state)
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

		[CompilerGenerated]
		private sealed class _003C_CombineTexturesIntoAtlases_003Ed__53 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MB3_TextureCombiner _003C_003E4__this;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public List<GameObject> objsToMesh;

			public CombineTexturesIntoAtlasesCoroutineResult result;

			public ProgressUpdateDelegate progressInfo;

			public Material resultMaterial;

			public List<Material> allowedMaterialsFilter;

			public bool onlyPackRects;

			public List<AtlasPackingResult> atlasPackingResult;

			public MB_AtlasesAndRects resultAtlasesAndRects;

			private Stopwatch _003Csw_003E5__2;

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
			public _003C_CombineTexturesIntoAtlases_003Ed__53(int _003C_003E1__state)
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

			private void _003C_003Em__Finally1()
			{
			}

			[DebuggerHidden]
			void IEnumerator.Reset()
			{
			}
		}

		[CompilerGenerated]
		private sealed class _003C__CombineTexturesIntoAtlases_003Ed__55 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MB3_TextureCombiner _003C_003E4__this;

			public MB3_TextureCombinerPipeline.TexturePipelineData data;

			public ProgressUpdateDelegate progressInfo;

			public CombineTexturesIntoAtlasesCoroutineResult result;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public MB_AtlasesAndRects resultAtlasesAndRects;

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
			public _003C__CombineTexturesIntoAtlases_003Ed__55(int _003C_003E1__state)
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

		[CompilerGenerated]
		private sealed class _003C__RunTexturePacker_003Ed__56 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MB3_TextureCombiner _003C_003E4__this;

			public MB3_TextureCombinerPipeline.TexturePipelineData data;

			public CombineTexturesIntoAtlasesCoroutineResult result;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public List<AtlasPackingResult> packingResult;

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
			public _003C__RunTexturePacker_003Ed__56(int _003C_003E1__state)
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
		protected bool _saveAtlasesAsAssets;

		[SerializeField]
		protected MB2_PackingAlgorithmEnum _packingAlgorithm;

		[SerializeField]
		protected bool _meshBakerTexturePackerForcePowerOfTwo;

		[SerializeField]
		protected List<ShaderTextureProperty> _customShaderPropNames;

		[SerializeField]
		protected bool _normalizeTexelDensity;

		[SerializeField]
		protected bool _considerNonTextureProperties;

		internal List<Texture2D> _temporaryTextures;

		internal List<ProceduralMaterialInfo> _proceduralMaterials;

		public static bool _RunCorutineWithoutPauseIsRunning;

		public MB2_TextureBakeResults textureBakeResults
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public int atlasPadding
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public int maxAtlasSize
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public bool resizePowerOfTwoTextures
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool fixOutOfBoundsUVs
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public int maxTilingBakeSize
		{
			get
			{
				return 0;
			}
			set
			{
			}
		}

		public bool saveAtlasesAsAssets
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public MB2_PackingAlgorithmEnum packingAlgorithm
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

		public List<ShaderTextureProperty> customShaderPropNames
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

		public static void RunCorutineWithoutPause(IEnumerator cor, int recursionDepth)
		{
		}

		public bool CombineTexturesIntoAtlases(ProgressUpdateDelegate progressInfo, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods = null, List<AtlasPackingResult> packingResults = null, bool onlyPackRects = false)
		{
			return false;
		}

		[IteratorStateMachine(typeof(_003CCombineTexturesIntoAtlasesCoroutine_003Ed__52))]
		public IEnumerator CombineTexturesIntoAtlasesCoroutine(ProgressUpdateDelegate progressInfo, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods = null, CombineTexturesIntoAtlasesCoroutineResult coroutineResult = null, float maxTimePerFrame = 0.01f, List<AtlasPackingResult> packingResults = null, bool onlyPackRects = false)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003C_CombineTexturesIntoAtlases_003Ed__53))]
		private IEnumerator _CombineTexturesIntoAtlases(ProgressUpdateDelegate progressInfo, CombineTexturesIntoAtlasesCoroutineResult result, MB_AtlasesAndRects resultAtlasesAndRects, Material resultMaterial, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, MB2_EditorMethodsInterface textureEditorMethods, List<AtlasPackingResult> atlasPackingResult, bool onlyPackRects)
		{
			return null;
		}

		private MB3_TextureCombinerPipeline.TexturePipelineData LoadPipelineData(Material resultMaterial, List<ShaderTextureProperty> texPropertyNames, List<GameObject> objsToMesh, List<Material> allowedMaterialsFilter, List<MB_TexSet> distinctMaterialTextures)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003C__CombineTexturesIntoAtlases_003Ed__55))]
		private IEnumerator __CombineTexturesIntoAtlases(ProgressUpdateDelegate progressInfo, CombineTexturesIntoAtlasesCoroutineResult result, MB_AtlasesAndRects resultAtlasesAndRects, MB3_TextureCombinerPipeline.TexturePipelineData data, MB2_EditorMethodsInterface textureEditorMethods)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003C__RunTexturePacker_003Ed__56))]
		private IEnumerator __RunTexturePacker(CombineTexturesIntoAtlasesCoroutineResult result, MB3_TextureCombinerPipeline.TexturePipelineData data, MB2_EditorMethodsInterface textureEditorMethods, List<AtlasPackingResult> packingResult)
		{
			return null;
		}

		public Texture2D _createTemporaryTexture(int w, int h, TextureFormat texFormat, bool mipMaps)
		{
			return null;
		}

		internal Texture2D _createTextureCopy(Texture2D t)
		{
			return null;
		}

		internal Texture2D _resizeTexture(Texture2D t, int w, int h)
		{
			return null;
		}

		internal void _destroyTemporaryTextures()
		{
		}

		public void _restoreProceduralMaterials()
		{
		}

		public void SuggestTreatment(List<GameObject> objsToMesh, Material[] resultMaterials, List<ShaderTextureProperty> _customShaderPropNames)
		{
		}

		private string PrintList(List<GameObject> gos)
		{
			return null;
		}
	}
}

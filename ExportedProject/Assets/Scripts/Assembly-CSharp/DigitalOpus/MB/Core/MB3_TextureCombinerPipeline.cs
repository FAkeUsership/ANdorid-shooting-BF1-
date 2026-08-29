using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	public class MB3_TextureCombinerPipeline
	{
		public struct CreateAtlasForProperty
		{
			public bool allTexturesAreNull;

			public bool allTexturesAreSame;

			public bool allNonTexturePropsAreSame;

			public override string ToString()
			{
				return null;
			}
		}

		internal class TexturePipelineData
		{
			internal MB2_TextureBakeResults _textureBakeResults;

			internal int _atlasPadding;

			internal int _maxAtlasSize;

			internal bool _resizePowerOfTwoTextures;

			internal bool _fixOutOfBoundsUVs;

			internal int _maxTilingBakeSize;

			internal bool _saveAtlasesAsAssets;

			internal MB2_PackingAlgorithmEnum _packingAlgorithm;

			internal bool _meshBakerTexturePackerForcePowerOfTwo;

			internal List<ShaderTextureProperty> _customShaderPropNames;

			internal bool _normalizeTexelDensity;

			internal bool _considerNonTextureProperties;

			internal MB3_TextureCombinerNonTextureProperties nonTexturePropertyBlender;

			internal List<MB_TexSet> distinctMaterialTextures;

			internal List<GameObject> allObjsToMesh;

			internal List<Material> allowedMaterialsFilter;

			internal List<ShaderTextureProperty> texPropertyNames;

			internal CreateAtlasForProperty[] allTexturesAreNullAndSameColor;

			internal int numAtlases;

			internal Material resultMaterial;
		}

		[CompilerGenerated]
		private sealed class _003CCalculateIdealSizesForTexturesInAtlasAndPadding_003Ed__7 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TexturePipelineData data;

			public MB2_LogLevel LOG_LEVEL;

			public MB3_TextureCombiner combiner;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public ProgressUpdateDelegate progressInfo;

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
			public _003CCalculateIdealSizesForTexturesInAtlasAndPadding_003Ed__7(int _003C_003E1__state)
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
		private sealed class _003C__Step1_CollectDistinctMatTexturesAndUsedObjects_003Ed__6 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TexturePipelineData data;

			public ProgressUpdateDelegate progressInfo;

			public MB2_LogLevel LOG_LEVEL;

			public MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public List<GameObject> usedObjsToMesh;

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
			public _003C__Step1_CollectDistinctMatTexturesAndUsedObjects_003Ed__6(int _003C_003E1__state)
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
		private sealed class _003C__Step3_BuildAndSaveAtlasesAndStoreResults_003Ed__10 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public TexturePipelineData data;

			public MB2_LogLevel LOG_LEVEL;

			public MB_ITextureCombinerPacker packer;

			public ProgressUpdateDelegate progressInfo;

			public MB3_TextureCombiner combiner;

			public AtlasPackingResult atlasPackingResult;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public StringBuilder report;

			public MB_AtlasesAndRects resultAtlasesAndRects;

			private Stopwatch _003Csw_003E5__2;

			private Texture2D[] _003Catlases_003E5__3;

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
			public _003C__Step3_BuildAndSaveAtlasesAndStoreResults_003Ed__10(int _003C_003E1__state)
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

		public static ShaderTextureProperty[] shaderTexPropertyNames;

		internal static bool _ShouldWeCreateAtlasForThisProperty(int propertyIndex, bool considerNonTextureProperties, CreateAtlasForProperty[] allTexturesAreNullAndSameColor)
		{
			return false;
		}

		internal static bool _CollectPropertyNames(TexturePipelineData data, MB2_LogLevel LOG_LEVEL)
		{
			return false;
		}

		internal static bool _ShouldWeCreateAtlasForThisProperty(int propertyIndex, CreateAtlasForProperty[] allTexturesAreNullAndSameColor, TexturePipelineData data)
		{
			return false;
		}

		[IteratorStateMachine(typeof(_003C__Step1_CollectDistinctMatTexturesAndUsedObjects_003Ed__6))]
		internal static IEnumerator __Step1_CollectDistinctMatTexturesAndUsedObjects(ProgressUpdateDelegate progressInfo, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, TexturePipelineData data, MB3_TextureCombiner combiner, MB2_EditorMethodsInterface textureEditorMethods, List<GameObject> usedObjsToMesh, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003CCalculateIdealSizesForTexturesInAtlasAndPadding_003Ed__7))]
		internal static IEnumerator CalculateIdealSizesForTexturesInAtlasAndPadding(ProgressUpdateDelegate progressInfo, MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, TexturePipelineData data, MB3_TextureCombiner combiner, MB2_EditorMethodsInterface textureEditorMethods, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		internal static AtlasPackingResult[] __Step3_RunTexturePacker(TexturePipelineData data, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		internal static MB_ITextureCombinerPacker CreatePacker(MB2_PackingAlgorithmEnum packingAlgorithm)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003C__Step3_BuildAndSaveAtlasesAndStoreResults_003Ed__10))]
		internal static IEnumerator __Step3_BuildAndSaveAtlasesAndStoreResults(MB3_TextureCombiner.CombineTexturesIntoAtlasesCoroutineResult result, ProgressUpdateDelegate progressInfo, TexturePipelineData data, MB3_TextureCombiner combiner, MB_ITextureCombinerPacker packer, AtlasPackingResult atlasPackingResult, MB2_EditorMethodsInterface textureEditorMethods, MB_AtlasesAndRects resultAtlasesAndRects, StringBuilder report, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		internal static StringBuilder GenerateReport(TexturePipelineData data)
		{
			return null;
		}

		internal static AtlasPackingResult[] __RuntTexturePackerOnly(TexturePipelineData data, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		internal static MB2_TexturePacker CreateTexturePacker(MB2_PackingAlgorithmEnum _packingAlgorithm)
		{
			return null;
		}

		internal static Vector2 GetAdjustedForScaleAndOffset2Dimensions(MeshBakerMaterialTexture source, Vector2 obUVoffset, Vector2 obUVscale, TexturePipelineData data, MB2_LogLevel LOG_LEVEL)
		{
			return default;
		}

		internal static DRect GetSourceSamplingRect(MeshBakerMaterialTexture source, Vector2 obUVoffset, Vector2 obUVscale)
		{
			return default;
		}

		internal static Color32 ConvertNormalFormatFromUnity_ToStandard(Color32 c)
		{
			return default;
		}

		internal static float GetSubmeshArea(Mesh m, int submeshIdx)
		{
			return 0f;
		}

		internal static bool IsPowerOfTwo(int x)
		{
			return false;
		}
	}
}

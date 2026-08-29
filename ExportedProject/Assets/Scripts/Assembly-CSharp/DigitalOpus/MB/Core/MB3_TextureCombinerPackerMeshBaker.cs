using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	internal class MB3_TextureCombinerPackerMeshBaker : MB_ITextureCombinerPacker
	{
		[CompilerGenerated]
		private sealed class _003CCopyScaledAndTiledToAtlas_003Ed__1 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public MeshBakerMaterialTexture source;

			public MB2_LogLevel LOG_LEVEL;

			public int targX;

			public int targY;

			public int targW;

			public int targH;

			public DRect srcSamplingRect;

			public MB3_TextureCombiner combiner;

			public MB3_TextureCombinerPipeline.TexturePipelineData data;

			public MB_TexSet sourceMaterial;

			public ShaderTextureProperty shaderPropertyName;

			public ProgressUpdateDelegate progressInfo;

			public Color[][] atlasPixels;

			public AtlasPadding padding;

			private int _003Cw_003E5__2;

			private int _003Ch_003E5__3;

			private int _003Ci_003E5__4;

			private int _003Cj_003E5__5;

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
			public _003CCopyScaledAndTiledToAtlas_003Ed__1(int _003C_003E1__state)
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
		private sealed class _003CCreateAtlases_003Ed__0 : IEnumerator<object>, IEnumerator, IDisposable
		{
			private int _003C_003E1__state;

			private object _003C_003E2__current;

			public AtlasPackingResult packedAtlasRects;

			public MB2_LogLevel LOG_LEVEL;

			public MB3_TextureCombinerPipeline.TexturePipelineData data;

			public Texture2D[] atlases;

			public ProgressUpdateDelegate progressInfo;

			public MB2_EditorMethodsInterface textureEditorMethods;

			public MB3_TextureCombiner combiner;

			private Rect[] _003CuvRects_003E5__2;

			private int _003CatlasSizeX_003E5__3;

			private int _003CatlasSizeY_003E5__4;

			private int _003CpropIdx_003E5__5;

			private Color[][] _003CatlasPixels_003E5__6;

			private bool _003CisNormalMap_003E5__7;

			private int _003CtexSetIdx_003E5__8;

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
			public _003CCreateAtlases_003Ed__0(int _003C_003E1__state)
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

		[IteratorStateMachine(typeof(_003CCreateAtlases_003Ed__0))]
		public IEnumerator CreateAtlases(ProgressUpdateDelegate progressInfo, MB3_TextureCombinerPipeline.TexturePipelineData data, MB3_TextureCombiner combiner, AtlasPackingResult packedAtlasRects, Texture2D[] atlases, MB2_EditorMethodsInterface textureEditorMethods, MB2_LogLevel LOG_LEVEL)
		{
			return null;
		}

		[IteratorStateMachine(typeof(_003CCopyScaledAndTiledToAtlas_003Ed__1))]
		internal static IEnumerator CopyScaledAndTiledToAtlas(MeshBakerMaterialTexture source, MB_TexSet sourceMaterial, ShaderTextureProperty shaderPropertyName, DRect srcSamplingRect, int targX, int targY, int targW, int targH, AtlasPadding padding, Color[][] atlasPixels, bool isNormalMap, MB3_TextureCombinerPipeline.TexturePipelineData data, MB3_TextureCombiner combiner, ProgressUpdateDelegate progressInfo = null, MB2_LogLevel LOG_LEVEL = MB2_LogLevel.info)
		{
			return null;
		}
	}
}

using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public abstract class MB3_MeshBakerGrouperCore
	{
		public GrouperData d;

		public abstract Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection);

		public abstract void DrawGizmos(Bounds sourceObjectBounds);

		public void DoClustering(MB3_TextureBaker tb, MB3_MeshBakerGrouper grouper)
		{
		}

		private Dictionary<int, List<Renderer>> GroupByLightmapIndex(List<Renderer> gaws)
		{
			return null;
		}

		private void AddMeshBaker(MB3_TextureBaker tb, string key, List<Renderer> gaws)
		{
		}
	}
}

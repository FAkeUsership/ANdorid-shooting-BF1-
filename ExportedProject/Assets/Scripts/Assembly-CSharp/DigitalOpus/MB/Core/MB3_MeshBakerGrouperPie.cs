using System;
using System.Collections.Generic;
using UnityEngine;

namespace DigitalOpus.MB.Core
{
	[Serializable]
	public class MB3_MeshBakerGrouperPie : MB3_MeshBakerGrouperCore
	{
		public MB3_MeshBakerGrouperPie(GrouperData data)
		{
		}

		public override Dictionary<string, List<Renderer>> FilterIntoGroups(List<GameObject> selection)
		{
			return null;
		}

		public override void DrawGizmos(Bounds sourceObjectBounds)
		{
		}

		public static void DrawCircle(Vector3 axis, Vector3 center, float radius, int subdiv)
		{
		}
	}
}

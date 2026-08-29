using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Cinemachine.Timeline
{
	public sealed class CinemachineShot : PlayableAsset, IPropertyPreview
	{
		public ExposedReference<CinemachineVirtualCameraBase> VirtualCamera;

		public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
		{
			return default;
		}

		public void GatherProperties(PlayableDirector director, IPropertyCollector driver)
		{
		}
	}
}

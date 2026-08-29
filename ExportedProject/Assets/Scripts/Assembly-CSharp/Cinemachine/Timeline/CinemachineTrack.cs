using System;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

namespace Cinemachine.Timeline
{
	[Serializable]
	[TrackBindingType(typeof(CinemachineBrain))]
	[TrackColor(0.53f, 0f, 0.08f)]
	[TrackClipType(typeof(CinemachineShot))]
	public class CinemachineTrack : TrackAsset
	{
		public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
		{
			return default;
		}
	}
}

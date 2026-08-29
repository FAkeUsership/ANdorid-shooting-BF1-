using UnityEngine.Playables;

namespace Cinemachine.Timeline
{
	public sealed class CinemachineMixer : PlayableBehaviour
	{
		private CinemachineBrain mBrain;

		private int mBrainOverrideId;

		private bool mPlaying;

		private float mLastOverrideFrame;

		public override void OnGraphStop(Playable playable)
		{
		}

		public override void ProcessFrame(Playable playable, FrameData info, object playerData)
		{
		}

		public override void PrepareFrame(Playable playable, FrameData info)
		{
		}
	}
}

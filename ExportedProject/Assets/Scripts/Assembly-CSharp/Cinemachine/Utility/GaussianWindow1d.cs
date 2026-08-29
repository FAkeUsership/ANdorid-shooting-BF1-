namespace Cinemachine.Utility
{
	internal abstract class GaussianWindow1d<T>
	{
		protected T[] mData;

		protected float[] mKernel;

		protected float mKernelSum;

		protected int mCurrentPos;

		public float Sigma { get; private set; }

		public int KernelSize => 0;

		private void GenerateKernel(float sigma, int maxKernelRadius)
		{
		}

		protected abstract T Compute(int windowPos);

		public GaussianWindow1d(float sigma, int maxKernelRadius = 10)
		{
		}

		public void Reset()
		{
		}

		public bool IsEmpty()
		{
			return false;
		}

		public void AddValue(T v)
		{
		}

		public T Filter(T v)
		{
			return default;
		}

		public T Value()
		{
			return default;
		}
	}
}

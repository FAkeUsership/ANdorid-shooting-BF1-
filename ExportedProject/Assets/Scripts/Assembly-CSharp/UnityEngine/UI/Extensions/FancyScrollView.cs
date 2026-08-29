using System.Collections.Generic;

namespace UnityEngine.UI.Extensions
{
	public class FancyScrollView<TData, TContext> : MonoBehaviour where TContext : class
	{
		[SerializeField]
		[Range(float.Epsilon, 1f)]
		private float cellInterval;

		[SerializeField]
		[Range(0f, 1f)]
		private float cellOffset;

		[SerializeField]
		private bool loop;

		[SerializeField]
		private GameObject cellBase;

		private float currentPosition;

		private readonly List<FancyScrollViewCell<TData, TContext>> cells;

		protected TContext context;

		protected List<TData> cellData;

		protected void Awake()
		{
		}

		protected void SetContext(TContext context)
		{
		}

		private FancyScrollViewCell<TData, TContext> CreateCell()
		{
			return null;
		}

		private void UpdateCellForIndex(FancyScrollViewCell<TData, TContext> cell, int dataIndex)
		{
		}

		private int GetLoopIndex(int index, int length)
		{
			return 0;
		}

		protected void UpdateContents()
		{
		}

		protected void UpdatePosition(float position)
		{
		}
	}
	public class FancyScrollView<TData> : FancyScrollView<TData, FancyScrollViewNullContext>
	{
	}
}

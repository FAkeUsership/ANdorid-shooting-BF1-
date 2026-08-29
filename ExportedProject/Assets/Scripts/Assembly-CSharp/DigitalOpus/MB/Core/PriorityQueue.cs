using System.Collections;
using System.Collections.Generic;

namespace DigitalOpus.MB.Core
{
	public class PriorityQueue<TPriority, TValue> : ICollection<KeyValuePair<TPriority, TValue>>, IEnumerable<KeyValuePair<TPriority, TValue>>, IEnumerable
	{
		public List<KeyValuePair<TPriority, TValue>> _baseHeap;

		private IComparer<TPriority> _comparer;

		public bool IsEmpty => false;

		public int Count => 0;

		public bool IsReadOnly => false;

		public PriorityQueue()
		{
		}

		public PriorityQueue(int capacity)
		{
		}

		public PriorityQueue(int capacity, IComparer<TPriority> comparer)
		{
		}

		public PriorityQueue(IComparer<TPriority> comparer)
		{
		}

		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data)
		{
		}

		public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data, IComparer<TPriority> comparer)
		{
		}

		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2)
		{
			return null;
		}

		public static PriorityQueue<TPriority, TValue> MergeQueues(PriorityQueue<TPriority, TValue> pq1, PriorityQueue<TPriority, TValue> pq2, IComparer<TPriority> comparer)
		{
			return null;
		}

		public void Enqueue(TPriority priority, TValue value)
		{
		}

		public KeyValuePair<TPriority, TValue> Dequeue()
		{
			return default;
		}

		public TValue DequeueValue()
		{
			return default;
		}

		public KeyValuePair<TPriority, TValue> Peek()
		{
			return default;
		}

		public TValue PeekValue()
		{
			return default;
		}

		private void ExchangeElements(int pos1, int pos2)
		{
		}

		private void Insert(TPriority priority, TValue value)
		{
		}

		private int HeapifyFromEndToBeginning(int pos)
		{
			return 0;
		}

		private void DeleteRoot()
		{
		}

		private void HeapifyFromBeginningToEnd(int pos)
		{
		}

		public void Add(KeyValuePair<TPriority, TValue> item)
		{
		}

		public void Clear()
		{
		}

		public bool Contains(KeyValuePair<TPriority, TValue> item)
		{
			return false;
		}

		public bool TryFindValue(TPriority item, out TValue foundVersion)
		{
			foundVersion = default;
			return false;
		}

		public void CopyTo(KeyValuePair<TPriority, TValue>[] array, int arrayIndex)
		{
		}

		public bool Remove(KeyValuePair<TPriority, TValue> item)
		{
			return false;
		}

		public IEnumerator<KeyValuePair<TPriority, TValue>> GetEnumerator()
		{
			return null;
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return null;
		}
	}
}

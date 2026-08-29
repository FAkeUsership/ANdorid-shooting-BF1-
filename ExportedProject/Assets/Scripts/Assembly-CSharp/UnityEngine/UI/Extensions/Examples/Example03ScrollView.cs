using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	public class Example03ScrollView : FancyScrollView<Example03CellDto, Example03ScrollViewContext>
	{
		[SerializeField]
		private ScrollPositionController scrollPositionController;

		private new void Awake()
		{
		}

		public void UpdateData(List<Example03CellDto> data)
		{
		}

		private void OnPressedCell(Example03ScrollViewCell cell)
		{
		}

		private void CellSelected(int cellIndex)
		{
		}
	}
}

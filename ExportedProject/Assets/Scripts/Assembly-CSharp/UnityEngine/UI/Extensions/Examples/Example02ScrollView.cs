using System.Collections.Generic;

namespace UnityEngine.UI.Extensions.Examples
{
	public class Example02ScrollView : FancyScrollView<Example02CellDto, Example02ScrollViewContext>
	{
		[SerializeField]
		private ScrollPositionController scrollPositionController;

		private new void Awake()
		{
		}

		public void UpdateData(List<Example02CellDto> data)
		{
		}

		private void OnPressedCell(Example02ScrollViewCell cell)
		{
		}

		private void CellSelected(int cellIndex)
		{
		}
	}
}

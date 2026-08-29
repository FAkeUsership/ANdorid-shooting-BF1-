namespace UnityEngine.UI.Extensions.Examples
{
	public class ScrollingCalendar : MonoBehaviour
	{
		public RectTransform monthsScrollingPanel;

		public RectTransform yearsScrollingPanel;

		public RectTransform daysScrollingPanel;

		public GameObject yearsButtonPrefab;

		public GameObject monthsButtonPrefab;

		public GameObject daysButtonPrefab;

		private GameObject[] monthsButtons;

		private GameObject[] yearsButtons;

		private GameObject[] daysButtons;

		public RectTransform monthCenter;

		public RectTransform yearsCenter;

		public RectTransform daysCenter;

		private UIVerticalScroller yearsVerticalScroller;

		private UIVerticalScroller monthsVerticalScroller;

		private UIVerticalScroller daysVerticalScroller;

		public InputField inputFieldDays;

		public InputField inputFieldMonths;

		public InputField inputFieldYears;

		public Text dateText;

		private int daysSet;

		private int monthsSet;

		private int yearsSet;

		private void InitializeYears()
		{
		}

		private void InitializeMonths()
		{
		}

		private void InitializeDays()
		{
		}

		public void Awake()
		{
		}

		public void SetDate()
		{
		}

		private void Update()
		{
		}

		public void DaysScrollUp()
		{
		}

		public void DaysScrollDown()
		{
		}

		public void MonthsScrollUp()
		{
		}

		public void MonthsScrollDown()
		{
		}

		public void YearsScrollUp()
		{
		}

		public void YearsScrollDown()
		{
		}
	}
}

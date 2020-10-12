using System.Windows;
using System.Windows.Controls;

namespace AudioDownloader.WpfClient
{
	/// <summary>
	/// Interaction logic for Time.xaml
	/// </summary>
	public partial class Time : UserControl
	{
		public string Info
		{
			get { return (string)GetValue(InfoProperty); }
			set { SetValue(InfoProperty, value); }
		}

		public static readonly DependencyProperty InfoProperty =
			DependencyProperty.Register(nameof(Info), typeof(string), typeof(Time), new PropertyMetadata(null));

		public string Label
		{
			get { return (string)GetValue(LabelProperty); }
			set { SetValue(LabelProperty, value); }
		}

		public static readonly DependencyProperty LabelProperty =
			DependencyProperty.Register(nameof(Label), typeof(string), typeof(Time), new PropertyMetadata(null));

		public string TimeDisplay
		{
			get { return (string)GetValue(TimeDisplayProperty); }
			set { SetValue(TimeDisplayProperty, value); }
		}

		public static readonly DependencyProperty TimeDisplayProperty =
			DependencyProperty.Register(nameof(TimeDisplay), typeof(string), typeof(Time), new PropertyMetadata(null));

		public double TimeValue
		{
			get { return (double)GetValue(TimeValueProperty); }
			set { SetValue(TimeValueProperty, value); }
		}

		public static readonly DependencyProperty TimeValueProperty =
			DependencyProperty.Register(nameof(TimeValue), typeof(double), typeof(Time), new PropertyMetadata(0.0));

		public double TimeValueMaximum
		{
			get { return (double)GetValue(TimeValueMaximumProperty); }
			set { SetValue(TimeValueMaximumProperty, value); }
		}

		public static readonly DependencyProperty TimeValueMaximumProperty =
			DependencyProperty.Register(nameof(TimeValueMaximum), typeof(double), typeof(Time), new PropertyMetadata(0.0));

		public double SmallSliderChange
		{
			get { return (double)GetValue(SmallSliderChangeProperty); }
			set { SetValue(SmallSliderChangeProperty, value); }
		}

		public static readonly DependencyProperty SmallSliderChangeProperty =
			DependencyProperty.Register(nameof(SmallSliderChange), typeof(double), typeof(Time), new PropertyMetadata(30.0));

		public double LargeSliderChange
		{
			get { return (double)GetValue(LargeSliderChangeProperty); }
			set { SetValue(LargeSliderChangeProperty, value); }
		}

		public static readonly DependencyProperty LargeSliderChangeProperty =
			DependencyProperty.Register(nameof(LargeSliderChange), typeof(double), typeof(Time), new PropertyMetadata(3000.0));

		public Time()
		{
			InitializeComponent();
		}
	}
}

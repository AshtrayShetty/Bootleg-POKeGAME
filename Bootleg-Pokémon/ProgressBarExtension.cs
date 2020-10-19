using System.Windows;
using System.Windows.Media.Animation;
using System.Windows.Controls.Primitives;
using System.Windows.Controls;

namespace Bootleg_Pokémon
{
	internal class ProgressBarExtension
	{
		public static readonly DependencyProperty AnimatedValueProperty =
	   DependencyProperty.RegisterAttached("AnimatedValue", typeof(int), typeof(ProgressBarExtension), new FrameworkPropertyMetadata(100, 
		   FrameworkPropertyMetadataOptions.None, AnimatedValuePropertyChanged));

		public static void SetAnimatedValue(DependencyObject progressBar, int value)
		{ progressBar.SetValue(AnimatedValueProperty, value); }

		public static int GetAnimatedValue(DependencyObject progressBar)
		{ return (int) progressBar.GetValue(AnimatedValueProperty); }

		static void AnimatedValuePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
		{
			if (!(d is ProgressBar progressBar)) return;
			DoubleAnimation da = new DoubleAnimation
			{
				To = GetAnimatedValue(d),
				Duration = new Duration(new System.TimeSpan(0, 0, 0, 0, 300)),
				AccelerationRatio = 0.8
			};
			progressBar.BeginAnimation(RangeBase.ValueProperty, da);
		}
	}
}

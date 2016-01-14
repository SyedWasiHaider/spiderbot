using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace spiderbot
{
	public partial class ColorPage : ContentPage
	{

		const double minColorValue = 0;
		const double maxColorValue = 255.0;

		public ColorPage ()
		{
			InitializeComponent ();

			redSlider.Minimum = greenSlider.Minimum = blueSlider.Minimum = minColorValue;
			redSlider.Maximum = greenSlider.Maximum = blueSlider.Maximum = maxColorValue;

			redSlider.ValueChanged += (object sender, ValueChangedEventArgs e) => {
				var currentColor = ColorBox.Color;
				ColorBox.Color = new Color(e.NewValue/maxColorValue, currentColor.G, currentColor.B);
			};

			blueSlider.ValueChanged += (object sender, ValueChangedEventArgs e) => {
				var currentColor = ColorBox.Color;
				ColorBox.Color = new Color(currentColor.R, currentColor.G, e.NewValue/maxColorValue);
			};

			greenSlider.ValueChanged += (object sender, ValueChangedEventArgs e) => {
				var currentColor = ColorBox.Color;
				ColorBox.Color = new Color(currentColor.R, e.NewValue/maxColorValue, currentColor.B);			
			};

			greenSlider.Value = 123;
			redSlider.Value = 123;
			blueSlider.Value = 123;

			SaveButton.Clicked += async (sender, e) => {
				webView.Eval(String.Format("wsSendCommand ('command', 'changeLEDColor {0} {1} {2}');", redSlider.Value, greenSlider.Value, blueSlider.Value));
				webView.Eval(String.Format("wsSendCommand ('command', 'saveCurrentLEDColor {0} {1} {2}');", redSlider.Value, greenSlider.Value, blueSlider.Value));

				await SaveButton.AnimateButton();
			};
		}
	}
}


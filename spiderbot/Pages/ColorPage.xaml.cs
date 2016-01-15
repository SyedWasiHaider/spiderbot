using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace spiderbot
{
	public partial class ColorPage : ContentPage
	{

		const int minColorValue = 0;
		const int maxColorValue = 255;

		public ColorPage ()
		{
			InitializeComponent ();

			redSlider.Minimum = greenSlider.Minimum = blueSlider.Minimum = minColorValue;
			redSlider.Maximum = greenSlider.Maximum = blueSlider.Maximum = maxColorValue;

			redSlider.ValueChanged += (object sender, ValueChangedEventArgs e) => {
				var currentColor = ColorBox.Color;
				ColorBox.Color = new Color(e.NewValue/maxColorValue, currentColor.G, currentColor.B);
				ChangeColor();
			};

			blueSlider.ValueChanged += (object sender, ValueChangedEventArgs e) => {
				var currentColor = ColorBox.Color;
				ColorBox.Color = new Color(currentColor.R, currentColor.G, e.NewValue/maxColorValue);
				ChangeColor();
			};

			greenSlider.ValueChanged += (object sender, ValueChangedEventArgs e) => {
				var currentColor = ColorBox.Color;
				ColorBox.Color = new Color(currentColor.R, e.NewValue/maxColorValue, currentColor.B);	
				ChangeColor();
			};

			greenSlider.Value = 123;
			redSlider.Value = 123;
			blueSlider.Value = 123;

			SaveButton.Clicked += async (sender, e) => {
				SaveColor();
				await SaveButton.AnimateButton();
			};
		}

		public void ChangeColor(){
			webView.Eval(String.Format("wsSendCommand ('command', 'changeLEDColor {0} {1} {2} 20');", 
				redSlider.Value/maxColorValue * 100, 
				greenSlider.Value/maxColorValue * 100, 
				blueSlider.Value/maxColorValue * 100));
		}

		public void SaveColor(){
			ChangeColor ();
			webView.Eval(String.Format("wsSendCommand ('command', 'saveCurrentLEDColor {0} {1} {2}');", 
				redSlider.Value/maxColorValue * 100, 
				greenSlider.Value/maxColorValue * 100, 
				blueSlider.Value/maxColorValue * 100));
		}
	}
}


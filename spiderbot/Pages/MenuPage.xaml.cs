using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace spiderbot
{
	public partial class MenuPage : ContentPage
	{
		StackLayout layout;
		public MenuPage (NavigationPage firstPage)
		{
			layout =  new StackLayout {
				Padding = new Thickness(0, Device.OnPlatform<int>(20, 0, 0), 0, 0),
				Children = {
					new MainLink(firstPage),
					new MainLink(new ColorPage() {Title="Eye Color"}),
					new MainLink(new PackMode() {Title="Pack Up" })
				}
			};
			Content = layout;
		
			Title = "Menu";
			BackgroundColor = Color.Gray.WithLuminosity(0.9);
			Icon = Device.OS == TargetPlatform.iOS ? "menu.png" : null;
		}
	}
}


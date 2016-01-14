using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace spiderbot
{
	public partial class App : Application
	{
		public static MasterDetailPage MasterDetailPage;

		public App ()
		{
			InitializeComponent ();

				var motionPage = new NavigationPage(new MotionPage (){ Title = "Motion Control" });
				MasterDetailPage = new MasterDetailPage {
					Detail = motionPage,
					Master = new MenuPage(motionPage)
				};

			MainPage = MasterDetailPage;
		}
		}
}


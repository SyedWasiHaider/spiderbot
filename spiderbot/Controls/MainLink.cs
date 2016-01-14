using System;
using Xamarin.Forms;

namespace spiderbot
{
	public class MainLink: Button
	{
		public MainLink(string pageTitle){
			Text = pageTitle;
		}

		public MainLink(NavigationPage navPage){
			Text = navPage.CurrentPage.Title;
			Command = new Command(o => {
				App.MasterDetailPage.Detail = navPage;
				App.MasterDetailPage.IsPresented = false;
			});
		}

		public MainLink(Page page) : this(new NavigationPage(page))
		{
		}
	}
}


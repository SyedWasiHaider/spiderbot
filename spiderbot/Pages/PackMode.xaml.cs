using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace spiderbot
{
	public partial class PackMode : ContentPage
	{
		public PackMode ()
		{
			InitializeComponent ();
			packingButton.Clicked +=  async (sender, e) => {
				await packingButton.AnimateButton();
			};
		}
	}
}


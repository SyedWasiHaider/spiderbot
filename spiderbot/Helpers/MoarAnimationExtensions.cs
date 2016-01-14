using System;
using Xamarin.Forms;
using System.Threading.Tasks;

namespace spiderbot
{
	public static class MoarAnimationExtensions
	{
		public static async Task AnimateButton (this View button)
		{
			await button.ScaleTo(0.7, 50, Easing.BounceIn);
			await button.ScaleTo(1.0,50, Easing.BounceOut);
		}
	}
}


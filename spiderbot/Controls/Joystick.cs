using System;
using Xamarin.Forms;

namespace spiderbot
{
	public class Joystick : View
	{
		public static readonly BindableProperty xProperty =
			BindableProperty.Create<Joystick, float> (p => p.xPosition, 0);

		public delegate void ValueChangedHandler(float xPos, float yPos);

		//This event can cause any method which conforms
		//to MyEventHandler to be called.
		public event ValueChangedHandler OnValueChanged;

		public void callHandler(float x, float y){
			OnValueChanged(x, y);
		}

		public float xPosition {
			get { return (float)GetValue (xProperty); }
			set { SetValue (xProperty, value); }
		}

		public static readonly BindableProperty YProperty =
			BindableProperty.Create<Joystick, float> (p => p.yPosition, 0);

		public float yPosition {
			get { return (float)GetValue (YProperty); }
			set { SetValue (YProperty, value); }
		}
	}
}


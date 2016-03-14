using System;
using System.Diagnostics;
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
			set { 
				SetValue (xProperty, value);
				this.OnPropertyChanged(); 
			}
		}

		public static readonly BindableProperty YProperty =
			BindableProperty.Create<Joystick, float> (p => p.yPosition, 0);

		public float yPosition {
			get { return (float)GetValue (YProperty); }
			set { 
				SetValue (YProperty, value);
				this.OnPropertyChanged();
			}
		}


		public static readonly BindableProperty ThumbCenterProperty =
			BindableProperty.Create<Joystick, Point>(p => p.ThumbCenter, Point.Zero);

		public Point ThumbCenter
		{
			get { return (Point)GetValue(ThumbCenterProperty); }
			set { 
				SetValue(ThumbCenterProperty, value);
				UpdateValues();
				this.OnPropertyChanged();
			}
		}

		public double Radius { get; set; }
		protected override void OnSizeAllocated(double width, double height)
		{
			base.OnSizeAllocated(width, height);
			ThumbCenter = new Point(width / 2, height / 2);
			Radius = Math.Min(width,height)/2;
		}

		public Point Center => new Point(Width / 2, Height / 2);
		public bool IsMoving { get; set; }

		public double DistanceFromJoyPad(Point point)
		{
			return point.Distance(Center);
		}

		public bool IsTouchingJoystick(Point point)
		{
			var length = DistanceFromJoyPad(point);
			return length < Radius;
		}
		Point touchPos;

		public void MoveJoystick(Point point)
		{
			MoveJoystick(point, IsMoving ? Center : ThumbCenter);
		}
		public void MoveJoystick(Point point, Point prev)
		{
			var offset = point.Subtract(prev);

			//touchPos = touchPos.Add(offset);
			touchPos = point;
			var delta = touchPos.Subtract(Center);
			var newPos = touchPos;
			var angle = delta.ToAngle();
			var joybtnDist = DistanceFromJoyPad(newPos);
			if (joybtnDist > Radius)
			{
				var direction = PointHelper.ForAngle(angle);
				newPos = Center.Add(direction.Multiply(Radius));
				joybtnDist = Radius;
			}
			ThumbCenter = newPos;
		}

		public void BeingTouch(Point point)
		{
			if (IsMoving)
				return;
			IsMoving = true;
			touchPos = Center;
			MoveJoystick(point);

		}

		public void Stop()
		{
			IsMoving = false;
			ThumbCenter = Center;
		}

		void UpdateValues()
		{
			var distance = ThumbCenter.Subtract(Center).Multiply(1/Radius);
			xPosition = (float)Math.Round(distance.X, 2);
			yPosition = (float)Math.Round(distance.Y, 2) * -1;
			Debug.WriteLine($"X = {xPosition} Y = {yPosition}");
		}
	}
}


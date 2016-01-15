using System;
using Xamarin.Forms;
using spiderbot;
using Xamarin.Forms.Platform.iOS;
using spiderbot.iOS;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using System.Drawing;

[assembly: ExportRenderer (typeof(Joystick), typeof(JoystickRenderer))]
namespace spiderbot.iOS
{
	public class JoystickRenderer : ViewRenderer<Joystick, UIView>
		{
		float r = 0;
		float dx = 0;
		float dy = 0;

		int max;
		int min;

		const int containerSize = 100;
		UIView container;
		CAShapeLayer mainStick;
		CAShapeLayer boundary;
		CGPoint centerPos;
		UILongPressGestureRecognizer panGesture;
		UILabel label;

		public JoystickRenderer(){
		}


		private void Setup(){

			min = -1 * containerSize/4;
			max = containerSize/4;

			label = new UILabel (new CGRect(0,40, 100, 100)); 
			mainStick = new CAShapeLayer ();
			boundary = new CAShapeLayer ();
			container = new UIView (new CGRect(0,0,containerSize,containerSize));

			mainStick.Path = UIBezierPath.FromOval (new CGRect (containerSize/2, containerSize/4, containerSize/2, containerSize/2)).CGPath;
			boundary.Path = UIBezierPath.FromOval (new CGRect (containerSize/4, 0, containerSize, containerSize)).CGPath;


			boundary.FillColor = new CGColor (23, 44, 100,0);
			boundary.StrokeColor = new CGColor (23, 44, 100);

			centerPos = mainStick.Position;
			panGesture = new UILongPressGestureRecognizer (() => {
				UpdateDrawing();
			});

			this.AddSubview (label);
			panGesture.MinimumPressDuration = 0;
			container.Layer.AddSublayer (mainStick);
			container.Layer.AddSublayer (boundary);
			container.AddGestureRecognizer (panGesture);
		}

		private void UpdateDrawing(){

			if ((panGesture.State == UIGestureRecognizerState.Began || panGesture.State == UIGestureRecognizerState.Changed) && (panGesture.NumberOfTouches == 1)) {

				var p0 = panGesture.LocationInView (container);


				if (dx == 0)
					dx = (float)(p0.X - mainStick.Frame.GetMidX());

				if (dy == 0)
					dy = (float)(p0.Y - mainStick.Frame.GetMidY());


				var oldPosX = (float)mainStick.Position.X;
				var oldPosY = (float)mainStick.Position.Y;
				var newPosX = (float)p0.X - dx;
				var newPosY = (float)p0.Y - dy;
				newPosX = (newPosX > max || newPosX < min) ? oldPosX : newPosX;
				newPosY = (newPosY > max || newPosY < min) ? oldPosY : newPosY;

				if (Math.Abs(newPosX-oldPosX) > 0.1 || Math.Abs(newPosY-oldPosY) > 0.1) {
					this.Element.callHandler (newPosX/max, newPosY/max);
				}

				CATransaction.Begin();
				CATransaction.DisableActions = true;

				mainStick.Position = new CGPoint(newPosX, newPosY);

				CATransaction.Commit();

				this.Element.xPosition = newPosX / max;
				this.Element.yPosition = newPosY / max;
				label.Text = "" + newPosX + ", " + newPosY;


			} else if (panGesture.State == UIGestureRecognizerState.Ended) {
				dx = 0;
				dy = 0;
				mainStick.Position = centerPos;
				this.Element.callHandler ((float)(centerPos.X/max), (float)(centerPos.Y/max));
			}
		}


		protected override void OnElementChanged (ElementChangedEventArgs<Joystick> e)
			{
				base.OnElementChanged (e);
				if (Control == null) {
					Setup ();
					SetNativeControl (container);
				}
			}

	}
}


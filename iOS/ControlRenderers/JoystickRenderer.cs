using System;
using Xamarin.Forms;
using spiderbot;
using Xamarin.Forms.Platform.iOS;
using spiderbot.iOS;
using UIKit;
using CoreGraphics;
using CoreAnimation;
using System.Drawing;
using System.Linq;

[assembly: ExportRenderer(typeof(Joystick), typeof(JoystickRenderer))]
namespace spiderbot.iOS
{
	public class JoystickRenderer : ViewRenderer<Joystick, UIView>
	{

		UIView container;
		UIView border;
		UIView thumb;
		UILabel label;
		UIGestureRecognizer panGesture;
		public JoystickRenderer()
		{
			container = new UIView(new CGRect(0, 0, 100, 100));
			container.Add(border = new UIView
			{
				BackgroundColor = UIColor.LightGray.ColorWithAlpha(.1f),
				Layer = {
					BorderWidth = 1f,
					BorderColor = UIColor.Black.CGColor,
				}
			});

			container.Add(thumb = new UIView
			{
				BackgroundColor = UIColor.DarkGray.ColorWithAlpha(.5f),
				Layer = {
					BorderWidth = 1f,
					BorderColor = UIColor.Black.ColorWithAlpha(.5f).CGColor,
				}
			});
			panGesture = new UIPanGestureRecognizer((g) =>
			{
				
			});
			panGesture.CancelsTouchesInView = false;
			panGesture.ShouldRecognizeSimultaneously = (s,e) => { return false;};
			this.AddGestureRecognizer(panGesture);
		}
			                                              

		nfloat lastWidth;
		nfloat thumbRadius;
		public override void LayoutSubviews()
		{
			base.LayoutSubviews();
			var bounds = Bounds;
			var width = NMath.Min(bounds.Width, bounds.Height);
			if (lastWidth == width)
				return;
			border.Frame = new CGRect(0, 0, width, width);
			border.Center = new CGPoint(bounds.GetMidX(), bounds.GetMidY());
			border.Layer.CornerRadius = width / 2;
			thumbRadius = width / 4;

			thumb.Frame = new CGRect(0, 0, width / 2, width / 2);
			thumb.Layer.CornerRadius = thumbRadius;
			UpdateThumb();
		}

		public override void TouchesBegan(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesBegan(touches, evt);
			if (touches.Count == 0)
				return;
			var touch = touches.ToArray<UITouch>().First();
			//touch.GestureRecognizers?.ToList()?.ForEach(x => x.c);
			var p = touch.LocationInView(this).ToPoint();
			Element.BeingTouch(p);
		}

		public override void TouchesMoved(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesMoved(touches, evt);
			if (touches.Count == 0)
				return;
			var touch = touches.ToArray<UITouch>().First();
			var p = touch.LocationInView(this).ToPoint();
			Element.MoveJoystick(p,	touch.PreviousLocationInView(this).ToPoint());
		}
		public override void TouchesEnded(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesEnded(touches, evt);
			Element.Stop();
		}
		public override void TouchesCancelled(Foundation.NSSet touches, UIEvent evt)
		{
			base.TouchesCancelled(touches, evt);
			Element.Stop();
		}

		protected override void OnElementChanged(ElementChangedEventArgs<Joystick> e)
		{
			base.OnElementChanged(e);
			if (Control == null)
			{
				//Setup();
				SetNativeControl(container);
				//SetNativeControl(this);
			}
		}

		protected override void OnElementPropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
		{
			base.OnElementPropertyChanged(sender, e);
			if (e.PropertyName == "ThumbCenter")
				UpdateThumb();

		}


		void UpdateThumb()
		{
			thumb.Center = new CGPoint(Element.ThumbCenter.X, Element.ThumbCenter.Y);
		}

		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				container?.RemoveGestureRecognizer(panGesture);
			}
			base.Dispose(disposing);
		}

	}
}


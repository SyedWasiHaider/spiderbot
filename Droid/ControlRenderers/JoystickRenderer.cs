using System;
using Xamarin.Forms;
using spiderbot;
using spiderbot.Droid;
using Xamarin.Forms.Platform.Android;
using Android.Util;
using Android.Graphics;
using Android.Content;
using Android.Views;
using Android.Support.V4.View;
using Android.Graphics.Drawables;
using Android.App;

[assembly: ExportRenderer (typeof(Joystick), typeof(JoystickRenderer))]
namespace spiderbot.Droid
{
	public class JoystickRenderer : ViewRenderer<Joystick, Android.Views.View>
	{
		class JoystickView : Android.Views.View{

			float xPos, yPos;
			float stickRadius, borderRadius;
			public bool OnDrag (Android.Views.View v, DragEvent e)
			{
				var newX = e.GetX () ;
				var newY = e.GetY ();
				if (e.Action == DragAction.Ended) {
					xPos = Width / 2;
					yPos = Height / 2;
				} else {
					
					if (newX <= Right - stickRadius && newX >= Left + stickRadius) {
						xPos = e.GetX (); 
					}

					if (newY <= Bottom - stickRadius && newY >= Top + stickRadius) {
						yPos = e.GetY ();
					}
				}

				Invalidate ();

				return true;
			}
				
			Context mContext;
			public JoystickView(Context context) :
			base(context)
 			{
				init (Context);
			}

			public JoystickView(Context context, IAttributeSet attrs) :
 			base(context, attrs)
 			{
	 				init (context);
	 		}
 
 			public JoystickView(Context context, IAttributeSet attrs, int defStyle) :
 			base(context, attrs, defStyle)
 			{
	 				init (context);
	 		}
 
 			private void init(Context ctx){
	 			mContext = ctx;
	 		}

			public override bool OnDragEvent (DragEvent e)
			{
				return OnDrag (this, e);
			}

			bool SizeNeedSetting = true;
			protected override void OnMeasure (int widthMeasureSpec, int heightMeasureSpec)
			{
				base.OnMeasure (widthMeasureSpec, heightMeasureSpec);

				if (SizeNeedSetting && Width > 0 && Height > 0) {
					xPos = Width / 2;
					yPos = Height / 2;
					borderRadius = Math.Min (Width, Height) / 2;
					stickRadius = borderRadius / 2;
					SizeNeedSetting = false;
				}
			}

			protected override void OnDraw (Android.Graphics.Canvas canvas)
			{
				base.OnDraw (canvas);
				var p = new Paint () { Color = Android.Graphics.Color.Black };
				canvas.DrawCircle (xPos, yPos, stickRadius, p);
				p.SetStyle(Paint.Style.Stroke);
				p.Color = Android.Graphics.Color.Gray;
				p.StrokeWidth = 4.0f;
				canvas.DrawCircle (Width/2, Height/2, borderRadius, p);
			}

			public override bool OnTouchEvent (MotionEvent e)
			{
				if (e.Action == MotionEventActions.Down) {
					ClipData dragData = ClipData.NewPlainText(""+e.GetX(), ""+e.GetY());
						Android.Views.View.DragShadowBuilder myShadow = new MyDragShadowBuilder(this) ;
					this.StartDrag(dragData,  // the data to be dragged
						myShadow,  // the drag shadow builder
						null,      // no need to use local data
						0          // flags (not currently used, set to 0)
					);
				}
				return base.OnTouchEvent (e);
			}
		}

		public JoystickRenderer ()
		{
		}

		protected override void OnElementChanged (ElementChangedEventArgs<Joystick> e)
		{
			base.OnElementChanged (e);
			if (this.Control == null) {
				SetNativeControl (new JoystickView(Context ));
			}
		}

		//Do I really need this? Seems like extra overhead.
		class MyDragShadowBuilder : Android.Views.View.DragShadowBuilder {

			// The drag shadow image, defined as a drawable thing
			private static Drawable shadow;

			// Defines the constructor for myDragShadowBuilder
			public MyDragShadowBuilder(Android.Views.View v) : base(v) {

				shadow = new ColorDrawable(Android.Graphics.Color.Transparent);
			}


			public override void OnProvideShadowMetrics (Android.Graphics.Point shadowSize, Android.Graphics.Point shadowTouchPoint){
				// Defines local variables
				int width, height;

				// Sets the width of the shadow to half the width of the original View
				width = View.Width / 2;

				// Sets the height of the shadow to half the height of the original View
				height = View.Height / 2;

				// The drag shadow is a ColorDrawable. This sets its dimensions to be the same as the
				// Canvas that the system will provide. As a result, the drag shadow will fill the
				// Canvas.
				shadow.SetBounds(0, 0, width, height);

				// Sets the size parameter's width and height values. These get back to the system
				// through the size parameter.
				shadowSize.Set(width, height);

				// Sets the touch point's position to be in the middle of the drag shadow
				shadowTouchPoint.Set(width / 2, height / 2);
			}

			// Defines a callback that draws the drag shadow in a Canvas that the system constructs
			// from the dimensions passed in onProvideShadowMetrics().

			public override void OnDrawShadow(Canvas canvas) {

				// Draws the ColorDrawable in the Canvas passed in from the system.
				shadow.Draw(canvas);
			}
		}

	}
}


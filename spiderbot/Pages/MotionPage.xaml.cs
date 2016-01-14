using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace spiderbot
{
	public partial class MotionPage : ContentPage
	{
		enum ScalarMotionState{
			FORWARD, BACK, STOPPED
		}

		enum DirectionalMotionState{
			LEFT,RIGHT,NONE
		}

		ScalarMotionState currentScalarState;
		DirectionalMotionState currentDirectionState;

		void GoForward ()
		{
			var instance = RobotService.Instance;
			instance.host = "ws://127.0.0.1";
			instance.GoForward ();

			if (currentScalarState == ScalarMotionState.BACK) {
				StopMoving ();
			}
			webView.Eval ("wsSendCommand ('command',  'walkY 120');");
			currentScalarState = ScalarMotionState.FORWARD;
		}

		void GoBack ()
		{
			if (currentScalarState == ScalarMotionState.FORWARD) {
				StopMoving ();
			}
			webView.Eval ("wsSendCommand('command',  'walkY 80');");
			currentScalarState = ScalarMotionState.BACK;
		}

		void StopMoving ()
		{
			if (currentScalarState != ScalarMotionState.STOPPED) {
				webView.Eval ("wsSendCommand('command',  'walkY 100');");
			}
			currentScalarState = ScalarMotionState.STOPPED;
		}

		void StopTurn ()
		{
			if (currentDirectionState != DirectionalMotionState.NONE){
				webView.Eval ("wsSendCommand ('command',  'walkTurnZ 100');");
			}
			currentDirectionState = DirectionalMotionState.NONE;
		}

		void TurnLeft ()
		{
			if (currentDirectionState == DirectionalMotionState.RIGHT) {
				StopTurn ();
			}
			webView.Eval("wsSendCommand ('command',  'walkTurnZ 130');");
			currentDirectionState = DirectionalMotionState.LEFT;
		}

		void TurnRight ()
		{
			if (currentDirectionState == DirectionalMotionState.LEFT) {
				StopTurn ();
			}
			webView.Eval ("wsSendCommand ('command',  'walkTurnZ 70');");
			currentDirectionState = DirectionalMotionState.RIGHT;
		}

		bool animating = false;
		public async Task StartAnimationSpider(){
			animating = true;
			while (animating) {
				await spiderImage.RelRotateTo (20, 120);
				await spiderImage.RelRotateTo (-40, 120);
				await spiderImage.RelRotateTo (20, 120);
			}
		}

		public async Task StopAnimationSpider(){
			animating = false;
		}

		public MotionPage()
		{

			InitializeComponent ();

			var defaultColor = goForwardButton.BackgroundColor;
			slider.Maximum = 130;
			slider.Minimum = 70;
			slider.Value = 100;
			slider.ValueChanged += (sender, e) => {
				if (e.NewValue > 80 && e.NewValue < 110){
					StopTurn();
				}else if (e.NewValue < 100){
					TurnLeft();
				}else{
					TurnRight();
				}
			};
				
			currentDirectionState = DirectionalMotionState.NONE;
			currentScalarState = ScalarMotionState.STOPPED;

			stopButton.Clicked += async (object sender, EventArgs e) => {
				StopMoving();
				StopTurn();
				await stopButton.AnimateButton();
				goForwardButton.BackgroundColor = defaultColor;
				goBackButton.BackgroundColor = defaultColor;
				StopAnimationSpider();
			};


			goForwardButton.Clicked += async (object sender, EventArgs e) => {

				GoForward();
				await goForwardButton.AnimateButton();
				goForwardButton.BackgroundColor = Color.Red;
				goBackButton.BackgroundColor = defaultColor;

				if (!animating){
					StartAnimationSpider();
				}
			
			};

			goBackButton.Clicked += async (object sender, EventArgs e) => {
				GoBack();
				await goBackButton.AnimateButton();
				goForwardButton.BackgroundColor = defaultColor;
				goBackButton.BackgroundColor = Color.Red;

				if (!animating){
					StartAnimationSpider();
				}

			};

		}
	}
}


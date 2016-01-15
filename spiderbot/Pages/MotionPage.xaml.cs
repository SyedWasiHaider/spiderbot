using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Diagnostics;

namespace spiderbot
{
	public partial class MotionPage : ContentPage
	{
		enum ScalarMotionState{
			FORWARD, BACK, STOPPED
		}

		enum DirectionalMotionState{
			LEFT,RIGHT,STRAIGHT
		}

		enum RotationalMotionState{
			LEFT,RIGHT,NONE
		}

		ScalarMotionState currentScalarState;
		DirectionalMotionState currentDirectionState;
		RotationalMotionState currentRotationalState;

		void GoForwardScalar ()
		{
			if (currentScalarState == ScalarMotionState.FORWARD) {
				return;
			}else if (currentScalarState == ScalarMotionState.BACK) {
				StopMoving ();
			}
			webView.Eval ("wsSendCommand ('command',  'walkY 120');");
			currentScalarState = ScalarMotionState.FORWARD;
		}

		void GoBackScalar ()
		{
			if (currentScalarState == ScalarMotionState.BACK) {
				return;
			}else if (currentScalarState == ScalarMotionState.FORWARD) {
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

		void StopTranslation ()
		{
			if (currentDirectionState != DirectionalMotionState.STRAIGHT){
				webView.Eval ("wsSendCommand ('command',  'walkX 100');");

			}
			currentDirectionState = DirectionalMotionState.STRAIGHT;
		}

		void StopRotation(){
			if (currentRotationalState != RotationalMotionState.NONE){
				webView.Eval ("wsSendCommand ('command',  'walkTurnZ 100');");
			}
			currentRotationalState = RotationalMotionState.NONE;
		}


		void RotateLeft ()
		{
			if (currentRotationalState == RotationalMotionState.LEFT) {
				return;
			}
			if (currentRotationalState == RotationalMotionState.RIGHT) {
				StopRotation ();
			}
			webView.Eval("wsSendCommand ('command',  'walkTurnZ 130');");

			currentRotationalState = RotationalMotionState.LEFT;
		}

		void RotateRight ()
		{
			if (currentRotationalState == RotationalMotionState.RIGHT) {
				return;
			}

			if (currentRotationalState == RotationalMotionState.LEFT) {
				StopRotation ();
			}
			webView.Eval ("wsSendCommand ('command',  'walkTurnZ 70');");
			currentRotationalState = RotationalMotionState.RIGHT;
		}

		void TranslateLeft ()
		{
			if (currentDirectionState == DirectionalMotionState.LEFT) {
				return;
			}
			if (currentDirectionState == DirectionalMotionState.RIGHT) {
				StopTranslation ();
			}
			webView.Eval ("wsSendCommand ('command',  'walkX 70');");

			currentDirectionState = DirectionalMotionState.LEFT;
		}

		void TranslateRight ()
		{
			if (currentDirectionState == DirectionalMotionState.RIGHT) {
				return;
			}

			if (currentDirectionState == DirectionalMotionState.LEFT) {
				StopTranslation ();
			}
			webView.Eval ("wsSendCommand ('command',  'walkX 130');");
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
			rotationSlider.Maximum = 130;
			rotationSlider.Minimum = 70;
			rotationSlider.Value = 100;
			rotationSlider.ValueChanged += (sender, e) => {
				if (e.NewValue > 80 && e.NewValue < 110){
					StopTranslation();
				}else if (e.NewValue < 100){
					TranslateLeft();
				}else{
					TranslateRight();
				}
			};
				
			currentDirectionState = DirectionalMotionState.STRAIGHT;
			currentScalarState = ScalarMotionState.STOPPED;

			joyStickTranslation.OnValueChanged += (float xPos, float yPos) => {

				Debug.WriteLine("" + xPos + ", " + yPos);
				if (xPos > 0.5){
					TranslateRight();
				}else if (xPos < -0.5){
					TranslateLeft();
				}else{
					StopTranslation();
				}

				if (yPos > 0.5){
					GoBackScalar();
				}else if (yPos < -0.5){
					GoForwardScalar();
				}else{
					StopMoving();
				}
					
			};

			joyStickRotation.OnValueChanged += (float xPos, float yPos) => {
				if (xPos > 0.5){
					RotateRight();
				}else if (xPos < -0.5){
					RotateLeft();
				}else{
					StopRotation();
				}
			};

			stopButton.Clicked += async (object sender, EventArgs e) => {
				StopMoving();
				StopTranslation();
				StopRotation();
				await stopButton.AnimateButton();
				goForwardButton.BackgroundColor = defaultColor;
				goBackButton.BackgroundColor = defaultColor;
				StopAnimationSpider();
			};


			goForwardButton.Clicked += async (object sender, EventArgs e) => {

				GoForwardScalar();
				await goForwardButton.AnimateButton();
				goForwardButton.BackgroundColor = Color.Red;
				goBackButton.BackgroundColor = defaultColor;

				if (!animating){
					StartAnimationSpider();
				}
			
			};

			goBackButton.Clicked += async (object sender, EventArgs e) => {
				GoBackScalar();
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


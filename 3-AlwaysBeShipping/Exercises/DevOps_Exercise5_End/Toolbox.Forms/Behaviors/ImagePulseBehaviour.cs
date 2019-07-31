using System;
using System.ComponentModel;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Toolbox.Forms.Behaviors
{
    public class ImagePulseBehaviour : BehaviorBase<Image>
    {
        private TapGestureRecognizer recognizer;
        private double defaultScale = 1;
        private bool animating;

		protected override void OnAttachedTo(Image bindable)
		{
			base.OnAttachedTo(bindable);
            this.defaultScale = bindable.Scale;
            this.recognizer = new TapGestureRecognizer { NumberOfTapsRequired = 2 };
            this.recognizer.Tapped += this.PulseImage;
            bindable.GestureRecognizers.Add(this.recognizer);
		}

        protected override void OnDetachingFrom(Image bindable)
		{
            bindable.GestureRecognizers.Remove(this.recognizer);
            this.recognizer.Tapped -= this.PulseImage;
            this.recognizer = null;
            this.AssociatedObject.Scale = this.defaultScale;
			base.OnDetachingFrom(bindable);
		}

		private async void PulseImage(object sender, EventArgs e)
		{
            if (!this.animating)
            {
                this.animating = true;
				ViewExtensions.CancelAnimations(this.AssociatedObject);
				await this.AssociatedObject.ScaleTo(2, 250, Easing.CubicIn);
				await this.AssociatedObject.ScaleTo(1, 250, Easing.CubicOut);
                this.animating = false;
            }
		}
    }
}

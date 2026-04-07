using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Controls.Behaviors
{
    public class ImageButtonBounceBehavior : Behavior<ImageButton>
    {
        protected override void OnAttachedTo(ImageButton button)
        {
            base.OnAttachedTo(button);
            button.Pressed += OnButtonPressed;
            button.Released += OnButtonReleased;
        }

        // Срабатывает, когда кнопка уничтожается (чтобы не было утечек памяти)
        protected override void OnDetachingFrom(ImageButton button)
        {
            base.OnDetachingFrom(button);
            button.Pressed -= OnButtonPressed;
            button.Released -= OnButtonReleased;
        }

        private async void OnButtonPressed(object sender, EventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.View view)
            {
                Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(view);
                await Task.WhenAll(
                    view.ScaleTo(0.85, 100, Easing.CubicOut),
                    view.FadeTo(0.7, 100, Easing.CubicOut)
                );
            }
        }

        private async void OnButtonReleased(object sender, EventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.View view)
            {
                await Task.WhenAll(
                    view.ScaleTo(1.0, 250, Easing.SpringOut),
                    view.FadeTo(1.0, 250)
                );
            }
        }
    }
}

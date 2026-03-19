using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Controls.Behaviors
{
    public class ButtonBounceBehavior : Behavior<Button>
    {
        private float _originalShadowOpacity = 1f;

        protected override void OnAttachedTo(Button button)
        {
            base.OnAttachedTo(button);
            // Используем Pressed, чтобы реакция была МОМЕНТАЛЬНОЙ
            button.Pressed += OnButtonPressed;
            button.Released += OnButtonReleased;
        }

        protected override void OnDetachingFrom(Button button)
        {
            base.OnDetachingFrom(button);
            button.Pressed -= OnButtonPressed;
            button.Released -= OnButtonReleased;
        }

        private async void OnButtonPressed(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(button);

                // ХАК ОТ КВАДРАТНОЙ ТЕНИ: Делаем тень невидимой на время нажатия
                if (button.Shadow != null)
                {
                    _originalShadowOpacity = button.Shadow.Opacity;
                    button.Shadow.Opacity = 0;
                }

                await Task.WhenAll(
                    button.ScaleTo(0.85, 100, Easing.CubicOut),
                    button.FadeTo(0.8, 100, Easing.CubicOut)
                );
            }
        }

        private async void OnButtonReleased(object sender, EventArgs e)
        {
            if (sender is Button button)
            {
                await Task.WhenAll(
                    button.ScaleTo(1.0, 250, Easing.SpringOut),
                    button.FadeTo(1.0, 250)
                );

                // Возвращаем тень обратно после того, как кнопка "отпружинила"
                if (button.Shadow != null)
                {
                    button.Shadow.Opacity = _originalShadowOpacity;
                }
            }
        }
    }
}

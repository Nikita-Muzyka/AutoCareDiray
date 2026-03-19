using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Controls.Behaviors
{
    public class ViewBounceBehavior : Behavior<Microsoft.Maui.Controls.View>
    {
        private PointerGestureRecognizer _pointerGesture;

        protected override void OnAttachedTo(Microsoft.Maui.Controls.View view)
        {
            base.OnAttachedTo(view);

            // Создаем распознаватель указателя (пальца)
            _pointerGesture = new PointerGestureRecognizer();

            // Подписываемся на МОМЕНТАЛЬНЫЕ события касания и отпускания
            _pointerGesture.PointerPressed += OnPointerPressed;
            _pointerGesture.PointerReleased += OnPointerReleased;

            // Добавляем этот распознаватель к нашему элементу (view)
            view.GestureRecognizers.Add(_pointerGesture);
        }

        protected override void OnDetachingFrom(Microsoft.Maui.Controls.View view)
        {
            base.OnDetachingFrom(view);
            if (_pointerGesture != null)
            {
                _pointerGesture.PointerPressed -= OnPointerPressed;
                _pointerGesture.PointerReleased -= OnPointerReleased;
                view.GestureRecognizers.Remove(_pointerGesture);
            }
        }

        // --- ЛОГИКА АНИМАЦИИ ---

        private async void OnPointerPressed(object sender, PointerEventArgs e)
        {
            // Получаем элемент, на который нажали, как универсальный View
            if (sender is Microsoft.Maui.Controls.View view)
            {
                Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(view);

                // Вдавливаем (очень быстро)
                await Task.WhenAll(
                    view.ScaleTo(0.92, 100, Easing.CubicOut), // Карточки лучше сжимать меньше (до 92%)
                    view.FadeTo(0.8, 100, Easing.CubicOut)     // Делаем чуть прозрачным
                );
            }
        }

        private async void OnPointerReleased(object sender, PointerEventArgs e)
        {
            if (sender is Microsoft.Maui.Controls.View view)
            {
                // Отпускаем с пружиной
                await Task.WhenAll(
                    view.ScaleTo(1.0, 300, Easing.SpringOut), // Возвращаем 100% с пружиной в конце
                    view.FadeTo(1.0, 300)                     // Возвращаем полную непрозрачность
                );
            }
        }
    }

}

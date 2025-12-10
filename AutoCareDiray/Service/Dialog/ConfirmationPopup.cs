using CommunityToolkit.Maui.Views;
using Microsoft.Maui.Controls.Shapes;
using Microsoft.Maui.Graphics;

public class ConfirmationPopup : Popup
{
    private TaskCompletionSource<bool> _tcs = new TaskCompletionSource<bool>();
    public Task<bool> Result => _tcs.Task;

    public ConfirmationPopup(string message)
    {
        // Контейнер с Border + тень
        Content = new Border
        {
            Padding = 15,
            BackgroundColor = Colors.White,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Stroke = Colors.LightGray,
            Shadow = new Shadow
            {
                Brush = Colors.Black,
                Offset = new Point(5, 5),
                Opacity = 0.3f,
                Radius = 10
            },
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new Label
                    {
                        Text = message,
                        FontSize = 14,
                        TextColor = Colors.Black,
                        HorizontalOptions = LayoutOptions.Center,
                        HorizontalTextAlignment = TextAlignment.Center
                    },
                    new HorizontalStackLayout
                    {
                        Spacing = 15,
                        HorizontalOptions = LayoutOptions.Center,
                        Children =
                        {
                            new Button
                            {
                                Text = "Да",
                                BackgroundColor = Colors.Red,
                                TextColor = Colors.White,
                                Command = new Command(() =>
                                {
                                    _tcs.TrySetResult(true);
                                    this.Handler?.DisconnectHandler(); // Закрываем Popup
                                })
                            },
                            new Button
                            {
                                Text = "Нет",
                                BackgroundColor = Colors.Gray,
                                TextColor = Colors.White,
                                Command = new Command(() =>
                                {
                                    _tcs.TrySetResult(false);
                                    this.Handler?.DisconnectHandler(); // Закрываем Popup
                                })
                            }
                        }
                    }
                }
            }
        };
    }
}

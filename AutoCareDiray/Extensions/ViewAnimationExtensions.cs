using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Extensions
{
    public static class ViewAnimationExtensions
    {
        public static async Task CardBounceAsync(this Microsoft.Maui.Controls.View view)
        {
            if (view == null) return;

            Microsoft.Maui.Controls.ViewExtensions.CancelAnimations(view);

            await view.ScaleTo(0.95, 100, Easing.CubicOut);
            await view.ScaleTo(1.0, 100, Easing.CubicIn);
        }
    }
}

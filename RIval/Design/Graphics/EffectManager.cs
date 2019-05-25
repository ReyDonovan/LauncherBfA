using Ignite.Core;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;

namespace Ignite.Design.Graphics
{
    public class EffectManager : Singleton<EffectManager>
    {
        public void ChangeTextBlockColor(TextBlock text, Color color)
        {
            ColorAnimation anim = new ColorAnimation()
            {
                From = Colors.LightGray,
                To = color,
                Duration = TimeSpan.FromSeconds(0.3)
            };

            text.Cursor = System.Windows.Input.Cursors.Hand;
            text.Foreground.BeginAnimation(SolidColorBrush.ColorProperty, anim, HandoffBehavior.Compose);
        }

        public void SetDropShadow(FrameworkElement control, Color color, float opacity)
        {
            DropShadowEffect shadow = new DropShadowEffect
            {
                Color = color,
                BlurRadius = 30,
                Direction = 320,
                ShadowDepth = 0,
                Opacity = opacity,
            };
            ColorAnimation anim = new ColorAnimation(color, TimeSpan.FromSeconds(0.4));
            anim.From = Colors.Black;
            anim.To = Colors.White;

            control.Cursor = System.Windows.Input.Cursors.Hand;

            control.Effect = shadow;
            control.Effect.BeginAnimation(DropShadowEffect.ColorProperty, anim, HandoffBehavior.Compose);
        }
    }
}

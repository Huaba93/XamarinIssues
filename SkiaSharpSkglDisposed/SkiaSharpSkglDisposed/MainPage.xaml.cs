using System;
using System.Threading;
using System.Threading.Tasks;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace SkiaSharpSkglDisposed
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            CreateView();
        }


        private void CreateView()
        {
            var canvasView = new SKGLView {WidthRequest = 400, HeightRequest = 400};
            canvasView.HasRenderLoop = true;
            canvasView.PaintSurface += CanvasViewOnPaintSurface;
            var button = new Button {Text = "Redraw"};
            button.Clicked += ButtonClicked;

            Task.Run(() =>
            {
                while (true)
                {
                    Thread.Sleep(1000 / 60);
                    canvasView.InvalidateSurface();
                }
            });

            Content = new StackLayout {Children = {canvasView, button}};
        }

        private void ButtonClicked(object sender, EventArgs e)
        {
            CreateView();
        }

        private void CanvasViewOnPaintSurface(object sender, SKPaintGLSurfaceEventArgs e)
        {
            if (Width <= 0 || Height <= 0)
            {
                return;
            }

            var surface = e.Surface;
            var canvas = surface.Canvas;

            canvas.Clear();

            var colors = new[]
            {
                Color.Red.ToSKColor(), Color.Green.ToSKColor(), Color.Yellow.ToSKColor(), Color.Gray.ToSKColor(),
                Color.Black.ToSKColor(), Color.Blue.ToSKColor()
            };

            var rand = new Random();
            var indexOfColor = rand.Next(0, colors.Length - 1);

            var paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = colors[indexOfColor],
                StrokeWidth = 25
            };

            canvas.DrawCircle((float) 200, 200, 100, paint);
        }
    }
}
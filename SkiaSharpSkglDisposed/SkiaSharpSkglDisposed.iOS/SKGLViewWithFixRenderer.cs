using System;
using CoreAnimation;
using Foundation;
using SkiaSharp.Views.Forms;
using SkiaSharpSkglDisposed;
using SkiaSharpSkglDisposed.iOS;
using Xamarin.Forms;
using SKNativeView = SkiaSharp.Views.iOS.SKGLView;

[assembly: ExportRenderer(typeof(SKGLViewWithFix), typeof(SKGLViewWithFixRenderer))]

namespace SkiaSharpSkglDisposed.iOS
{
    public class SKGLViewWithFixRenderer : SKGLViewRendererBase<SKGLViewWithFix, SKNativeView>
    {
        private CADisplayLink _displayLink;

        public SKGLViewWithFixRenderer()
        {
            SetDisablesUserInteraction(true);
        }

        protected override SKNativeView CreateNativeControl()
        {
            var view = GetType() == typeof(SKGLViewRenderer)
                ? new SKNativeView()
                : base.CreateNativeControl();

            // Force the opacity to false for consistency with the other platforms
            view.Opaque = false;

            return view;
        }

        protected override void Dispose(bool disposing)
        {
            // stop the render loop
            if (_displayLink != null)
            {
                _displayLink.Invalidate();
                _displayLink.Dispose();
                _displayLink = null;
            }

            base.Dispose(disposing);
        }

        protected override void SetupRenderLoop(bool oneShot)
        {
            // only start if we haven't already
            if (_displayLink != null)
                return;

            // bail out if we are requesting something that the view doesn't want to
            if (!oneShot && !Element.HasRenderLoop)
                return;

            // if this is a one shot request, don't bother with the display link
            if (oneShot)
            {
                var nativeView = Control;
                nativeView?.BeginInvokeOnMainThread(() =>
                {
                    if (nativeView.Handle != IntPtr.Zero)
                        nativeView.Display();
                });
                return;
            }

            // create the loop
            _displayLink = CADisplayLink.Create(() =>
            {
                var nativeView = Control;
                var formsView = Element;

                // stop the render loop if this was a one-shot, or the views are disposed
                if (nativeView == null || formsView == null || nativeView.Handle == IntPtr.Zero ||
                    !formsView.HasRenderLoop)
                {
                    _displayLink.Invalidate();
                    _displayLink.Dispose();
                    _displayLink = null;
                    return;
                }

                // redraw the view
                nativeView.Display();
            });
            _displayLink.AddToRunLoop(NSRunLoop.Current, NSRunLoop.NSDefaultRunLoopMode);
        }
    }
}
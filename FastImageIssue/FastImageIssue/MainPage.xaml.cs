using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FastImageIssue
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            var visibleImage = new Image
            {
                Source = "song_entry_premium_symbol.png",
                HorizontalOptions = LayoutOptions.Start, HeightRequest = 50
            };
            var invisibleImage = new Image
            {
                Source = "song_entry_premium_symbol.png",
                HorizontalOptions = LayoutOptions.Start, IsVisible = false,
                HeightRequest = 50
            };
            var transparentImage = new Image
            {
                Source = "song_entry_premium_symbol.png",
                HorizontalOptions = LayoutOptions.Start, Opacity = 0,
                HeightRequest = 50
            };

            var offScreenImage = new Image
            {
                Source = "song_entry_premium_symbol.png",
                HorizontalOptions = LayoutOptions.Start, TranslationX = 1000,
                TranslationY = 1000, HeightRequest = 50
            };

            var visibleContainer = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = "song_entry_premium_symbol.png",
                        HorizontalOptions = LayoutOptions.Start, HeightRequest = 50
                    }
                }
            };

            var invisibleContainer = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = "song_entry_premium_symbol.png",
                        HorizontalOptions = LayoutOptions.Start, HeightRequest = 50
                    }
                },
                IsVisible = false
            };

            var transparentContainer = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = "song_entry_premium_symbol.png",
                        HorizontalOptions = LayoutOptions.Start, HeightRequest = 50
                    }
                },
                Opacity = 0
            };


            var offScreenContainer = new StackLayout
            {
                Children =
                {
                    new Image
                    {
                        Source = "song_entry_premium_symbol.png",
                        HorizontalOptions = LayoutOptions.Start, HeightRequest = 50
                    }
                },
                TranslationX = 1000, TranslationY = 1000
            };

            var contentWrapper = new StackLayout();
            contentWrapper.Children.Add(visibleImage);
            contentWrapper.Children.Add(invisibleImage);
            contentWrapper.Children.Add(transparentImage);
            contentWrapper.Children.Add(offScreenImage);

            contentWrapper.Children.Add(visibleContainer);
            contentWrapper.Children.Add(invisibleContainer);
            contentWrapper.Children.Add(transparentContainer);
            contentWrapper.Children.Add(offScreenContainer);

            Task.Run(async () =>
            {
                await Task.Delay(2000);
                await MainThread.InvokeOnMainThreadAsync(async () =>
                {
                    invisibleImage.IsVisible = true;
                    await Task.WhenAll(transparentImage.FadeTo(1), offScreenImage.TranslateTo(0, 0));

                    invisibleContainer.IsVisible = true;
                    await Task.WhenAll(transparentContainer.FadeTo(1), offScreenContainer.TranslateTo(0, 0));
                    
                    //when forcing layout on parent layout, it works. But that is not an acceptable solution
                    transparentContainer.ForceLayout();
                });
            });
            Content = contentWrapper;
        }
    }
}
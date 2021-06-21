using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using Akavache;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace AkavacheMultipleCachedCalls
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Task.Run(() =>
            {
                Registrations.Start(Guid.NewGuid().ToString());
                foreach (var workItem in _workItems.GetConsumingEnumerable())
                {
                    workItem();
                }
            });
        }

        private async Task<string> GetData1Async()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(1);
            return "TestData";
        }

        private async Task<string> GetData2Async()
        {
            Console.WriteLine(Thread.CurrentThread.ManagedThreadId);
            await Task.Delay(1);
            return "TestData2";
        }

        private Task InternalFetchAsync(string cacheKey, Func<Task<string>> fetchFunction)
        {
            return MakeAkavacheHappyAsync(cacheKey, fetchFunction);
        }

        private readonly BlockingCollection<Action> _workItems = new BlockingCollection<Action>();

        private Task<T> MakeAkavacheHappyAsync<T>(string cacheKey, Func<Task<T>> asyncAction)
        {
            var tcs = new TaskCompletionSource<T>();

            Console.WriteLine($"{cacheKey} started");

            var now = DateTime.UtcNow;
            var absoluteExpiration = now + TimeSpan.FromMinutes(100);
            _workItems.Add(() =>
            {
                Console.WriteLine($"Start Akavache {Thread.CurrentThread.ManagedThreadId}");

                var observable = BlobCache.LocalMachine.GetAndFetchLatest(
                    cacheKey,
                    asyncAction,
                    creationTime =>
                    {
                        TimeSpan elapsed = now - creationTime;
                        return elapsed > TimeSpan.FromSeconds(10);
                    },
                    absoluteExpiration,
                    true);
                observable.Subscribe(r =>
                {
                    tcs.TrySetResult(r);
                    Console.WriteLine("finished single one");
                });
                observable.Catch((Exception e) =>
                {
                    tcs.TrySetException(e);
                    return Observable.Never<T>();
                });
            });

            return tcs.Task;
        }

        private async void FetchAndGetLatestSameFetchActionClicked(object sender, EventArgs e)
        {
            FetchAndGetLatestSameFetchActionLabel.Text = "Started";
            var sw = new Stopwatch();
            sw.Start();
            var tasks = new List<Task>();

            for (var i = 0; i < 100000; i++)
            {
                var i1 = i;
                tasks.Add(InternalFetchAsync(
                    nameof(FetchAndGetLatestSameFetchActionClicked) + nameof(GetData1Async) + i1, GetData1Async));
            }

            await Task.WhenAll(tasks);
            Console.WriteLine($"{nameof(FetchAndGetLatestSameFetchActionClicked)} success");
            FetchAndGetLatestSameFetchActionLabel.Text = $"Finished in {sw.ElapsedMilliseconds} ms";
        }

        private void FetchAndGetLatestDifferentFetchActionClicked(object sender, EventArgs e)
        {
            Task.Run(async () =>
            {
                MainThread.BeginInvokeOnMainThread(() => FetchAndGetLatestDifferentFetchActionLabel.Text = "Started");

                var sw = new Stopwatch();
                sw.Start();
                var tasks = new List<Task>();

                for (var i = 0; i < 100000; i++)
                {
                    var i1 = i;
                    tasks.Add(Task.Run(() =>
                        InternalFetchAsync(
                            nameof(FetchAndGetLatestDifferentFetchActionClicked) + nameof(GetData1Async) + i1,
                            GetData1Async)));
                    tasks.Add(Task.Run(() =>
                        InternalFetchAsync(
                            nameof(FetchAndGetLatestDifferentFetchActionClicked) + nameof(GetData2Async) + i1,
                            GetData2Async)));
                }

                await Task.WhenAll(tasks);
                Console.WriteLine($"{nameof(FetchAndGetLatestDifferentFetchActionClicked)} success");
                MainThread.BeginInvokeOnMainThread(() =>
                    FetchAndGetLatestDifferentFetchActionLabel.Text = $"Finished in {sw.ElapsedMilliseconds} ms");
            });
        }
    }
}
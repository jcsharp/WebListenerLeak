using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MemoryClient
{
    public class LoadTest
    {
        int count;
        HttpClient client;
        Stopwatch stopwatch;

        public LoadTest(string uri)
        {
            client = new HttpClient(
                new HttpClientHandler { UseDefaultCredentials = true, PreAuthenticate = true })
            { BaseAddress = new Uri(uri) };
        }

        public void Run(TimeSpan time, int threads = 4)
        {
            Console.WriteLine($"Running load test on {client.BaseAddress} for {time}");
            var response = client.GetAsync($"api/Values/1", HttpCompletionOption.ResponseContentRead).Result;

            stopwatch = Stopwatch.StartNew();
            Parallel.For(0, threads, i => RunThread(time));
            stopwatch.Stop();

            Console.WriteLine($"Submitted {count} requests in {stopwatch.Elapsed} ({count / stopwatch.Elapsed.TotalSeconds} requests / s)");
        }

        private void RunThread(TimeSpan time)
        {
            var r = new Random();
            while (stopwatch.Elapsed < time)
            {
                int id = r.Next(1, 1800000);
                var response = client.GetAsync($"api/Values/{id}", HttpCompletionOption.ResponseContentRead).Result;
                Interlocked.Increment(ref count);
                if (!response.IsSuccessStatusCode) Console.WriteLine($"Failed: {response.StatusCode}");
            }
        }
    }
}
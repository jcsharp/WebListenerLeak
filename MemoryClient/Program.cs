using System;
using System.Threading.Tasks;

namespace MemoryClient
{
    class Program
    {
        static void Main(string[] args)
        {
            var perf = new LoadTest("http://localhost:5000");
            perf.Run(TimeSpan.FromMinutes(10), 16);
        }
    }
}

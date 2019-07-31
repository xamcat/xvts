using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Algorithms.Sorting;

namespace async_ex
{
    class Program
    {
        static void Main(string[] args)
        {
            var comparison = new Comparison();

            comparison.Compare().Wait();
        }
    }
}

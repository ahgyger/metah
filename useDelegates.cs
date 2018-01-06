using System;
using System.Diagnostics;
using System.Text;
using System.Threading;

namespace Threads
{
    class Program
    {
        public delegate string myDelegate(string txt);

        static void Main(string[] args)
        {
            SynchronousDelegateSample();
            AsynchronousDelegateSample();

            Console.WriteLine(“Main thread exits.”);
            Console.ReadKey();
        }

        static void SynchronousDelegateSample()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            /*
             * All the calls to the delegate will be synchronous
             * This mean that the order will always be 1,2,3,4,5,6
             * The thread used will always be the same.
             */           

            myDelegate dm = new myDelegate(DelegateMethod);
            Console.WriteLine(dm(“calling delegate (1)”));
            Console.WriteLine(dm(“calling delegate (2)”));
            Console.WriteLine(dm(“calling delegate (3)”));
            Console.WriteLine(dm(“calling delegate (4)”));
            Console.WriteLine(dm(“calling delegate (5)”));
            Console.WriteLine(dm(“calling delegate (6)”));

            sw.Stop();

            Console.WriteLine(“All work was done in: {0} milliseconds.nn”, sw.ElapsedMilliseconds);
        }

        static void AsynchronousDelegateSample()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            /*
             * All the work will be invoked in a blink.
             * Multiple thread will be used to compute the result of the method.
             */

            myDelegate dm = new myDelegate(DelegateMethod);

            IAsyncResult result1 = dm.BeginInvoke(“calling delegate (1)”, null, null);
            IAsyncResult result2 = dm.BeginInvoke(“calling delegate (2)”, null, null);
            IAsyncResult result3 = dm.BeginInvoke(“calling delegate (3)”, null, null);
            IAsyncResult result4 = dm.BeginInvoke(“calling delegate (4)”, null, null);
            IAsyncResult result5 = dm.BeginInvoke(“calling delegate (5)”, null, null);
            IAsyncResult result6 = dm.BeginInvoke(“calling delegate (6)”, null, null);
 

            /*
             * EndInvoke is synchronous we force to wait for the asynchronous results.
             */

            string r1 = dm.EndInvoke(result1);
            string r2 = dm.EndInvoke(result2);
            string r3 = dm.EndInvoke(result3);
            string r4 = dm.EndInvoke(result4);
            string r5 = dm.EndInvoke(result5);
            string r6 = dm.EndInvoke(result6);
 
            sw.Stop();

            Console.WriteLine(“All asynchronous  work was done in: {0}”,
                sw.ElapsedMilliseconds);
        }              

        ///<summary>
        /// Method called by our delegate
        /// Will wait for a random time (between 1 and 10000 milliseconds).
        ///</summary>
        ///<param name=”txt”>Any string are okay.</param>
        ///<returns>A string confirming the end of the process.</returns>

        static string DelegateMethod(string txt)
        {
            // Wait 5 seconds.
            Thread.Sleep(5000);

            // Work is finished
            Console.WriteLine(txt + ” n> Thread ID is: {0}”,
                Thread.CurrentThread.ManagedThreadId.ToString());

            return “>> “ + txt + ” Done !”;
        }
    }
}
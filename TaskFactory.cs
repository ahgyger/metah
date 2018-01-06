using System;
using System.Threading;
using System.Threading.Tasks;
 
namespace TasksDemo
{
    class Program
    {
        static void Main(string[] args)
        {
            UseTask demoTask = new UseTask();
            Console.ReadKey();
        }
    }
 
    class UseTask
    {
        public UseTask()
        {
            // Set the TaskFactory - Create and schedule objects
            var tf = new TaskFactory(
                TaskCreationOptions.AttachedToParent,
                TaskContinuationOptions.AttachedToParent);
 
            var F = tf.StartNew(() =&gt; DoSomething(‘F’));
            var E = tf.StartNew(() =&gt; DoSomething(‘E’));
            var D = tf.StartNew(() =&gt; DoSomething(‘D’));
 
            // C will be started only when F, E or D has been completed
            var C = tf.ContinueWhenAny(new Task[]{F, E, D}, tasks =&gt; DoSomething(‘C’)); 
 
            var B = tf.StartNew(() =&gt; DoSomething(‘B’));
 
            // A will only happens once B and C are completed.
            var A = tf.ContinueWhenAll(new Task[] {B, C}, tasks =&gt; DoSomething(‘A’));
        }
 
        private void DoSomething(char @char)
        {
            Console.WriteLine(“DoSomething called - {0}”, @char);
            Random rd = new Random();
            Thread.Sleep(rd.Next(1000, 3000));
            Console.WriteLine(“DoSomething called - {0} - DONE”, @char);
        }
    }
}
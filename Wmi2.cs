using System;
using System.Collections.Generic;
using System.Text;
using System.Management;

namespace WmiNamespace
{
    class Program
    {
        static void Main(string[] args)
        {
            //Represents the scoop for management operations
            ManagementScope ms = new ManagementScope();

            //Represents the management query
            ObjectQuery wql = new ObjectQuery("select * from Win32_DiskDrive");

            //Retrieve a collection of management objects
            ManagementObjectSearcher searcher = 
                new ManagementObjectSearcher(ms, wql);

            //Represents different collections
            ManagementObjectCollection oc = searcher.Get();

            //The enumerator of the collection
            ManagementObjectCollection.ManagementObjectEnumerator oe = 
                oc.GetEnumerator();

            Console.WriteLine("This class got " + oc.Count + " instance(s)n");

            //Enumerate the collection
            while (oe.MoveNext())
            {
                Console.WriteLine("n********* " +
                                  oe.Current.GetPropertyValue("Name") + "n");

                //Foreach of the properties existing in one of our instance
                //display the propety name and her value
                foreach (PropertyData prop in oe.Current.Properties)
                {
                    Console.WriteLine("&gt; " + prop.Name + " (" + prop.Value + ")");
                }
            }

            Console.ReadLine();
        }
    }
}
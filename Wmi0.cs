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

            //Provides a wrapper for building paths to WMI objects
            ManagementPath path = new ManagementPath(@"\localhostroot");
            ms.Path = path;

            ms.Connect();

            if (ms.IsConnected)
            //At this point we are connected to WMI
            {
                //Represents a management query that returns instances or classes
                ObjectQuery wql = new ObjectQuery("select * from __Namespace");

                //Retrieves a collection of 
                //management objects based on the specified query
                ManagementObjectSearcher searcher = 
                    new ManagementObjectSearcher(ms, wql);

                //Represents different collections 
               //of management objects retrieved through WMI
                ManagementObjectCollection oc = searcher.Get();

                //Represents the enumerator on the collection
                ManagementObjectCollection.ManagementObjectEnumerator oe =
                    oc.GetEnumerator();

                while (oe.MoveNext())
                {
                    foreach (PropertyData prop in oe.Current.Properties)
                    {
                        Console.WriteLine("t{0}", prop.Value);
                    }
                }
            }
            Console.ReadLine();
        }
    }
}
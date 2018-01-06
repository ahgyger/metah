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
            //Here we connect to the namespace called "CIMV2"
            ManagementPath path = new ManagementPath(@"\localhostrootCIMV2");

            //Represents a CIM management class 
            ManagementClass newClass = new ManagementClass(path);

            //Provides a base class for query and enumeration-related options objects
            EnumerationOptions options = new EnumerationOptions();

            //Return class members that are immediately related to the class
            options.EnumerateDeep = false;

            //Retrieve the  subclasses using the specified options
            foreach (ManagementObject o in newClass.GetSubclasses(options))
            {
                Console.WriteLine(Convert.ToString(o["__Class"]));
            }
            Console.ReadLine();
        }
    }
}
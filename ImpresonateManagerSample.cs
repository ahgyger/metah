class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Before impersonation: " + WindowsIdentity.GetCurrent().Name);

        try
        {
            ImpersonateManager.ImpersonateUser("domainName", "userName", "password");
            Console.WriteLine("Impersonated User: " + WindowsIdentity.GetCurrent().Name);
        }
        catch (System.ComponentModel.Win32Exception e)
        {
            Console.WriteLine("Exception while trying to impersonate: " + e);
        }

        ImpersonateManager.StopImpersonation();
        Console.WriteLine("After impersonation: " + WindowsIdentity.GetCurrent().Name);

        Console.ReadKey();
    }
}
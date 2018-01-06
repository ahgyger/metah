class Program
{
    static void Main(string[] args)
    {
        // Will impersonate the process based on a user existing on the same domain
        ProcessImpersonator.ImpersonateProcess_WithProfile(@"C:test.exe",
            "domain", "user", "password");

        // Will impersonate the call from the process based on a user on a domain
        // with no trust relationship.
        ProcessImpersonator.ImpersonateProcess_NetCredentials(@"C:test.exe",
            "Otherdomain", "user", "password");
        Console.ReadKey();
    }
}
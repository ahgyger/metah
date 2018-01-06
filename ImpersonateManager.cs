using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security.Principal;
using System.Security.Permissions;

[assembly: SecurityPermissionAttribute(SecurityAction.RequestMinimum, UnmanagedCode = true)]
[assembly: PermissionSetAttribute(SecurityAction.RequestMinimum, Name = "FullTrust")]
namespace ImpersonateThread
{
    public class ImpersonateManager
    {
        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Unicode)]
        public static extern bool LogonUser(String lpszUsername, String lpszDomain, String lpszPassword,
            int dwLogonType, int dwLogonProvider, ref IntPtr phToken);

        [DllImport("kernel32.dll", CharSet = System.Runtime.InteropServices.CharSet.Auto)]
        private unsafe static extern int FormatMessage(int dwFlags, ref IntPtr lpSource,
            int dwMessageId, int dwLanguageId, ref String lpBuffer, int nSize, IntPtr* Arguments);

        [DllImport("kernel32.dll", CharSet = CharSet.Auto)]
        public extern static bool CloseHandle(IntPtr handle);

        [DllImport("advapi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public extern static bool DuplicateToken(IntPtr ExistingTokenHandle,
            int SECURITY_IMPERSONATION_LEVEL, ref IntPtr DuplicateTokenHandle);

        // OurIdentity
        private static WindowsImpersonationContext _impersonatedUser;

        // Tokens
        private static IntPtr tokenHandle = new IntPtr(0);
        private static IntPtr dupeTokenHandle = new IntPtr(0);

        // If you incorporate this code into a DLL, be sure to demand FullTrust.
        [PermissionSetAttribute(SecurityAction.Demand, Name = "FullTrust")]
        public static void ImpersonateUser(string domainName, string userName, string password)
        {
            const int LOGON32_PROVIDER_DEFAULT = 0;
            const int LOGON32_LOGON_INTERACTIVE = 2;

            tokenHandle = IntPtr.Zero;

            // Call LogonUser to obtain a handle to an access token.
            bool returnValue = LogonUser(userName, domainName, password,
                LOGON32_LOGON_INTERACTIVE, LOGON32_PROVIDER_DEFAULT,
                ref tokenHandle);

            if (false == returnValue)
            {
                // Logon failure
                int ret = Marshal.GetLastWin32Error();
                throw new System.ComponentModel.Win32Exception(ret);
            }

            // Use the token handle returned by LogonUser.
            WindowsIdentity newId = new WindowsIdentity(tokenHandle);

            // Now the thread is impersonated.
            _impersonatedUser = newId.Impersonate();
        }

        public static void StopImpersonation()
        {
            // Stop impersonating the thread.
            _impersonatedUser.Undo();

            // Free the tokens.
            if (tokenHandle != IntPtr.Zero)
            {
                CloseHandle(tokenHandle);
            }
        }
    }
}
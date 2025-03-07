namespace Andreas.PowerGrip.Shared.Utilities;

public static class PamAuthenticator
{
    #region PAM Data Structures
    [StructLayout(LayoutKind.Sequential)]
    private struct pam_message
    {
        public int msg_style;
        public IntPtr msg;
    }

    [StructLayout(LayoutKind.Sequential)]
    private struct pam_response
    {
        public IntPtr resp;
        public int resp_retcode;
    }

    private delegate int PamConversationDelegate(int num_msg, IntPtr msg, out IntPtr resp, IntPtr appdata_ptr);

    [StructLayout(LayoutKind.Sequential)]
    private struct pam_conv
    {
        public PamConversationDelegate conv;
        public IntPtr appdata_ptr;
    }
    #endregion

    #region PAM functions from libpam.so.0 imports
    [DllImport("libpam.so.0", CharSet = CharSet.Ansi)]
    private static extern int pam_start(string serviceName, string user, ref pam_conv conv, out IntPtr pamh);

    [DllImport("libpam.so.0", CharSet = CharSet.Ansi)]
    private static extern int pam_authenticate(IntPtr pamh, int flags);

    [DllImport("libpam.so.0", CharSet = CharSet.Ansi)]
    private static extern int pam_acct_mgmt(IntPtr pamh, int flags);

    [DllImport("libpam.so.0", CharSet = CharSet.Ansi)]
    private static extern int pam_end(IntPtr pamh, int pamStatus);
    #endregion

    private static int Conversation(int num_msg, IntPtr msg, out IntPtr resp, IntPtr appdata_ptr)
    {
        resp = IntPtr.Zero;
        int structSize = Marshal.SizeOf(typeof(pam_response));

        try
        {
            // Allocate memory for responses
            resp = Marshal.AllocHGlobal(structSize * num_msg);
            string password = Marshal.PtrToStringAnsi(appdata_ptr) ?? string.Empty;

            for (int i = 0; i < num_msg; i++)
            {
                // For each message, create a response (assuming password input for all)
                pam_response pr = new()
                {
                    resp = Marshal.StringToHGlobalAnsi(password),
                    resp_retcode = 0
                };

                // Write the response struct to the allocated memory
                IntPtr ptr = resp + (i * structSize);
                Marshal.StructureToPtr(pr, ptr, false);
            }

            return (int)PamResult.PamSuccess;
        }
        catch
        {
            // Cleanup on failure
            if (resp != IntPtr.Zero)
            {
                for (int i = 0; i < num_msg; i++)
                {
                    IntPtr ptr = resp + (i * structSize);
                    pam_response pr = Marshal.PtrToStructure<pam_response>(ptr);
                    if (pr.resp != IntPtr.Zero)
                    {
                        Marshal.FreeHGlobal(pr.resp);
                    }
                }
                Marshal.FreeHGlobal(resp);
                resp = IntPtr.Zero;
            }
            return (int)PamResult.PamConversationError;
        }
    }

    public static bool Authenticate(string username, string password)
    {
        IntPtr appdata_ptr = IntPtr.Zero;
        IntPtr pamh = IntPtr.Zero;
        int returnValue = (int)PamResult.PamSuccess;

        try
        {
            appdata_ptr = Marshal.StringToHGlobalAnsi(password);
            pam_conv conv = new()
            {
                conv = Conversation,
                appdata_ptr = appdata_ptr
            };

            returnValue = pam_start("login", username, ref conv, out pamh);
            if (returnValue != (int)PamResult.PamSuccess)
            {
                return false;
            }

            try
            {
                returnValue = pam_authenticate(pamh, 0);
                if (returnValue != (int)PamResult.PamSuccess)
                {
                    return false;
                }

                returnValue = pam_acct_mgmt(pamh, 0);
                return returnValue == (int)PamResult.PamSuccess;
            }
            finally
            {
                pam_end(pamh, returnValue);
            }
        }
        finally
        {
            if (appdata_ptr != IntPtr.Zero)
            {
                Marshal.FreeHGlobal(appdata_ptr);
            }
        }
    }
}
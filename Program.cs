namespace LoL_AutoAccept
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool createdNew;
            using (var mutex = new Mutex(true, "LAA_SingleInstanceMutex", out createdNew))
            {
                if (!createdNew)
                {
                    MessageBox.Show("���ɃA�v���P�[�V�������N�����Ă��܂��B", "���d�N���h�~", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Form1());
            }
        }
    }
}
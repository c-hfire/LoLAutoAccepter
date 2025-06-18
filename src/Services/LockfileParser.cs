namespace LoLAutoAccepter.Services
{
    /// <summary>
    /// lockfile �̉�͂��s�����[�e�B���e�B�N���X
    /// </summary>
    public static class LockfileParser
    {
        /// <summary>
        /// lockfile �̓��e����͂��AAPI�̃x�[�XURL�ƔF�؏����擾���܂��B
        /// </summary>
        /// <param name="lockfileContent">lockfile �̓��e</param>
        /// <param name="baseUrl">API�̃x�[�XURL</param>
        /// <param name="auth">�F�؏��</param>
        /// <returns>��͂ɐ��������ꍇ�� true</returns>
        public static bool TryParse(string lockfileContent, out string baseUrl, out string auth)
        {
            baseUrl = string.Empty;
            auth = string.Empty;
            var parts = lockfileContent.Split(':');
            if (parts.Length < 5) return false;

            string port = parts[2];
            string token = parts[3];
            string protocol = parts[4].Trim();

            baseUrl = $"{protocol}://127.0.0.1:{port}";
            auth = Convert.ToBase64String(System.Text.Encoding.ASCII.GetBytes($"riot:{token}"));
            return true;
        }
    }
}
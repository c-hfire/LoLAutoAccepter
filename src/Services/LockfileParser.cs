namespace LoLAutoAccepter.Services
{
    /// <summary>LCU の lockfile を解析するユーティリティ</summary>
    public static class LockfileParser
    {
        /// <summary>
        /// lockfile の内容から LCU の base URL と Basic 認証文字列を取得します。解析に失敗した場合は false を返します。
        /// </summary>
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
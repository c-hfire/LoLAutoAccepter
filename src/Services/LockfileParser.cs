namespace LoLAutoAccepter.Services
{
    /// <summary>
    /// lockfile の解析を行うユーティリティクラス
    /// </summary>
    public static class LockfileParser
    {
        /// <summary>
        /// lockfile の内容を解析し、APIのベースURLと認証情報を取得します。
        /// </summary>
        /// <param name="lockfileContent">lockfile の内容</param>
        /// <param name="baseUrl">APIのベースURL</param>
        /// <param name="auth">認証情報</param>
        /// <returns>解析に成功した場合は true</returns>
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
using System.Net.Http.Headers;

namespace LoLAutoAccepter.Services
{
    /// <summary>
    /// LCU API への接続用 HttpClient を生成するユーティリティクラス
    /// </summary>
    public static class LcuApiClient
    {
        /// <summary>
        /// 認証情報付きの HttpClient を作成します。
        /// </summary>
        /// <param name="auth">認証情報</param>
        /// <returns>HttpClient インスタンス</returns>
        public static HttpClient Create(string auth)
        {
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };
            var client = new HttpClient(handler)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", auth);
            return client;
        }
    }
}
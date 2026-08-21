using System.Net.Http.Headers;

namespace LoLAutoAccepter.Services
{
    /// <summary>LCU API 接続用の HttpClient を生成するユーティリティ</summary>
    public static class LcuApiClient
    {
        /// <summary>Basic 認証を設定した HttpClient を作成します（自己署名証明書を許可します）</summary>
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
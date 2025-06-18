using System.Net.Http.Headers;

namespace LoLAutoAccepter.Services
{
    /// <summary>
    /// LCU API �ւ̐ڑ��p HttpClient �𐶐����郆�[�e�B���e�B�N���X
    /// </summary>
    public static class LcuApiClient
    {
        /// <summary>
        /// �F�؏��t���� HttpClient ���쐬���܂��B
        /// </summary>
        /// <param name="auth">�F�؏��</param>
        /// <returns>HttpClient �C���X�^���X</returns>
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
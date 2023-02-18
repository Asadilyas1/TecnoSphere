using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Net;

namespace TecnoSphere.Models.GoogleCaptcha
{
    public class GoogleCaptchaService
    {
        private readonly IOptionsMonitor<GoogleCaptchaConfig> _config;

        public GoogleCaptchaService(IOptionsMonitor<GoogleCaptchaConfig> config)
        {
            _config = config;
        }
        public async Task<bool> verifyToken(string token)
        {
            try
            {
                var url = $"https://www.google.com/recaptcha/api/siteverify?secret={_config.CurrentValue.SecretKey}&response={token}";
                using (var client = new HttpClient())
                {
                    var httpResult = await client.GetAsync(url);
                    if (httpResult.StatusCode != HttpStatusCode.OK)
                    {
                        return false;
                    }
                    var responce = await httpResult.Content.ReadAsStringAsync();
                    var GoogleResult = JsonConvert.DeserializeObject<GoogleCaptchaResponce>(responce);
                    return GoogleResult.success && GoogleResult.score >= 0.5;
                }
            }
            catch
            {
                return false;
            }
        }
    }
}

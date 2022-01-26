using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace XYMJPYPrice.Net
{
    public class Http
    {
        private readonly HttpClient _httpClient = null;

        public string Url { get; set; }

        public Http()
        {
            if (_httpClient == null) { _httpClient = new HttpClient(); }
        }

        ~Http()
        {
            Dispose();
        }

        public void Dispose()
        {
            if (_httpClient != null) { _httpClient.Dispose(); }
        }

        public async Task<(int statuscode, string content)> HttpContent()
        {
            try
            {
                HttpResponseMessage response = await _httpClient.GetAsync(Url).ConfigureAwait(false);
                response.EnsureSuccessStatusCode();
                HttpStatusCode statuscode = response.StatusCode;
                string content = await response.Content.ReadAsStringAsync();
                return ((int)statuscode, content);
            }
            catch (HttpRequestException e)
            {
                System.Diagnostics.Debug.WriteLine("Exception Message :{0} ", e.Message);
            }
            return (-1, null);
        }
    }
}

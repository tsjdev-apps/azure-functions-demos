using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.ProjectOxford.Vision;

namespace CognitiveServicesAzureFunctions
{
    public static class ImageDescriptionFunction
    {
        [FunctionName("ImageDescriptionFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "get", "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            try
            {
                log.Info("ImageDescriptionFunction - Started");

                var queryNameValuePairs = req.GetQueryNameValuePairs().ToList();

                // get url and apikey
                var url = queryNameValuePairs.FirstOrDefault(q => string.Compare(q.Key, "url", StringComparison.OrdinalIgnoreCase) == 0).Value;
                var apiKey = queryNameValuePairs.FirstOrDefault(q => string.Compare(q.Key, "apikey", StringComparison.OrdinalIgnoreCase) == 0).Value;
                var domainEndpoint = queryNameValuePairs.FirstOrDefault(q => string.Compare(q.Key, "domain", StringComparison.OrdinalIgnoreCase) == 0).Value;
                
                dynamic data = await req.Content.ReadAsAsync<object>();

                // get url from body
                if (url == null)
                    url = data?.url?.ToString();

                // get apikey from body
                if (apiKey == null)
                    apiKey = data?.apikey?.ToString();

                if (domainEndpoint == null)
                    domainEndpoint = data?.domain?.ToString();

                if (string.IsNullOrEmpty(url))
                    return req.CreateResponse(HttpStatusCode.BadRequest, "No image provided");

                if (string.IsNullOrEmpty(apiKey))
                    return req.CreateResponse(HttpStatusCode.BadRequest, "No apikey provided");

                if (string.IsNullOrEmpty(domainEndpoint))
                    return req.CreateResponse(HttpStatusCode.BadRequest, "No domain endpoint provided");

                log.Info($"ImageDescriptionFunction - Url: {url}");
                log.Info($"ImageDescriptionFunction - ApiKey: {apiKey}");
                log.Info($"ImageDescriptionFunction - Endpoint: {domainEndpoint}");

                // analyze image from url with the provided apikey
                var service = new VisionServiceClient(apiKey, $"https://{domainEndpoint}.api.cognitive.microsoft.com/vision/v1.0");
                var visualFeatures = new[] { VisualFeature.Description };
                var result = await service.AnalyzeImageAsync(HttpUtility.UrlDecode(url), visualFeatures);

                // send the result back
                return req.CreateResponse(HttpStatusCode.OK, result?.Description?.Captions);
            }
            catch (Exception e)
            {
                return req.CreateResponse(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}

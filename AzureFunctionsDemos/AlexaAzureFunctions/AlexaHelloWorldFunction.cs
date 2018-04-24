using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AlexaAzureFunctions
{
    public static class AlexaHelloWorldFunction
    {
        [FunctionName("AlexaHelloWorldFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("AlexaHelloWorldFunction - Started.");

            await Task.Yield();

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.0",
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Hallo Welt aus einer Azure Function!"
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Hello World",
                        content = "Hello World from Azure Function."
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}

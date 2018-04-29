using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AlexaAzureFunctions
{
    public static class AlexaHelloNameFunction
    {
        [FunctionName("AlexaHelloNameFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("AlexaHelloNameFunction - Started");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // get name from body data
            string name = data?.request?.intent?.slots?.name?.value?.ToString();

            if (name == null)
            {
                log.Info("AlexaHelloNameFunction - No name detected");

                return req.CreateResponse(HttpStatusCode.OK, new
                {
                    version = "1.0",
                    response = new
                    {
                        outputSpeech = new
                        {
                            type = "PlainText",
                            text = "Ich habe leider deinen Namen nicht richtig verstanden..."
                        },
                        card = new
                        {
                            type = "Simple",
                            title = "Hello Name!",
                            content = "Leider wurde dein Name nicht erkannt..."
                        },
                        shouldEndSession = true
                    }
                });
            }

            log.Info($"AlexaHelloNameFunction - Name: {name}");

            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.0",
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = $"Wie geht es denn so, {name.ToUpper()}? Freut mich dich kennenzulernen."
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Hello Name!",
                        content = $"Hallo {name.ToUpper()}!"
                    },
                    shouldEndSession = true
                }
            });
        }
    }
}

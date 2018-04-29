using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Azure.WebJobs.Host;

namespace AlexaAzureFunctions
{
    public static class AlexaCalculatorFunction
    {
        [FunctionName("AlexaCalculatorFunction")]
        public static async Task<HttpResponseMessage> Run([HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = null)]HttpRequestMessage req, TraceWriter log)
        {
            log.Info("AlexaCalculatorFunction - Started");

            // Get request body
            dynamic data = await req.Content.ReadAsAsync<object>();

            // check the request type for Launch Request
            if (data.request.type == "LaunchRequest")
            {
                // default launch request
                log.Info("AlexaCalculatorFunction - LaunchRequest");

                return DefaultRequest(req);
            }

            // check the request type for Intent Request
            if (data.request.type == "IntentRequest")
            {
                var intentName = data.request.intent.name;

                log.Info($"AlexaCalculatorFunction - IntentRequest: {intentName}");

                var num1 = Convert.ToDouble(data.request.intent.slots["firstnum"].value);
                var num2 = Convert.ToDouble(data.request.intent.slots["secondnum"].value);
                double result;

                switch (intentName.Value)
                {
                    case "AddIntent":
                        result = num1 + num2;

                        return req.CreateResponse(HttpStatusCode.OK, new
                        {
                            version = "1.0",
                            sessionAttributes = new { },
                            response = new
                            {
                                outputSpeech = new
                                {
                                    type = "PlainText",
                                    text = $"Das Ergebnis aus der Addition von {num1} und {num2} lautet: {result}."
                                },
                                card = new
                                {
                                    type = "Simple",
                                    title = "Alexa Taschenrechner",
                                    content = $"{num1} + {num2} = {result}."
                                },
                                shouldEndSession = true
                            }
                        });
                    case "SubstractIntent":
                        result = num1 - num2;

                        return req.CreateResponse(HttpStatusCode.OK, new
                        {
                            version = "1.0",
                            sessionAttributes = new { },
                            response = new
                            {
                                outputSpeech = new
                                {
                                    type = "PlainText",
                                    text = $"Das Ergebnis aus der Subtraktion von {num1} und {num2} lautet: {result}."
                                },
                                card = new
                                {
                                    type = "Simple",
                                    title = "Alexa Taschenrechner",
                                    content = $"{num1} - {num2} = {result}."
                                },
                                shouldEndSession = true
                            }
                        });
                    case "MultiplyIntent":
                        result = num1 * num2;

                        return req.CreateResponse(HttpStatusCode.OK, new
                        {
                            version = "1.0",
                            sessionAttributes = new { },
                            response = new
                            {
                                outputSpeech = new
                                {
                                    type = "PlainText",
                                    text = $"Das Ergebnis aus der Multiplikation von {num1} und {num2} lautet: {result}."
                                },
                                card = new
                                {
                                    type = "Simple",
                                    title = "Alexa Taschenrechner",
                                    content = $"{num1} * {num2} = {result}."
                                },
                                shouldEndSession = true
                            }
                        });
                    case "DivideIntent":
                        if (num2 == 0)
                        {
                            return req.CreateResponse(HttpStatusCode.OK, new
                            {
                                version = "1.0",
                                sessionAttributes = new { },
                                response = new
                                {
                                    outputSpeech = new
                                    {
                                        type = "PlainText",
                                        text = "Du hast gerade versucht durch 0 zu teilen. Das klappt leider nicht. Versuche es bitte mit einer anderen Aufgaben."
                                    },
                                    card = new
                                    {
                                        type = "Simple",
                                        title = "Alexa Taschenrechner",
                                        content = "Du hast gerade versucht durch 0 zu teilen. Das klappt leider nicht. Versuche es bitte mit einer anderen Aufgaben."
                                    },
                                    shouldEndSession = true
                                }
                            });
                        }
                        else
                        {
                            result = num1 / num2;

                            return req.CreateResponse(HttpStatusCode.OK, new
                            {
                                version = "1.0",
                                sessionAttributes = new { },
                                response = new
                                {
                                    outputSpeech = new
                                    {
                                        type = "PlainText",
                                        text = $"Das Ergebnis aus der Division von {num1} und {num2} lautet: {Math.Round(result, 2)}."
                                    },
                                    card = new
                                    {
                                        type = "Simple",
                                        title = "Alexa Taschenrechner",
                                        content = $"{num1} / {num2} = {result}."
                                    },
                                    shouldEndSession = true
                                }
                            });
                        }
                    default:
                        return DefaultRequest(req);
                }
            }
            else
            {
                return DefaultRequest(req);
            }
        }

        private static HttpResponseMessage DefaultRequest(HttpRequestMessage req)
        {
            return req.CreateResponse(HttpStatusCode.OK, new
            {
                version = "1.0",
                sessionAttributes = new { },
                response = new
                {
                    outputSpeech = new
                    {
                        type = "PlainText",
                        text = "Willkommen zum Alexa-Taschenrechner. Ich kann zwei Zahlen addieren, subtrahieren, multiplizieren und auch dividieren. Frage mich zum Beispiel: Was ist drei plus zwei."
                    },
                    card = new
                    {
                        type = "Simple",
                        title = "Alexa Taschenrechner",
                        content = "Willkommen zum Alexa-Taschenrechner. Ich kann zwei Zahlen addieren, subtrahieren, multiplizieren und auch dividieren. Frage mich zum Beispiel: 3 + 2."
                    },
                    shouldEndSession = false
                }
            });
        }

    }
}

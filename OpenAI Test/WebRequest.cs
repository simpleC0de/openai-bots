using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace OpenAI_Test
{
    class WebRequest
    {
        public static async Task<bool> createCompletionAPIRequest(string api_key, string question)
        {

            try
            {
                Dictionary<string, object> requestBody = new Dictionary<string, object>
            {
                {"prompt", question },
                {"temperature", 0.5 },
                {"max_tokens", 50 },
                {"top_p", 1 },
                {"frequency_penalty", 0 },
                {"presence_penalty", 1 },
                {"stop", "\n" }
            };


                string myJson = JsonConvert.SerializeObject(requestBody);


                using (var client = new HttpClient())
                {
                    // Creating bearer authorization key
                    client.DefaultRequestHeaders.Authorization =
                        new AuthenticationHeaderValue("Bearer", api_key);

                    // Sending the POST request
                    HttpResponseMessage response = await client.PostAsync(
                        "https://api.openai.com/v1/engines/ada/completions",
                         new StringContent(myJson, Encoding.ASCII, "application/json"));

                    // Parsing the returned JSON object of the POST request
                    JObject obj = JObject.Parse(response.Content.ReadAsStringAsync().Result);

                    // Deserializing the body to get access to the 'text' property
                    Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(obj.ToString());

                    // API continues to respond with various codes, might be better to remove them
                    Console.WriteLine("Bot: " + myDeserializedClass.choices[0].text.Replace("«", ""));
                    return true;
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.StackTrace);
                return false;
            }

        }
        

        // Struct of the json body for the POST respond
        public class Choice
        {
            public string text { get; set; }
            public int index { get; set; }
            public object logprobs { get; set; }
            public string finish_reason { get; set; }
        }

        public class Root
        {
            public string id { get; set; }
            public string @object { get; set; }
            public int created { get; set; }
            public string model { get; set; }
            public List<Choice> choices { get; set; }
        }
    }

}

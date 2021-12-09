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
    class CompletionRequest
    {
        public static async Task<bool> createApiRequest(string api_key, string question, Dictionary<string, object> config)
        {

            try
            {
                Dictionary<string, object> requestBody = new Dictionary<string, object>();
                try
                {
                    requestBody = new Dictionary<string, object>
                {
                    {"prompt", question },
                    {"temperature", Math.Round(double.Parse(config["temperature"].ToString()), 1) },
                    {"max_tokens", int.Parse(config["max_tokens"].ToString()) },
                    {"top_p", Math.Round(double.Parse(config["top_p"].ToString()), 1) },
                    {"frequency_penalty", Math.Round(double.Parse(config["frequency_penalty"].ToString()), 1) },
                    {"presence_penalty", Math.Round(double.Parse(config["presence_penalty"].ToString()), 1) },
                    {"stop", "\n" }
                };

                }
                catch (Exception)
                {
                    Console.Clear();
                    Console.WriteLine("Error reading config\nCheck the config, or replace with original one and restart the application");
                    System.Threading.Thread.Sleep(5000);
                }

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
                    
                    //This prints the whole respond message
                    //Console.WriteLine(obj.ToString());
                    
                    
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
                Console.WriteLine("Press Enter to close");
                Console.ReadLine();
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

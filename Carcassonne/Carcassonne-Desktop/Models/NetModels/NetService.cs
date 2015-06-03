using System;
using System.IO;
using System.Security;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Carcassonne_Desktop.Models.NetModels
{
    public class NetService
    {
        private Uri baseURI { get; set; }

        public NetService()
        {
            baseURI = new Uri(resources.URLResource.BaseURL);
        }

        protected async Task<string> PostFormContent(string requestURI, FormUrlEncodedContent parameters)
        {
            string json;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseURI;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded")); 
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.PostAsync(requestURI, parameters).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        using (Stream s = await response.Content.ReadAsStreamAsync())
                        {
                            json = new StreamReader(s).ReadToEnd();
                        }

                        return json;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }catch(Exception){
                throw new Exception();
            }
        }

        protected async Task<string> GetJSON(Uri requestURI)
        {
            string json;
            try
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = baseURI;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded"));
                    client.DefaultRequestHeaders.Accept.Add(
                        new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = await client.GetAsync(requestURI).ConfigureAwait(false);

                    if (response.IsSuccessStatusCode)
                    {
                        using (Stream s = await response.Content.ReadAsStreamAsync())
                        {
                            json = new StreamReader(s).ReadToEnd();
                        }

                        return json;
                    }
                    else
                    {
                        throw new Exception();
                    }
                }
            }
            catch (Exception)
            {
                throw new Exception();
            }
        }

    }
}

using System.Collections.Generic;
using System.Net.Http.Json;
using GPO_BLAZOR.Client.Parts;
using Microsoft.AspNetCore.Components;

namespace GPO_BLAZOR.Client.Class.Date
{



    public static class IPaddress
    {
        public const string IPAddress = "localhost:3001";
    }

    
    public static class StatmensTable
    {



        public async static Task<IEnumerable<(string, string, int, int)>> GetStatmens(string Token)
        {
            //Получение Информации
            //id tyme status type
            var sentDate = new { userlogin = Token };


            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/getstatmens/user:{Token}") ;
            JsonContent content = JsonContent.Create(sentDate);

            /*--------*/
            try
            {
                using HttpResponseMessage response = await httpClient.GetAsync(httpClient.BaseAddress);

                string responseText = await response.Content.ReadAsStringAsync();
                responseText = responseText.Trim(new char[] { '[', ']', '{', '}' });
                IEnumerable<string> a = responseText.Split("}, {");
                IEnumerable<IEnumerable<string>> b = a.Select(item => item.Split(", "));
                IEnumerable<IEnumerable<IEnumerable<string>>> c = b
                    .Select(item => item
                    .Select(temp => temp.Split(": ")));

                
            var res = c.Select(item => (
            (item.ToArray()[0].ToArray()[1], item
            .ToArray()[1].ToArray()[1], Convert.ToInt32(item
            .ToArray()[2].ToArray()[1]), Convert.ToInt32(item
            .ToArray()[3].ToArray()[1]))
            ));

                throw new Exception("SpecialString");
                
                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message+"Up Component");
                return new (string, string, int, int)[]{ ("1", "1", 1, 1), ("2", "2", 2, 2), ("3", "3", 3, 3) };
            }
            // /getstatmens
        }
    }

    public static class GetStatmen
    {

    }
}

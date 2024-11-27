using Microsoft.AspNetCore.Components;
using System.Net;
using System.Text;

namespace GPO_BLAZOR.Client.Class.Date
{
    public abstract class StatmenCreator : ComponentBase
    {

        protected Dictionary<string, string> fields;

        protected Dictionary<string, IEnumerable<string>> values;

        protected bool isLoading { get; set; } = true;

        [Parameter]
        public EventCallback<string> callback { get; set; }


        static async Task<string> SentRequestion(Dictionary<string, string> values)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    Uri uri = new Uri($"http://{IPaddress.IPAddress}/GetNuwPostInfo/{values.Select(x => $"{x.Key}:{x.Value}").Aggregate((x, y) => (x + "/" + y))}");
                    Console.WriteLine(uri.ToString());
                    client.BaseAddress = uri;
                    var c = await client.GetAsync(client.BaseAddress);
                    var result = c.Content.ToString();

                    return result;
                }
            } catch 
            {
                return "Error";
            }

        }

        static async Task<Dictionary<string,IEnumerable<string>>> GetAtributes()
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    var result = new Dictionary<string, IEnumerable<string>>();
                    Uri uri = new Uri($"http://{IPaddress.IPAddress}/GetAtributes");
                    client.BaseAddress = uri;
                    HttpResponseMessage responce = await client.GetAsync(uri);
                    if (responce.IsSuccessStatusCode)
                    {
                        if (responce.StatusCode == System.Net.HttpStatusCode.OK)
                        {

                            ///Разобраться с парсингом
                            string content = responce.Content.ReadAsStream().ToString();
                            return content.Split("],").Select(x => x.Split(",[")).Select(x => new KeyValuePair<string, IEnumerable<string>>(x[0], x[1].Split(",\n"))).ToDictionary();

                        }
                        else
                        {
                            var a = responce.StatusCode.ToString();
                            result.Add("ErrorMessage", new string[] { a.ToString() });
                            return result;
                        }
                    }
                    else
                    {
                        result.Add("ErrorMessage", new string[] { "Неудачный запрос" });
                        return result;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                var result = new Dictionary<string, IEnumerable<string>>
                {
                    { "ErrorMessage", new string[] { "Ошибка в запросе" } }
                };
                return result;
            }
        }

        protected async Task ButtonClicked()
        {
            string result = await SentRequestion(fields);
            if (result != "Error")
            callback.InvokeAsync(result);
            
        }

        protected async override Task OnInitializedAsync()
        {
            base.OnInitializedAsync();

            values = await GetAtributes();
            fields = values.Select(x=>new KeyValuePair<string, string> (x.Key, "")).ToDictionary();
        }

        protected async override Task OnAfterRenderAsync(bool firstRender)
        {
            base.OnAfterRenderAsync(firstRender);

            isLoading = false;
        }


    }

}

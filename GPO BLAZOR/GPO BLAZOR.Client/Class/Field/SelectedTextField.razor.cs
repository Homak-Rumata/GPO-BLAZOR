using Microsoft.AspNetCore.Components;

namespace GPO_BLAZOR.Client.Class.Field
{
    public partial class SelectedTextField: Date.Field
    {

        [Parameter]
        public Date.ICollectionValue<string> field { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                field.CollectionValues = (await GetCollectionValues(field.namevalue)).ToList();
            }
            finally
            {
                base.OnInitializedAsync();
            }
        }

        public override string GetValue ()
        {
            return $"\"{idvalue.ToString()}\": \"{value.ToString()}\"";
        }

        private async static Task<IEnumerable<string>> GetCollectionValues(string Field)
        {
            try
            {
                HttpClient httpclient = new HttpClient();
                Uri uri = new Uri($"http://{Date.IPaddress.IPAddress}/GetAtributes/{Field}");
                httpclient.BaseAddress = uri;
                var request = await httpclient.GetAsync(httpclient.BaseAddress);
                if (request.IsSuccessStatusCode && (request.StatusCode == System.Net.HttpStatusCode.OK))
                {
                    try
                    {
                        string result = await request.Content.ReadAsStringAsync();
                        var c = result.Split(",\n");
                        return c;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"{Field}: {request.StatusCode.ToString()} -> {ex}");
                        return null;
                    }
                }
                else
                {
                    return new List<string>();
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine($"{Field} -> {ex}");
                return null;
            }
        }
        


    }
}

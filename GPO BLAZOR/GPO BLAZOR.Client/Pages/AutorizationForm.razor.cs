using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;
using GPO_BLAZOR.Client.Class.JSRunTimeAccess;

namespace GPO_BLAZOR.Client.Pages
{
    public partial class AutorizationForm
    {
        //[Parameter]
        //public CookieStorageAccessor cookieStorage {  get; set; }

        [Parameter]
        public IAuthorizationDate AuthorizationInterface { get; set; }

        [Parameter]
        public EventCallback<IAuthorizationDate> AuthorizationInterfaceChanged { get; set; }

        IJSRuntime runtime;

        private string message = null;
        private bool isLoading { get; set; } = true;

        async Task ButtonClicked()
        {
            

            if (AuthorizationInterface.IsCookies)
            {
                try
                {

                    await AuthorizationInterfaceChanged.InvokeAsync(AuthorizationInterface);
                }
                catch
                {

                }
            }
            else
            {

                message = "Неверное имя пользователя или пароль";
            }

        }

        string value = "value";

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            isLoading = false;
        }

        protected override async Task OnParametersSetAsync()
        {

        }



        protected override async Task OnInitializedAsync()
        {
            isLoading = true;
            AuthorizationInterface._writer = WriteCookies;
            AuthorizationInterface._reader = ReadCookies;
            try
            {
                await AuthorizationInterface.GetValues(ReadCookies);
                await AuthorizationInterfaceChanged.InvokeAsync(AuthorizationInterface);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                await Checer();
            }
            finally
            {
            }



        }



        protected async Task Checer()
        {
            await AuthorizationInterface.GetValues(ReadCookies);
            await AuthorizationInterfaceChanged.InvokeAsync();
        }




        protected async Task WriteCookies(string value)
        {
            //await JSRuntime.InvokeAsync<string>("WriteCookie.WriteCookie", "token", value, DateTime.Now.AddMinutes(1));

            await cookieStorage.WriteCookieAsync("token", value, DateTime.Now.AddMinutes(1));
        }


        protected async Task<string> ReadCookies()
        {
            var temp = await cookieStorage.ReadCookieAsync<string>("token");
            try
            {
                if (temp != null || temp != "")
                {
                    await AuthorizationInterfaceChanged.InvokeAsync(AuthorizationInterface);
                }
                return temp;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return ("");
            }
        }

        async Task PostR()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri("https://localhost:3001/userinfo");
            var temp = new { lol = "kek" };
            JsonContent cx = JsonContent.Create(temp);
            using var response3 = await httpClient.PostAsync(httpClient.BaseAddress, cx);

            StringContent ghd = new StringContent("{\"field1\":\"111\", \"field2\":\"112\"}");
            ghd.Headers.Add("SecreteCode", ["Anuthing", "DoubleValue"]);
            /*--------*/
            await httpClient.PostAsync(httpClient.BaseAddress, ghd);
            Console.WriteLine(ghd.ReadAsStringAsync().Result);
        }
    }
}

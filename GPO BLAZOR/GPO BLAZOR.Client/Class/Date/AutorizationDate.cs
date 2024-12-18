using Microsoft.AspNetCore.Authorization;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GPO_BLAZOR.Client.Class.JSRunTimeAccess;
using System.Net.Http.Headers;
using System.Net;
using System.Timers;

namespace GPO_BLAZOR.Client.Class.Date
{
    /// <summary>
    /// Класс - точка входа для авторизации
    /// </summary>
    public class AuthorizationDate:IAuthorizationDate, IDisposable
    {
        /// <summary>
        /// Конструктор точки доступа
        /// </summary>
        /// <param name="reader"> Делегат чтения в хранилище </param>
        /// <param name="writer"> Делегат записи в хранилище </param>
        public AuthorizationDate(Reader reader, Writer writer) 
            :this()
        {
            _reader = reader;
            _writer = writer;
            
        }



        /// <summary>
        /// Пустой блок - на случай ошибки доступа
        /// </summary>
        public AuthorizationDate()
        {
            Requesting.ErrorAutorization += AutorizationErrorAction;
        }

        private void AutorizationErrorAction()
        {
            this.IsCookies = false;
        }

        public void Dispose()
        {
            Requesting.ErrorAutorization -= AutorizationErrorAction;
        }

        ~AuthorizationDate ()
        {
            this.Dispose();
        }

        /// <summary>
        /// Дополнительный асинхронный метод для инициализации
        /// </summary>
        /// <param name="reader"> Делегат для чтения из хранилища </param>
        /// <returns></returns>
        public async Task GetValues (Reader reader, System.Timers.Timer timer, IAutorizationStruct autorizationStruct)
        {
            try
            { 
                var IsReader = reader is not null;
                if (IsReader)
                {
                    string temp = (await reader("Autorization")) ?? "";
                    if (temp != null && temp != "")
                    {
                        IsCookies = true;
                        await RewriteJWT();
                        autorizationStruct.Role = roles.Select(AutorizationStruct.RoleSelector).ToArray();
                        TimeSkipAndRewrite(timer);
                    }
#if DEBUG
                    else
                    {
                        Console.WriteLine("HAS TEMP IS VAR OF ::: "+temp);
                    }
#endif
                }
#if DEBUG
                else
                {
                    Console.WriteLine("HAS TEMP IS VAR OF ->->->");
                }
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine("GetValueError -> "+ex.Message); 
            }
        }
        /// <summary>
        /// Есть ли соответствующая запись в хранилище
        /// </summary>
        public bool IsCookies { get; private set; }
        /// <summary>
        /// Сообщение пришедшее с сервера (на случай ошибки)
        /// </summary>
        public string RequestMessage { get; private set;  } = null;
        /// <summary>
        /// Поле заполненного логина
        /// </summary>
        private string _name;
        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string[] roles;

        /// <summary>
        /// Поле для хранения заполненного пароля
        /// </summary>
        public string Password { get; set; }

        
        /// <summary>
        /// Поле хранящее делегат чтения
        /// </summary>
        public Reader _reader { get; private set; }
        /// <summary>
        /// Поле хранящее делегат записи
        /// </summary>
        public Writer _writer { get; private set; }

        /// <summary>
        /// Класс для описания контракта получаемых данных
        /// </summary>
        private class Date
        {
            public string token { get; set; }
            public string[] role { get; set; }
            public string jwt { get; set; }
        }

        /// <summary>
        /// Контракт описания ошибки получения доступа
        /// </summary>
        private class ErrorMessage
        {
            public string messege { get; set; }
        }

        private record struct NewJWTResponce
        {
            public string jwt { get; set; }
        }

        /// <summary>
        /// Обновление  JWT
        /// </summary>
        /// <returns></returns>
        private async void RewriteJWT(object source, ElapsedEventArgs e)
        {
            await RewriteJWT();
        }

        /// <summary>
        /// Обновление  JWT
        /// </summary>
        /// <returns></returns>
        private async Task RewriteJWT ()
        {
            try
            {
                HttpClient httpClient = new HttpClient();
                httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/newJWT");
#if DEBUG
                Console.WriteLine("Start jwt Synchronistaion");
#endif
                
                    using var requestMessage = new HttpRequestMessage(HttpMethod.Get, httpClient.BaseAddress);
#if DEBUG
                    Console.WriteLine("InWhile");
#endif
                    //await Task.Delay(new TimeSpan(0, 0, 20));
#if DEBUG
                    Console.WriteLine("Start jwt synfronisationcpocedure");
#endif
                    var jwt = await _reader("Autorization");
                    requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
#if DEBUG
                    Console.WriteLine("Send jwt sync requestion.....");
#endif 
                    HttpResponseMessage tempresponce;
                    try
                    {
                        tempresponce = await httpClient.SendAsync(requestMessage);
                    }
                    catch
                    {
                        //await Task.Delay(100);
                        tempresponce = await httpClient.SendAsync(requestMessage);
                    }
#if DEBUG
                    Console.WriteLine("SendOldJWT");
#endif
                    if (tempresponce.IsSuccessStatusCode)
                    {
                        var result = await tempresponce.Content.ReadFromJsonAsync<Date>();
                        string newjwt = result.jwt;
                        await _writer("Autorization", newjwt);
                        if (roles == null)
                            roles = result.role;
                        
#if DEBUG
                        Console.WriteLine("GetNewJWT: " + newjwt);
#endif
                    }
                    else
                    {
                        await _writer("Autorization", "");
                        IsCookies = false;
                        ErrorInAutorization();
                        throw (new Exception("Invalid Status Code: "+tempresponce.StatusCode));
                    }

                
            }
            catch (Exception ex)
            {
                ErrorInAutorization();
                Console.WriteLine($"JSON sync error -> {ex.Message}");
            }
            
        }

        

        private void TimeSkipAndRewrite(System.Timers.Timer timer)
        {
            timer = new System.Timers.Timer(20*60*1000);
            timer.AutoReset = true;
            timer.Elapsed += new ElapsedEventHandler(RewriteJWT);
            timer.Start();
        }

        /// <summary>
        /// Событие - ошибка авторизации
        /// </summary>
        public Action ErrorInAutorization;

        /// <summary>
        /// Отправка данных и запись оных в внутреннее хранилище
        /// </summary>
        /// <returns></returns>
        public async Task SendDate(System.Timers.Timer timer, IAutorizationStruct autorizer)
            ///Отправка
        {

            var sentDate = new { 
                                login = (Name==""?"Defaultlogin":Name), 
                                Password = (Password == "" ? "DefaultPassword" : Password) 
                                };


            ///Формирование строки запроса
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/autorization");
            JsonContent content = JsonContent.Create(sentDate);


            try
            {
                ////Отправка запроса
                using HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);
#if DEBUG
                Console.WriteLine($"Запрос на авторизацию {content.Value} + -> "+sentDate.login + "->" + Name);
#endif
                ///Проверка ответа
                try
                {
                    
                    if (response.IsSuccessStatusCode)
                    {
                        Date newPerson = await response.Content.ReadFromJsonAsync<Date>();

                        autorizer.Role = newPerson.role.Select(AutorizationStruct.RoleSelector)
                            .ToArray();
                        

                        if (newPerson != null) { 


                            await _writer("token", newPerson.token);
                            await _writer("Autorization", newPerson.jwt);
                            TimeSkipAndRewrite(timer);
#if DEBUG
                            Console.WriteLine("Токен записан");
#endif
                        }
                    }
                    else
                    {
                        try
                        {
                            //Тут ошибка выполнения
                            var error = await response.Content.ReadFromJsonAsync<ErrorMessage>();

                            if (error != null) RequestMessage = error.messege;
                        }
                        catch (Exception ex)
                        {
#if DEBUG
                            Console.WriteLine("Aurotization error :" + ex.Message);
#endif
                            RequestMessage = "Response have not body!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Response Autorization Error -> {ex.Message}");
                }
#if DEBUG
                finally
                {
                    
                    string responseText = await response
                        .Content.ReadAsStringAsync();

                    if (responseText != null && responseText != "")
                    Console.WriteLine("Финальный блок авторизации: " + responseText);

                }
#endif
            }
            catch (Exception ex)
            {
                ErrorInAutorization();
                Console.WriteLine($"Cookie Interfase SendDate -> " + ex.Message);
            }
        }

        /// <summary>
        /// Заглушка записи
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task Send(string value, System.Timers.Timer timer, IAutorizationStruct autorizaer)
        {
            try
            {
                await SendDate(timer, autorizaer);
                string temp = await _reader("Autorization") ?? "";
                if (temp != "")
                {
                    IsCookies = true;

                    
#if DEBUG
                    Console.WriteLine("Set CookieTrue");
#endif
                }
                /*else
                {
                    _writer(value);
                }*/
            }
            catch (Exception ex)
            {
                ErrorInAutorization();
                Console.WriteLine("Cookie Interfase Send -> "+ex.Message);
            }
        }
    }

}

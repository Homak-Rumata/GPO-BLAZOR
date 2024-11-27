using Microsoft.AspNetCore.Authorization;
using System;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using Microsoft.EntityFrameworkCore.Diagnostics;
using static System.Runtime.InteropServices.JavaScript.JSType;
using GPO_BLAZOR.Client.Class.JSRunTimeAccess;

namespace GPO_BLAZOR.Client.Class.Date
{
    /// <summary>
    /// Класс - точка входа для авторизации
    /// </summary>
    public class AuthorizationDate:IAuthorizationDate
    {
        /// <summary>
        /// Конструктор точки доступа
        /// </summary>
        /// <param name="reader"> Делегат чтения в хранилище </param>
        /// <param name="writer"> Делегат записи в хранилище </param>
        public AuthorizationDate(Reader reader, Writer writer) 
        {
            _reader = reader;
            _writer = writer;
            
        }

        /// <summary>
        /// Пустой блок - на случай ошибки доступа
        /// </summary>
        public AuthorizationDate()
        {

        }

        /// <summary>
        /// Дополнительный асинхронный метод для инициализации
        /// </summary>
        /// <param name="reader"> Делегат для чтения из хранилища </param>
        /// <returns></returns>
        public async Task GetValues (Reader reader)
        {
            try
            {
                if (reader != null)
                {
                    string temp = await reader();
                    if (temp != null && temp != "")
                    {
                        IsCookies = true;
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message); 
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
        public string Name { get; set; }
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
            public string role { get; set; }
//            public string jwt { get; set; }
        }

        /// <summary>
        /// Контракт описания ошибки получения доступа
        /// </summary>
        private class ErrorMessage
        {
            public string messege { get; set; }
        }

        
        /// <summary>
        /// Отправка данных и запись оных в внутреннее хранилище
        /// </summary>
        /// <returns></returns>
        public async Task SendDate()
            ///Отправка
        {

            var sentDate = new { login = Name, Password = Password };


            ///Формирование строки запроса
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/autorization");
            JsonContent content = JsonContent.Create(sentDate);


            try
            {
                ///Отправка запроса
                using HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);

                ///Проверка ответа
                try
                {
                    
                    if (response.IsSuccessStatusCode)
                    {
                        Date newPerson = await response.Content.ReadFromJsonAsync<Date>();
                        if (newPerson != null) { 
                            switch (newPerson.role)
                            {
                                case "censor":
                                    InterfaceColor.TusurColor = "#3c388d";
                                    break;
                                default:
                                case "student":
                                    InterfaceColor.TusurColor = "#3c388d";
                                    break;

                            }

                            await _writer(newPerson.token);
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
                            Console.WriteLine("Aurotization error :" + ex.Message);
                            RequestMessage = "Response have not body!";
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Response Autorization Error -> {ex.Message}");
                }
                finally
                {
                    
                    string responseText = await response
                        .Content.ReadAsStringAsync();

                    if (responseText != null && responseText != "")
                    Console.WriteLine("Autorization error: " + responseText);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cookie Interfase SendDate -> " + ex.Message);
            }
        }

        /// <summary>
        /// Заглушка записи
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task Send(string value)
        {
            try
            {
                await SendDate();
                string temp = await _reader();
                if (temp != null && temp != "")
                {
                    IsCookies = true;
                }
                /*else
                {
                    _writer(value);
                }*/
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cookie Interfase Send "+ex.Message);
            }
        }
    }

}

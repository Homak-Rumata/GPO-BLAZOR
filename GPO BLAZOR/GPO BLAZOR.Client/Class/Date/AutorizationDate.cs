using Microsoft.AspNetCore.Authorization;
using System;
using System.Net.Http.Json;
using System.Reflection.PortableExecutable;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GPO_BLAZOR.Client.Class.Date
{
    public interface IAuthorizationDate { 
        
        bool IsCookies { get; }
        string RequestMessage { get;  }
        string Name { get; set; }
        string Password { get; set; }

        public Reader _reader { get; set; }
        public Writer _writer { get; set; }

        public Task GetValues(Reader reader);

        //public Task<string> SendLogin();
        Task Send(string value);

        


    }

    public delegate Task Writer (string value);
    public delegate Task<string> Reader();

    public class AuthorizationDate:IAuthorizationDate
{
        public AuthorizationDate(Reader reader, Writer writer) 
        {
            _reader = reader;
            _writer = writer;
            Task task = GetValues(reader);
            task.RunSynchronously();
        }
        public AuthorizationDate()
        {

        }

        public async Task GetValues (Reader reader)
        {
            
            if (reader != null)
            {
                string temp = await reader();
                Console.WriteLine(temp+" tempchange GetValues");
                if (temp != null && temp != "")
                {
                    IsCookies = true;
                    Console.WriteLine("Is cookies set GetValues");
                }
            }
        }
        public bool IsCookies { get; private set; }
        public string RequestMessage { get; private set;  } = null;
        public string Name { get; set; }
        public string Password { get; set; }

        

        public Reader _reader { get; set; }
        public Writer _writer { get; set; }

        private class Date
        {
            public string token { get; set; }
//            public string jwt { get; set; }
        }

        private class ErrorMessage
        {
            public string messege { get; set; }
        }

        

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
                        if (newPerson != null) await _writer(newPerson.token);
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
                            RequestMessage = "Response have not body!";
                        }
                    }
                }
                finally
                {
                    
                    string responseText = await response
                        .Content.ReadAsStringAsync();

                    if (responseText == null || responseText == "")
                    Console.WriteLine("Autorizationerror: " + responseText);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cookie Interfase SendDate " + ex.Message);
            }
        }
        public async Task Send( string value)
        {
            try
            {               
            Console.WriteLine(IsCookies.ToString());
            
                await SendDate();
                string temp = await _reader();
                if (temp != null && temp != "")
                {
                    IsCookies = true;
                }
                else
                {
                    _writer(value);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Cookie Interfase Send "+ex.Message);
            }
        }
    }

}

using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Net.Http.Json;

namespace GPO_BLAZOR.Client.Class.Date
{

    /// <summary>
    /// Модель значений выпадющего списка
    /// </summary>
    public record CollectionValues
    {
        

        private CollectionValues(string[] value)
        {


            if (value != null)
                Values = value;
            else Values = null;
        }

        public string[] Values { get; init; }

        public static async Task<CollectionValues> Create(string Name, IJSRuntime jsr)
        {
            return new CollectionValues(await GetAtributes(Name, jsr));
        }

        private static async Task<string[]> GetAtributes(string Field, IJSRuntime jsr)
        {
            return await Requesting.AutorizationedGetRequest<string[]>(new Uri($"https://{IPaddress.IPAddress}/GetAtributes/{Field}"), jsr);
        }

    }
}
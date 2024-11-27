using System.Collections.Generic;
using System.Net.Http.Json;

namespace GPO_BLAZOR.Client.Class.Date
{
    public class Statmen: DictionaryValueGetter, IStatmen
    {
        public Page[] Date { get; set; }

        private string _id = "";

        private string Id 
        {
            get
            {
                return _id;
            }
            set
            {
                _id = (_id==null)||(_id=="")?value:_id;
            } 
        }


        public override IEnumerable<KeyValuePair<string, IField>> GetValues()
        {
            var result = base.GetValues(Date);

            IEnumerable < KeyValuePair<string, IField> > AddId (IEnumerable<KeyValuePair<string, IField>> values)
            {
                yield return new KeyValuePair<string, 
                    IField>("id", (new Field { Id = "Id", ClassType = "HiddenField", Name = "Id", Text = "", value = Id }));
                foreach (var item in  values) 
                {
                    yield return item;
                }
                yield break;
            }

            return AddId(result);
        }

        public async static Task<IStatmen> Create(string id)
        {
            Dictionary<string, string> values;
            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/getformDate:{id}");
                    values = await httpClient.GetFromJsonAsync<Dictionary<string, string>>(httpClient.BaseAddress);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"getFormDateException -> {ex.Message}");
                values = null;
            }

            try
            {
                using (HttpClient httpClient = new HttpClient())
                {
                    httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/getTepmlate/{values["Template"]}");

                    var addId = (Statmen statmen, string id) =>
                    {
                        statmen.Id = id;
                        return statmen;
                    };

                    IStatmen StatmenTemplate = addId(await httpClient.GetFromJsonAsync<Statmen>(httpClient.BaseAddress), id);

                    var t = FillTemplate(values, StatmenTemplate);

                    return t;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"getTemplateException -> {ex.Message}");
                return null;
            }


        }

        private static IStatmen FillTemplate(Dictionary<string, string> values, IStatmen voidTemplate)
        {
            var teplateDictionarytemp = voidTemplate.GetValues();
            var teplateDictionary = teplateDictionarytemp.ToDictionary();
            foreach (var item in values)
                if (teplateDictionary.ContainsKey(item.Key))
                {
                    //Console.WriteLine($"|||| {item.Key} : {item.Value} : {teplateDictionary[item.Key].value}");
                    teplateDictionary[item.Key].value = item.Value;
                    //Console.WriteLine($"---- {item.Key} : {item.Value} : {teplateDictionary[item.Key].value}");
                    
                };

            return voidTemplate;
        }

        public async Task<string> SendDate()
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/getInfo");
            Dictionary<string, string> temp = new Dictionary<string, string>();
            foreach (var item in GetValues())
            {
                temp.TryAdd(item.Key, item.Value.value);
            }

            var response = await httpClient.PostAsJsonAsync(httpClient.BaseAddress, temp);

            //var a = (httpClient.Send(new HttpRequestMessage())).Content.ReadAsStream();


            return await response.Content.ReadAsStringAsync();
        }
    }
}

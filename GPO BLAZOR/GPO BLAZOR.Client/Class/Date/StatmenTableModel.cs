using System.Net.Http.Json;

namespace GPO_BLAZOR.Client.Class.Date
{
    public class StatmenTableModel: IStatmenTableModel
    {

        private IStatmenTableLineModel[] _lines;

        public IStatmenTableLineModel[] Lines
        {
            get
            {
                ref IStatmenTableLineModel[] result = ref _lines;
                return result;
            }
            set
            {
                _lines = value;
            }
        }

        private StatmenTableModel(IStatmenTableLineModel[] components)
        {
            
            Lines = components;
        }

        static async public Task<StatmenTableModel> Create(string? token = null)
        {
            return new StatmenTableModel(await GetLines(token));
        }

        private StatmenTableModel(string token)
        {
            var response = GetLines(token);

            while (!response.IsCompleted)
            {
                
            }
            Lines = response.Result;

        }

        private static async Task<IStatmenTableLineModel[]> GetLines(string? token)
        {
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/getstatmens/user:{(token!=null?"46":"23") }");
            var response = await httpClient.GetFromJsonAsync<StatmenTableLineModel[]>(httpClient.BaseAddress);

            int calculator = 0;
            foreach (var item in response)
            {
                item.Number = calculator++;
            }

            return response;
        }

        public void Add(IStatmenTableLineModel addingItem)
        {
            Array.Resize(ref _lines, Lines.Length+1);
            _lines[_lines.Length-1] = addingItem;
        }
    }
}

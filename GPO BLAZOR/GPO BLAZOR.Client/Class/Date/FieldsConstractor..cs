using System.Linq;
using System.Net.Http.Json;

namespace GPO_BLAZOR.Client.Class.Date
{
    public static partial class FieldsConstractor
    {

        public static async Task<IStatmenValue> GetBlock()
        {
            //Получение структуры
            return new StatmenValue(GetVoidPage(DefaultInfo
                .Select(page => (page.Item1, GetVoisBlockValues(page.Item2
                .Select(block => (block.Item1, GetVoidField<string>(block.Item2)))
                .ToArray())))
                .ToArray()),
                GetDictionary());

        }

        public static async Task<IStatmenValue> GetBlock(string ID)
        {
            return await GetBlockToServer(ID, await GetBlock());
        }

        public static async Task<IStatmenValue> GetBlockToServer(string ID, IStatmenValue values)
        {
            //Получить структуру
            HttpClient httpClient = new HttpClient();
           //потом заменить
            using HttpResponseMessage response = await httpClient.GetAsync($"https://{IPaddress.IPAddress}/getformDate:{ID}");
            string responseText = await response.Content.ReadAsStringAsync();
            IEnumerable<string> message = responseText.Split(',');
            foreach (string line in message)
            {
                ITextField<string> temp = (ITextField<string>)values.ContainsField[line.Substring(0, line.IndexOf(':'))];
                temp.value = line.Substring(line.IndexOf(' ')+1);
            }
            return values;
        }

        public static async Task<string> SetBlockDate(IStatmenValue Date, int Num)
        {
            //Запись нового состояния данных
            using HttpClient httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri($"https://{IPaddress.IPAddress}/getInfo");
            StringContent content = new StringContent("{"+$"\"id\":\"{Num}\",\n"+Date.GetValue()+"}");

            /*--------*/

            using HttpResponseMessage response = await httpClient.PostAsync(httpClient.BaseAddress, content);
            string responseText = await response.Content.ReadAsStringAsync();
            return responseText;
            ///getInfo
            ///

        }

        private static IPageValues[] GetVoidPage((string, IBlockValues[])[] Values)
        {
            return Values.Select(item => new PageValues(item.Item1, item.Item2)).ToArray();
        }



        private static IBlockValues[] GetVoisBlockValues((string, IField[])[] Values)
        {
            return Values.Select(item => new BlockValue(item.Item1, item.Item2)).ToArray();
        }

        private static IField[] GetVoidField<T>((string id, string classname, string name, string text)[] Values)
        {

            return Values.Select(item => {
                keyValuePairs.Add(key: item.name, value: FieldSelector<T>(item));
                return keyValuePairs[item.Item3]; })
                .ToArray();
        }

        private static IField FieldSelector<T>((string id, string classname, string name, string text) args)
        {
            switch (args.classname)
            {
                case "InputInformationField":
                    return new TextValue<T> { idvalue = args.id, classvvalue = args.classname, namevalue = args.name, textvalue = args.text };
                case "CollectionInformationField":
                    return new CollectionValue<T> { idvalue = args.id, classvvalue = args.classname, namevalue = args.name, textvalue = args.text };
                default: return new TextValue<T> { idvalue = args.id, classvvalue = args.classname, namevalue = args.name, textvalue = args.text };

            }
        }

        private static Dictionary<string, IField> keyValuePairs = new Dictionary<string, IField>();

        private static Dictionary<string, IField> GetDictionary ()
        {
            Dictionary<string, IField> temp = keyValuePairs;
            keyValuePairs = new Dictionary<string, IField>();
            return temp;
        }

        private static (string PageName, (string BlockName, (string Name, string Type, string Id, string Text)[])[])[] DefaultInfo =
            //Структура
            
            /*{("Пользовательские данные",
                new (string BlockName, (string Name, string Type, string  Id, string Text)[])[]{
                    ("Основные данные",
                        new (string Name, string Type, string Id, string Text)[] {
                            ("FirstNameTextField", "InputInformationField", "FirstNameTextField", "Имя"),
                            ("SecondNameTextField", "InputInformationField", "SecondNameTextField", "Фамилия"),
                            ("TreeNameTextField", "InputInformationField", "TreeNameTextField", "Отчество (при наличии)")
                        }),
                    ("",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("Group", "CollectionInformationField", "Groupp", "Группа"),
                            ("Direction", "CollectionInformationField", "Direction", "Направление подготовки")
                        }),
                    ("Практики",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("PracticeSort", "CollectionInformationField", "PracticeSort", "Вид практики"),
                            ("PracticeType", "CollectionInformationField", "PracticeType", "Тип практики"),
                            ("date", "TymeInformationField", "date", "Тип практики")
                        }),
                    ("Предприятие",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("Adress", "InputInformationField", "PracticeSort", "Адресс"),
                            ("FactoryName", "CollectionInformationField", "FactoryName", "Тип практики"),
                        })
                    }
            ),
            };*/


            {("Пользовательские данные",
                new (string BlockName, (string Name, string Type, string  Id, string Text)[])[]{
                    ("Основные данные",
                        new (string Name, string Type, string Id, string Text)[] {
                            ("FirstNameTextField", "InputInformationField", "FirstNameTextField", "Имя"),
                            ("SecondNameTextField", "InputInformationField", "SecondNameTextField", "Фамилия"),
                            ("TreeNameTextField", "InputInformationField", "TreeNameTextField", "Отчество (при наличии)")
                        }),
                    ("",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("Grp", "CollectionInformationField", "Grp", "Группа"),
                            ("Direction", "CollectionInformationField", "Direction", "Направление подготовки")
                        }),
                    ("Практики",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("PracticeSort", "CollectionInformationField", "PracticeSort", "Вид практики"),
                            ("PracticeType", "CollectionInformationField", "PracticeType", "Тип практики"),
                            ("date", "TymeInformationField", "date", "Тип практики")
                        }),
                    }
            ),
            ("Руководители",
                new (string BlockName, (string Name, string Type, string  Id, string Text)[])[]{
                    ("Руководитель практики",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("WorkLeaderFirstNameTextField", "InputInformationField", "WorkLeaderFirstNameTextField", "Имя"),
                            ("WorkLeaderSecondNameTextField", "InputInformationField", "WorkLeaderSecondNameTextField", "Фамилия"),
                            ("WorkLeaderTreeNameTextField", "InputInformationField", "WorkLeaderTreeNameTextField", "Отчество (при наличии)")
                        }),
                    ("Заведующий кафеды",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("CafedralLeaderFirstNameTextField", "CollectionInformationField", "CafedralLeaderFirstNameTextField", "Имя"),
                            ("CafedralLeaderSecondNameTextField", "CollectionInformationField", "CafedralLeaderSecondNameTextField", "Фамилия"),
                            ("CafedralLeaderTreeNameTextField", "TymeInformationField", "CafedralLeaderTreeNameTextField", "Отчество")
                        }),
                    }
            ),
             ("Предприятия",
                new (string BlockName, (string Name, string Type, string  Id, string Text)[])[]{
                    ("Реквизиты Предприятия",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("FactoryNameTextField", "InputInformationField", "FactoryNameTextField", "Наименование")
                        }),
                    ("Адресс",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("RegionNameTextField", "CollectionInformationField", "RegionNameTextField", "Область"),
                            ("DistrictNameTextField", "CollectionInformationField", "DistrictNameTextField", "Район"),
                            ("LocalityNameTextField", "TymeInformationField", "LocalityNameTextField", "Населённый пункт")
                        }),
                    ("",
                        new (string Name, string Type, string  Id, string Text)[] {
                            ("StreetTextField", "CollectionInformationField", "StreetTextField", "Улица"),
                            ("buildingNumberTextField", "CollectionInformationField", "buildingNumberTextField", "Строение"),
                            ("MailPostNumberTextField", "NumberInformationField", "MailPostNumberTextField", "Номер ящика")
                        })
                    }
            )
        };
            
        
    }
}


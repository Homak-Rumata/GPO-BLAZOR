using GPO_BLAZOR.Client.Class.Date;
using GPO_BLAZOR.Components;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Components.Authorization;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using GPO_BLAZOR.Client.Class.JSRunTimeAccess;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Xml;
using System.Xml.Serialization;
using System;
using GPO_BLAZOR.DBAgents;
using Microsoft.AspNetCore.Identity;
using GPO_BLAZOR.DBAgents;
using GPO_BLAZOR.DBAgents.DBModels;

namespace GPO_BLAZOR
{
    class Date
    {
        public Guid token { get; set; }
        //public string jwt { get; set; }
        public string role { get; set; }
    }

    record FieldDateContainer
    {

        public FieldDateContainer (string name, string type, string id, string text, bool disabled)
        {
            Name = name;
            ClassType = type;
            Id = id;
            Text = text;
            IsDisabled = disabled;
        }

        public string Name { get; set; }
        public string ClassType { get; set; }
        public string Id { get; set; }
        public string Text { get; set; }

        public bool IsDisabled { get; init; }

    }

    class BlockDateContainer
    {
        public BlockDateContainer(string blockname, IEnumerable<FieldDateContainer> date)
        {
            BlockName = blockname;
            Date = date.ToArray();
        }

        public string BlockName { get; set; }
        public FieldDateContainer[] Date { get; set; }
    }

    class PageDateContainer
    {
        public PageDateContainer(string pagename, IEnumerable<BlockDateContainer> date)
        {
            PageName = pagename;
            Date = date.ToArray();
        }

        public string PageName { get; set; }
        public BlockDateContainer[] Date { get; set; }
    }

    class StatmenDateContainer
    {
        public StatmenDateContainer(IEnumerable<PageDateContainer> date)
        {
            //StatmenName = statmenname;
            Date = date.ToArray();
        }

        //public string StatmenName { get; set; }
        public PageDateContainer[] Date { get; set; }
    }

    static class StatmenDate
    {
        static public StatmenDateContainer DefaultInfo = new StatmenDateContainer (
            new PageDateContainer[]{new PageDateContainer("Пользовательские данные",
                new BlockDateContainer[]{
                    new BlockDateContainer("Основные данные",
                         new FieldDateContainer[]{
                            new FieldDateContainer("FirstNameTextField", "InputInformationField", "FirstNameTextField", "Имя", false),
                            new FieldDateContainer("SecondNameTextField", "InputInformationField", "SecondNameTextField", "Фамилия", false),
                            new FieldDateContainer("TreeNameTextField", "InputInformationField", "TreeNameTextField", "Отчество (при наличии)", false)
                        }),
                    new BlockDateContainer("",
                        new FieldDateContainer[] {
                            new FieldDateContainer("Grp", "CollectionInformationField", "Grp", "Группа", false),
                            new FieldDateContainer("Direction", "CollectionInformationField", "Direction", "Направление подготовки", false)
                        }),
                    new BlockDateContainer("Практики",
                        new FieldDateContainer[] {
                            new FieldDateContainer("PracticeSort", "CollectionInformationField", "PracticeSort", "Вид практики", false),
                            new FieldDateContainer("PracticeType", "CollectionInformationField", "PracticeType", "Тип практики", false),
                            new FieldDateContainer("date", "TymeInformationField", "date", "Дата подачи заявления", true)
                        }),
                    }
            ),
                new PageDateContainer("Руководители",
                new BlockDateContainer[]{
                    new BlockDateContainer("Руководитель практики",
                        new FieldDateContainer[]{
                            new FieldDateContainer("WorkLeaderFirstNameTextField", "InputInformationField", "WorkLeaderFirstNameTextField", "Имя", false),
                            new FieldDateContainer("WorkLeaderSecondNameTextField", "InputInformationField", "WorkLeaderSecondNameTextField", "Фамилия", false),
                            new FieldDateContainer("WorkLeaderTreeNameTextField", "InputInformationField", "WorkLeaderTreeNameTextField", "Отчество (при наличии)", false)
                        }),
                    new BlockDateContainer("Заведующий кафеды",
                        new FieldDateContainer[] {
                            new FieldDateContainer("CafedralLeaderFirstNameTextField", "CollectionInformationField", "CafedralLeaderFirstNameTextField", "Имя", false),
                            new FieldDateContainer("CafedralLeaderSecondNameTextField", "CollectionInformationField", "CafedralLeaderSecondNameTextField", "Фамилия", false),
                            new FieldDateContainer("CafedralLeaderTreeNameTextField", "CollectionInformationField", "CafedralLeaderTreeNameTextField", "Отчество", false)
                        }),
                    }
            ),
                new PageDateContainer("Предприятия",
                 new BlockDateContainer[]{
                    new BlockDateContainer("Реквизиты Предприятия",
                        new FieldDateContainer[] {
                            new FieldDateContainer("FactoryNameTextField", "InputInformationField", "FactoryNameTextField", "Наименование", false)
                        }),
                    new BlockDateContainer("Адресс",
                        new FieldDateContainer[] {
                            new FieldDateContainer("RegionNameTextField", "CollectionInformationField", "RegionNameTextField", "Область", false),
                            new FieldDateContainer("DistrictNameTextField", "CollectionInformationField", "DistrictNameTextField", "Район", false),
                            new FieldDateContainer("LocalityNameTextField", "TymeInformationField", "LocalityNameTextField", "Населённый пункт", false)
                        }),
                    new BlockDateContainer("",
                        new FieldDateContainer[] {
                            new FieldDateContainer("StreetTextField", "CollectionInformationField", "StreetTextField", "Улица", false),
                            new FieldDateContainer("buildingNumberTextField", "CollectionInformationField", "buildingNumberTextField", "Строение", false),
                            new FieldDateContainer("MailPostNumberTextField", "NumberInformationField", "MailPostNumberTextField", "Номер ящика", false)
                        })
                    }
            )
        });
    }

    class tempc
    {
        public tempc(string id, string Time, int val1, int val2)
        {
            this.id = id;
            this.Time = DateTime.Now.AddHours(2);
            this.State = val1;
            this.PracticType = val2;
        }

        public string id { get; set; }
        public DateTime Time { get; set; }
        public int State { get; set; }
        public int PracticType { get; set; }
    }

    
    record AutorizationDate
    {
        public string login { get; init; }
        public string Password { get; init; }
    }

    class ErrorMessage
    {
        public string messege { get; set; }
    }

    public class Program
    {
        static Dictionary<string, List<string>> SpecialArray = new Dictionary<string, List<string>>()
        {
            {"Grp", new List<string>() {"711-1", "721-1", "731-1", "731-2", "741-1", "761-1" } } ,
            {"Direction", new List<string>(){"Информационная безопасность", "7Безопасность автоматизированных систем", "Безопасность телекомуникационных систем", "Аналитическая безопасность", "Экономическая безопасность" } },
            {"PracticeSort", new List<string>(){"Производственная", "Преддипломаная" } },
            {"PracticeType", new List<string>(){"Эксплуатационная"} }
        };

        public static void Main(string[] args)
        {
            Gpo2Context cntx = new Gpo2Context();
            
            
           // DBConnector.F(null);

            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddScoped<CookieStorageAccessor>();
            builder.Services.AddScoped<LocalStorageAccessor>();

            // Add services to the container.
            builder.Services.AddRazorComponents()
                .AddInteractiveWebAssemblyComponents();

            builder.Services.AddControllers()
                .AddXmlSerializerFormatters();

            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            builder.Services.AddScoped<AuthenticationStateProvider, IdentetyAuthenticationStateProvider>();




            //builder.Services.AddAuthorizationCore();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseWebAssemblyDebugging();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }



            List<tempc> b = new List<tempc>()
            {
                new tempc(1.ToString(), 1.ToString(), 1, 1),
                new tempc(2.ToString(), 2.ToString(), 1, 1),
                new tempc(3.ToString(), 3.ToString(), 1, 1)
            };

            Dictionary<string, Dictionary<string, string>> temp = new Dictionary<string, Dictionary<string, string>>()
            {
                {"1", new Dictionary<string, string>() { {"id", "1" }, {"Template", "based" } } },
                {"2", new Dictionary<string, string>() { {"id", "2" }, { "Template", "based" } } },
                {"3", new Dictionary<string, string>() { {"id", "3" }, { "Template", "based" } } }
            };

            XmlDocument fgh = new XmlDocument();
            fgh.LoadXml("<reply success=\"true\">More nodes go here</reply>");



            bool AddOnDictionary<Key, Value>(Dictionary<Key, Value> dictionary, KeyValuePair<Key, Value> value)
            {
                if (dictionary.TryAdd(value.Key, value.Value))
                    return true;
                else
                    dictionary[value.Key] = value.Value;
                return true;
            }

            int calculator = 0;



            app.UseSession();
            app.UseHttpsRedirection();
            app.UseCookiePolicy();
            app.UseStaticFiles();
            app.UseAntiforgery();

            app.MapGet("/GetAtributes/{Field}", (string Field) => SpecialArray[Field]);
            app.MapGet("/GetAtributes", () => new string[] { "A", "Б", "В" });
            app.MapPost("/autorization", (AutorizationDate date) =>
            {
                try
                {                  
                    return !(date.login == "censor" && date.Password=="12345678")?
                    Results.Problem("not login or password", "nonautorization", 401, "bad login or password)", "nontype", new Dictionary<string, object> { { "messege", "bad login or password"} }) :
                    Results.Json(new Date() { token = (Guid.NewGuid()), role = "student" });
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                    return Results.Json("token: student,\t\n role = student ");
                }
            });
            app.MapGet("/getstatmens/user:{Token}",()=>b);
            app.MapGet("/getformDate:{ID}", (string ID) => { app.Logger.LogInformation($"{ID}: {temp[ID]}"); return temp[ID]; });
            app.MapPost("/getInfo", (Dictionary<string, string> x)=>
            {
                Console.WriteLine("------------------------------------------------");
                string accamulator = "";

                if (temp.TryAdd(x["id"], new Dictionary<string, string>()))
                {
                    app.Logger.LogError($"AddedStatmen: {x["id"]}");
                }

                string id = x["id"];

                x.Remove("id");

                foreach (var item in x)
                {
                     accamulator+=$"{item.Key}: {(item.Value==null||item.Value==("")?("none"):item.Value)}: {AddOnDictionary(temp[id], item)}\n";
                    //Console.WriteLine($"{item.Key} ^ {item.Value} - WriteLine");
                }
                    app.Logger.LogInformation((new EventId(calculator++, "getInfo")), accamulator);
                    return Results.Ok("sucsefull");
            });
            app.MapGet("/getTepmlate/{TeplateName}", (string TeplateName) => (StatmenDate.DefaultInfo));


            app.MapRazorComponents<App>()
                .AddInteractiveWebAssemblyRenderMode()
                .AddAdditionalAssemblies(typeof(Client._Imports).Assembly);

            app.Run();
        }
    }

    public class IdentetyAuthenticationStateProvider: AuthenticationStateProvider
    {
        public async override Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            return new AuthenticationState( new System.Security.Claims.ClaimsPrincipal() );
        }
    }
}

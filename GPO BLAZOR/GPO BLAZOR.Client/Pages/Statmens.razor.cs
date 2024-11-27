using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;
using Navigation = Microsoft.AspNetCore.Components.NavigationManager;


namespace GPO_BLAZOR.Client.Pages
{
    public partial class Statmens
    {
        //private Statmens c { get; set; } = new Statmens();

        //
        public List<IStatmentDate> Date { get; set; }

        protected override async Task OnInitializedAsync()
        {


            isLoadind = false;

            try
            {

                IEnumerable<(string, string, int, int)> temp = await StatmensTable
            .GetStatmens("");
                var temp2 = temp.Select(x => GetStatmentDate(x)).ToList();
                Date = temp2;

            }
            catch (Exception ex)
            {

            }
        }

        protected override async Task OnParametersSetAsync()
        {
            isLoadind = true;
        }

        private bool isLoadind { get; set; } = false;

        private IStatmentDate GetStatmentDate((string, string, int, int) item)
        {
            return new StatmentDate(item.Item1, item.Item2, item.Item3, item.Item4);
        }


        [Parameter]
        public EventCallback<(string, int)> ViemStatmen { get; set; }


        async Task Click((string id, int num) item)
        {
            var command = (async (string id, int num) => {
                string way = $"statmen/{num}";
                Navigation.NavigateTo(way);
            });

            command(item.id, item.num);
            // ВызовКолбека
            // Но должен быть новый пост
            await ViemStatmen.InvokeAsync(item);


        }


        async Task NewPost()
        {

        }
    }
}

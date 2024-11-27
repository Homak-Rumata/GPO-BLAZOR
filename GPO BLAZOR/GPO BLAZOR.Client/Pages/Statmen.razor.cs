using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;


namespace GPO_BLAZOR.Client.Pages
{
    public partial class Statmen
    {
        private bool isLoading;

        [Parameter]
        public IStatmenValue Date { get; set; }

        [Parameter]
        public int Number { get; set; }

        private IStatmentDate CurrentObject { get; set; }

        [Parameter]
        public EventCallback Return { get; set; } 

        private IPageValues _tempPage;

        private async Task SetDate()
        {
            string setdate = Date.GetValue();
            Console.WriteLine(setdate);

            Console.WriteLine("Send 1");
            try
            {
                FieldsConstractor.SetBlockDate(Date, Int32.Parse(CurrentObject.ID));
                Console.WriteLine("send 2");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            Navigation.NavigateTo("/");
            if (Return.HasDelegate)
            await Return.InvokeAsync();
            else
            {

            }
            
        }

        protected override async Task OnInitializedAsync()
        {
            isLoading = false;
            try
            {
                CurrentObject = StatmentDate.Codificator[Number < StatmentDate.Codificator.Count ? Number : StatmentDate.Codificator.Count % Number];
            }
            catch (Exception ex)
            {

            }



            if (Date == null)
            {
                try
                {
                    Date = await FieldsConstractor.GetBlock(CurrentObject.ID);
                }
                catch (Exception ex)
                {
                    Date = await FieldsConstractor.GetBlock();
                }
            }
            
        }

        protected override void OnParametersSet()
        {
            _tempPage = Date.TextValues.First();

            isLoading = true;
        }
    }
}

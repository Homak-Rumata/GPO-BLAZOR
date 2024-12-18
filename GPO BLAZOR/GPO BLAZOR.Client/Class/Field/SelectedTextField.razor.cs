using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace GPO_BLAZOR.Client.Class.Field
{
    public partial class SelectedTextField: Field
    {

        private bool IsLoading;

        private CollectionValues? collection;

        [Inject]
        public IJSRuntime JSRuntime {get; set;}

        protected override async Task OnInitializedAsync()
        {
            IsLoading = false;
            try
            {
                collection = (Date is not null)?await CollectionValues.Create(Date.Id, JSRuntime) :collection;
            }
            finally
            {
                await base.OnInitializedAsync();
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                if (collection == null) 
                    collection = await CollectionValues.Create(Date.Id, JSRuntime);
                

            }
            catch (Exception ex)
            {
                Console.WriteLine($"SelectedTextFieldException -> {ex.Message}");
            }
            finally
            {
                await base.OnParametersSetAsync();
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            IsLoading = true;
            await base.OnAfterRenderAsync(firstRender);
        }
    }
}

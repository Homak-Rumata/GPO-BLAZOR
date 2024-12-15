using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;


namespace GPO_BLAZOR.Client.Parts
{
    public partial class RoleController
    {
        [Inject]
        Class.Date.IAutorizationStruct Autorize { get; set; }

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        [Parameter]
        public Class.Date.Roles Role { get; set; }
    }
}

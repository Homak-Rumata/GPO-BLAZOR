using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;

namespace GPO_BLAZOR.Client.Class.Field
{
    public partial class TextField: Date.Field, Date.ITextField
    {
        [Parameter]
        [EditorRequired]
        public ITextField<string> field {  get; set; }

        public delegate void SetDelegate (Func<string> func);

        public override string GetValue()
        {
            return $"\"{idvalue.ToString()}\": \"{value.ToString()}\"";
        }
        public void setValue(string info) 
        {
            value = info;
        }

        protected override void OnInitialized()
        {

            base.OnInitialized();
        }

        protected override void OnParametersSet()
        {
            base.OnParametersSet();
        }

        protected override bool ShouldRender()
        {
            return base.ShouldRender();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            base.OnAfterRender(firstRender);
        }
    }
}

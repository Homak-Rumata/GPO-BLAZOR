using Microsoft.JSInterop;

namespace GPO_BLAZOR.Client.Class.Date;

public interface IStatmen: IDictionaryFieldValue
{
    int State { get; set; }
    Page[] Date { get; set; }

    Task<string> SendDate(IJSRuntime jsr);
}
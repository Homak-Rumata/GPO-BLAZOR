using GPO_BLAZOR.Client.Class.Date;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using MigraDoc.Rendering;
using PdfSharp.Fonts;
using MigraDoc.RtfRendering;
using PdfSharp;
using System.Xml.Serialization;


namespace GPO_BLAZOR.Client.Class.Field
{
    public partial class Print: ComponentBase
    {
        [Inject]
        IJSRuntime JSRuntime { get; set; }

        [Parameter]
        //[EditorRequired]
        public string TemplateName { get; set; }

        //[EditorRequired]
        public Statmen Statmen { get; set; }       

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (Capabilities.Build.IsCoreBuild)
                GlobalFontSettings.FontResolver = new PdfFilePrinting.DocumentService.MyFontResolver();
            base.OnAfterRenderAsync(firstRender);
            //ReadXml("");
        }


        protected void ButtonCliced()
        {

        }
        private async void ReadXml()
        {

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(PdfFilePrinting.DocumentService.Document));
            var reques = await Date.Requesting.AutorizationedRequest(new Uri($"https://{IPaddress.IPAddress}/GetTemplate"), JSRuntime);
            var result = xmlSerializer.Deserialize(reques) as PdfFilePrinting.DocumentService.Document?;

            PdfDocumentRenderer pdfDocumentRenderer = new PdfDocumentRenderer();
            try
            {
                pdfDocumentRenderer.Document = result.Value.Render();
            //pdfDocumentRenderer.Document = pdfrend;
                pdfDocumentRenderer.RenderDocument();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error print -> " + ex.Message);
            }
            try
            {
                using (var memoryStream = new MemoryStream())
                {
                    pdfDocumentRenderer.PdfDocument.Save(memoryStream);
                    await JSRuntime.InvokeVoidAsync("saveAsFile", "fileName.pdf", Convert.ToBase64String(memoryStream.ToArray()));
                    RtfDocumentRenderer h = new RtfDocumentRenderer();
                    h.Render(result.Value.Render(), memoryStream, "/");
                    await JSRuntime.InvokeVoidAsync("saveAsFile", "fileName.rtf", Convert.ToBase64String(memoryStream.ToArray()));

                }
            }
            catch (Exception ex)
            {

                Console.WriteLine("Error print2 -> " + ex.Message);
            }
            //var url = $"data:application/pdf;base64,{base64}";
            //await JSRutime.InvokeVoidAsync("open", url, "_blank");
        }

    }

   
}

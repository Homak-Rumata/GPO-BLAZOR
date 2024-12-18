using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using RenderingDocument = MigraDoc.DocumentObjectModel.Document;

namespace PdfFilePrinting.DocumentService
{
    [DataContract]
    //[XmlRoot(Namespace = "Templates", ElementName = "Document", IsNullable = false, DataType = "string")]
    [XmlInclude(typeof(Paragrapf))]
    public struct Document
    {
        public Document()
        {

        }
        [XmlArray]
        public Section[] Sections { get; set; }
        public Margins margin { get; set; } = new(45, 60, 60, 60);

        public RenderingDocument Render()
        {
            var document = new RenderingDocument();
            foreach (var section in Sections)
            {
                section.Render(document);
            }

            return document;
        }

        public IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetNames()
        {
            var temp = Sections.SelectMany(x => x.GetNames());

            return temp;
        }
    }
}

using RenderingDocument = MigraDoc.DocumentObjectModel.Document;
using RenderingSection = MigraDoc.DocumentObjectModel.Section;
using RenderingTable = MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Internals;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Serialization;
using System.Reflection.Metadata;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using System.Text;
using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using MigraDoc.DocumentObjectModel.Tables;

namespace GPO_BLAZOR.PDFConstructor.DocumentService
{
    static class F
    {
        public static Document FA()
        {
            return new Document()
            {
                Sections = new Section[]
                {
                    new Section()
                    {
                        paragrapfs = new Paragrapf[]
                    {
                        new Paragrapf()
                        {
                            Bold = true,
                            text = new BaseElement[]
                        {
                            new RawText { TextValue = "Договор о практической подготовке обучающихся в форме практики №" },
                            new InjectElement { Name = "ContractNumber", TextValue = "1" },
                            new RawText { TextValue = "г. Томск" },
                        }
                        },
                        new Paragrapf()
                        {
                            text = new BaseElement[]
                            {
                                new RawText {TextValue = ("Федеральное государственное автономное образовательное учреждение высшего образования «Томский государственный университет систем управления и радиоэлектроники» (ТУСУР), именуемое в дальнейшем «Университет», в лице директора центра карьеры И.А. Трубчениновой, действующего на основании доверенности от 19.09.2024 №20/3460, с одной стороны, и" ).ToString()},
                                new InjectElement() {Name = "CompanyName", TextValue="ООО ДИВИЛАЙН" } 
                                }
                            }
                        }
                    }
                    
                }
            };
        }
    }

    public interface IElement
    {
        //void Render();
    }

    public interface IElement<T> : IElement
    {
        void Render (in T element);
    }

    public interface IDocument : IElement;
    public interface ISections : IElement<RenderingDocument>;


    public interface IParagraph : IElement<RenderingSection>;

    
    public interface IBaseElement : IElement<Paragraph>
    {
        string TextValue { get; set; }
    }
    public interface IInjectValue : IBaseElement
    {
    }
    public interface IText: IBaseElement
    {
    }

    public record struct Margins
    { 
        public Margins()
        {

        }
        public Margins(int right, int left, int top, int bottom)
        {
            Right = right;
            Left = left;
            Top = top;
            Bottom = bottom;
        }
        [XmlAttribute("right")]
        public int Right { get; init; }
        [XmlAttribute("left")]
        public int Left { get; init; }
        [XmlAttribute("top")]
        public int Top { get; init; }
        [XmlAttribute("bottom")]
        public int Bottom { get; init; }
    }



    [DataContract]
    //[XmlRoot(Namespace = "Templates", ElementName = "Document", IsNullable = false, DataType = "string")]
    [XmlInclude(typeof(Paragrapf))]
    public struct Document: IDocument
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
    }

    public record struct Section: ISections
    {
        public Section()
        {

        }
        [XmlArray]
        [XmlArrayItem("Paragrapf", typeof(Paragrapf))]
        [XmlArrayItem("Table", typeof(Table))]
        public BaseParagraph[] paragrapfs { get; set; }

        public void Render(in RenderingDocument document)
        {
            var section = document.AddSection();
            foreach (IParagraph temp in paragrapfs)
            {
                temp.Render(section);
            }
        }

    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(FormatedElement), "FormatedElement")]
    public abstract record class FormatedElement
    {
        private bool? _bold;
        [XmlAttribute]
        public bool Bold { get=>_bold ?? false; set=>_bold=value; }
        private int? _size;
        [XmlAttribute]
        public int Size { get=> _size ?? 14; set => _size = value; }
        private ParagraphAlignment? _alignment;
        [XmlAttribute]
        public ParagraphAlignment Alignment { get => _alignment ?? ParagraphAlignment.Justify; set=>_alignment=value; }
        private Underline? _underline;
        [XmlAttribute]
        public Underline Underline { get => _underline ?? MigraDoc.DocumentObjectModel.Underline.None; set => _underline = value; }
        public bool? _italic;
        [XmlAttribute]
        public bool Italic { get => _italic ?? false; set => _italic=value; }
        public Borders Borders { get; set; }

        protected ParagraphFormat SetParametress(ParagraphFormat paragraphFormat)
        {
            paragraphFormat.Font.Name = "Times";
            paragraphFormat.Font.Bold = Bold;
            paragraphFormat.Font.Size = Size;
            paragraphFormat.Font.Underline = Underline;
            paragraphFormat.Font.Italic = Italic;
            paragraphFormat.Borders = Borders;
            paragraphFormat.Alignment = Alignment;
            return paragraphFormat;
        }
    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(Paragrapf), "Paragrapf")]
    [JsonDerivedType(typeof(Table), "Table")]
    public abstract record class BaseParagraph : FormatedElement, IParagraph
    {
        public abstract void Render(in RenderingSection element);
    }


    [Serializable]
    public record class Paragrapf: BaseParagraph, IParagraph
    {
        public Paragrapf()
        {

        }
        [XmlArray("Text")]
        [XmlArrayItem("InjectElement", typeof(InjectElement))]
        [XmlArrayItem("Text", typeof(RawText))]
        public BaseElement[] text { get; set; }


        public override void Render(in RenderingSection section)
        {
            var paragraph = section.AddParagraph();
            SetParametress(paragraph.Format);

            foreach (IBaseElement temp in text)
            {
                temp.Render(paragraph);
            }
        }




    }

    public record class Table: BaseParagraph
    {
        public Row Head { get; set; }

        [XmlArray]
        public Row[]Rows { get; set; }

        [XmlArray]
        public Column[] Columns { get; set; }
        public override void Render(in RenderingSection section)
        {
            var Table = section.AddTable();
            SetParametress(Table.Format);

            Head.Render(Table, true);
            foreach (Row row in Rows)
                row.Render(Table);

        }
    }

    public record class  Row: FormatedElement
    {
        public Cell[] Cells { get; set; }

        public void Render(in RenderingTable.Table section, bool isHead = false)
        {
            var row = section.AddRow();
            SetParametress(row.Format);
            row.HeadingFormat = isHead;
            foreach (Cell cell in Cells)
                cell.Render(row);
        }
    }

    public record class Column: FormatedElement
    {
        public Cell[] Cells { get; set; }

        public void Render(in RenderingTable.Table section, bool isHead = false)
        {
            var column = section.AddColumn();
            SetParametress(column.Format);
            column.HeadingFormat = isHead;
            foreach (Cell cell in Cells)
                cell.Render(column);
        }
    }

    public record class Cell: FormatedElement
    {
        public Paragrapf[] Text { get; set; }
        public void Render(in RenderingTable.Row section)
        {
            var t = section.Cells[0];
        }

        public void Render(in RenderingTable.Column section)
        {
            throw new NotImplementedException();
        }
    }



    [JsonDerivedType(typeof(InjectElement), "InjectElement")]
    [JsonDerivedType(typeof(RawText), "Text")]
    public abstract record class BaseElement : IBaseElement
    {
        public BaseElement()
        {

        }
        [XmlText]
        public abstract string TextValue { get; set; }
        public abstract void Render(in Paragraph element);
    }


    
    public record class InjectElement: BaseElement, IInjectValue
    {
        public InjectElement()
        {

        }
        [DataMember]
        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public override string TextValue { get; set; }

        public override void Render(in Paragraph paragraph)
        {
            paragraph.AddFormattedText($"{TextValue} ");
        }
    }


    
    public record class RawText : BaseElement, IText
    {
        public RawText()
        {

        }
        [XmlText]
        public override string TextValue { get; set; }

        public override void Render(in Paragraph paragraph)
        {
            paragraph.AddFormattedText($"{TextValue} ");
        }
    }
}

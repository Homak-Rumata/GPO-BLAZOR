using RenderingDocument = MigraDoc.DocumentObjectModel.Document;
using RenderingSection = MigraDoc.DocumentObjectModel.Section;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Internals;
using System.Xml.Serialization;
using System.Xml.Schema;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using System.Runtime.Serialization;
using System.Reflection.Metadata;

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
                            new Text { TextValue = "Договор о практической подготовке обучающихся в форме практики №" },
                            new InjectElement { Name = "ContractNumber", TextValue = "1" },
                            new Text { TextValue = "г. Томск" },
                        }
                        },
                        new Paragrapf()
                        {
                            text = new BaseElement[]
                            {
                                new Text {TextValue = "Федеральное государственное автономное образовательное учреждение высшего образования «Томский государственный университет систем управления и радиоэлектроники» (ТУСУР), именуемое в дальнейшем «Университет», в лице директора центра карьеры И.А. Трубчениновой, действующего на основании доверенности от 19.09.2024 №20/3460, с одной стороны, и" },
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
        public int Right { get; init; }
        public int Left { get; init; }
        public int Top { get; init; }
        public int Bottom { get; init; }
    }




    public struct Document: IDocument
    {
        public Document()
        {

        }
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
        public Paragrapf[] paragrapfs { get; set; }

        public void Render(in RenderingDocument document)
        {
            var section = document.AddSection();
            
            foreach (IParagraph temp in paragrapfs)
            {
                temp.Render(section);
            }
        }

    }

    [DataContract]
    public record struct Paragrapf: IParagraph
    {
        public Paragrapf()
        {

        }
        public BaseElement[] text { get; set; }

        public bool Bold { get; set; } = false;
        public int Size { get; set; } = 14;
        public ParagraphAlignment Alignment { get; set; } = ParagraphAlignment.Justify;
        public Underline Underline { get; set; } = Underline.None;
        public bool Italic { get; set; } = false;
        public Borders Borders { get; set; }

        

        public void Render(in RenderingSection section)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Name = "Times";
            paragraph.Format.Font.Bold = Bold;
            paragraph.Format.Font.Size = Size;
            paragraph.Format.Font.Underline = Underline;
            paragraph.Format.Font.Italic = Italic;
            paragraph.Format.Borders = Borders;
            paragraph.Format.Alignment = Alignment;

            foreach (IBaseElement temp in text)
            {
                temp.Render(paragraph);
            }
        }




    }

    [DataContract]
    //[XmlRoot(Namespace = Constants.Namespace)]
    [XmlInclude(typeof(InjectElement))]
    [XmlInclude(typeof(Text))]
    public abstract record class BaseElement : IBaseElement
    {
        public BaseElement()
        {

        }
        public abstract string TextValue { get; set; }
        public abstract void Render(in Paragraph element);
    }

    [DataContract]
    public record class InjectElement: BaseElement, IInjectValue
    {
        public InjectElement()
        {

        }
        public string Name { get; set; }
        public override string TextValue { get; set; }

        public override void Render(in Paragraph paragraph)
        {
            paragraph.AddFormattedText($"{TextValue} ");
        }
    }

    [DataContract]
    public record class Text : BaseElement, IText
    {
        public Text()
        {

        }
        public override string TextValue { get; set; }

        public override void Render(in Paragraph paragraph)
        {
            paragraph.AddFormattedText($"{TextValue} ");
        }
    }
}

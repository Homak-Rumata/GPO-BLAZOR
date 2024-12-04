
using PdfFilePrinting;
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
//using Microsoft.VisualStudio.TestPlatform.PlatformAbstractions.Interfaces;
using MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel.Visitors;
using static System.Collections.Specialized.BitVector32;
using PdfSharp.Pdf.IO;
using static MigraDoc.DocumentObjectModel.Text;

namespace PdfFilePrinting.DocumentService
{

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

        [XmlElementAttribute(Type = typeof(Paragrapf))]
        [XmlElementAttribute(Type = typeof(MyltiplyParagraph))]
        [XmlElementAttribute(Type = typeof(Table))]
        public BaseParagraph[] paragrapfs { get; set; }

        public void Render(in RenderingDocument document)
        {
            var section = document.AddSection();
            document.AddStyle("OS TUSUR", "normal");
            section.PageSetup = document.DefaultPageSetup.Clone();
            section.PageSetup.PageFormat = PageFormat.A4;
            foreach (IParagraph temp in paragrapfs)
            {
                temp.Render(section);
            }
        }

    }


    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(TextFormat), "TextFormat")]
    public abstract record class TextFormat
    {

        [XmlAttribute]
        public int Size { get => _size ?? 14; set => _size = value; }
        protected int? _size;
        [XmlAttribute]
        public bool Bold { get => _bold ?? false; set => _bold = value; }
        protected bool? _bold;

        [XmlAttribute]
        public Underline Underline { get => _underline ?? Underline.None; set => _underline = value; }
        protected Underline? _underline;

        [XmlAttribute]
        public bool Italic { get => _italic ?? false; set => _italic = value; }
        protected bool? _italic;

        [XmlAttribute]
        public int SpaceBefore { get; set; }
        [XmlAttribute]
        public int SpaceAfter { get; set; }
    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(FormatedElement), "FormatedElement")]
    public abstract record class FormatedElement: TextFormat
    {
        [XmlAttribute]
        public ParagraphAlignment Alignment { get => _alignment ?? ParagraphAlignment.Justify; set => _alignment = value; }
        private ParagraphAlignment? _alignment;
        public Borders Borders { get; set; }
        [XmlAttribute]
        public bool KeepWithNext { get; set; }
        [XmlAttribute]
        public bool KeepTogether { get; set; }

        protected ParagraphFormat SetParametress(ParagraphFormat paragraphFormat)
        {
            paragraphFormat.KeepWithNext = KeepWithNext;
            paragraphFormat.KeepTogether = KeepTogether;
            paragraphFormat.Font.Name = "Times";
            paragraphFormat.Font.Bold = Bold;
            paragraphFormat.Font.Size = Size;
            paragraphFormat.Font.Underline = Underline;
            paragraphFormat.Font.Italic = Italic;
            paragraphFormat.SpaceBefore = SpaceBefore * paragraphFormat.Font.Size;
            paragraphFormat.SpaceAfter = SpaceAfter * paragraphFormat.Font.Size;
            if (Borders is not null)
            paragraphFormat.Borders = Borders;
            paragraphFormat.Alignment = Alignment;
            return paragraphFormat;
        }
    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(Paragrapf), "Paragrapf")]
    [JsonDerivedType(typeof(MyltiplyParagraph), "MyltiplyParagraph")]
    [JsonDerivedType(typeof(Table), "Table")]
    public abstract record class BaseParagraph : FormatedElement, IParagraph
    {
        public abstract void Render(in RenderingSection element);
        public abstract void Render(in RenderingTable.Cell element);
        public abstract void Render(in RenderingTable.Cell element, Unit with);
    }


    [Serializable]
    [JsonDerivedType(typeof(MyltiplyParagraph), "MyltiplyParagraph")]
    [XmlType("Paragrapf")]
    public record class Paragrapf: BaseParagraph, IParagraph
    {
        public Paragrapf()
        {

        }
        [XmlElementAttribute(Type = typeof(RawText))]
        [XmlElementAttribute(Type = typeof(MyltiplyInjectElement))]
        [XmlElementAttribute(Type = typeof(InjectElement))]
        public virtual BaseElement[] text { get; set; }
        
        [XmlAttribute]
        public int SpaceNum { get=>spaceNum; set=>spaceNum=value; }
        private int spaceNum = 0;

        [XmlAttribute]
        public bool Tab { get => tab; set => tab = value; }
        private bool tab = false;

        public override void Render(in RenderingSection section)
        {
            var paragraph = section.AddParagraph();
            SetParametress(paragraph.Format);
            paragraph.AddSpace(SpaceNum);
            if (tab) paragraph.AddTab();
            if (text is not null)
                foreach (IBaseElement temp in text)
                {
                    temp.Render(paragraph);
                }
        }

        public override void Render(in RenderingTable.Cell section)
        {
            var paragraph = section.AddParagraph();

            SetParametress(paragraph.Format);
            paragraph.AddSpace(SpaceNum);
            if (tab) paragraph.AddTab();
            if (text is not null)
                foreach (IBaseElement temp in text)
                {
                    temp.Render(paragraph);
                }
        }
        public override void Render(in RenderingTable.Cell section, Unit With)
        {
            var paragraph = section.AddParagraph();
            SetParametress(paragraph.Format);
            paragraph.Format.LeftIndent = With;
            paragraph.Format.RightIndent = With;

            paragraph.AddSpace(SpaceNum);
            if (tab) paragraph.AddTab();
            if (text is not null)
                foreach (IBaseElement temp in text)
                {
                    temp.Render(paragraph);
                }
        }




    }

    [XmlType("MyltiplyParagraph")]
    public record class MyltiplyParagraph : Paragrapf
    {
        [XmlElementAttribute(Type = typeof(RawText))]
        [XmlElementAttribute(Type = typeof(MyltiplyInjectElement))]
        [XmlElementAttribute(Type = typeof(InjectElement))]
        public override BaseElement[] text { get; set; }
        public override void Render(in RenderingTable.Cell section)
        {
            var count = text.Select(x => x as MyltiplyInjectElement).Where(x=> x !=null).Select(x=> x.Map = x.Map.Reverse().ToArray()).Max(x=>x.Count());
            //section.MergeDown = count;
            var rowIndex = section.Row.Index;
            var RowCount = section.Table.Rows.Count();
            if (rowIndex+1 == RowCount)
            {
                for (int i = 0; i < section.Table.Columns.Count(); i++)
                    section.Row.Cells[i].MergeDown = count-1;
                var size = Enumerable.Range(0, count-1).ToArray();
                foreach (var x in size)
                    section.Table.AddRow();
            }
            for (int i = count-1; i >= 0; i--)
            {
                
                var temp = i!=0 ? section.Column[rowIndex + i] : section;
                var u = section.Column.Index;
                if (temp is null)
                {
                    var t = section.Table.Rows[rowIndex + i];
                    temp = t[u];                      

                }
                if (temp is not null)
                {
                    if (RowCount > rowIndex + i+1)
                    {
                        var changed = section.Table.Rows[rowIndex + i + 1][u];
                        changed.MergeDown = temp.MergeDown > 0 ? temp.MergeDown - 1 : 0;
                    }
                    temp.MergeDown = 0;
                    base.Render(temp);
                }
            }
        }

        public override void Render(in RenderingSection section)
        {
            var count = text.Count(x => x is MyltiplyInjectElement);
            for (int i = 0; i < count; i++)
            {
                base.Render(section);
            }
        }
    }

    [XmlType("Table")]
    public record class Table: BaseParagraph
    {
        public Table()
        {

        }
        public Borders TableBorders { get; set; }
        public Row Head { get; set; }

        [XmlArray]
        public Row[] Rows { get; set; }

        [XmlArray]
        public Column[] Columns { get; set; }
        public override void Render(in RenderingSection section)
        {
            var Table = section.AddTable();
            Table.KeepTogether = KeepTogether;
            SetParametress(Table.Format);
            Table.Borders = TableBorders;
            Unit width = (Table.Section.PageSetup.PageWidth - Table.Section.PageSetup.LeftMargin - Table.Section.PageSetup.RightMargin) / Columns.Sum(x=>x.Priorety);
            foreach (Column column in Columns)
                column.Render(Table, width*column.Priorety);
            if (Head is not null)
            {
                Head.Render(Table, true);
            }
            if (Rows is not null)
            foreach (Row row in Rows)
                row.Render(Table);
        }
        public override void Render(in RenderingTable.Cell section)
        {
            TableRender (section.AddTextFrame().AddTable(), section.Column.Width);
        }

        public override void Render(in RenderingTable.Cell section, Unit with)
        {

            var Table = section.AddTextFrame().AddTable();
            TableRender(Table, with);

        }

        public void TableRender (RenderingTable.Table Table, Unit with = default(Unit))
        {
            Table.KeepTogether = KeepTogether;
            SetParametress(Table.Format);
            Table.Borders = TableBorders;
            Unit width;
            if (!with.Equals(default(Unit)))
                width = (with) / Columns.Sum(x => x.Priorety);
            else
                width = (Table.Section.PageSetup.PageWidth - Table.Section.PageSetup.LeftMargin - Table.Section.PageSetup.RightMargin) / Columns.Sum(x => x.Priorety);
            foreach (Column column in Columns)
                column.Render(Table, width * column.Priorety);
            if (Head is not null)
            {
                Head.Render(Table, true);
            }
            if (Rows is not null)
                foreach (Row row in Rows)
                    row.Render(Table);
        }
    }

    public record class  Row: FormatedElement
    {
        public Row()
        {

        }
        [XmlAttribute("Count")]
        public int Count { get; set; }
        
        public Cell[] Cells { get; set; }
        [XmlAttribute]
        public int KeepWith { get; set; }
        public virtual void Render(in RenderingTable.Table section, bool isHead = false)
        {
            var row = section.AddRow();
            row.KeepWith = KeepWith;
            SetParametress(row.Format);
            row.HeadingFormat = isHead;
            if (Cells is not null)
                for (int i = 0; Cells.Length > i; i++)
                {
                    Cells[i].Render(row.Cells[i]);
                }
        }
    }




    public record class Column: FormatedElement
    {
        public Column()
        {

        }
        [XmlAttribute]
        public int Priorety { get; set; }
        public Cell[] Cells { get; set; }

        public void Render(in RenderingTable.Table section, Unit width, bool isHead = false)
        {
            RenderingTable.Column column;
            var dob = width.Value;
            if (!dob.Equals(double.NaN))
                column = section.AddColumn(width);
            else
                column = section.AddColumn();
            SetParametress(column.Format);
            column.HeadingFormat = isHead;
            if (Cells is not null)
            {
                for (int i = 0; Cells.Length > i; i++)
                    Cells[i].Render(column[i]);
            }
            return;
        }
    }

    public record class Cell: FormatedElement
    {
        public Cell()
        {

        }
        [XmlAttribute]
        public int MergeDown { get; set; }
        [XmlArrayItem("Table", typeof(Table))]
        [XmlArrayItem("Paragrapf", typeof(Paragrapf))]
        [XmlArrayItem("MyltiplyParagraph", typeof(MyltiplyParagraph))]
        public BaseParagraph[] Text { get; set; }
        public void Render(in RenderingTable.Cell cell)
        {
            if (cell is not null)
            {
                cell.MergeDown = MergeDown;
                SetParametress(cell.Format);
                if (Text is not null)
                foreach (var text in Text)
                    text.Render(cell);
            }
            return;
        }

        public void Render(in RenderingTable.Cell cell, Unit with)
        {
            if (cell is not null)
            {
                SetParametress(cell.Format);
                foreach (var text in Text)
                    text.Render(cell, with);
            }
            return;
        }

    }



    [JsonDerivedType(typeof(InjectElement), "InjectElement")]
    [JsonDerivedType(typeof(MyltiplyInjectElement), "MyltiplyInjectElement")]
    [JsonDerivedType(typeof(RawText), "Text")]
    public abstract record class BaseElement : TextFormat, IBaseElement
    {
        public BaseElement()
        {

        }
        [XmlAttribute]
        public bool SpecialLine;
        [XmlAttribute]
        public bool SuperScript;
        [XmlAttribute]
        public bool SubScript;
        [XmlText]
        public abstract string TextValue { get; set; }
        public void Render(in Paragraph paragraph)
        {
            var formatedText = paragraph.AddFormattedText($"{TextValue}");
            if (SpecialLine)
            {
                formatedText.Italic = Italic;
                formatedText.Underline = Underline;
                formatedText.Bold = Bold;
                formatedText.Subscript = SubScript;
                formatedText.Superscript = SuperScript;
            }
        }
    }


    [JsonDerivedType(typeof(MyltiplyInjectElement), "MyltiplyInjectElement")]
    [XmlType("InjectElement")]
    public record class InjectElement: BaseElement, IInjectValue
    {
        public InjectElement()
        {

        }

        [XmlAttribute]
        public string Name { get; set; }
        [XmlText]
        public override string TextValue { get; set; }
    }
    [XmlType("MyltiplyInjectElement")]
    public record class MyltiplyInjectElement: InjectElement
    {

        [XmlArray("TextValue")]
        public string[] Map 
        { 
            get => map2.ToArray(); 
            set{ 
                map = new Stack<string>(value); 
                map2 = new Stack<string>(value); 
            } 
        }
        [XmlIgnore]
        public Stack<string> map;
        private Stack<string> map2;

        [XmlIgnore]
        public override string TextValue { get 
            {
                string result;
                if (map.TryPop(out result))
                {
                    if (map.Count == 0)
                        map = new Stack<string>(map2.Reverse().ToArray());
                    return result;
                }
                else
                {
                    map = new Stack<string>(map2.ToArray());
                    return map.Pop();
                }
            } 
                set 
                {
                    if (map == null)
                    {
                        map = new Stack<string>();
                        map2 = new Stack<string>();
                    }
                    map.Push(value);
                    map2.Push(value);

                } 
            }
    }
    [XmlType("RawText")]
    public record class RawText : BaseElement, IText
    {
        public RawText()
        {

        }
        [XmlText]
        public override string TextValue { get; set; }

    }
}

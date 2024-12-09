using RenderingDocument = MigraDoc.DocumentObjectModel.Document;
using RenderingSection = MigraDoc.DocumentObjectModel.Section;
using RenderingTable = MigraDoc.DocumentObjectModel.Tables;
using MigraDoc.DocumentObjectModel;
using System.Xml.Serialization;
using System.Xml;
using System.Runtime.Serialization;
using System.Text.Json.Serialization;
using Newtonsoft.Json;
using static System.Net.Mime.MediaTypeNames;
using static MigraDoc.DocumentObjectModel.Text;
using System.ComponentModel;
using System.Collections.Generic;

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

        public IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetNames()
        {
            var temp = Sections.SelectMany(x => x.GetNames());

            return temp;
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

        public IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetNames()
        {
            var temp = paragrapfs.SelectMany(x => x.GetName());

            return temp;
        }

    }


    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(TextFormat), "TextFormat")]
    public abstract record class TextFormat
    {

        [XmlAttribute]
        [DefaultValue(14)]
        public int Size { get; set; } = 14;
        [XmlAttribute]
        [DefaultValue(false)]
        public bool Bold { get; set; } = false;

        [XmlAttribute]
        [DefaultValue(Underline.None)]
        public Underline Underline { get; set; } = Underline.None;

        [XmlAttribute]
        [DefaultValue(false)]
        public bool Italic { get; set; } = false;

        [XmlAttribute]
        [DefaultValue(0)]
        public int SpaceBefore { get; set; } = 0;
        [XmlAttribute]
        [DefaultValue(0)]
        public int SpaceAfter { get; set; } = 0;
    }

    [JsonArray]
    [Serializable]
    [JsonDerivedType(typeof(FormatedElement), "FormatedElement")]
    public abstract record class FormatedElement: TextFormat
    {
        [XmlAttribute]
        [DefaultValue(ParagraphAlignment.Justify)]
        public ParagraphAlignment Alignment { get => _alignment ?? ParagraphAlignment.Justify; set => _alignment = value; }
        private ParagraphAlignment? _alignment;
        public Borders Borders { get; set; }
        [XmlAttribute]
        [DefaultValue(false)]
        public bool KeepWithNext { get; set; } = false;
        [XmlAttribute]
        [DefaultValue(false)]
        public bool KeepTogether { get; set; } = false;

        protected ParagraphFormat SetParametress(ParagraphFormat paragraphFormat)
        {
            paragraphFormat.KeepWithNext = KeepWithNext;
            paragraphFormat.KeepTogether = KeepTogether;
            paragraphFormat.Font = new MigraDoc.DocumentObjectModel.Font("Times New Roman", Size==0?14:Size);
            paragraphFormat.Font.Bold = Bold;
            paragraphFormat.Font.Underline = Underline;
            paragraphFormat.Font.Italic = Italic;
            paragraphFormat.SpaceBefore = SpaceBefore * paragraphFormat.Font.Size;
            paragraphFormat.SpaceAfter = SpaceAfter * paragraphFormat.Font.Size;
            if (Borders is null || Borders.BordersCleared)
            {
                paragraphFormat.Borders = new Borders();
            }
            else
            {
                paragraphFormat.Borders = new Borders();
                paragraphFormat.Borders.Visible = Borders.Visible;

                paragraphFormat.Borders.Top.Visible = Borders.Top.Visible;
                paragraphFormat.Borders.Bottom.Visible = Borders.Bottom.Visible;
                paragraphFormat.Borders.Left.Visible = Borders.Left.Visible;
                paragraphFormat.Borders.Right.Visible = Borders.Right.Visible;
            }
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

        public abstract IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetName();
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
        [DefaultValue(0)]
        public int SpaceNum { get=>spaceNum; set=>spaceNum=value; }
        private int spaceNum = 0;

        [XmlAttribute]
        [DefaultValue(false)]
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

        public override IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetName()
        {
            if (text is not null)
            {
                var u = text.Select(x => x.GetName()).Where(x => x is not null).Select(x => x.Value);
                return u;
            }
            else
            {
                return Enumerable.Empty < (string Name, Func<string> getter, Action<string> setter) > ();
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

        public override IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetName()
        {
            var tempRows = Rows.SelectMany(x => x.GetName());
            var tempColumns = Columns.SelectMany(x => x.GetName());
            var tempHead = Head is not null ? Head.GetName() : null;
            var tempsumm = tempRows.Concat(tempColumns);
            if (tempHead is not null)
                return tempsumm.Concat(tempHead);
            return tempsumm;
        }

        public override void Render(in RenderingSection section)
        {
            var Table = section.AddTable();
            Table.KeepTogether = KeepTogether;

            SetParametress(Table.Format);

            Table = SetBorders(Table);
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

        private RenderingTable.Table SetBorders(RenderingTable.Table Table)
        {
            if (TableBorders is not null)
            {
                var a = new Borders();
                var b = TableBorders;
                if (b.Visible)
                    a.Visible = b.Visible;

                if (b.Bottom.Values.BorderCleared.HasValue && ((!b.Bottom.Values.BorderCleared.Value)))
                {
                    a.Bottom = new Border();
                    a.Bottom.Visible = b.Bottom.Visible;
                }
                if (b.Top.Values.BorderCleared.HasValue && (!b.Top.Values.BorderCleared.Value))
                {
                    a.Top = new Border();
                    a.Top.Visible = b.Top.Visible;
                }
                if (b.Left.Values.BorderCleared.HasValue && (!b.Left.Values.BorderCleared.Value))
                {
                    a.Left = new Border();
                    a.Left.Visible = b.Left.Visible;
                }
                if (b.Right.Values.BorderCleared.HasValue && (!b.Right.Values.BorderCleared.Value))
                {
                    a.Right = new Border();
                    a.Right.Visible = b.Right.Visible;
                }
                TableBorders = a;
                ///
                Table.Borders = TableBorders;
            }
            return Table;
        }

        public void TableRender (RenderingTable.Table Table, Unit with = default(Unit))
        {
            Table.KeepTogether = KeepTogether;

            SetParametress(Table.Format);

            Table = SetBorders(Table);

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
        [DefaultValue(0)]
        public int Count { get; set; }
        
        public Cell[] Cells { get; set; }
        [DefaultValue(0)]
        [XmlAttribute]
        public int KeepWith { get; set; } = 0;
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

        public IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetName()
        {
            var temp = Cells.SelectMany(x => x.GetName());
            return temp;
        }
    }




    public record class Column: FormatedElement
    {
        public Column()
        {

        }
        [XmlAttribute]
        [DefaultValue(1)]
        public int Priorety { get; set; } = 1;
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

        public IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetName()
        {
            IEnumerable<(string Name, Func<string> getter, Action<string> setter)> temp;
            if (Cells is not null)
                temp = Cells.SelectMany(x => x.GetName());
            else
                temp = Enumerable.Empty<(string Name, Func<string> getter, Action<string> setter)>();
            return temp;
        }
    }

    public record class Cell: FormatedElement
    {
        public Cell()
        {

        }
        [XmlAttribute]
        [DefaultValue(0)]
        public int MergeDown { get; set; } = 0;
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

        public IEnumerable<(string Name, Func<string> getter, Action<string> setter)> GetName()
        {
            var temp = Text.SelectMany(x => x.GetName());
            return temp;
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
        [DefaultValue(false)]
        public bool SpecialLine { get; set; } = false;
        [XmlAttribute]
        [DefaultValue(false)]
        public bool SuperScript { get; set; } = false;
        [XmlAttribute]
        [DefaultValue(false)]
        public bool SubScript { get; set; } = false;
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

        public abstract (string  Name, Func<string> getter, Action<string> setter)? GetName();
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

        public override (string, Func<string>, Action<string>)? GetName()
        {
            var setter = () => (TextValue);
            var getter = (string val) => { TextValue = val; };
            return (Name, setter, getter);
        }
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
        [XmlIgnore]
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

        public override (string, Func<string>, Action<string>)? GetName()
        {
            return null;
        }

    }
}

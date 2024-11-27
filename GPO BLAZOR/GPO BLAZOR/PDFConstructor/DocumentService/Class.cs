using RenderingDocument = MigraDoc.DocumentObjectModel.Document;
using RenderingSection = MigraDoc.DocumentObjectModel.Section;
using MigraDoc.DocumentObjectModel;
using MigraDoc.DocumentObjectModel.Internals;

namespace GPO_BLAZOR.PDFConstructor.DocumentService
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
    public interface ISections<T> : IElement<T>;
    public interface IParagraph<T> : IElement<T>;

    public interface IBaseElement<T> : IElement<T>
    {
        string TextValue { get; }
    }
    public interface IInjectValue<T> : IBaseElement<T>
    {
        string TextValue { set; }
    }
    public interface IText<T>: IBaseElement<T>
    {
        string TextValue { init; }
    }

    record struct Margins
    { 
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
        private IEnumerable<ISections<RenderingDocument>> Sections;
        private static Margins margin = new(45, 60, 60, 60);
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

    public record struct Section: ISections<RenderingDocument>
    {
        private IEnumerable<IParagraph<RenderingSection>> paragrapfs { get; set; }

        public void Render(in RenderingDocument document)
        {
            var section = document.AddSection();
            foreach (IParagraph<RenderingSection> temp in paragrapfs)
            {
                temp.Render(section);
            }
        }
    }

    public record struct Paragrapf: IParagraph<RenderingSection>
    {
        private IEnumerable<IBaseElement<Paragraph>> text { get; init; }
        public void Render(in RenderingSection section)
        {
            var paragraph = section.AddParagraph();
            paragraph.Format.Font.Name = "Times";
            foreach (IBaseElement<Paragraph> temp in text)
            {
                temp.Render(paragraph);
            }
        }
    }

    public record struct InjectElement: IInjectValue<Paragraph>
    {
        public string Name { get; init; }
        public string TextValue { get; set; }
        public void Render(in Paragraph paragraph)
        {
            paragraph.AddFormattedText($"{TextValue} ");
        }
    }
    public record struct Text : IText<Paragraph>
    {
        public string TextValue { get; init; }
        public void Render(in Paragraph paragraph)
        {
            paragraph.AddFormattedText($"{TextValue} ");
        }
    }
}

using DBAgent;
using GPO_BLAZOR.FiledConfiguration.Document;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using PdfSharp.Pdf.Content.Objects;
using System.Collections.Frozen;
using System.Collections.Immutable;
using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;
using System.Xml.Serialization;


namespace GPO_BLAZOR.FiledConfiguration
{
    abstract class Field
    {
        public record Path
        {
            [XmlAttribute]
            public string Name { get; init; }
            [XmlAttribute]
            public string Page { get; init; }
            [XmlAttribute]
            public string Block { get; init; }
        }
        public Path path { get; init; }
        public FieldDateContainer field { get; init; }

    }

    public interface IPathElement
    {
        string Name { get; init; }
    }

    class PathElementComparer : IEqualityComparer<FieldDateContainer>, IEqualityComparer<IPathElement>
    {
        public bool Equals(IPathElement? b1, IPathElement? b2)
        {
            if (ReferenceEquals(b1, b2))
                return true;

            if (b2 is null || b1 is null)
                return false;

            return b1.Name == b2.Name;
        }

        public bool Equals(FieldDateContainer b1, FieldDateContainer b2)
        {
            if (ReferenceEquals(b1, b2))
                return true;

            return b1.Name == b2.Name;
        }

        public int GetHashCode(FieldDateContainer Container) => Container.GetHashCode();

        public int GetHashCode(IPathElement Container) => Container.GetHashCode();
    }

    public interface IDateContainer<DateType> : IFronzenContainer<DateType>, IPathElement where DateType : IPathElement
    {
        IEnumerable<DateType> Date { get; init; }

        IDateContainer<DateType> Summ(IDateContainer<DateType> A);
    }

    public interface IDateSuperContainer<DateContainer, IValue> : IDateContainer<DateContainer>
        where DateContainer : IDateContainer<IValue>
        where IValue : IPathElement
    {
        //        IDateSuperContainer<DateContainer, IValue> Summ(IDateSuperContainer<DateContainer, IValue> A);
    }

    public interface IFronzenContainer<T> where T : IPathElement
    {
        IDateContainer<T> ToFronzeFieldContainer();
    }

    record struct FieldDateContainer : IPathElement
    {
        public FieldDateContainer(string Name, string ClassType, string Id, string Text, bool IsDisabled)
        {
            this.Name = Name;
            this.ClassType = ClassType;
            this.Id = Id;
            this.Text = Text;
            this.IsDisabled = IsDisabled;
        }
        public string Name { get; init; }
        public string ClassType { get; init; }
        public string Id { get; init; }
        public string Text { get; set; }

        public bool IsDisabled { get; set; }
    }

    abstract record class Based<DateType> : IDateContainer<DateType> where DateType : IPathElement
    {
        [JsonIgnore]
        [XmlIgnore]
        public abstract string Name { get; init; }


        private IEnumerable<DateType> _date;

        public IEnumerable<DateType> Date
        {
            get
            {
                return _date;
            }
            init
            {
                _date = value;
            }
        }

        abstract public IDateContainer<DateType> ToFronzeFieldContainer();
        abstract public IDateContainer<DateType> Summ(IDateContainer<DateType> A);
    }

    abstract record class SecondBased<DataContainer, DateType> : Based<DataContainer>, IDateContainer<DataContainer>
        where DataContainer : class, IDateContainer<DateType>
        where DateType : IPathElement
    {
        public override IDateContainer<DataContainer> ToFronzeFieldContainer()
        {
            var newDate = base.Date.Select(x => x.ToFronzeFieldContainer() as DataContainer);
            if (newDate.Any(x => x != null))
            {
                var newContainer = this with { Date = newDate.ToImmutableArray() };
                return newContainer;
            }
            return this with { Date = base.Date.ToImmutableArray() };
        }

        protected static Container2 Summ<Container, Container2>(Container A, Container2 B)
            where Container2 : IDateContainer<DataContainer>
            where Container : Container2, new()
        {
            if (B.Date == null)
            {
                return A;
            }

            if (A.Date == null)
            {
                return B;
            }

            IEqualityComparer<DataContainer> jk = (IEqualityComparer<DataContainer>)new PathElementComparer();



            var uniue = A.Date
                .Except(B.Date, jk)
                .Concat(B.Date
                    .Except(A.Date, jk));

            var intersectA = A.Date.Intersect(B.Date, jk);
            var intersectB = B.Date.Intersect(A.Date, jk);

            var intersecAB = intersectA.Select(x => intersectB.First(a => a.Name == x.Name).Summ(x));

            return new Container() { Name = A.Name, Date = uniue.Concat(intersectA).Concat(intersectB) };
        }

        //        public abstract IDateSuperContainer<DataContainer, DateType> Summ(IDateSuperContainer<DataContainer, DateType> A);
    }

    record class BlockDateContainer : Based<FieldDateContainer>, IDateContainer<FieldDateContainer>
    {
        public BlockDateContainer()
        {

        }
        public BlockDateContainer(string name, IEnumerable<FieldDateContainer> date)
        {
            Name = name;
            Date = date.ToList();
        }

        [JsonPropertyName("BlockName")]
        [XmlAttribute("BlockName")]
        public override string Name { get; init; }

        public override IDateContainer<FieldDateContainer> ToFronzeFieldContainer()
        {
            var newContainer = this with { Date = base.Date.ToImmutableArray() };
            return newContainer;
        }

        public static IDateContainer<FieldDateContainer> operator +(BlockDateContainer A, BlockDateContainer B)
        {
            return A.Summ(B);
        }

        public override IDateContainer<FieldDateContainer> Summ(IDateContainer<FieldDateContainer> dateContainer)
        {
            var result = this.Date.Union(dateContainer.Date);
            if (result.Any())
            {
                return new BlockDateContainer(Name, result);
            }
            return null;
        }

    }

    record class PageDateContainer : SecondBased<BlockDateContainer, FieldDateContainer>, IDateContainer<BlockDateContainer>
    {
        public PageDateContainer()
        {

        }
        public PageDateContainer(string name, IEnumerable<BlockDateContainer> date)
        {
            Name = name;
            Date = date.ToImmutableArray();
        }
        [JsonPropertyName("PageName")]
        [XmlAttribute("PageName")]
        public override string Name { get; init; }


        public override IDateContainer<BlockDateContainer> ToFronzeFieldContainer()
        {
            return this with { Date = base.ToFronzeFieldContainer().Date };
        }

        public static IDateContainer<BlockDateContainer> operator +(PageDateContainer A, PageDateContainer B)
        {
            return A.Summ(B);
        }

        public override IDateContainer<BlockDateContainer> Summ(IDateContainer<BlockDateContainer> dateContainer)
        {
            return Summ(this, dateContainer);
        }
    }

    record class StatmenDateContainer : SecondBased<PageDateContainer, BlockDateContainer>, IDateContainer<PageDateContainer>
    {
        public StatmenDateContainer()
        {

        }
        public StatmenDateContainer(IEnumerable<PageDateContainer> date)
        {
            //StatmenName = statmenname;
            Date = date.ToImmutableArray();
        }
        [JsonIgnore]
        [XmlIgnore]
        public override string Name { get; init; }

        //public string StatmenName { get; set; }
        public override IDateContainer<PageDateContainer> ToFronzeFieldContainer()
        {
            return this with { Date = base.ToFronzeFieldContainer().Date };
        }


        public static IDateContainer<PageDateContainer> operator +(StatmenDateContainer A, StatmenDateContainer B)
        {
            return A.Summ(B);
        }

        public override IDateContainer<PageDateContainer> Summ(IDateContainer<PageDateContainer> dateContainer)
        {
            return Summ(this, dateContainer);
        }
    }


    static class StatmenDate
    {
        /// <summary>
        /// Метод сопоставления Списка полей и Полей в документе
        /// </summary>
        /// <returns></returns>
        public static (IDictionary<string, IDateContainer<PageDateContainer>> Templates, IDictionary<string, FieldCont.IField> Values) ExperementalTemplate()
        {
            IDocument FieldInDocument = new Documnet()
            {
                Name = "Заявление",
                Description = "Что-то",
                Fields = new[] {
                    (IFields)(new Fields() { Name = "FirstNameTextField", Attribute = "2"}),
                    (IFields)(new Fields() { Name = "SecondNameTextField" }),
                    (IFields)(new Fields() { Name = "TreeNameTextField" }) }
            };

            FieldCont.IField[] FieldCollextion = new[] {
                (FieldCont.IField)new FieldCont.Field() {
                Name = "FirstNameTextField",
                Path = new FieldCont.Path() {
                    Block = "Основные данные",
                    Page = "Пользовательские данные", },
                Template = new FieldDateContainer("FirstNameTextField", "InputInformationField", "FirstNameTextField1", "Имя", false), },
                 (FieldCont.IField)new FieldCont.Field() {
                Name = "SecondNameTextField",
                Path = new FieldCont.Path() {
                    Block = "Основные данные",
                    Page = "Пользовательские данные" },
                Template = new FieldDateContainer("SecondNameTextField", "InputInformationField", "SecondNameTextField", "Фамилия", false), },
                  (FieldCont.IField)new FieldCont.Field() {
                Name = "TreeNameTextField",
                Path = new FieldCont.Path() {
                    Block = "Основные данные",
                    Page = "Пользовательские данные" },
                Template = new FieldDateContainer("TreeNameTextField", "InputInformationField", "TreeNameTextField", "Отчество (при наличии)", false), }};

            IDictionary<string, FieldCont.IField> t;

            return (Constructor.GetFields(FieldCollextion, new[] { FieldInDocument }, out t), t);
        }


    }
}




    /*
    DBSET

    Field

    path
    */


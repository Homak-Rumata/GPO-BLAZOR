using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.Web.Virtualization;
using Microsoft.Extensions.Options;
using Microsoft.VisualBasic;
using System.Collections;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Windows.Markup;
using System.Xml.Serialization;
using System.Xml.XPath;
using System.Text.Json;
using System.ComponentModel.Design;


namespace GPO_BLAZOR.Client.Class.Date
{
    public enum PracticType
    {
        Factory,
        Studing

    }
    public enum State
    {
        Full,
        Partial,
        Agree
    }


    public interface IStatmentDate
    {
        public string ID { get; }
        public int Num { get; }
        DateTime? Tyme { get; }
        PracticType PracticType { get; }
        State State { get; }

        void GetDate();

    }



    public class StatmentDate : IStatmentDate
    {
        static int number = 0;
        public StatmentDate(string ID, string tyme, int status, int type)
        {
            this.ID = ID;
            try
            {
                this.Tyme = DateAndTime.TimeValue(tyme);
            }
            catch
            {
                this.Tyme = DateAndTime.Now;
            }
            this.State = (State)status;
            this.PracticType = (PracticType)type;
            Num = number;
            number++;
            Codificator.Add(Num, this);
        }

        public static Dictionary<int, IStatmentDate> Codificator = new Dictionary<int, IStatmentDate>();

        public static void Restore()
        {
            Codificator = new Dictionary<int, IStatmentDate>();
            number = 0;
        }

        public string ID { get; }
        public int Num { get; }
        public DateTime? Tyme { get; }
        public PracticType PracticType { get; }
        public State State { get; }

        public void GetDate()
        {

        }
    }

    interface IContainer
    {
        string GetStringValue();
        Dictionary<string, string> GetDictionaryValue();
    }

    class PageContainer : IContainer
    {
        public PageContainer((string, string)[] Value)
        {
            this._value = Value.ToDictionary(item => item.Item1, value => value.Item2);
        }
        private Dictionary<string, string> _value;

        public string GetStringValue()
        {
            return JsonSerializer.Serialize(_value.Select(item => $"{item.Key}:{item.Value}"));
        }
        public Dictionary<string, string> GetDictionaryValue()
        {
            return _value;
        }
    }

    class BlockContainer : IContainer
    {
        public BlockContainer(IContainer[] ContainedDate)
        {
            _containedDate = ContainedDate;
        }

        IContainer[] _containedDate;

        public string GetStringValue()
        {
            char[] symbols = new char[] { '{', '}' };
            return string.Join(", \n", _containedDate.Select(item => item.GetStringValue()).Select(item => item.Trim().Trim(symbols)));
        }
        public Dictionary<string, string> GetDictionaryValue()
        {
            //Возможна ошибка
            return _containedDate.Select(item => item.GetDictionaryValue()).Aggregate((a, b) => (Dictionary<string, string>)a.Union(b));
        }
    }


    public interface IValuesFromJSON
    {
        string GetValue();
    }

    public interface IField : IValuesFromJSON, Microsoft.AspNetCore.Components.IComponent
    {
        string idvalue { get; }
        string classvvalue { get; }
        string namevalue { get; }
        string textvalue { get; }

        public string value { get; set; }
        public void SetDelegate(Func<string> SetedDelegate);
    }



    public interface ITextField : IField
    {
        public void setValue(string info);
    }

    public interface ITextField<T> : IField
    {
        public T value { get; set; }
        public void setValue(T info);
    }

    public interface ITimeValue : IField
    {
        public DateTime value { get; set; }
    }

    public interface ICollectionValue<T> : ITextField<T>
    {
        public List<T> CollectionValues { get; set; }
    }

    public interface ICollectionValue : ITextField
    {
        public List<string> CollectionValues { get; }
    }

    public interface IBlockValues : IValuesFromJSON
    {
        public IField[] TextValues { get; }

        public string NameBlock { get; }

    }

    public interface IPageValues : IValuesFromJSON
    {
        public IBlockValues[] TextValues { get;}

        public string NamePageBlock { get;}
    }

    public interface IStatmenValue : IValuesFromJSON
    {
        public IPageValues[] TextValues { get; }
        Dictionary<string, IField> ContainsField { get; set; }
    }



    public abstract class Field : Microsoft.AspNetCore.Components.ComponentBase, IField
    {
        //Поле
        /*public Field Fill(string idvalue, string classvvalue, string namevalue, string textvalue)
        {
            new Field { 
            this.idvalue = idvalue;
            this.textvalue = textvalue;
            this.namevalue = namevalue;
            this.classvvalue = classvvalue;
        }*/


        public string idvalue { get; init; }
        public string classvvalue { get; init; }
        public string namevalue { get; init; }
        public string textvalue { get; init; }

        public string value { get; set; }

        public abstract string GetValue();

        delegate string ValueDelegate();

        private ValueDelegate GetValueDelegate;

        void SetDelegate(ValueDelegate value)
        {
            GetValueDelegate = value;
        }

        public virtual void SetDelegate(Func<string> SetedDelegate)
        {
            ValueDelegate temp = () => (SetedDelegate());
            SetDelegate(temp);
        }

    }

    public class TextValue<T> : Field, ITextField<T>
    {
        public TextValue ()
            {
                GetValueDelegate = () => { 
                    Console.WriteLine();
                    try
                    {
                        return $"\"{idvalue.ToString()}\": \"{value.ToString()}\"";
                    }
                    catch (Exception ex) 
                    {
                        return $"\"{idvalue.ToString()}\": \"\"";
                    }
                };
            }

        public T value { get; set; }

        public virtual void setValue (T info)
        {
            value = info;
        }

        delegate string ValueDelegate();

        private ValueDelegate GetValueDelegate;

        void SetDelegate (ValueDelegate value)
        {
            GetValueDelegate = value;
        }

        public override void SetDelegate(Func<string> SetedDelegate)
        {
            ValueDelegate temp = () => (SetedDelegate());
            SetDelegate(temp);
        }

        public override string GetValue()
        {
            return GetValueDelegate();
        }
    }


    public class CollectionValue<T> : TextValue<T>, ICollectionValue<T>
    {


        public List<T> CollectionValues { get; set; }

        public void SetString(int valuenum)
        {
            value = CollectionValues[valuenum];
        }
    }

    public abstract record GetBlockValue: IValuesFromJSON
    {
        public abstract IValuesFromJSON[] TextValues { get; }
            public string GetValue()
            {
                return TextValues.Select(value => value.GetValue()).Aggregate((x, y) => x + ", " + y);
            }
    }

    public record BlockValue: GetBlockValue, IBlockValues
    {
        //Информация блока
        public BlockValue (string name, IField[] Fields)
        {
            NameBlock = name;
            TextValues = Fields;
        }
        public override IField[] TextValues { get; }
        public string NameBlock { get; }
    }

    public record PageValues: GetBlockValue, IPageValues
    {
        //Информация Страницы
        public PageValues(string name, IBlockValues[] Block)
        {
            NamePageBlock = name;
            TextValues = Block;
        }
        public override IBlockValues[] TextValues { get; }
        public string NamePageBlock { get; }
    }

    public record StatmenValue: GetBlockValue, IStatmenValue
    {
        //Информация заявления
        public StatmenValue(IPageValues[] Page, Dictionary<string, IField> dictionary)
        {
            TextValues = Page;
            ContainsField = dictionary;
        }

        public Dictionary<string, IField> ContainsField { get; set; }
        public override IPageValues[] TextValues { get; }

    }
    

    
}

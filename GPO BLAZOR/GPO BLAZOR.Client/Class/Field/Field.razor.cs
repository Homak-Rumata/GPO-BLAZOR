namespace GPO_BLAZOR.Client.Class.Field
{
    public partial class Field<T> : Date.Field, Date.ITextField<T>
    {


        public override string GetValue()
        {
            return $"\"{idvalue.ToString()}\": \"{value.ToString()}\"";
        }
        public void setValue(T info)
        {
            value = info;
        }

        delegate string ValueDelegate();

        private ValueDelegate GetValueDelegate;

        void SetDelegate(ValueDelegate value)
        {
            GetValueDelegate = value;
        }

        public override void SetDelegate(Func<string> SetedDelegate)
        {
            ValueDelegate temp = () => (SetedDelegate());
            SetDelegate(temp);
        }
    }
}

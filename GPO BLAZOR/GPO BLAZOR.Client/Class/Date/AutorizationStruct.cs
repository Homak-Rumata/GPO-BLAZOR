
namespace GPO_BLAZOR.Client.Class.Date
{
    public enum Roles
    {
        Student,
        CafedralLeader,
        FieldCommander,
    }
    public interface IAutorizationStruct : IAsyncDisposable
    {
        string GetString();
        Roles[] Role { get; set; }

        bool HasValue(Roles role);
    }
    public class AutorizationStruct : IAutorizationStruct
    {
        public AutorizationStruct()
        {

        }

        public ValueTask DisposeAsync()
        {
            throw new NotImplementedException();
        }

        public string GetString()
        {
            return "1";
        }
        public Roles[] Role { get; set; }

        public bool HasValue(Roles role)
        {
            try
            {
                return Role.Contains(role);
            }
            catch
            {
#if DEBUG
                Console.WriteLine(role);
#endif
                return false;
            }
        }
    }
}

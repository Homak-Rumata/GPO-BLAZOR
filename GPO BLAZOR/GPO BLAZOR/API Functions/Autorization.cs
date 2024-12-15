using DBAgent;

namespace GPO_BLAZOR.API_Functions
{
    public class Autorization
    {
        public record AutorizationDate
        {
            public string login { get; init; }
            public string Password { get; init; }
        }

        static public async Task<(bool, int)> checkuser(AutorizationDate Date, Gpo2Context contex)
        {
            var users = (contex.Users
                        .Where(x => (x.Email == Date.login && x.Password == Date.Password))
                        .FirstOrDefault());
            if (users == null)
            {
                return (false, 0);
            }
            return (users is not null, users.Id);
        }
    }
}

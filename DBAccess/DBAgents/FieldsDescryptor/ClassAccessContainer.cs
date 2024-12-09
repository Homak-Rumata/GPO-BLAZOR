using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using DBAccess;
using DBAgent;
using DBAgent.Models;
using Microsoft.EntityFrameworkCore;

namespace DBAccess.DBAgents.FieldsDescryptor
{
    internal class ClassAccessContainer
    {

    }

    public interface IAccessor
    {
        Gpo2Context _dataBase { get; init; }
        public string Get(string Name);
        public void Set(string Name, string Value);
    }

    struct FirstName : IAccessor
    {
        public string Get(string Name)
        {
            return _getter(_dataBase, Name);
        }

        public FirstName(Gpo2Context DataBase)
        {
            var _getter = EF.CompileQuery(static (Gpo2Context DB, string Name) => (get(DB, Name)));
        }

        public Func<Gpo2Context, string, string> _getter;
        public Gpo2Context _dataBase { get; init; }
        private static string get(Gpo2Context DataBase, string Name)
        {
            return DataBase.Users
                    .AsNoTracking()
                    .Where(x => x.Email == Name)
                    .Select(x => x.FirstName)
                    .Aggregate(new StringBuilder(),(x,y)=>x
                        .Append(y))
                    .ToString();
        }
        public void Set(string Name, string Value)
        {
            User f = new User() { FirstName = Value };
            _dataBase.Users.Add(f);
        }

        public void D()
        {
            _dataBase.Users();
        }
    }


    class Contract
    {
        public void D()
        {

        }
    }
}

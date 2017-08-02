using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateWay.BaseInterfaces
{
    public interface IAgent
    {
         string NameFull { get; set; }
         string ServName { get; set; }
         string RS { get; set; }
         int Number { get; set; }
         string Tranz { get; set; }
         string KorSH { get; set; }
         string INN { get; set; }
         string KBK { get; set; }
         string OKATO { get; set; }
         string KPP { get; set; }
         string BIK { get; set; }
         string BankRub { get; set; }
         string Region { get; set; }
         int KodPol { get; set; }
         double Tax { get; set; }
         double TaxMin { get; set; }
         double TaxMax { get; set; }
         string DopNaz { get; set; }
         string Contact { get; set; }
         string Email { get; set; }
         int ID_SERV { get; set; }
         string Kass_symbol { get; set; }
         double Summa { get; set; }

        bool Validate(out string error);
        IQueryable<IAgent> Get(string parametr, string flag = null);
       
    }
}

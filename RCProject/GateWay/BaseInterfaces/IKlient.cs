using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GateWay.BaseInterfaces
{
    public interface IKlient
    {
         string FIO { get; set; }
         string Adres { get; set; }
         double? DiasoftNumber { get; set; }
         double? DiasoftDocNumber { get; set; }
         int DocType { get; set; }
         string NumberDoc { get; set; }
         string SeriyaDoc { get; set; }
         string TaxNumber { get; set; }
         string Resident { get; set; }
         string Document { get; set; }
         string DocumentId { get; set; }
         string DocumentDate { get; set; }
         string PayerInn { get; set; }

        bool Validate(out string error);
        IQueryable<IKlient> Get(string parametr , string login = null , string password = null);
    }
}

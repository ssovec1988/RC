using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GateWay.BaseInterfaces
{
    public interface IKassa
    {
        int Np { get; set; }
        string Pachka { get; set; }
        string Sc { get; set; }
        string Doh { get; set; }
        string City { get; set; }
        string Pachka_vnesh { get; set; }
        string Tranzit_vnesh { get; set; }
        string Nds { get; set; }
        string Nds_komiss { get; set; }
        string Pachka_vnut { get; set; }
    }
}
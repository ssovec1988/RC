using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GateWay.BaseInterfaces
{
    public interface IPay
    {
        double Summa { get; set; }
        double? SummaTax { get; set; }
        string KassSymbol { get; set; }
        string Purpose { get; set; }
        string TaxPeriod { get; set; }
        string TaxReason { get; set; }
        int DrawStat { get; set; }

        bool Validate(bool noBase,bool isBudjet,out string error);
    }
}

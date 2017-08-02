using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GateWay.BaseInterfaces;
using GateWay.Entity;
using Common;

namespace GateWay.PaymentToOrganization
{
    public class Kassa : IKassa
    {
        public int Np { get; set; }
        public string Pachka { get; set; }
        public string Sc { get; set; }
        public string Doh { get; set; }
        public string City { get; set; }
        public string Pachka_vnesh { get; set; }
        public string Tranzit_vnesh { get; set; }
        public string Nds { get; set; }
        public string Nds_komiss { get; set; }
        public string Pachka_vnut { get; set; }


        public Kassa GetKass(int np)
        {
            ModelInitializer init = new ModelInitializer();
            EFGenericRepository<KASSESDB_> model = new EFGenericRepository<KASSESDB_>(init.Dictionary);
            KASSESDB_ k = model.Get(x => x.NP == np).FirstOrDefault();

            Kassa temp = new Kassa
            {
                City = k.CITY,
                Doh = k.KOMIS_S_KL,
                Nds = k.NDS_SCH,
                Nds_komiss = k.NDS_KOMISS,
                Np = (int)k.NP,
                Pachka = k.PACHKA,
                Pachka_vnesh = k.PACHKA_VNESH,
                Pachka_vnut = k.PACHKA_VNUT,
                Sc = k.SC,
                Tranzit_vnesh = k.TRANZIT_VNESH
            };

            return temp;
        }
    }
}
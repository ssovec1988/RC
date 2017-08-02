using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GateWay.Entity;
using Common;

namespace GateWay.Log
{
    public static class Logger
    {
        public static void Loging(int np , string error , string kassir)
        {
            ModelInitializer init = new ModelInitializer();
            EFGenericRepository<Loging> model = new EFGenericRepository<Loging>(init.Log);

            model.Add(new Loging { Data = DateTime.Now.ToShortDateString() + DateTime.Now.ToShortTimeString(), ERROR = error, N_KASS = np, KASSIR = kassir });
        }
    }
}
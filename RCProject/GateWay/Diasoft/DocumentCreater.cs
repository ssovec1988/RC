using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Globalization;
using GateWay.PaymentToOrganization;

namespace GateWay.Diasoft
{
    public static class DocumentCreater
    {
        #region Создание типов кассовых ордеров

        /// <summary>
        /// Создать кассовый ордер для платежа ПДС
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <param name="kassa"></param>
        /// <param name="summa"></param>
        /// <param name="purpose"></param>
        /// <param name="kassirFio"></param>
        /// <param name="packetIdent"></param>
        /// <returns></returns>
        public static KassOrder CreatePdsOrder(Agent a, Klient k, Kassa kassa , double summa , string purpose , string kassirFio , string packetIdent , string symbol = null)
        {
             KassOrder order = new KassOrder();

            order.OperationDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "d.M.yyyy", CultureInfo.InvariantCulture);
            order.BatchBrief = kassa.Pachka;
            order.Number = 0;
            order.Date = DateTime.Now.ToShortDateString();
            order.Purpose = order.Purpose = "ПЛАТЕЖ В ПОЛЬЗУ ПОЛУЧАТЕЛЯ " + a.ServName + "В КАССЕ НОМЕР: " + kassa.Np + "ПЛАТЕЛЬЩИК: " + k.FIO + " " + purpose;

            order.DebitAccountNumber = kassa.Sc;
            order.CreditAccountNumber = a.Tranz;
            order.DebitAmount = summa.ToString().Replace(',', '.');
            order.CreditAmmount = summa.ToString().Replace(',', '.');
            order.ProfitAccountNumber = a.Tranz;

            string[] fio = k.FIO.Split(new char[]{' '} , StringSplitOptions.RemoveEmptyEntries);

            if (fio.Length == 3)
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = fio[2];
            }
            else
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = " ";
            }
        
            order.DebitCurencyCode = "810";
            order.OperationCode = "4";
            order.StaffBrief = kassirFio;
            order.NUMBER = 0;
            order.Klients_Ad = " ";
            order.PayeeName = a.NameFull;
            order.Packet_ident = packetIdent;
            order.Agent = a.ID_SERV;

            if (symbol != null)
                order.KassSymbol = symbol;
            else
                order.KassSymbol = a.Kass_symbol;

            if (k.DiasoftNumber == null)
            {
                order.DocumentId = 0;
                order.PersonId = 0;
            }
            else
            {
                order.DocumentId = (double)k.DiasoftDocNumber;
                order.PersonId = (double)k.DiasoftNumber;
            }

            return order;
        }

        /// <summary>
        /// Создать кассовый ордер для бюджетного/небюджетного платежа
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <param name="kassa"></param>
        /// <param name="summa"></param>
        /// <param name="purpose"></param>
        /// <returns></returns>
        public static KassOrder CreateOrderForNoPds (Agent a, Klient k, Kassa kassa , double summa , string purpose , string symbol)
        {
            KassOrder order = new KassOrder();

            order.OperationDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "d.M.yyyy", CultureInfo.InvariantCulture);
            order.BatchBrief = kassa.Pachka;
            order.Number = 0;
            order.Date = DateTime.Now.ToShortDateString();
            order.Purpose = purpose;

            order.DebitAccountNumber = kassa.Sc;
            order.CreditAccountNumber = kassa.Tranzit_vnesh;
            order.DebitAmount = summa.ToString().Replace(',', '.');
            order.CreditAmmount = summa.ToString().Replace(',', '.');
            order.ProfitAccountNumber = kassa.Tranzit_vnesh;

            string[] fio = k.FIO.Split(' ');

            if (fio.Length == 3)
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = fio[2];
            }
            else
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = " ";
            }
           
            order.DebitCurencyCode = "810";
            order.OperationCode = "4";
            order.StaffBrief = " ";
            order.NUMBER = 0;
            order.Klients_Ad = " "; // не знаю нужен или нет?????
            order.PayeeName = a.NameFull;
            order.PayeeBankBIC = a.BIK;
            order.PayeeINN = a.INN;
            order.PayeeAcccountNumber = a.RS;
            order.PayeeKPP = a.KPP;
            order.KassSymbol = symbol;

            if (k.DiasoftNumber == null)
            {
                order.DocumentId = 0;
                order.PersonId = 0;
            }
            else
            {
                order.DocumentId = (double)k.DiasoftDocNumber;
                order.PersonId = (double)k.DiasoftNumber;
            }
            return order;
        }

        /// <summary>
        /// Кассовый ордер пополнения счета
        /// </summary>
        /// <param name="a"></param>
        /// <param name="_k"></param>
        /// <param name="kl"></param>
        /// <param name="symbol"></param>
        /// <param name="purpose"></param>
        /// <param name="summa"></param>
        /// <returns></returns>
        //public static KassOrder CreateOrderForAccount(Account a, Kassa _k, Klient kl, string symbol, string purpose, double summa)
        //{
        //    KassOrder order = new KassOrder();

        //    order.OperationDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "d.M.yyyy", CultureInfo.InvariantCulture);
        //    order.BatchBrief = _k.Pachka;
        //    order.Number = 0;
        //    order.Date = DateTime.Now.ToShortDateString();
        //    order.Purpose = purpose;

        //    order.DebitAccountNumber = _k.Sc;
        //    order.CreditAccountNumber = a.AccNumber;
        //    order.DebitAmount = summa.ToString().Replace(',', '.');
        //    order.CreditAmmount = summa.ToString().Replace(',', '.');
        //    order.ProfitAccountNumber = a.AccNumber;

        //    string[] fio = kl.FIO.Split(' ');

        //    if (fio.Length == 3)
        //    {
        //        order.PersonName = fio[1];
        //        order.PersonSurName = fio[0];
        //        order.PersonPatronymicName = fio[2];
        //    }
        //    else
        //    {
        //        order.PersonName = fio[1];
        //        order.PersonSurName = fio[0];
        //        order.PersonPatronymicName = " ";
        //    }
           
        //    order.DebitCurencyCode = "810";
        //    order.OperationCode = "4";
        //    order.StaffBrief = " ";
        //    order.NUMBER = 0;
        //    order.Klients_Ad = kl.Adres;
        //    order.PayeeName = "АО ГЕНБАНК";
        //    order.Packet_ident = " ";
        //    order.KassSymbol = symbol;
        //    return order;
        //}


        /// <summary>
        /// Создать ордер за заполнение бланка
        /// </summary>
        /// <param name="kl"></param>
        /// <param name="k"></param>
        /// <param name="isNds"></param>
        /// <returns></returns>
        public static KassOrder CreateBlankOrder(Klient kl, Kassa k, bool isNds)
        {
            KassOrder order = new KassOrder();

            order.OperationDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "d.M.yyyy", CultureInfo.InvariantCulture);
            order.BatchBrief = k.Pachka;
            order.Number = 0;
            order.Date = DateTime.Now.ToShortDateString();
            order.DebitAccountNumber = k.Sc;

            if (!isNds)
            {
                order.DebitAmount = "42.37";
                order.CreditAmmount = "42.37";
                order.CreditAccountNumber = k.Nds_komiss;
                order.ProfitAccountNumber = k.Nds_komiss;
                order.Purpose = "Комиссия за заполнение бланка Заявления на перевод без открытия счета";
            }
            else
            {
                order.DebitAmount = "7.63";
                order.CreditAmmount = "7.63";
                order.CreditAccountNumber = k.Nds;
                order.ProfitAccountNumber = k.Nds;
                order.Purpose = " НДС с комиссии за заполнение бланка Заявления на перевод без открытия счета";
            }



            string[] fio = kl.FIO.Split(' ');

            if (fio.Length == 3)
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = fio[2];
            }
            else
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = " ";
            }
           

            order.DebitCurencyCode = "810";
            order.OperationCode = "4";
            order.StaffBrief = " ";
            order.NUMBER = 0;
            order.Klients_Ad = " "; // не знаю нужен или нет?????

            order.PayeeName = " ";
            order.KassSymbol = "32";

            return order;
        }

        /// <summary>
        /// Создание комиссии по недоговорным платежам
        /// </summary>
        /// <param name="a"></param>
        /// <param name="k"></param>
        /// <param name="kl"></param>
        /// <param name="summa"></param>
        /// <returns></returns>
        public static KassOrder KomissNoPds(Agent a , Kassa k, Klient kl , double summa)
        {
            KassOrder order = new KassOrder();
            order.OperationDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "d.M.yyyy", CultureInfo.InvariantCulture);
            order.Number = 0;
            order.Date = DateTime.Now.ToShortDateString();
            order.Purpose = "Комиссия с плательщика за перевод без открытия счета  НДС не обл.";


            order.DebitAccountNumber = k.Sc;
            order.BatchBrief = k.Pachka;


            order.CreditAccountNumber = k.Doh;
            order.DebitAmount = summa.ToString().Replace(',', '.');
            order.CreditAmmount = summa.ToString().Replace(',', '.');
            order.ProfitAccountNumber = k.Doh;

            string[] fio = kl.FIO.Split(' ');

            if (fio.Length == 3)
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = fio[2];
            }
            else
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = " ";
            }
            

            order.DebitCurencyCode = "810";
            order.OperationCode = "4";
            order.StaffBrief = " ";
            order.NUMBER = 0;
            order.Klients_Ad = " "; // не знаю нужен или нет?????

            order.PayeeName = a.NameFull;
            order.KassSymbol = "32";

            return order;
        }

        /// <summary>
        ///  Создание комиссии за договорной платеж
        /// </summary>
        /// <param name="a"></param>
        /// <param name="kl"></param>
        /// <param name="k"></param>
        /// <param name="summa"></param>
        /// <returns></returns>
        public static KassOrder KomissPds(Agent a, Klient kl, Kassa k, double summa)
        {
            KassOrder order = new KassOrder();
            order.OperationDate = DateTime.ParseExact(DateTime.Now.ToShortDateString(), "d.M.yyyy", CultureInfo.InvariantCulture);
            order.Number = 0;
            order.Date = DateTime.Now.ToShortDateString();
            order.Purpose = "КОМИССИЯ С ПЛАТЕЛЬЩИКА ОТ ПРИЕМА ПЛАТЕЖЕЙ В КАССЕ НОМЕР " + k.Np + "ПЛАТЕЛЬЩИК: " + kl.FIO;


            order.DebitAccountNumber = k.Sc;
            order.BatchBrief = k.Pachka;


            order.CreditAccountNumber = k.Doh;
            order.DebitAmount = summa.ToString().Replace(',', '.');
            order.CreditAmmount = summa.ToString().Replace(',', '.');
            order.ProfitAccountNumber = k.Doh;

            string[] fio = kl.FIO.Split(' ');

            if (fio.Length == 3)
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = fio[2];
            }
            else
            {
                order.PersonName = fio[1];
                order.PersonSurName = fio[0];
                order.PersonPatronymicName = " ";
            }
          

            order.DebitCurencyCode = "810";
            order.OperationCode = "4";
            order.StaffBrief = " ";
            order.NUMBER = 0;
            order.Klients_Ad = " "; // не знаю нужен или нет?????

            order.PayeeName = a.NameFull;
            order.KassSymbol = "32";

            return order;
        }

        #endregion

        #region Платежное поручение

        /// <summary>
        /// Создание платежного поручения
        /// </summary>
        /// <param name="s"></param>
        /// <param name="k"></param>
        /// <param name="kl"></param>
        /// <param name="summa"></param>
        /// <param name="drawStat"></param>
        /// <param name="purpose"></param>
        /// <param name="isAccount"></param>
        /// <param name="a"></param>
        /// <param name="taxPeriod"></param>
        /// <param name="taxReason"></param>
        /// <param name="payerInn"></param>
        /// <returns></returns>
        public static PlatPoruch CreatePlatPoruch(Agent s, Kassa k, Klient kl, double summa, int drawStat, string purpose,
            string taxPeriod = null, string taxReason = null, string payerInn = null)
        {
            PlatPoruch t = new PlatPoruch();

            t.Amount = summa;
            t.Bic = s.BIK;
            if (k.Np < 20000)
                t.Corsc = "30102810800030000123";
            else
                t.Corsc = "30101810845250000656";

            t.Debet = k.Tranzit_vnesh;
        
            t.Credit = s.RS;
            t.DrawStat = drawStat;
            t.Fio = kl.FIO;
            t.Inn = s.INN;
            t.Kbk = s.KBK;
            t.Kpp = s.KPP;
            t.NalogNumber = kl.TaxNumber;
            t.Okato = s.OKATO;
            t.Pachka = k.Pachka_vnesh;

            if (s.BIK == "043510123")
            {
                t.Pachka = k.Pachka_vnut;
            }
            else
            {
                t.Pachka = k.Pachka_vnesh;
            }
            
            t.PayeeeName = s.NameFull;
            t.Purpose = purpose;
            t.Sc_klient = s.RS;

            t.Tranzit = k.Tranzit_vnesh;

            

            t.Uin = 
            t.Adres = kl.Adres;
            t.TaxPeriod = taxPeriod;
            t.TaxReason = taxReason;
            t.PayerINN = payerInn;

            return t;
        }

        /// <summary>
        /// Создать платежное поручение ЕД 108
        /// </summary>
        /// <param name="a"></param>
        /// <param name="kl"></param>
        /// <param name="k"></param>
        /// <param name="summa"></param>
        /// <param name="drawStat"></param>
        /// <param name="purpose"></param>
        /// <param name="uin"></param>
        /// <returns></returns>
        public static PlatPoruch CreateEd108(Agent a, Klient kl, Kassa k , double summa , int drawStat , string purpose , string uin = null)
        {
            PlatPoruch poruch = new PlatPoruch();

            poruch.Amount = summa;
            poruch.Bic = a.BIK;
            poruch.Corsc = "30102810800030000123";

            poruch.Debet = k.Tranzit_vnesh;
            poruch.Credit = a.RS;

            poruch.DrawStat = drawStat;
            poruch.Fio = kl.FIO;
            poruch.Inn = a.INN;
            poruch.Kbk = a.KBK;
            poruch.Kpp = a.KPP;
            poruch.NalogNumber = kl.TaxNumber.Replace(" ", string.Empty).Trim();
            poruch.Okato = a.OKATO;

            poruch.Pachka = k.Pachka_vnesh;

            poruch.PayeeeName = a.NameFull;
            poruch.Purpose = purpose;

            poruch.Sc_klient = a.RS;
            poruch.Tranzit = k.Tranzit_vnesh;

            if (uin != null)
            {
                poruch.Uin = uin;
            }
            else
            {
                poruch.Uin = " ";
            }
           
            poruch.Adres = kl.Adres;


            poruch.TaxPeriod = null;
            poruch.TaxReason = null;
            poruch.PayerINN = null;

            return poruch;
        }

        public static PlatPoruch CreatEasyPoruch(Agent a, Klient kl, Kassa k, double summa , string purpose)
        {
            PlatPoruch poruch = new PlatPoruch();

            poruch.Amount = summa;
            poruch.Bic = a.BIK;
            poruch.Corsc = "30102810800030000123";

            poruch.Debet = k.Tranzit_vnesh;
            poruch.Credit = a.RS;

            poruch.DrawStat = 0;
            poruch.Fio = kl.FIO;
            poruch.Inn = a.INN;
            poruch.Kbk = a.KBK;
            poruch.Kpp = a.KPP;
            poruch.NalogNumber = " ";
            poruch.Okato = a.OKATO;

            if (a.BIK == "043510123")
            {
                poruch.Pachka = k.Pachka_vnut;
            }
            else
            {
                poruch.Pachka = k.Pachka_vnesh;
            }

            poruch.PayeeeName = a.NameFull;
            poruch.Purpose = purpose;

            poruch.Sc_klient = a.RS;
            poruch.Tranzit = k.Tranzit_vnesh;


            poruch.Adres = kl.Adres;


            poruch.TaxPeriod = null;
            poruch.TaxReason = null;
            poruch.PayerINN = null;

            return poruch;
        }
      
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Globalization;
using System.Data.Entity.Core.Objects;
using System.Data;
using System.Data.SqlClient;
using Common;
using GateWay.Log;

namespace GateWay.Diasoft
{ 
    public class DiasoftTranzaction
    {
        /// <summary>
        /// Кассовый ордер
        /// </summary>
        /// <param name="temp_order"></param>
        /// <param name="parrentDoc"></param>
        /// <param name="isBudjet"></param>
        /// <param name="connection"></param>
        /// <param name="np"></param>
        /// <param name="resident"></param>
        /// <param name="error"></param>
        /// <param name="personId"></param>
        /// <param name="documentId"></param>
        /// <returns></returns>
        public double SendDificalPayToDiasoft(KassOrder temp_order, double parrentDoc, bool isBudjet, SqlConnection connection, int np, int resident, out string error, double? personId = null, double? documentId = null)
        {
            error = null;
            CultureInfo inf = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = inf;
            inf.NumberFormat.NumberDecimalSeparator = ".";

            temp_order.DebitAmount = temp_order.DebitAmount.Replace(",", ".");
            temp_order.CreditAmmount = temp_order.CreditAmmount.Replace(",", ".");

            string commandString = "dbo.RCKrimea_CreatePayFizik_NEW";

            SqlParameter[] parametr = new SqlParameter[27];

            parametr[0] = new SqlParameter("@BatchBrief", temp_order.BatchBrief);
            parametr[1] = new SqlParameter("@Number", temp_order.Number);
            parametr[2] = new SqlParameter("@Purpose", temp_order.Purpose);

            parametr[3]= new SqlParameter( "@DebitAccountNumber", temp_order.DebitAccountNumber);
            parametr[4]= new SqlParameter("@CreditAccountNumber", temp_order.CreditAccountNumber);
            parametr[5]= new SqlParameter("@DebitAmount", temp_order.DebitAmount);
            parametr[6]= new SqlParameter("@CreditAmount", temp_order.CreditAmmount);
            parametr[7]= new SqlParameter("@ProfitAccountNumber", temp_order.ProfitAccountNumber);
            parametr[8]= new SqlParameter("@PersonName", temp_order.PersonName);
            parametr[9]= new SqlParameter("@PersonSurName", temp_order.PersonSurName);
            parametr[10]= new SqlParameter("@PersonPatronymicName", temp_order.PersonPatronymicName);

            parametr[11]= new SqlParameter("@DebitCurencyCode", temp_order.DebitCurencyCode);
            parametr[12]= new SqlParameter("@OperationCode", temp_order.OperationCode);
            parametr[13]= new SqlParameter("@StaffBrief", temp_order.StaffBrief);
            parametr[14]= new SqlParameter("@Klients_Ad", temp_order.Klients_Ad);
            parametr[15]= new SqlParameter("@PayeeName", temp_order.PayeeName);
            parametr[16]= new SqlParameter("@KassSymbol", temp_order.KassSymbol);
            parametr[17]= new SqlParameter("@ParentDoc", parrentDoc);

            if (personId != null && documentId != null)
            {
                parametr[18]= new SqlParameter("@KlientId", personId);
                parametr[19]= new SqlParameter("@ValueId", documentId);
            }
            else
            {
                parametr[18]= new SqlParameter("@KlientId", null);
                parametr[19]= new SqlParameter("@ValueId", null);
            }

            if (isBudjet)
            {
                parametr[20]= new SqlParameter("@payeeBic", temp_order.PayeeBankBIC);
                parametr[21]= new SqlParameter("@payeeINN", temp_order.PayeeINN);
                parametr[22]= new SqlParameter("@payAcount", temp_order.PayeeAcccountNumber);
                parametr[23]= new SqlParameter("@payeeKPP", temp_order.PayeeKPP);
            }
            else
            {
                parametr[20]= new SqlParameter("@payeeBic", null);
                parametr[21]= new SqlParameter("@payeeINN", null);
                parametr[22]= new SqlParameter("@payAcount", null);
                parametr[23]= new SqlParameter("@payeeKPP", null);
            }

            if (np > 20000)
            {
                parametr[24]= new SqlParameter("@balanceName", "ГЕНБАНК ф-л Москва");
                parametr[25]= new SqlParameter("@balanceId", 10001060444);
            }
            else
            {
                parametr[24]= new SqlParameter("@balanceName", null);
                parametr[25]= new SqlParameter("@balanceId", null);
            }
            parametr[26]= new SqlParameter("@resident", resident);

            AdoGenericModel model = new AdoGenericModel();

            DataTable t = model.Get(commandString, connection, parametr, 600);

            double orderId = 0;

            foreach (DataRow row in t.Rows)
            {
                orderId = Convert.ToDouble(row["CsOrdID"]);
            }

            if (orderId == 0)
            {
                string commandString2 = "SELECT * FROM dbo.pAPI_CsOrd_NotifyList with (nolock) WHERE SPID=@@SPID";
                DataTable errorTable = model.Get(commandString2, connection);

                if (errorTable != null)
                {
                    if (errorTable.Rows.Count != 0)
                    {
                        foreach (DataRow row in errorTable.Rows)
                            Logger.Loging(np, row["NTFMessage"].ToString(), " ");
                    }
                }
            }
            return orderId;
        }

        /// <summary>
        /// Платежное поручение в бюджет
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="np"></param>
        /// <param name="connection"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public double SendPlatOrderToDiasoft(PlatPoruch temp, int np, SqlConnection connection, out string error)
        {
            error = null;
            CultureInfo inf = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = inf;
            inf.NumberFormat.NumberDecimalSeparator = ".";

            string summa = temp.Amount.ToString();
            summa = summa.Replace(",", ".");
            temp.Amount = Convert.ToDouble(summa);

            double id = 0;

            string commandString = "dbo.RCKrimea_PayToBudjet_NEW";
            SqlParameter[] parametr = new SqlParameter[26];

            parametr[0]= new SqlParameter("@amount", temp.Amount);
            parametr[1]= new SqlParameter("@tranzit", temp.Tranzit);
            parametr[2]= new SqlParameter("@corsc", temp.Corsc);
            parametr[3]= new SqlParameter("@inn", temp.Inn);
            parametr[4]= new SqlParameter("@kpp", temp.Kpp);
            if (np > 20000)
            {
                parametr[5]= new SqlParameter("@name", "Филиал AО \"ГЕНБАНК\" в г.Москва//" + temp.Fio + "//" + temp.Adres + "//"); // поменять тут!!!!!!!!!!!!!!!
            }
            else
            {
                parametr[5]= new SqlParameter("@name", " AО \"ГЕНБАНК\"//" + temp.Fio + "//" + temp.Adres + "//"); // поменять тут!!!!!!!!!!!!!!!
            }

            parametr[6]= new SqlParameter("@bic", temp.Bic);
            parametr[7]= new SqlParameter("@purpose", temp.Purpose);
            parametr[8]= new SqlParameter("@kbk", temp.Kbk);
            parametr[9]= new SqlParameter("@okato", temp.Okato);
            parametr[10]= new SqlParameter("@debet", temp.Debet);
            parametr[11]= new SqlParameter("@credit", temp.Credit);
            parametr[12]= new SqlParameter("@sc_kli", temp.Sc_klient);
            parametr[13]= new SqlParameter("@drawStat", temp.DrawStat);
            parametr[14]= new SqlParameter("@pachka", temp.Pachka);
            parametr[15]= new SqlParameter("@payeeName", temp.PayeeeName);
            parametr[16]= new SqlParameter("@nalogNumber", temp.NalogNumber);
            parametr[17]= new SqlParameter("@uin", temp.Uin);
            parametr[18]= new SqlParameter("@data", DateTime.Now.ToShortDateString().Replace(".", string.Empty));


            if (np > 20000)
            {
                parametr[19]= new SqlParameter("@balanceName", "ГЕНБАНК ф-л Москва");
                parametr[20]= new SqlParameter("@balanceId", 10001060444);
                parametr[21]= new SqlParameter("@balanceBic", "044525656");
                parametr[22]= new SqlParameter("@balanceCorsc", "30102810300000000656");

            }
            else
            {
                parametr[19]= new SqlParameter("@balanceName", null);
                parametr[20]= new SqlParameter("@balanceId", null);
                parametr[21]= new SqlParameter("@balanceBic", null);
                parametr[22]= new SqlParameter("@balanceCorsc", null);
            }

            if (!string.IsNullOrWhiteSpace(temp.TaxReason))
            {
                parametr[23]= new SqlParameter("@taxPaymentReason", temp.TaxReason);
            }
            else
            {
                parametr[23]= new SqlParameter("@taxPaymentReason", null);
            }

            if (!string.IsNullOrWhiteSpace(temp.TaxPeriod))
            {
                parametr[24]= new SqlParameter("@taxPeriod", temp.TaxPeriod);
            }
            else
            {
                parametr[24]= new SqlParameter("@taxPeriod", null);
            }

            if (!string.IsNullOrWhiteSpace(temp.PayerINN))
            {
                parametr[25]= new SqlParameter("@klientInn", temp.PayerINN);
            }
            else
            {
                parametr[25]= new SqlParameter("@klientInn", null);
            }

            AdoGenericModel model = new AdoGenericModel();

            DataTable t = model.Get(commandString, connection, parametr, 600);

            foreach (DataRow row in t.Rows)
                id = Convert.ToDouble(row["id"]);

            if (id == 0)
            {
                string commandString2 = "SELECT * FROM dbo.pAPI_CsOrd_NotifyList with (nolock) WHERE SPID=@@SPID";
                DataTable errorTable = model.Get(commandString2, connection);

                if (errorTable != null)
                {
                    if (errorTable.Rows.Count != 0)
                    {
                        foreach (DataRow row in errorTable.Rows)
                            Logger.Loging(np, row["NTFMessage"].ToString(), " ");
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// Проводка платежного поручения не в бюджет
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="connection"></param>
        /// <param name="np"></param>
        /// <returns></returns>
        public  double SendPPNoBudjet(PlatPoruch temp, SqlConnection connection, int np, out string error)
        {
            error = null;
            CultureInfo inf = new CultureInfo(System.Threading.Thread.CurrentThread.CurrentCulture.Name);
            System.Threading.Thread.CurrentThread.CurrentCulture = inf;
            inf.NumberFormat.NumberDecimalSeparator = ".";

            double id = 0;
            string commandString = "dbo.Rckrime_CreatePPNoBudjet_NEW";

            SqlParameter[] parametr = new SqlParameter[17];

            parametr[0]= new SqlParameter("@amount", temp.Amount);
            parametr[1]= new SqlParameter("@tranzit", temp.Tranzit);
            parametr[2]= new SqlParameter("@corsc", temp.Corsc);
            parametr[3]= new SqlParameter("@inn", temp.Inn);
            parametr[4]= new SqlParameter("@kpp", temp.Kpp);
            if (np > 20000)
            {
                parametr[5]= new SqlParameter("@name", "Филиал AО \"ГЕНБАНК\" в г.Москва//" + temp.Fio + "//" + temp.Adres + "//"); // поменять тут!!!!!!!!!!!!!!!
            }
            else
            {
                parametr[5]= new SqlParameter("@name", " AО \"ГЕНБАНК\"//" + temp.Fio + "//" + temp.Adres + "//"); // поменять тут!!!!!!!!!!!!!!!
            }
            parametr[6]= new SqlParameter("@bic", temp.Bic);
            parametr[7]= new SqlParameter("@purpose", temp.Purpose);
            parametr[8]= new SqlParameter("@debet", temp.Debet);
            parametr[9]= new SqlParameter("@credit", temp.Credit);
            parametr[10]= new SqlParameter("@sc_kli", temp.Sc_klient);
            parametr[11]= new SqlParameter("@BatchBrief", temp.Pachka);
            parametr[12]= new SqlParameter("@payeeName", temp.PayeeeName);
            if (np > 20000)
            {
                parametr[13]= new SqlParameter("@balanceName", "ГЕНБАНК ф-л Москва");
                parametr[14]= new SqlParameter("@balanceId", 10001060444);
                parametr[15]= new SqlParameter("@balanceBic", "044525656");
                parametr[16]= new SqlParameter("@balanceCorsc", "30102810300000000656");

            }
            else
            {
                parametr[13]= new SqlParameter("@balanceName", null);
                parametr[14]= new SqlParameter("@balanceId", null);
                parametr[15]= new SqlParameter("@balanceBic", null);
                if (temp.Bic == "043510123")
                    parametr[16]= new SqlParameter("@balanceCorsc", temp.Sc_klient);
                else
                    parametr[16]= new SqlParameter("@balanceCorsc", null);
            }

            AdoGenericModel model = new AdoGenericModel();
            DataTable t = model.Get(commandString, connection, parametr, 600);

            foreach (DataRow row in t.Rows)
                id = Convert.ToDouble(row["id"]);

            if (id == 0)
            {
                string commandString2 = "SELECT * FROM dbo.pAPI_CsOrd_NotifyList with (nolock) WHERE SPID=@@SPID";
                DataTable errorTable = model.Get(commandString2, connection);

                if (errorTable != null)
                {
                    if (errorTable.Rows.Count != 0)
                    {
                        foreach (DataRow row in errorTable.Rows)
                            Logger.Loging(np, row["NTFMessage"].ToString(), " ");
                    }
                }
            }
            return id;
        }

        /// <summary>
        /// Платежное поручение ЕД 108
        /// </summary>
        /// <param name="temp"></param>
        /// <param name="connection"></param>
        /// <param name="np"></param>
        /// <param name="fls"></param>
        /// <param name="fio"></param>
        /// <param name="adres"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public  double SendPlatOrderEd108(PlatPoruch temp,SqlConnection connection, int np, string fls, string fio, string adres, string login, string password)
        {
            double id = 0;
            string commandString = "dbo.RCKrimea_PayToBudjet_NEW";

            SqlParameter[] parametr = new SqlParameter[29];

            parametr[0]= new SqlParameter("@amount", temp.Amount);
            parametr[1]= new SqlParameter("@tranzit", temp.Tranzit);
            parametr[2]= new SqlParameter("@corsc", temp.Corsc);
            parametr[3]= new SqlParameter("@inn", temp.Inn);
            parametr[4]= new SqlParameter("@kpp", temp.Kpp);
            if (np > 20000)
            {
                parametr[5]= new SqlParameter("@name", "Филиал AО \"ГЕНБАНК\" в г.Москва//" + temp.Fio + "//" + temp.Adres + "//"); // поменять тут!!!!!!!!!!!!!!!
            }
            else
            {
                parametr[5]= new SqlParameter("@name", " AО \"ГЕНБАНК\"//" + temp.Fio + "//" + temp.Adres + "//"); // поменять тут!!!!!!!!!!!!!!!
            }

            parametr[6]= new SqlParameter("@bic", temp.Bic);
            parametr[7]= new SqlParameter("@purpose", temp.Purpose);
            parametr[8]= new SqlParameter("@kbk", temp.Kbk);
            parametr[9]= new SqlParameter("@okato", temp.Okato);
            parametr[10]= new SqlParameter("@debet", temp.Debet);
            parametr[11]= new SqlParameter("@credit", temp.Credit);
            parametr[12]= new SqlParameter("@sc_kli", temp.Sc_klient);
            parametr[13]= new SqlParameter("@drawStat", 24);
            parametr[14]= new SqlParameter("@pachka", temp.Pachka);
            parametr[15]= new SqlParameter("@payeeName", temp.PayeeeName);
            parametr[16]= new SqlParameter("@nalogNumber", temp.NalogNumber);
            parametr[17]= new SqlParameter("@uin", temp.Uin);
            parametr[18]= new SqlParameter("@data", DateTime.Now.ToShortDateString().Replace(".", string.Empty));


            if (np > 20000)
            {
                parametr[19]= new SqlParameter("@balanceName", "ГЕНБАНК ф-л Москва");
                parametr[20]= new SqlParameter("@balanceId", 10001060444);
                parametr[21]= new SqlParameter("@balanceBic", "044525656");
                parametr[22]= new SqlParameter("@balanceCorsc", "30102810300000000656");

            }
            else
            {
                parametr[19]= new SqlParameter("@balanceName", null);
                parametr[20]= new SqlParameter("@balanceId", null);
                parametr[21]= new SqlParameter("@balanceBic", null);
                parametr[22]= new SqlParameter("@balanceCorsc", null);
            }

            if (!string.IsNullOrWhiteSpace(temp.TaxReason))
            {
                parametr[23]= new SqlParameter("@taxPaymentReason", temp.TaxReason);
            }
            else
            {
                parametr[23]= new SqlParameter("@taxPaymentReason", null);
            }

            if (!string.IsNullOrWhiteSpace(temp.TaxPeriod))
            {
                parametr[24]= new SqlParameter("@taxPeriod", temp.TaxPeriod);
            }
            else
            {
                parametr[24]= new SqlParameter("@taxPeriod", null);
            }

            if (!string.IsNullOrWhiteSpace(temp.PayerINN))
            {
                parametr[25]= new SqlParameter("@klientInn", temp.PayerINN);
            }
            else
            {
                parametr[25]= new SqlParameter("@klientInn", null);
            }
            parametr[26]= new SqlParameter("@fls", fls);
            parametr[27]= new SqlParameter("@fio", fio);
            parametr[28]= new SqlParameter("@adres", adres);

            AdoGenericModel model = new AdoGenericModel();
            DataTable t = model.Get(commandString, connection, parametr, 600);

            foreach (DataRow row in t.Rows)
                id = Convert.ToDouble(row["id"]);

            if (id == 0)
            {
                string commandString2 = "SELECT * FROM dbo.pAPI_CsOrd_NotifyList with (nolock) WHERE SPID=@@SPID";
                DataTable errorTable = model.Get(commandString2, connection);

                if (errorTable != null)
                {
                    if (errorTable.Rows.Count != 0)
                    {
                        foreach (DataRow row in errorTable.Rows)
                            Logger.Loging(np, row["NTFMessage"].ToString(), " ");
                    }
                }
            }
            return id;
        }

    }
}
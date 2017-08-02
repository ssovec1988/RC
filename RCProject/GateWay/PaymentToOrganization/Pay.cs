using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using GateWay.BaseInterfaces;
using GateWay.Entity;
using Common;

namespace GateWay.PaymentToOrganization
{
    public class Pay : IPay
    {
        public double Summa { get; set; }
        public double? SummaTax { get; set; }
        public string KassSymbol { get; set; }
        public string Purpose { get; set; }
        public string TaxPeriod { get; set; }
        public string TaxReason { get; set; }
        public string Uin { get; set; }
        public string TaxInfo { get; set; }
        public IAgent Agent { get; set; }
        public int DrawStat { get; set; }

        ModelInitializer initializer = new ModelInitializer();

        /// <summary>
        /// Валидация
        /// </summary>
        /// <param name="noBase"></param>
        /// <param name="isBudjet"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Validate(bool noBase, bool isBudjet, out string error)
        {
            error = null;
            if (Summa == 0)
            {
                error = "Введите сумму платежа";
                return false;
            }

            if (noBase)
                if (Summa >= 15000)
                {
                    error = "Для платежа свыше 15 000 рублей обязательна идентификация клиента в АБС Диасофт";
                    return false;
                }

            if (string.IsNullOrWhiteSpace(KassSymbol))
            {
                error = "Выберите кассовый символ";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Purpose))
            {
                error = "Введите назначение платежа";
                return false;
            }

            if (isBudjet)
            {
                if (string.IsNullOrWhiteSpace(TaxPeriod))
                {
                    error = "Выберите период платежа";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(TaxReason))
                {
                    error = "Выберите основание налогового платежа";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(Uin))
                {
                    error = "Введите UIN";
                    return false;
                }

                if (Uin != "0")
                {
                    int uinChecker = CheckUin(Uin, out error);
                    if (uinChecker == 1)
                        return false;
                }
            }
            return true;

        }

        /// <summary>
        /// Проверка Uin
        /// </summary>
        /// <param name="uin"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        private int CheckUin(string uin, out string error)
        {
            error = null;
            string commandString = "SELECT dbo.gen_CheckUIN('" + uin + "')";
            AdoGenericModel model = new AdoGenericModel();
            string connectionString = "Data Source=5nt;Network Library=DBMSSOCN;Initial Catalog=diaswork;User ID=rckrime;Password=123456789"; // строка подключения к серверу diasoft
            SqlConnection connecton = model.OpenConnections(connectionString);

            int result = 1;

            DataTable t = model.Get(commandString, connecton);

            foreach (DataRow row in t.Rows)
            {
                try
                {
                    result = Convert.ToInt32(row[0]);
                }
                catch
                {
                    result = 1;
                }
            }
            if (result == 1)
            {
                error = "Ошибка проверки UIN платежа,проверьте UIN,если в UIN 25 символов нажмите «ОК» и продолжайте его вводить";
            }
            return result;
        }

        /// <summary>
        /// Проверка ключа КБК для бюджетных платежей
        /// </summary>
        /// <returns></returns>
        private int IsTaxes()
        {
            int isTaxes = 0;
            char[] aKbk = Agent.KBK.ToCharArray();

            string kbk345 = aKbk[3].ToString() + aKbk[4].ToString() + aKbk[5].ToString();
            string kbk_last = aKbk[17].ToString() + aKbk[18].ToString() + aKbk[19].ToString();

            string[] kbkMassiv = new string[] { "110", "120", "130", "140", "160", "180", "011", "100", "000" };
            string[] kbk345Massiv = new string[] { "111", "113", "114", "117" };

            if (kbkMassiv.Contains(kbk_last))
            {
                if (kbk345Massiv.Contains(kbk345))
                {
                    isTaxes = 1;
                }
            }
            else
            {
                isTaxes = 0;
            }

            return isTaxes;
        }

        /// <summary>
        /// Расчет комиссии платежа
        /// </summary>
        /// <param name="isAppendTaxes"></param>
        /// <param name="isAccount"></param>
        /// <param name="isComunTaxes"></param>
        /// <param name="np"></param>
        public void CheckTax(bool isAppendTaxes, bool isAccount, bool isComunTaxes, int np)
        {
            if (Agent.Tax != 0)
            {
                double taxSumm = 0;
                taxSumm = (Summa / 100) * Agent.Tax;

                if (taxSumm < Agent.TaxMin)
                    taxSumm = Agent.TaxMin;

                if (taxSumm > Agent.TaxMax)
                    taxSumm = Agent.TaxMax;

                if (Agent.Tax != 0)
                {
                    SummaTax = taxSumm;
                    TaxInfo = "C этого платежа будет взыматься комиссия" + " " + "Сумма комиссии: " + taxSumm.ToString("C2", new CultureInfo("ru-RU"));
                }
                else
                {
                    TaxInfo = "С этого платежа не взимается комиссия";
                }

            }
            else
            {
                int isTaxes = 0;

                if (!string.IsNullOrWhiteSpace(Agent.KBK))
                {
                    char[] a = Agent.BIK.ToCharArray();
                    string key = a[6].ToString() + a[7].ToString() + a[8].ToString();

                    if (key == "000" || key == "001" || key == "002")
                    {
                        if (Agent.RS.StartsWith("40101"))
                        {
                            isTaxes = IsTaxes();
                        }
                        else
                        {
                            isTaxes = 1;
                        }
                    }

                }
                if (isTaxes == 0)
                {
                    TaxInfo = "С этого платежа не взимается комиссия";
                }
                else
                {
                    double summTaxes = 0;
                    double percent = 0;
                    double minComission = 0;
                    double maxCommission = 0;

                    if (!isAccount)
                    {
                        if ((Agent.BIK != "043510123"))
                        {
                            string commandString = "select * from dbo.FizKommission with (nolock)";

                            string con = string.Empty;

                            if (np > 20000)
                            {
                                con = "Data Source=" + "10.0.70.19,1433;" + "Network Library=DBMSSOCN" + ";Initial Catalog=IRCRUSSIA;User ID=sa;Password=Cheburawka1988";
                            }
                            else
                            {
                                con = "Data Source=" + "10.0.70.19,1433;" + "Network Library=DBMSSOCN" + ";Initial Catalog=IRCDICTIONARY;User ID=sa;Password=Cheburawka1988";
                            }
                            AdoGenericModel model = new AdoGenericModel();
                            SqlConnection connection = model.OpenConnections(con);

                            DataTable table = model.Get(commandString, connection);

                            foreach (DataRow row in table.Rows)
                            {
                                percent = Convert.ToDouble(row["PERCENT"]);
                                minComission = Convert.ToDouble(row["MinKommission"]);
                                maxCommission = Convert.ToDouble(row["MaxKomission"]);
                            }
                            connection.Close();
                        }
                        else
                        {
                            if (Agent.BIK == "043510123")
                            {
                                percent = 2;
                                minComission = 100;
                                maxCommission = 2000;
                            }

                            if (Agent.BIK == "044525656")
                            {
                                percent = 2;
                                minComission = 50;
                                maxCommission = 2000;
                            }
                        }

                        if (np == 20001 || np == 20006)
                        {
                            percent = 2;
                            minComission = 50;
                            maxCommission = 2000;
                        }

                        if (isComunTaxes)
                        {
                            percent = 1.5;
                            minComission = 30;
                            maxCommission = 1000;
                        }
                    }
                    else
                    {
                        if (Agent.BIK == "043510123" || Agent.BIK == "044525656")
                        {
                            if (Agent.BIK == "043510123")
                            {
                                percent = 0.5;
                                minComission = 50;
                                maxCommission = 1000;
                            }

                            if (Agent.BIK == "044525656")
                            {
                                percent = 0.5;
                                minComission = 25;
                                maxCommission = 1000;
                            }

                        }
                        else
                        {
                            if (np < 20000)
                            {
                                percent = 1;
                                minComission = 100;
                                maxCommission = 2000;
                            }
                            else
                            {
                                percent = 1;
                                minComission = 25;
                                maxCommission = 2000;
                            }

                        }
                    }

                    summTaxes = (Summa / 100) * percent;
                    if (summTaxes < minComission)
                        summTaxes = minComission;

                    if (summTaxes > maxCommission)
                        summTaxes = maxCommission;

                    summTaxes = Math.Round(summTaxes, 2);
                    SummaTax = summTaxes;
                    TaxInfo = "Сумма комиссии: " + summTaxes.ToString("C2", new CultureInfo("ru-RU"));

                }

            }
        }

        /// <summary>
        /// Получения налоговых периодов
        /// </summary>
        /// <returns></returns>
        public IQueryable<TaxPeriod> GetTaxPeriod()
        {
            EFGenericRepository<TaxPeriod> model = new EFGenericRepository<TaxPeriod>(initializer.Dictionary);
            return model.Get().AsQueryable();
        }

        /// <summary>
        /// Получение оснований налогового платежа
        /// </summary>
        /// <returns></returns>
        public IQueryable<TaxReason> GetTaxReason()
        {
            EFGenericRepository<TaxReason> model = new EFGenericRepository<TaxReason>(initializer.Dictionary);
            return model.Get().AsQueryable();
        }

        /// <summary>
        /// Получить список кассовых символов
        /// </summary>
        /// <returns></returns>
        public IQueryable<KassSymbol> GetKassSymbol()
        {
            EFGenericRepository<KassSymbol> model = new EFGenericRepository<KassSymbol>(initializer.Dictionary);
            return model.Get().AsQueryable();
        }

        /// <summary>
        /// Заполнить платеж из шаблона
        /// </summary>
        /// <param name="shablon"></param>
        public void PayFromShablon(BUDJET_SHABLON shablon)
        {
            Purpose = shablon.PURPOSE;
            Uin = shablon.UIN;
        }
    }
}
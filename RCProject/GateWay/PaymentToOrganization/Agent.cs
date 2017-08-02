using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GateWay.BaseInterfaces;
using GateWay.Entity;
using Common;
using System.Data.SqlClient;
using System.Data;

namespace GateWay.PaymentToOrganization
{
    public class Agent : IAgent
    {
        public string NameFull { get; set; }
        public string ServName { get; set; }
        public string RS { get; set; }
        public int Number { get; set; }
        public string Tranz { get; set; }
        public string KorSH { get; set; }
        public string INN { get; set; }
        public string KBK { get; set; }
        public string OKATO { get; set; }
        public string KPP { get; set; }
        public string BIK { get; set; }
        public string BankRub { get; set; }
        public string Region { get; set; }
        public int KodPol { get; set; }
        public double Tax { get; set; }
        public double TaxMin { get; set; }
        public double TaxMax { get; set; }
        public string DopNaz { get; set; }
        public string Contact { get; set; }
        public string Email { get; set; }
        public int ID_SERV { get; set; }
        public string Kass_symbol { get; set; }
        public double Summa { get; set; }

        ModelInitializer initializer = new ModelInitializer();

        /// <summary>
        /// Получить список агентов для филиала Москва
        /// </summary>
        /// <param name="parametr"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public IQueryable<IAgent> Get(string parametr, string flag = null)
        {
            List<Agent> returnedAgentList = new List<Agent>();

            int kass = Convert.ToInt32(parametr);

            ModelInitializer model = new ModelInitializer();
            EFGenericRepository<AgentRussia> agentBd = new EFGenericRepository<AgentRussia>(model.Dictionary);

            List<AgentRussia> listOfAgent = agentBd.Get(x => x.KassFlag == kass).ToList();

            foreach (AgentRussia temp in listOfAgent)
            {
                Agent agent = new Agent();
                agent.BIK = temp.Bic;
                agent.BankRub = GetBank(temp.Bic)[0];
                agent.INN = temp.Inn.ToString();
                agent.KBK = temp.Kbk;
                agent.KPP = temp.Kpp.ToString();
                agent.KorSH = temp.Ks;
                agent.NameFull = temp.Name;
                agent.OKATO = temp.Okato;
                agent.RS = temp.RS;
                agent.Tax = (double)temp.TaxPercent;
                agent.TaxMax = (double)temp.TaxMax;
                agent.TaxMin = (double)temp.TaxMin;
                returnedAgentList.Add(agent);
            }
            return returnedAgentList.AsQueryable();
        }

        /// <summary>
        /// Валидация агента
        /// </summary>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool Validate(out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(BIK))
            {
                error = "Не введен БИК получателя платежа";
                return false;
            }

            if (BIK.Length < 9)
            {
                error = "В БИК должно быть 9 цифр";
                return false;
            }

            if (string.IsNullOrWhiteSpace(BankRub))
            {
                error = "Поле БАНК не заполнено, для его заполнения нужно ввести БИК";
                return false;
            }

            if (string.IsNullOrWhiteSpace(INN))
            {
                error = "Не заполнено поле ИНН получателя";
                return false;
            }

            if (INN.Length < 10)
            {
                error = "В поле ИНН не может быть меньше десяти знаков";
                return false;
            }

            if (string.IsNullOrWhiteSpace(KPP))
            {
                error = "Не заполнено поле КПП";
                return false;
            }

            if (KPP.Length < 9)
            {
                if (KPP != "0")
                {
                    error = "В поле КПП не должно быть меньше 9 цифр, также поле КК может быть равно 0";
                    return false;
                }
            }

            if (string.IsNullOrWhiteSpace(RS))
            {
                error = "Не заполнено поле РАСЧЕТНЫЙ СЧЕТ получателя";
                return false;
            }

            if (RS.Length < 20)
            {
                error = "Поле РАСЧЕТНЫЙ СЧЕТ должно содержать двадцать цифр";
                return false;
            }

            bool accountCheck = AccountChecker();

            if (!accountCheck)
            {
                error = "Ошибка в ключевом разряде счета получателя";
                return false;
            }

            if (string.IsNullOrWhiteSpace(NameFull))
            {
                error = "Не введено название получателя";
                return false;
            }
            char[] a = BIK.ToCharArray();
            string key = a[6].ToString() + a[7].ToString() + a[8].ToString();

            if (key == "000" || key == "001" || key == "002")
            {
                if (string.IsNullOrWhiteSpace(KBK))
                {
                    error = "Не введено КБК получателя";
                    return false;
                }

                if (KBK.Length < 20)
                {
                    error = "Поле КБК должно содержать 20 символов";
                    return false;
                }

                if (string.IsNullOrWhiteSpace(OKATO))
                {
                    error = "Не введено поле ОКАТО";
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Получить банк получателя платежа
        /// </summary>
        /// <param name="bic"></param>
        /// <returns></returns>
        public string[] GetBank(string bic)
        {
            AdoGenericModel model = new AdoGenericModel();
            string commandString = "select * from dbo.Bnkseek with (nolock) WHERE _NEWNUM LIKE + '%' +'" + bic + "' + '%'";
            string connectionString = "Data Source=5nt;Network Library=DBMSSOCN;Initial Catalog=diaswork;User ID=rckrime;Password=123456789"; // строка подключения к серверу diasoft
            SqlConnection connecton = model.OpenConnections(connectionString);

            DataTable t = model.Get(commandString, connecton);
            string[] info = new string[2];

            foreach (DataRow row in t.Rows)
                info[0] = row["_NAMEP"].ToString();

            string commandString1 = "select AccRcv from dbo.tInstitution with (nolock) where BIC='" + info[0] + "'";
            SqlConnection connection2 = model.OpenConnections(connectionString);

            DataTable t2 = model.Get(commandString1, connection2);

            foreach (DataRow row in t2.Rows)
                info[1] = row["AccRcv"].ToString();

            return info;
        }

        /// <summary>
        /// Проверка ключевого разряда счета
        /// </summary>
        /// <returns></returns>
        private bool AccountChecker()
        {
            bool isTrue = false;
            SqlParameter[] parametr = new SqlParameter[2];
            parametr[0] = new SqlParameter("@account", SqlDbType.VarChar, 20);
            parametr[1] = new SqlParameter("@bic", SqlDbType.VarChar, 9);

            parametr[0].Value = RS;
            parametr[1].Value = BIK;

            AdoGenericModel model = new AdoGenericModel();
            string connectionString = "Data Source=5nt;Network Library=DBMSSOCN;Initial Catalog=diaswork;User ID=rckrime;Password=123456789"; // строка подключения к серверу diasoft
            SqlConnection connecton = model.OpenConnections(connectionString);

            DataTable t = model.Get("dbo.RCKrime_AccountChecker", connecton, parametr);

            foreach (DataRow row in t.Rows)
            {
                int request = Convert.ToInt32(row["info"]);
                if (request == 1)
                    isTrue = true;
            }
            return isTrue;

        }

        /// <summary>
        /// Получить список шаблонов клиента
        /// </summary>
        /// <param name="personId"></param>
        /// <returns></returns>
        public IQueryable<BUDJET_SHABLON> GetShablonList(double personId)
        {
            EFGenericRepository<BUDJET_SHABLON> model = new EFGenericRepository<BUDJET_SHABLON>(initializer.Fiziki);
            return model.Get(x => x.PERSONID == personId).AsQueryable();
        }

        /// <summary>
        /// Заполнить агента из шаблона
        /// </summary>
        /// <param name="shablon"></param>
        public void AgentFromShablon(BUDJET_SHABLON shablon)
        {
            RS = shablon.RS;
            BIK = shablon.BIC;
            BankRub = shablon.BANK;
            INN = shablon.INN;
            NameFull = shablon.AGENT;
            OKATO = shablon.OKATO;
            KBK = shablon.KBK;
            KPP = shablon.KPP;
        }
    }
}
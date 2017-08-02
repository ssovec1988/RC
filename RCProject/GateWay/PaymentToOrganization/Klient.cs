using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using GateWay.Entity;
using GateWay.BaseInterfaces;
using Common;

namespace GateWay.PaymentToOrganization
{
    public class Klient : IKlient
    {
        public string FIO { get; set; }
        public string Adres { get; set; }
        public double? DiasoftNumber { get; set; }
        public double? DiasoftDocNumber { get; set; }
        public int DocType { get; set; }
        public string NumberDoc { get; set; }
        public string SeriyaDoc { get; set; }
        public string TaxNumber { get; set; }
        public string Resident { get; set; }
        public string Document { get; set; }
        public string DocumentId { get; set; }
        public string DocumentDate { get; set; }
        public string PayerInn { get; set; }
        public string KemVidan { get; set; }

        ModelInitializer initializer = new ModelInitializer();

        /// <summary>
        /// Получисть список клиентов из БД Diasoft
        /// </summary>
        /// <param name="parametr">ФИО клиета</param>
        /// <param name="login">login в Diasoft</param>
        /// <param name="password">password в Diasoft</param>
        /// <returns></returns>
        public IQueryable<IKlient> Get(string parametr, string login = null, string password = null)
        {
            AdoGenericModel model = new AdoGenericModel();
            string connectionString = "Data Source=5nt;Network Library=DBMSSOCN;Initial Catalog=diaswork;User ID=" + login + ";Password=" + password; // строка подключения к серверу diasoft
            SqlConnection connecton = model.OpenConnections(connectionString);

            string lastName = null;
            string firstName = null;
            string middleName = null;

            string[] tempArray = parametr.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (tempArray.Length < 2)
                return null;

            List<string> clearArray = new List<string>();

            foreach (string temp in tempArray)
            {
                if (string.IsNullOrWhiteSpace(temp))
                    continue;
                clearArray.Add(temp);
            }

            for (int i = 0; i < clearArray.Count; i++)
            {
                switch (i)
                {
                    case 0:
                        lastName = clearArray[i];
                        break;
                    case 1:
                        firstName = clearArray[i];
                        break;
                    case 2:
                        middleName = clearArray[i];
                        break;
                }
            }

            SqlParameter[] parametrAdo = new SqlParameter[5];

            parametrAdo[0] = new SqlParameter("@surName", SqlDbType.VarChar, 50);
            parametrAdo[0].Value = lastName;

            parametrAdo[1] = new SqlParameter("@name", SqlDbType.VarChar, 50);
            parametrAdo[1].Value = firstName;

            parametrAdo[2] = new SqlParameter("@patronymicName", SqlDbType.VarChar, 50);
            parametrAdo[2].Value = middleName;

            parametrAdo[3] = new SqlParameter("@regSeries", SqlDbType.VarChar, 50);
            parametrAdo[3].Value = null;

            parametrAdo[4] = new SqlParameter("@regNumber", SqlDbType.VarChar, 50);
            parametrAdo[4].Value = null;

            DataTable t = model.Get("dbo.RCKrimea_GetKlientsFromDiasoft", connecton, parametrAdo);

            List<Klient> returnedKlientList = new List<Klient>();
            if (t != null)
            {
                foreach (DataRow row in t.Rows)
                {
                    Klient infoTemp = new Klient();
                    infoTemp.DiasoftNumber = Convert.ToDouble(row["PersonID"]);
                    infoTemp.DiasoftDocNumber = Convert.ToDouble(row["ICardID"]);

                    infoTemp.Adres = row["Name"].ToString();

                    infoTemp.FIO = row["fio"].ToString();
                    infoTemp.NumberDoc = row["RegNumber"].ToString();
                    infoTemp.SeriyaDoc = row["RegSeries"].ToString();
                    infoTemp.Resident = row["Resident"].ToString();

                    string docBrief = row["DocType"].ToString();

                    string commandString5 = string.Empty;
                    commandString5 = "select Name,Brief from dbo.tDocType with (nolock) where Brief='" + docBrief + "'";


                    SqlConnection connecton1 = model.OpenConnections(connectionString);
                    DataTable table5 = model.Get(commandString5, connecton1);
                    string docType = string.Empty;
                    string brief = string.Empty;

                    foreach (DataRow row5 in table5.Rows)
                    {
                        docType = row5["Name"].ToString();
                        brief = row5["Brief"].ToString();
                    }
                    brief = brief.Trim();
                    switch (brief)
                    {
                        case "21":
                            infoTemp.DocumentId = "01";
                            break;
                        case "27":
                        case "08":
                            infoTemp.DocumentId = "02";
                            break;
                        case "04":
                        case "24":
                            infoTemp.DocumentId = "04";
                            break;
                        case "15":
                        case "07":
                            infoTemp.DocumentId = "05";
                            break;
                        case "14":
                        case "26":
                        case "25":
                            infoTemp.DocumentId = "06";
                            break;
                        case "05":
                            infoTemp.DocumentId = "07";
                            break;
                        case "10":
                        case "Виза":
                        case "31":
                            infoTemp.DocumentId = "08";
                            break;
                        case "12":
                            infoTemp.DocumentId = "09";
                            break;
                        case "34":
                            infoTemp.DocumentId = "10";
                            break;
                        case "13":
                        case "37":
                        case "38":
                            infoTemp.DocumentId = "11";
                            break;
                        case "39":
                            infoTemp.DocumentId = "12";
                            break;
                        case "200":
                            infoTemp.DocumentId = "14";
                            break;
                            /* default:
                                 infoTemp.DocumentId = "01";
                                 break;*/
                    }


                    infoTemp.Document = docType + " " + row["Document"].ToString();
                    infoTemp.TaxNumber = infoTemp.DocumentId + ";" + infoTemp.SeriyaDoc.Replace(" ", string.Empty).Trim() + infoTemp.NumberDoc;
                    returnedKlientList.Add(infoTemp);
                }
            }
            return returnedKlientList.AsQueryable();
        }

        public bool Validate(out string error)
        {
            error = null;

            if (string.IsNullOrWhiteSpace(FIO))
            {
                error = "Введите ФИО клиента";
                return false;
            }

            if (string.IsNullOrWhiteSpace(Adres))
            {
                error = "Требуется ввести адрес клиента";
                return false;
            }

            return true;
        }

        public bool ValidateBudjet(int drawStat, out string error)
        {
            error = null;
            if (string.IsNullOrWhiteSpace(NumberDoc))
            {
                error = "Требуется ввести номер документа";
                return false;
            }


            if (string.IsNullOrWhiteSpace(Document))
            {
                error = "Требуется ввести тип докмента";
                return false;
            }

            if (string.IsNullOrWhiteSpace(SeriyaDoc) && NumberDoc != "14")
            {
                error = "Требуется ввести серию документа";
                return false;
            }

            if (string.IsNullOrWhiteSpace(KemVidan))
            {
                error = "Требуется ввести кем выдан докумет";
                return false;
            }

            if (drawStat == 13)
            {
                if (string.IsNullOrWhiteSpace(PayerInn))
                {
                    error = "Введите ИНН отправителя платежа";
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Проверка правильности заполнения анкеты клиента в разрезе резидентности
        /// </summary>
        /// <param name="k"></param>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <param name="error"></param>
        /// <returns></returns>
        public bool ResidentChecker(Klient k, string login, string password, out string error)
        {
            error = null;
            decimal[] russianDocs = new decimal[] { 24, 23, 21, 20, 19, 13, 12, 11, 10, 9, 5, 4, 10000000001, 10000000024, 10000000008, 10000000009, 10000000010, 10000000011, 10000000025, 10000000027, 10000000030, 10000000015, 17, 10000000032 };

            if (k.Resident == "1")
            {
                string[] fio = k.FIO.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                string commandString = string.Empty;

                if (fio.Length == 3)
                {
                    commandString = string.Format("EXEC  API_Prs_FindListByFIO @SurName ='{0}', @Name ='{1}', @PatronymicName ='{2}' select * from pAPI_Prs_ICard document WITH (NOLOCK) where SPID=@@SPID and PersonID={3}", fio[0], fio[1], fio[2], k.DiasoftNumber);
                }
                else
                {
                    commandString = string.Format("EXEC  API_Prs_FindListByFIO @SurName ='{0}', @Name ='{1}' select * from pAPI_Prs_ICard document WITH (NOLOCK) where SPID=@@SPID and PersonID={2}", fio[0], fio[1], k.DiasoftNumber);
                }

                AdoGenericModel model = new AdoGenericModel();
                string connectionString = "Data Source=5nt;Network Library=DBMSSOCN;Initial Catalog=diaswork;User ID=" + login + ";Password=" + password; // строка подключения к серверу diasoft
                SqlConnection connecton = model.OpenConnections(connectionString);

                DataTable t = model.Get(commandString, connecton);

                decimal[] klientsDoc = new decimal[t.Rows.Count];

                int count = 0;
                foreach (DataRow row in t.Rows)
                {
                    klientsDoc[count] = Convert.ToDecimal(row["DocTypeID"]);
                    count++;
                }

                bool isResident = false;

                foreach (decimal docs in klientsDoc)
                {
                    if (russianDocs.Contains(docs))
                        isResident = true;
                }

                if (!isResident)
                {
                    error = "У данного клиента отсутствует вид на жительство, проверьте корректность установки признака «Резидент» в АБС Diasoft";
                    return false;
                }
                else
                {
                    return true;
                }


            }
            else
            {
                return true;
            }


        }

        /// <summary>
        /// Получить список документов
        /// </summary>
        /// <returns></returns>
        public IQueryable<DocumentDictionary> GetDocumentList()
        {
            EFGenericRepository<DocumentDictionary> model = new EFGenericRepository<DocumentDictionary>(initializer.Dictionary);
            return model.Get().AsQueryable();
        }
    }
}
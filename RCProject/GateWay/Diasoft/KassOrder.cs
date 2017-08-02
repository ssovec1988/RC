using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GateWay.Diasoft
{
    public class KassOrder
    {
        private string batchBrief, date, purpose, debetAccountNumber, creditAccountNumber, debitAmount, creditAmount, profitAccountNumber, personName, personSurName, personPatronymicName,
      payeeInn, payeeKPP, payeeBancBic, payeeAcountNumber, payeeName, payeeOkatoValue, payeeBankName, debitCurrencyCode, operationCode, staffbrief, klients_ad, payName, packet_ident, kassSymbol;
        private int number_prov, number;
        private DateTime operationDate;

        /// <summary>
        /// Операционная дата документа
        /// </summary>
        public DateTime OperationDate
        {
            get { return operationDate; }
            set { operationDate = value; }
        }

        /// <summary>
        /// Идентификатор пачки документа
        /// </summary>
        public string BatchBrief
        {
            get { return batchBrief; }
            set { batchBrief = value; }
        }

        /// <summary>
        /// Номер
        /// </summary>
        public int Number
        {
            get { return number; }
            set { number = value; }
        }

        /// <summary>
        /// Дата состовления документа
        /// </summary>
        public string Date
        {
            get { return date; }
            set { date = value; }
        }

        /// <summary>
        /// Назначение платежа
        /// </summary>
        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }

        /// <summary>
        /// Счет по дебету
        /// </summary>
        public string CreditAccountNumber
        {
            get { return creditAccountNumber; }
            set { creditAccountNumber = value; }
        }

        /// <summary>
        /// Счет по кредету
        /// </summary>
        public string DebitAccountNumber
        {
            get { return debetAccountNumber; }
            set { debetAccountNumber = value; }
        }

        /// <summary>
        /// Сумма по дебиту
        /// </summary>
        public string DebitAmount
        {
            get { return debitAmount; }
            set { debitAmount = value; }
        }

        /// <summary>
        /// Сумма по кредиту
        /// </summary>
        public string CreditAmmount
        {
            get { return creditAmount; }
            set { creditAmount = value; }
        }

        /// <summary>
        /// Счет доходов
        /// </summary>
        public string ProfitAccountNumber
        {
            get { return profitAccountNumber; }
            set { profitAccountNumber = value; }
        }

        /// <summary>
        /// Имя
        /// </summary>
        public string PersonName
        {
            get { return personName; }
            set { personName = value; }
        }

        /// <summary>
        /// Фамилия
        /// </summary>
        public string PersonSurName
        {
            get { return personSurName; }
            set { personSurName = value; }
        }

        /// <summary>
        /// Отчество
        /// </summary>
        public string PersonPatronymicName
        {
            get { return personPatronymicName; }
            set { personPatronymicName = value; }
        }

        /// <summary>
        /// ИНН получателя
        /// </summary>
        public string PayeeINN
        {
            get { return payeeInn; }
            set { payeeInn = value; }
        }

        /// <summary>
        /// КПП получателя
        /// </summary>
        public string PayeeKPP
        {
            get { return payeeKPP; }
            set { payeeKPP = value; }
        }

        /// <summary>
        /// БИК банка получателя
        /// </summary>
        public string PayeeBankBIC
        {
            get { return payeeBancBic; }
            set { payeeBancBic = value; }
        }

        /// <summary>
        /// Счет получателя
        /// </summary>
        public string PayeeAcccountNumber
        {
            get { return payeeAcountNumber; }
            set { payeeAcountNumber = value; }
        }

        /// <summary>
        /// Наиминование получателя
        /// </summary>
        public string PayeeName
        {
            get { return payeeName; }
            set { payeeName = value; }
        }

        /// <summary>
        /// ОКАТО
        /// </summary>
        public string PayeeOkatoValue
        {
            get { return payeeOkatoValue; }
            set { payeeOkatoValue = value; }
        }

        /// <summary>
        /// Наиминование банка получателя
        /// </summary>
        public string PayeeBankName
        {
            get { return payeeBankName; }
            set { payeeBankName = value; }
        }

        /// <summary>
        /// цифровой код валюты по дебету
        /// </summary>
        public string DebitCurencyCode
        {
            get { return debitCurrencyCode; }
            set { debitCurrencyCode = value; }
        }

        /// <summary>
        /// вид операции
        /// </summary>
        public string OperationCode
        {
            get { return operationCode; }
            set { operationCode = value; }
        }

        /// <summary>
        /// Инициалы сотрудника добавившего кассовый ордер
        /// </summary>
        public string StaffBrief
        {
            get { return staffbrief; }
            set { staffbrief = value; }
        }

        public int NUMBER
        {
            get { return number_prov; }
            set { number_prov = value; }
        }

        public string Klients_Ad
        {
            get { return klients_ad; }
            set { klients_ad = value; }
        }


        public string KassSymbol
        {
            get { return kassSymbol; }
            set { kassSymbol = value; }
        }

        public string Packet_ident
        {
            get { return packet_ident; }
            set { packet_ident = value; }
        }

        public double PersonId { get; set; }
        public double DocumentId { get; set;}

        public int Agent { get; set; }

    }
}
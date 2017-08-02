using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GateWay.Diasoft
{ 
    public class PlatPoruch
    {
        private double amount;
        private string tranzit, corsc, inn, kpp, name, bic, purpose, kbk, okato, debet, credit, sc_cli, pachka, payeeName, nalogNumber, uin, adres, taxReason, taxPeriod, payerINN;
        private int draw_stat;

        /// <summary>
        /// Сумма платежа
        /// </summary>
        public double Amount
        {
            get { return amount; }
            set { amount = value; }
        }

        /// <summary>
        /// Транзитный счет отделения
        /// </summary>
        public string Tranzit
        {
            get { return tranzit; }
            set { tranzit = value; }
        }

        /// <summary>
        /// Корреспонденский счет банка
        /// </summary>
        public string Corsc
        {
            get { return corsc; }
            set { corsc = value; }
        }

        /// <summary>
        /// Инн получателя платежа
        /// </summary>
        public string Inn
        {
            get { return inn; }
            set { inn = value; }
        }

        /// <summary>
        /// Кпп получателя платежа
        /// </summary>
        public string Kpp
        {
            get { return kpp; }
            set { kpp = value; }
        }

        /// <summary>
        /// Фио получателя платежа
        /// </summary>
        public string Fio
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// Бик получателя платежа
        /// </summary>
        public string Bic
        {
            get { return bic; }
            set { bic = value; }
        }

        /// <summary>
        /// Назначение получателя платежа
        /// </summary>
        public string Purpose
        {
            get { return purpose; }
            set { purpose = value; }
        }

        /// <summary>
        /// Кбк получателя платежа
        /// </summary>
        public string Kbk
        {
            get { return kbk; }
            set { kbk = value; }
        }

        /// <summary>
        /// Окато получателя платежа
        /// </summary>
        public string Okato
        {
            get { return okato; }
            set { okato = value; }
        }

        /// <summary>
        /// Дебет
        /// </summary>
        public string Debet
        {
            get { return debet; }
            set { debet = value; }
        }

        /// <summary>
        /// Кредит
        /// </summary>
        public string Credit
        {
            get { return credit; }
            set { credit = value; }
        }

        /// <summary>
        /// Счет клиента
        /// </summary>
        public string Sc_klient
        {
            get { return sc_cli; }
            set { sc_cli = value; }
        }

        /// <summary>
        /// Статус отправителя платежа
        /// </summary>
        public int DrawStat
        {
            get { return draw_stat; }
            set { draw_stat = value; }
        }

        /// <summary>
        /// Пачка внешних ручных документов отделения
        /// </summary>
        public string Pachka
        {
            get { return pachka; }
            set { pachka = value; }
        }

        /// <summary>
        /// Полное наиминование плательщика
        /// </summary>
        public string PayeeeName
        {
            get { return payeeName; }
            set { payeeName = value; }
        }

        /// <summary>
        /// Налоговый номер документа
        /// </summary>
        public string NalogNumber
        {
            get { return nalogNumber; }
            set { nalogNumber = value; }
        }

        /// <summary>
        /// Уникальный идентификатор платежа
        /// </summary>
        public string Uin
        {
            get { return uin; }
            set { uin = value; }
        }

        public string Adres
        {
            get { return adres; }
            set { adres = value; }
        }

        public string TaxReason { get; set; }
        public string TaxPeriod { get; set; }
        public string PayerINN { get; set; }
    }
}
﻿//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace GateWay.Entity
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class IRCDICTIONARYEntities : DbContext
    {
        public IRCDICTIONARYEntities()
            : base("name=IRCDICTIONARYEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<AGENT> AGENT { get; set; }
        public virtual DbSet<AgentRussia> AgentRussia { get; set; }
        public virtual DbSet<DocumentDictionary> DocumentDictionary { get; set; }
        public virtual DbSet<KASSESDB_> KASSESDB_ { get; set; }
        public virtual DbSet<KassSymbol> KassSymbol { get; set; }
        public virtual DbSet<KLIENTS> KLIENTS { get; set; }
        public virtual DbSet<KODPL> KODPL { get; set; }
        public virtual DbSet<PACHKA> PACHKA { get; set; }
        public virtual DbSet<SpecSymbol> SpecSymbol { get; set; }
        public virtual DbSet<TaxPeriod> TaxPeriod { get; set; }
        public virtual DbSet<TaxReason> TaxReason { get; set; }
        public virtual DbSet<RosmedProduct> RosmedProduct { get; set; }
    }
}

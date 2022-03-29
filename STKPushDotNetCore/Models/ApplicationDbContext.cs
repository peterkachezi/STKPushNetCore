using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace STKPushDotNetCore.Models
{
    public class ApplicationDbContext :DbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {

        }


        public virtual DbSet<CheckoutRequest> CheckoutRequests { get; set; }
        public virtual DbSet<MpesaPayment> MpesaPayments { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);


            modelBuilder.Entity<MpesaPayment>(entity =>
            {
                entity.Property(e => e.Amount).HasColumnType("decimal(18,4)");
                entity.Property(e => e.Balance).HasColumnType("decimal(18,4)");

            });

      


            //modelBuilder.Entity<Commission>(entity =>
            //{
            //    entity.Property(e => e.CommissionAmount).HasColumnType("decimal(18,2)");
            //    entity.HasKey(e => e.Id);
            //    entity.Property(e => e.Id).HasDefaultValueSql("NEWID()");

            //});

    

        }


    }
}

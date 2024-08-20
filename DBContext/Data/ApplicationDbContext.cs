﻿using DAL.Models;
using DAL.Models.Domain.Damage;
using DAL.Models.Domain.MasterSetup;
using DAL.Models.Domain.SocialMobilization;
using DAL.Models.Domain.SocialMobilization.Training;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IFRAPMIS.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<ApplicationUser> AppUsers { get; set; }
        public DbSet<BeneficiaryIP> BeneficiaryIPs { get; set; }
        public DbSet<BeneficiaryPDMA> BeneficiaryPDMAs { get; set; }
        public DbSet<BeneficiaryVerified> BeneficiaryVerifieds { get; set; }
        public DbSet<CICIG> CICIGs { get; set; }
        public DbSet<CICIGTrainings> CICIGTrainings { get; set; }
        public DbSet<CIMember> CIMembers { get; set; }
        public DbSet<CITrainingMember> CITrainingMembers { get; set; }
        public DbSet<CITrainingParticipation> CITrainingParticipations { get; set; }
        public DbSet<TrainingHead> TrainingHeads { get; set; }
        public DbSet<DamageAssessmentHTS> DamageAssessmentHTSs { get; set; }
        public DbSet<DamageAssessmentLivestock> DamageAssessmentLivestocks { get; set; }
        public DbSet<DamageIP> DamageIPs { get; set; }
        public DbSet<DamagePDMA> DamagePDMAs { get; set; }
        public DbSet<DamageVerified> DamageVerifieds { get; set; }
        public DbSet<District> Districts { get; set; }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Provience> Proviences { get; set; }
        public DbSet<Tehsil> Teshsils { get; set; }
        public DbSet<UnionCouncil> UnionCouncils { get; set; }
        public DbSet<Village> Villages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure the foreign key for Village in CICIGTrainings
            modelBuilder.Entity<CICIGTrainings>()
                .HasOne(ct => ct.Village)
                .WithMany(v => v.CICIGTrainings)
                .HasForeignKey(ct => ct.VillageId)
                .OnDelete(DeleteBehavior.NoAction);

            // Configure the foreign key for CICIG in CICIGTrainings
            //modelBuilder.Entity<CICIGTrainings>()
            //    .HasOne(ct => ct.CICIG)
            //    .WithMany()
            //    .HasForeignKey(ct => ct.CICIGId)
            //    .OnDelete(DeleteBehavior.NoAction);




            //// Configure the foreign key for CIMember in BeneficiaryVerified
            //modelBuilder.Entity<BeneficiaryVerified>()
            //    .HasOne(bv => bv.CIMember)
            //    .WithOne(cm => cm.BeneficiaryVerified)
            //    .HasForeignKey<BeneficiaryVerified>(bv => bv.CIMemberId)
            //    .OnDelete(DeleteBehavior.NoAction);




            // Configure the foreign key for CICIG in BeneficiaryVerified
            //modelBuilder.Entity<BeneficiaryVerified>()
            //    .HasOne(bv => bv.CICIG)
            //    .WithMany()
            //    .HasForeignKey(bv => bv.CICIGId)
            //    .OnDelete(DeleteBehavior.NoAction);

            // Configure the foreign key for CICIG in CIMember
            //modelBuilder.Entity<CIMember>()
            //    .HasOne(cm => cm.CICIG)
            //    .WithMany()
            //    .HasForeignKey(cm => cm.CICIGId)
            //    .OnDelete(DeleteBehavior.NoAction);

            //modelBuilder.Entity<CITrainingParticipation>()
            //   .HasOne(bv => bv.CICIGTrainings)
            //   .WithOne(cm => cm.CITrainingParticipation)
            //   .HasForeignKey<CITrainingParticipation>(bv => bv.CICIGTrainings)
            //   .OnDelete(DeleteBehavior.NoAction);
            // Configure the foreign key for CICIGTrainings in CITrainingParticipation


            //modelBuilder.Entity<CITrainingParticipation>()
            //    .HasOne(tp => tp.CICIGTrainings)
            //    .WithOne(ct => ct.CITrainingParticipation)
            //    .HasForeignKey<CITrainingParticipation>(tp => tp.CICIGTrainingsId)
            //    .OnDelete(DeleteBehavior.NoAction);
        }

    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Nrc9.Data.Models;

public partial class NrcContext : DbContext
{
    public NrcContext(DbContextOptions<NrcContext> options)
        : base(options)
    {
        string projectPath = AppDomain.CurrentDomain.BaseDirectory;
        IConfigurationRoot configuration =
            new ConfigurationBuilder()
                .SetBasePath(projectPath)
        .AddJsonFile(MyConstants.AppSettingsFile)
        .Build();
        Database.SetCommandTimeout(9000);
        MyConnectionString =
            configuration.GetConnectionString(MyConstants.ConnectionString);
    }

    public string MyConnectionString { get; set; }

    public virtual DbSet<AthenaInputFileForThisNrcRunMaster> AthenaInputFileForThisNrcRunMasters { get; set; }
    public virtual DbSet<qy_GetNrcConfigOutputColumns> qy_GetNrcConfigOutputColumnsList { get; set; }
    public virtual DbSet<dd_InputFileOutputColumns> dd_InputFileOutputColumnsList { get; set; }
    public virtual DbSet<di_FinalizeInputFileOutputColumns> di_FinalizeInputFileOutputColumnsList { get; set; }
    public virtual DbSet<qy_GetOutputFileLinesOutputColumns> qy_GetOutputFileLinesOutputColumnsList { get; set; }
    public virtual DbSet<dd_ExtraSurveyQuestionWorkDaysOutputColumns> dd_ExtraSurveyQuestionWorkDaysOutputColumnsList { get; set; }
    public virtual DbSet<di_ExtraSurveyQuestionWorkDaysOutputColumns> di_ExtraSurveyQuestionWorkDaysOutputColumnsList { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<di_ExtraSurveyQuestionWorkDaysOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<dd_ExtraSurveyQuestionWorkDaysOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<qy_GetOutputFileLinesOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<di_FinalizeInputFileOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<dd_InputFileOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<qy_GetNrcConfigOutputColumns>(entity =>
        {
            entity.HasNoKey();
        });

        modelBuilder.Entity<AthenaInputFileForThisNrcRunMaster>(entity =>
        {
            entity.ToTable("AthenaInputFileForThisNrcRunMaster", "nrc");

            entity.Property(e => e.AthenaInputFileForThisNrcRunMasterId).HasColumnName("AthenaInputFileForThisNrcRunMasterID");
            entity.Property(e => e.ApptCheckOutDate).HasColumnType("datetime");
            entity.Property(e => e.ApptDate).HasColumnType("datetime");
            entity.Property(e => e.ApptId).HasMaxLength(30);
            entity.Property(e => e.CurrDeptBillName).HasMaxLength(300);
            entity.Property(e => e.CurrDeptNpiNo).HasMaxLength(30);
            entity.Property(e => e.Ethnicity).HasMaxLength(100);
            entity.Property(e => e.FullFileName)
                .HasMaxLength(300)
                .IsUnicode(false);
            entity.Property(e => e.GuarantorEmail).HasMaxLength(300);
            entity.Property(e => e.GuarantorPhone).HasMaxLength(30);
            entity.Property(e => e.ImportTimestamp).HasColumnType("datetime");
            entity.Property(e => e.PatientAddress1).HasMaxLength(300);
            entity.Property(e => e.PatientCity).HasMaxLength(300);
            entity.Property(e => e.PatientDob).HasColumnType("datetime");
            entity.Property(e => e.PatientEmail).HasMaxLength(300);
            entity.Property(e => e.PatientFirstName).HasMaxLength(300);
            entity.Property(e => e.PatientHomePhone).HasMaxLength(20);
            entity.Property(e => e.PatientId).HasMaxLength(20);
            entity.Property(e => e.PatientLang).HasMaxLength(100);
            entity.Property(e => e.PatientLastName).HasMaxLength(300);
            entity.Property(e => e.PatientMaritalStatus).HasMaxLength(100);
            entity.Property(e => e.PatientMobileNo).HasMaxLength(20);
            entity.Property(e => e.PatientPrimaryInsHldrFi).HasMaxLength(300);
            entity.Property(e => e.PatientPrimaryInsHldrLa).HasMaxLength(300);
            entity.Property(e => e.PatientPrimaryInsPkgName).HasMaxLength(300);
            entity.Property(e => e.PatientPrimaryInsPkgType).HasMaxLength(300);
            entity.Property(e => e.PatientSex).HasMaxLength(20);
            entity.Property(e => e.PatientState).HasMaxLength(30);
            entity.Property(e => e.PatientZip)
                .HasMaxLength(20)
                .HasColumnName("patientZip");
            entity.Property(e => e.ProcCode).HasMaxLength(30);
            entity.Property(e => e.PtntDcsdYsn).HasMaxLength(100);
            entity.Property(e => e.Race).HasMaxLength(100);
            entity.Property(e => e.RndrngPrvdrFrstNme).HasMaxLength(300);
            entity.Property(e => e.RndrngPrvdrLstNme).HasMaxLength(300);
            entity.Property(e => e.RndrngPrvdrNpiNo).HasMaxLength(30);
            entity.Property(e => e.RndrngPrvdrType).HasMaxLength(300);
            entity.Property(e => e.SvcDeptId).HasMaxLength(20);
            entity.Property(e => e.SvcDprtmnt).HasMaxLength(20);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

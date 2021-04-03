using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoesHuntBackup.Data.Mapping
{
    public partial class DllFileMap
        : IEntityTypeConfiguration<ShoesHuntBackup.Data.Entities.DllFile>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShoesHuntBackup.Data.Entities.DllFile> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("DllFile");

            // key
            builder.HasKey(t => t.Id);

            // properties
            builder.Property(t => t.Id)
                .IsRequired()
                .HasColumnName("Id")
                .HasColumnType("INTEGER")
                .ValueGeneratedOnAdd();

            builder.Property(t => t.DllName)
                .HasColumnName("DllName")
                .HasColumnType("TEXT");

            builder.Property(t => t.DllContentType)
                .HasColumnName("DllContentType")
                .HasColumnType("TEXT");

            builder.Property(t => t.DllData)
                .HasColumnName("DllData")
                .HasColumnType("blob");

            builder.Property(t => t.CreatedDate)
                .HasColumnName("CreatedDate")
                .HasColumnType("TEXT");

            builder.Property(t => t.UpdatedDate)
                .HasColumnName("UpdatedDate")
                .HasColumnType("TEXT");

            // relationships
            #endregion
        }

        #region Generated Constants
        public struct Table
        {
            public const string Schema = "";
            public const string Name = "DllFile";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string DllName = "DllName";
            public const string DllContentType = "DllContentType";
            public const string DllData = "DllData";
            public const string CreatedDate = "CreatedDate";
            public const string UpdatedDate = "UpdatedDate";
        }
        #endregion
    }
}

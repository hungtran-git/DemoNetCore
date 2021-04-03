using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace ShoesHuntBackup.Data.Mapping
{
    public partial class DllMap
        : IEntityTypeConfiguration<ShoesHuntBackup.Data.Entities.Dll>
    {
        public void Configure(Microsoft.EntityFrameworkCore.Metadata.Builders.EntityTypeBuilder<ShoesHuntBackup.Data.Entities.Dll> builder)
        {
            #region Generated Configure
            // table
            builder.ToTable("Dll");

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
            public const string Name = "Dll";
        }

        public struct Columns
        {
            public const string Id = "Id";
            public const string DllName = "DllName";
            public const string DllData = "DllData";
            public const string CreatedDate = "CreatedDate";
            public const string UpdatedDate = "UpdatedDate";
        }
        #endregion
    }
}

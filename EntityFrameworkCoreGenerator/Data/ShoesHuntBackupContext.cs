using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ShoesHuntBackup.Data
{
    public partial class ShoesHuntBackupContext : DbContext
    {
        public ShoesHuntBackupContext(DbContextOptions<ShoesHuntBackupContext> options)
            : base(options)
        {
        }

        #region Generated Properties
        public virtual DbSet<ShoesHuntBackup.Data.Entities.Dll> Dlls { get; set; }

        #endregion

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            #region Generated Configuration
            modelBuilder.ApplyConfiguration(new ShoesHuntBackup.Data.Mapping.DllMap());
            #endregion
        }
    }
}

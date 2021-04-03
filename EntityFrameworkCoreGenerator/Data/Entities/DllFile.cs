using System;
using System.Collections.Generic;

namespace ShoesHuntBackup.Data.Entities
{
    public partial class DllFile
    {
        public DllFile()
        {
            #region Generated Constructor
            #endregion
        }

        #region Generated Properties
        public long Id { get; set; }

        public string DllName { get; set; }

        public string DllContentType { get; set; }

        public Byte[] DllData { get; set; }

        public string CreatedDate { get; set; }

        public string UpdatedDate { get; set; }

        #endregion

        #region Generated Relationships
        #endregion

    }
}

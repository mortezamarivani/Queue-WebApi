using System;
using System.ComponentModel.DataAnnotations;

namespace Gx.DataLayer.Entities.Common
{
    public class BaseEntity
    {
        [Key]
        public long Id { get; set; }

        public bool Status { get; set; }

        public DateTime CreateDate { get; set; }

        public DateTime LastUpdateDate { get; set; }

        public long CreateUserId { get; set; }

        public long UpdateUserId { get; set; }




    }
}

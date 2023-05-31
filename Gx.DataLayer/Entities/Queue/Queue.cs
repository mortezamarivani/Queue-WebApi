using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Gx.DataLayer.Entities.Common;

namespace Gx.DataLayer.Entities.Queue
{
    public class Queue : BaseEntity
    {
        #region properties


        [Display(Name = "نام ")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Name { get; set; }

        [Display(Name = "نام خانوادگی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(100, ErrorMessage = "تعداد کاراکتر های {0} نمیتواند بیشتر از {1} باشد")]
        public string Family { get; set; }

        [Display(Name = "شماره نوبت")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public Int16 QueueNumber { get; set; }

        [Display(Name = "تاریخ ویزیت")]
        public int VisitDate { get; set; }

        [Display(Name = "ساعت ویزیت")]
        public int VisitTime { get; set; }

        [Display(Name = "وضعیت صف")]
        public Int16 QueueStatuse { get; set; }

        [Display(Name = "مبلغ پرداختی")]
        public Decimal Payment { get; set; }

        [Display(Name = "وضعیت پرداختی")]
        public Int16 PaymentStatus { get; set; }

        [Display(Name = "شماره موبایل")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Phone { get; set; }

        [Display(Name = "ساعت شروع ویزیت")]
        public int StartVisitTime { get; set; }

        [Display(Name = "ساعت پایان ویزیت")]
        public int EndVisitTime { get; set; }

        #endregion

        #region Relations

        //public ICollection<UserRole> UserRoles { get; set; }

        #endregion
    }
}

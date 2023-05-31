using System;
using System.Collections.Generic;


namespace Gx.Core.DTOs.Queue
{
    public class QueueDTO 
    {
        public long Id { get; set; }
        public bool Status { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime LastUpdateDate { get; set; }
        public long CreateUserId { get; set; }
        public long UpdateUserId { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public Int16 QueueNumber { get; set; }
        public int VisitDate { get; set; }
        public int VisitTime { get; set; }
        public Int16 QueueStatuse { get; set; }
        public Decimal Payment { get; set; }
        public Int16 PaymentStatus { get; set; }
        public string Phone { get; set; }
        public int StartVisitTime { get; set; }
        public int EndVisitTime { get; set; }

        public string UserFirstName { get; set; } 
        public string UserLastNAme { get; set; } 
    }

}

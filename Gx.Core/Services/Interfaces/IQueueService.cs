using Gx.Core.DTOs.Queue;
using Gx.DataLayer.Entities.Queue;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Gx.Core.Services.Interfaces
{
    public interface IQueueService :IDisposable
    {
    
        Task<Queue> GetQueueById(long Id);

        Task<List<QueueDTO>> GetAllQueueByVisitDateAndDr(int visitDate, long drId);
        Task  EditByPaymentStatuse(long id , Int16 status, long UpdateUserId);
        Task EditByQueueStatuse(long id , Int16 status, long UpdateUserId);
        
        Task Edit(Queue Queue);
        Task Create(Queue Queue);
        Task Delete(long Id);


    }
}

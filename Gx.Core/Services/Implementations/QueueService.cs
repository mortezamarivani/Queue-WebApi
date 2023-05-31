using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Gx.Core.Services.Interfaces;
using Gx.DataLayer.Repository;
using Gx.Core.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gx.DataLayer.Entities.Queue;
using Gx.Core.DTOs.Queue;

namespace Gx.Core.Services.Implementations
{
    public class QueueService : IQueueService
    {
        #region constructor

        private IGenericRepository<Queue> genericRepository;
        private IGenericRepository<Gx.DataLayer.Entities.Account.Users> userRepository;
        public QueueService(IGenericRepository<Queue> _genericRepository, IGenericRepository<Gx.DataLayer.Entities.Account.Users> _userRepository)
        {
            this.genericRepository = _genericRepository;
            this.userRepository = _userRepository;
        }

        #endregion

        #region User Section

        public async Task<Queue> GetQueueById(long Id)
        {
            return await genericRepository.GetEntityById(Id);
        }


        public async Task<List<QueueDTO>> GetAllQueueByVisitDateAndDr(int visitDate, long drId)
        {
            var retQ = await (from queue in genericRepository.GetEntitiesQuery()
                              join user in userRepository.GetEntitiesQuery() on queue.CreateUserId equals user.Id into __user
                              from user in __user.DefaultIfEmpty()
                              select new QueueDTO
                              {
                                  CreateUserId = queue.CreateUserId,
                                  Id = queue.Id,
                                  CreateDate = queue.CreateDate,
                                  EndVisitTime = queue != null ? queue.EndVisitTime : 0 ,
                                  Family= queue != null ? queue.Family : "-" ,
                                  LastUpdateDate=queue.LastUpdateDate,
                                  Name= queue != null ? queue.Name : "-" ,
                                  Payment= queue != null ? queue.Payment : 0 ,
                                  PaymentStatus=queue.PaymentStatus,
                                  Phone= queue != null ? queue.Phone : "-" ,
                                  QueueNumber= queue != null ? queue.QueueNumber : Convert.ToInt16(0),
                                  QueueStatuse = queue != null ? queue.QueueStatuse : Convert.ToInt16(0),
                                  StartVisitTime= queue != null ? queue.StartVisitTime : 0,
                                  Status= queue != null ? queue.Status : false ,
                                  UpdateUserId=queue.UpdateUserId,
                                  UserFirstName = user != null ? user.FirstName : "-" ,
                                  UserLastNAme = user != null ? user.LastName : "-" ,
                                  VisitDate = queue != null ? queue.VisitDate : 0 ,
                                  VisitTime= queue != null ? queue.VisitTime : 0
                                  //LessonId = _lesson != null ? _lesson.Id : 0,
                                  //LessonName =  != null ?  : "-",
                              })
                              .Where(r=> r.CreateUserId == drId && r.VisitDate == visitDate)
                              .ToListAsync();
            return retQ;
        }

        public async Task Delete(long id)
        {
            await genericRepository.DeleteEntity(id);
        }

        public async Task Edit(Queue _InputObj)
        {
            Queue obj = await GetQueueById(_InputObj.Id);

            if (obj != null)
            {
                obj.Id = _InputObj.Id;
                obj.VisitTime = _InputObj.VisitTime;
                obj.VisitDate = _InputObj.VisitDate;
                obj.UpdateUserId = _InputObj.UpdateUserId;
                obj.Status = _InputObj.Status;
                obj.StartVisitTime = _InputObj.StartVisitTime;
                obj.QueueStatuse = _InputObj.QueueStatuse;
                obj.QueueNumber = _InputObj.QueueNumber;
                obj.Phone = _InputObj.Phone;
                obj.PaymentStatus = _InputObj.PaymentStatus;
                obj.Payment = _InputObj.Payment;
                obj.Name = _InputObj.Name;
                obj.Family = _InputObj.Family;
                obj.LastUpdateDate = System.DateTime.Now;
                obj.CreateUserId = _InputObj.CreateUserId;
                obj.UpdateUserId = _InputObj.UpdateUserId;

                genericRepository.UpdateEntity(obj);
                await genericRepository.SaveChanges();
            }
        }

        public async Task EditByPaymentStatuse(long id, Int16 status,long UpdateUserId)
        {
            Queue obj = await GetQueueById(id);

            if (obj != null)
            {
                obj.Id = id;
                obj.PaymentStatus = status;
                obj.LastUpdateDate = System.DateTime.Now;
                obj.UpdateUserId = UpdateUserId;

                genericRepository.UpdateEntity(obj);
                await genericRepository.SaveChanges();
            }
        }

        public async Task EditByQueueStatuse(long id, Int16 status, long UpdateUserId)
        {
            Queue obj = await GetQueueById(id);

            if (obj != null)
            {
                obj.Id = id;
                obj.QueueStatuse = status;
                obj.LastUpdateDate = System.DateTime.Now;
                obj.UpdateUserId = UpdateUserId;

                //در حال ویزیت
                if (status == 4)
                    obj.StartVisitTime = Convert.ToInt32(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString());

                //اتمام ویزیت
                if (status == 5)
                    obj.EndVisitTime = Convert.ToInt32(DateTime.Now.Hour.ToString() + DateTime.Now.Minute.ToString());

                genericRepository.UpdateEntity(obj);
                await genericRepository.SaveChanges();
            }
        }

        public async Task Create(Queue _InputObj)
        {
            await genericRepository.AddEntity(_InputObj);
            await genericRepository.SaveChanges();
        }

        #endregion

        #region dispose

        public void Dispose()
        {
            genericRepository?.Dispose();
        }

     



        #endregion
    }
}
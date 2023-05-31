using Microsoft.AspNetCore.Mvc;
using Gx.Core.Services.Interfaces;
using System.Threading.Tasks;
using Gx.Core.Utilities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Gx.WebApi.Controllers;
using Gx.DataLayer.Entities.Queue;

namespace Queuening.WebApi.Controllers
{
    public class QueueController : SiteBaseController
    {
        #region constractor

        private IQueueService db;
        private IUserService userDb;
        public QueueController(IQueueService _db, IUserService _userDb)
        {
            this.db = _db;
            this.userDb = _userDb;
        }

        #endregion

        #region users select 

        [HttpGet("GetQueueById/{id:long}")]
        public async Task<IActionResult> GetQueueById([FromRoute] long id)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

            var retObj = await db.GetQueueById(id);
            return JsonResponseStatus.Success(retObj);
        }


        [HttpGet("GetAllQueueByVisitDateAndDr")]
        public async Task<IActionResult> GetAllQueueByVisitDateAndDr([FromBody] Queue queue)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

            var retObj = await db.GetAllQueueByVisitDateAndDr(queue.VisitDate, queue.CreateUserId);
            return JsonResponseStatus.Success(retObj);
        }



        [HttpPut("Edit")]
        public async Task<IActionResult> Edit(Queue _Queue)//, 
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");


            try
            {
                if (ModelState.IsValid)
                {

                    await db.Edit(_Queue);
                    return JsonResponseStatus.Success();
                }

                return JsonResponseStatus.Error();
            }
            catch (DbUpdateConcurrencyException er)
            {
                return NotFound(er.Message);
            }

            return Ok(_Queue);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create( Queue data)
        {

            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

            Queue newObj = new Queue();
            try
            {
                if (ModelState.IsValid)
                {
                    newObj.Name = data.Name;
                    newObj.Family = data.Family;
                    newObj.CreateUserId = Convert.ToInt64(data.CreateUserId);
                    newObj.UpdateUserId = newObj.CreateUserId;
                    newObj.Payment = Convert.ToInt64(data.Payment);
                    newObj.CreateDate = DateTime.Now;
                    newObj.LastUpdateDate = newObj.CreateDate;
                    newObj.PaymentStatus = Convert.ToInt16(data.PaymentStatus);
                    newObj.Phone = data.Phone;
                    newObj.QueueNumber = Convert.ToInt16(data.QueueNumber);
                    newObj.QueueStatuse = Convert.ToInt16(data.QueueStatuse);
                    newObj.Status = Convert.ToBoolean(data.Status);
                    newObj.VisitDate = Convert.ToInt32(data.VisitDate);
                    newObj.VisitTime = Convert.ToInt32(data.VisitTime);

                    await db.Create(newObj);

                    return JsonResponseStatus.Success(newObj);
                }

                return JsonResponseStatus.Error();
            }
            catch (DbUpdateConcurrencyException er)
            {
                return NotFound(er.Message);
            }

            return Ok(newObj);
        }

        
        [HttpPut("EditByQueueStatuse")]
        public async Task<IActionResult> EditByQueueStatuse(Queue _Queue)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

            try
            {
                if (ModelState.IsValid)
                {

                    await db.EditByQueueStatuse(_Queue.Id,_Queue.QueueStatuse,_Queue.UpdateUserId);
                    return JsonResponseStatus.Success();
                }

                return JsonResponseStatus.Error();
            }
            catch (DbUpdateConcurrencyException er)
            {
                return NotFound(er.Message);
            }

            return Ok(_Queue);
        }

        [HttpPut("EditByPaymentStatuse")]
        public async Task<IActionResult> EditByPaymentStatuse(Queue _Queue)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

            try
            {
                if (ModelState.IsValid)
                {

                    await db.EditByPaymentStatuse(_Queue.Id, _Queue.PaymentStatus,_Queue.UpdateUserId);
                    return JsonResponseStatus.Success();
                }

                return JsonResponseStatus.Error();
            }
            catch (DbUpdateConcurrencyException er)
            {
                return NotFound(er.Message);
            }

            return Ok(_Queue);
        }


        [HttpDelete("Delete")]
        public async Task<IActionResult> Delete(Queue _Queue)
        {
            if (!User.Identity.IsAuthenticated)
                return JsonResponseStatus.Error("UnAuthenticate");

            try
            {
                if (ModelState.IsValid)
                {
                    await db.Delete(_Queue.Id);
                    return JsonResponseStatus.Success();
                }

                return JsonResponseStatus.Error();
            }
            catch (DbUpdateConcurrencyException er)
            {
                return NotFound(er.Message);
            }

            return Ok();
        }

        #endregion


    }
}

using Microsoft.AspNetCore.Mvc;
using Gx.Core.Services.Interfaces;
using System.Threading.Tasks;
using Gx.Core.Utilities.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using Gx.DataLayer.Entities.Plan;

namespace Gx.WebApi.Controllers
{
    public class ShopListController : SiteBaseController
    {
        #region constractor

        private IPlanService db;
        public PlanController(IPlanService _db)
        {
            this.db = _db;
        }

        #endregion

        #region users select 

        [HttpGet("GetAllPlan")]
        public async Task<IActionResult> GetAllPlan()
        {
            var allPlan= await db.GetAllPlan();
            return JsonResponseStatus.Success(allPlan);
        }

        [HttpGet("GetPlanById/{id:long}")]
        public async Task<IActionResult> GetPlanById([FromRoute] long id)
        {
            var retObj = await db.GetFirstPlanById(id);
            return JsonResponseStatus.Success(retObj);
        }


        [HttpGet("GetAllPlanByRowNumber/{id:int}")]
        public async Task<IActionResult> GetAllPlanByRowNumber([FromRoute] Int16 id)
        {
            //var allPlan = await db.GetAllPlanByRowNumber(id);
            return JsonResponseStatus.Success(null);
        }


        [HttpPut("Edit/{id:long}")]
        public async Task<IActionResult> Edit([FromRoute] long id, [FromBody] Gx.DataLayer.Entities.Plan.Plan _Plan)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    ///for rol 1 
                    var ret = await db.GetPlanByDayTimePerson(_Plan.fldDay, _Plan.fldStartTime, _Plan.fldEndTime,
                        _Plan.PersonId, _Plan.fldTerm, _Plan.fldAcademicYear ,"update" ,_Plan.Id);
                    if (ret.Count > 0)
                    {
                        Err retErr = new Err();
                        retErr.ErrCode = 1;
                        retErr.ErrMessage = "در یک روز یک ساعت نمی توان برای استاد کلاس اختصاص داد ،قبلا همچنین برنامه ثبت شده است";
                        return JsonResponseStatus.Error(retErr);
                    }

                    await db.Edit(_Plan);

                    List<Gx.DataLayer.Entities.Plan.Plan> RetPlanForRowId = new List<Gx.DataLayer.Entities.Plan.Plan>();
                    RetPlanForRowId = await db.GetPlanForUpdateMaxRowId(_Plan.fldDay, _Plan.fldStartTime, _Plan.fldEndTime, _Plan.LessonId, _Plan.fldTerm, _Plan.fldAcademicYear);
                    if(RetPlanForRowId != null)
                    {
                        Int16 rowId = 0; 
                        foreach (Gx.DataLayer.Entities.Plan.Plan item in RetPlanForRowId)
                        {
                            rowId = Convert.ToInt16(rowId + 1);
                            item.fldRow = rowId;
                            await db.Edit(item);
                        }
                    }
                    else
                    {
                        _Plan.fldRow = 1;
                        await db.Edit(_Plan);
                    }

                    return JsonResponseStatus.Success();
                }

                return JsonResponseStatus.Error();
            }
            catch (DbUpdateConcurrencyException er)
            {
                return NotFound(er.Message);
            }

            return Ok(_Plan);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> Create( IFormCollection data)
        {
            Gx.DataLayer.Entities.Plan.Plan newObj = new Gx.DataLayer.Entities.Plan.Plan();
            try
            {
                Int16 day = Convert.ToInt16(data["fldDay"]);
                Int16 StartTime = Convert.ToInt16(data["fldStartTime"]);
                Int16 EndTime = Convert.ToInt16(data["fldEndTime"]);
                Int64 PersonId = Convert.ToInt64(data["PersonId"]);
                string Section = data["fldSection"];
                Int16 Term = Convert.ToInt16(data["fldTerm"]);
                Int16 AcademicYear = Convert.ToInt16(data["fldAcademicYear"]);
                Int64 LessonId = Convert.ToInt64(data["LessonId"]);
                Int16 type = Convert.ToInt16(data["fldSex"]);
                Int64 groupId = Convert.ToInt16(data["GroupId"]);



                

                if (ModelState.IsValid)
                {
                    newObj.Id= GetMaxId()+1;
                    newObj.Code = newObj.Id;
                    newObj.GroupId = Convert.ToInt64(data["GroupId"]);
                    newObj.MajorId = Convert.ToInt64(data["MajorId"]);
                    newObj.fldDay = day;
                    newObj.fldEndTime = EndTime;
                    newObj.fldStartTime = StartTime;
                    newObj.PersonId= PersonId;
                    newObj.fldSection = "0";
                    newObj.fldTerm = Term;
                    newObj.fldGxForTerm = Convert.ToInt16(data["fldGxForTerm"]);
                    newObj.fldAcademicYear = AcademicYear;
                    newObj.LessonId = LessonId;
                    newObj.LessonOrientationId = Convert.ToInt64(data["LessonOrientationId"]);
                    newObj.fldDegree = Convert.ToInt16(data["fldDegree"]);
                    newObj.fldSex = Convert.ToInt16(data["fldSex"]);
                    newObj.MajorOrientationId = Convert.ToInt64(data["MajorOrientationId"]);
                    newObj.fldCol = Convert.ToInt16(data["fldCol"]);
                    newObj.CreateDate = DateTime.Now;
                    newObj.LastUpdateDate = newObj.CreateDate;
                    newObj.CreateUserId = 0;
                    newObj.UpdateUserId = 0;
                    newObj.Status = 1;
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

        [HttpGet("GetPlanByDayAndTime/{param}")]
        [Route("GetPlanByDayAndTime/{param}")]
        public async Task<IActionResult> GetPlanByDayAndTime([FromRoute] string param)
        {
            Int16 day = Convert.ToInt16(param.Split(';')[0]);
            Int16 StartTime = Convert.ToInt16(param.Split(';')[1]);
            Int16 EndTime = Convert.ToInt16(param.Split(';')[2]);
            Int64 PersonId = Convert.ToInt64(param.Split(';')[3]);
            //string Section = param.Split(';')[4];
            Int16 Term = Convert.ToInt16(param.Split(';')[5]);
            Int16 AcademicYear = Convert.ToInt16(param.Split(';')[6]);

            var ret = await db.GetPlanByDayTimePerson(day, StartTime,EndTime,PersonId,Term,AcademicYear,"", 0);

            return JsonResponseStatus.Success(ret);
        }


        [HttpGet("GetPlanByYerTermDegreeGroupMajor/{param}")]
        [Route("GetPlanByYerTermDegreeGroupMajor/{param}")]
        public async Task<IActionResult> GetPlanByYerTermDegreeGroupMajor([FromRoute] string param)
        {
            Int16 yer = Convert.ToInt16(param.Split(';')[0]);
            Int16 term = Convert.ToInt16(param.Split(';')[1]);
            Int16 degree = Convert.ToInt16(param.Split(';')[2]);
            Int16 groupId = Convert.ToInt16(param.Split(';')[3]);
            Int64 majorId = Convert.ToInt64(param.Split(';')[4]);
            Int16 planingTerm = Convert.ToInt16(param.Split(';')[5]);
            Int64 majorOriantaionId = Convert.ToInt64(param.Split(';')[6]);


            //var ret = await db.GetPlanByYerTermDegreeGroupMajor(yer,term,degree,groupId,majorId , planingTerm, majorOriantaionId);

            return JsonResponseStatus.Success(null);
        }

        public  Int64 GetMaxId()
        {
            return  db.GetMaxId();
        }

        [HttpDelete("Delete/{id:long}")]
        public async Task<IActionResult> Delete([FromRoute] long id)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await db.Delete(id);
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


        [HttpGet("GetFirstPlanById/{id:long}")]
        public async Task<IActionResult> GetFirstPlanById(long Id)
        {
            var PlanById = await db.GetFirstPlanById(Id);
            return JsonResponseStatus.Success(PlanById);
        }

        [HttpGet("GetAllPlanByIDs/{Ids}")]
        public async Task<IActionResult> GetAllPlanByIDs(string Ids)
        {
            var PlanById = await db.GetAllPlanByIDs(Ids);
            return JsonResponseStatus.Success(PlanById);
        }


        #endregion


    }
}

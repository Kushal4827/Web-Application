using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using VDEM.Lib.Interface.TkmAppDetails;
using VDEM.Lib.BusinessLogic.TkmAppDetails;

namespace VDEM.WEB.APP.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TkmAppDetailsController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public TkmAppDetailsController(IConfiguration configuration, IWebHostEnvironment env)
        {
            _configuration = configuration;
        }

        /// <summary>
        /// Webapi to fetch Zone master table.
        /// </summary>
        /// <returns>Json object containing Zone master data</returns>
        [Route("GetAppDetails")]
        [HttpGet]
        public JsonResult GetAppDetails()
        {
            GetAppDetailsIP ip = new GetAppDetailsIP();
            GetAppDetailsOP op = new GetAppDetailsOP();
            try
            {
                ip.databaseCon = _configuration.GetConnectionString("DatabaseConnection");
                TkmAppDetailsMainBL bl = new TkmAppDetailsMainBL();
                bl.GetAppDetails(ref ip, ref op);
            }
            catch (Exception ex)
            {
                op.returnMessage = ex.Message;
                op.returnValue = -2;
            }
            return new JsonResult(op);
        }
        [Route("AddAppDetails")]
        [HttpPost]
        public JsonResult AddAppDetail(AddAppDetailsIP ip)
        {
            AddAppDetailsOP op = new AddAppDetailsOP();
            try
            {
                ip.databaseCon = _configuration.GetConnectionString("DatabaseConnection");
                AddAppDetailsMainBL bl = new AddAppDetailsMainBL();
                bl.AddAppDetails(ref ip, ref op);
            }
            catch (Exception ex)
            {
                op.returnMessage = ex.Message;
                op.returnValue = -2;
            }
            return new JsonResult(op);
        }


    }
}
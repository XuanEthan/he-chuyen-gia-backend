using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using SbsSW.SwiPlCs;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;

namespace he_chuyen_gia_1.Controllers
{
    [ApiController]
    [Route("api/chan-doan")]
    public class ChanDoanController : ControllerBase
    {
        private static readonly object _lock = new object();

        private PrologService _prolog = new PrologService();

        [HttpGet("danh-sach-trieu-chung")]
        public ActionResult<List<string>> GetSystompList()
        {
            lock (_lock)
            {
                var res = _prolog.GetSymptoms();
                return Ok(res);
            }
        }
        [HttpPost("chuan-doan-benh")]
        public IActionResult GetDisease([FromBody] string[] symptoms) 
        { 
            //input la 1 mang trieu chung
            lock (_lock)
            {
                var result = _prolog.GetDiseases(symptoms);
                return Ok(result);
            }
        }
    }
}

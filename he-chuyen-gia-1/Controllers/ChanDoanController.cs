using Microsoft.AspNetCore.Mvc;
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

        private PrologHelper _prolog = new PrologHelper();
        public ChanDoanController()
        {
        }

        [HttpGet("danh-sach-trieu-chung")]
        public ActionResult<List<string>> GetSystompList()
        {
            lock (_lock)
            {
                var res = _prolog.GetSymptoms();
                return Ok(res);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using SbsSW.SwiPlCs;
using System.Collections.Generic;
using System.Linq;

namespace he_chuyen_gia_1.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ChanDoanController : ControllerBase
    {
        // Static flag để đảm bảo PlEngine chỉ initialize 1 lần
        private static bool _isPrologInitialized = false;
        private static readonly object _lock = new object();

        public ChanDoanController()
        {
            InitializeProlog();
        }

        private void InitializeProlog()
        {
            if (_isPrologInitialized) return;

            lock (_lock) // thread-safe
            {
                if (!_isPrologInitialized)
                {
                    string[] param = { "-q", "-f", @"c:/users/huy/onedrive/documents/documents/prolog/kt1.pl" };
                    if (!PlEngine.IsInitialized)
                        PlEngine.Initialize(param);
                       _isPrologInitialized = true;
                }
            }
        }

        // GET: /ChanDoan/danhsachtrieuchung
        [HttpGet("danhsachtrieuchung")]
        public ActionResult<List<string>> GetDanhSachTrieuChung()
        {
            try
            {
                List<string> results = new List<string>();

                using (PlQuery query = new PlQuery("cat_of(tom, X)"))
                {
                    foreach (PlQueryVariables vars in query.SolutionVariables)
                    {
                        results.Add(vars["X"].ToString());
                    }
                }

                return Ok(results); // Trả JSON danh sách các giá trị X
            }
            catch (System.Exception ex)
            {
                // Trả lỗi nếu Prolog query gặp vấn đề
                return BadRequest(new { error = ex.Message });
            }
        }
    }
}

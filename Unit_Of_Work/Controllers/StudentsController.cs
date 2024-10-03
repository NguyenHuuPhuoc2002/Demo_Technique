using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using Unit_Of_Work.Data;
using Unit_Of_Work.Models;
using Unit_Of_Work.Repository.Interface;

namespace Unit_Of_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ISinhVienRepository _sinhVien;

        public StudentsController(ISinhVienRepository sinhVien, IDistributedCache cache)
        {
            _sinhVien = sinhVien;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
                var students = await _sinhVien.GetAll();
                cacheData = JsonConvert.SerializeObject(students);
                await _cache.SetStringAsync(cacheKey, cacheData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                   
        }


        [HttpPost]
        public async Task<IActionResult> Add(SinhVienModel model)
        {

            // Chuyển đổi từ SinhVienModel sang SinhVien
            var result = new SinhVien
            {
                MaSv = model.MaSv,
                HoTen = model.HoTen,
                NgaySinh = model.NgaySinh
            };
            await _sinhVien.AddItem(result);
            return Ok(model);


        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _sinhVien.RemoveItem(id);
            return NoContent();
        }
    }
}

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
        private readonly IDistributedCache _cache;
        private string cacheKey = "myCacheKey";

        public StudentsController(ISinhVienRepository sinhVien, IDistributedCache cache)
        {
            _sinhVien = sinhVien;
            _cache = cache;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            string cacheData = await _cache.GetStringAsync(cacheKey);
            if(string.IsNullOrEmpty(cacheData))
            {
                var students = await _sinhVien.GetAll();
                cacheData = JsonConvert.SerializeObject(students);
                await _cache.SetStringAsync(cacheKey, cacheData, new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(30),
                   
                });
            }
            var deserializedStudents = JsonConvert.DeserializeObject<List<SinhVienModel>>(cacheData);
            return Ok(deserializedStudents);
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
            _cache.Remove(cacheKey);
            return Ok(model);


        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _sinhVien.RemoveItem(id);
            _cache.Remove(cacheKey);
            return NoContent();
        }
    }
}

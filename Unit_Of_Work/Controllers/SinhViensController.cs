using Common.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Unit_Of_Work.Data;
using Unit_Of_Work.Models;
using Unit_Of_Work.Repository.Interface;
using Unit_Of_Work.Settings;

namespace Unit_Of_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SinhViensController : ControllerBase
    {
        private readonly ISinhVienRepository _sinhVien;
        private readonly IMemoryCache _cache;
        private readonly CacheSetting _cacheSettings;
        private const string CacheKey = "AllStudents";

        public SinhViensController(ISinhVienRepository sinhVien, IMemoryCache cache,
                                   CacheSetting cacheSettings)
        {
            _sinhVien = sinhVien;
            _cache = cache;
            _cacheSettings = cacheSettings;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                if (!_cache.TryGetValue(CacheKey, out var students))
                {
                    students = await _sinhVien.GetAll();

                    _cache.Set(CacheKey, students, _cacheSettings.Duration);
                }

                return Ok(students);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetByIdAsync(string id)
        {
            var result = await _sinhVien.GetByIdAsync(id);
            return Ok(result);
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
            _cache.Remove(CacheKey);
            return Ok(model);


        }
        [HttpDelete]
        public async Task<IActionResult> Delete(string id)
        {
            await _sinhVien.RemoveItem(id);
            _cache.Remove(CacheKey);
            return NoContent();
        }

    }
}

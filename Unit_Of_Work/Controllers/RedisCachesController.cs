using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using Newtonsoft.Json;
using System.Text;
using Unit_Of_Work.Common;
using Unit_Of_Work.Data;
using Unit_Of_Work.Models;
using Unit_Of_Work.Repository.Interface;
using Unit_Of_Work.Services;

namespace Unit_Of_Work.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RedisCachesController : ControllerBase
    {
        private readonly ISinhVienRepository _sinhVien;
        private readonly RedisConfiguration _redisConfiguration;
        private readonly IResponseCacheService _responseCacheService;

        public RedisCachesController(ISinhVienRepository sinhVien, RedisConfiguration redisConfiguration, IResponseCacheService responseCacheService)
        {
            _sinhVien = sinhVien;
            _redisConfiguration = redisConfiguration;
            _responseCacheService = responseCacheService;
        }
        [HttpGet("getall")]
        [Cache(1000)]
        public async Task<IActionResult> GetAll(string? key)
        {
            var students = await _sinhVien.GetAll();
            return Ok(students);
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
            await _responseCacheService.RemoveCacheResponseAsync("/api/RedisCaches/");
            return Ok(model);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {            
            await _sinhVien.RemoveItem(id);
            await _responseCacheService.RemoveCacheResponseAsync("/api/RedisCaches/");
            return NoContent();
        }
    }
}

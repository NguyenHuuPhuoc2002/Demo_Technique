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
        private readonly ILogger<StudentsController> _logger;

        public StudentsController(ISinhVienRepository sinhVien, ILogger<StudentsController> logger)
        {
            _sinhVien = sinhVien;
            _logger = logger;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var students = await _sinhVien.GetAll();
            var studentsJson = System.Text.Json.JsonSerializer.Serialize(students);
            _logger.LogInformation("Get students success! => {students}", studentsJson);
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

using Common.Exceptions;
using Microsoft.EntityFrameworkCore;
using Unit_Of_Work.Data;
using Unit_Of_Work.Models;
using Unit_Of_Work.Repository.Interface;

namespace Unit_Of_Work.Repository
{
    public class SinhVienRepository : Repository<SinhVien>, ISinhVienRepository
    {
        private readonly IUnitOfWorkRepository _unitOfWork;

        public SinhVienRepository(QlSinhVienContext context, IUnitOfWorkRepository unitOfWork) : base(context)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task AddItem(SinhVien sinhVien)
        {
            try
            {
                await Add(sinhVien);
            }
            catch (Exception)
            {

                throw ;
            }
            
        }
        public async Task<SinhVienModel> GetByIdAsync(string id)
        {
            var student = await _unitOfWork.Students.GetById(id);
            if (student == null)
            {
                throw new KeyNotFoundException($"Không tìm thấy sinh viên với id {id}");
            }
            var result = new SinhVienModel
                {
                    HoTen = student.HoTen,
                    MaSv = student.MaSv,
                    NgaySinh = student.NgaySinh,
                };
                return result;
           
        }
        public async Task<bool> RemoveItem(string Id)
        {
            var student = await GetById(Id);
            if (student != null)
            {
                await Delete(student);
                return true;
            }
            return false;
        }

        /*public async Task<SinhVien> UpdateItem(SinhVien sinhVien)
        {
            var s = await Entities.Include(p => p.MonHocs).FirstOrDefaultAsync(p => p.HoTen ==sinhVien.HoTen);
            return s;
        }*/

        public async Task<List<SinhVienModel>> GetAll()
        {
            var students = await Entities.Include(p => p.MaLhNavigation).ToListAsync();

            var studentDTOs = students.Select(s => new SinhVienModel
            {
                MaSv = s.MaSv,
                HoTen = s.HoTen,
                NgaySinh = s.NgaySinh
                
            }).ToList();

            return studentDTOs;
        }

        public Task<SinhVien> UpdateItem(SinhVien sinhVien)
        {
            throw new NotImplementedException();
        }
    }
}

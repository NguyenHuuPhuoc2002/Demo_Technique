using Unit_Of_Work.Data;
using Unit_Of_Work.Models;

namespace Unit_Of_Work.Repository.Interface
{
    public interface ISinhVienRepository : IRepository<SinhVien>
    {
        Task<List<SinhVienModel>> GetAll();
        Task<SinhVienModel> GetByIdAsync(string id);
        Task<bool> RemoveItem(string Id);
        Task AddItem(SinhVien sinhVien);
        Task<SinhVien> UpdateItem(SinhVien sinhVien);

    }
}

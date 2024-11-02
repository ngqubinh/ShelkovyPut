using Domain.Models.Management;

namespace Application.Interfaces.Setting
{
    public interface IExcelFileImportService<T> where T : class
    {
        List<T> ImportAddressesFromExcel(string filePath);
        Task SaveToDatabase(List<T> addresses);
    }
}

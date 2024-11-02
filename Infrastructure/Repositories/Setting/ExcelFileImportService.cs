using Application.Interfaces.Setting;

namespace Infrastructure.Repositories.Setting
{
    public class ExcelFileImportService<T> : IExcelFileImportService<T> where T : class
    {
        
        public List<T> ImportAddressesFromExcel(string filePath)
        {
            throw new NotImplementedException();
        }

        public Task SaveToDatabase(List<T> addresses)
        {
            throw new NotImplementedException();
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Domain.Models.Management;
using Application.ViewModels.Setting;
using Infrastructure.Data;

namespace ShelkovyPut_Main.Controllers.Setting
{
    public class ExcelController : Controller 
    {
        private readonly ShelkobyPutDbContext _context;

        public ExcelController(ShelkobyPutDbContext context)
        {
            _context = context;
        }

        [HttpGet("upload")]
        public IActionResult Upload()
        {
            return View();
        }

        [HttpPost("import")]
        public async Task<ActionResult> ImportExcel(IFormFile file) {
            if (file == null || file.Length == 0)
                return Json(new { success = false, message = "No file uploaded" });

            var filePath = Path.GetTempFileName();
            using(var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            using (var client = new HttpClient())
            {
                var form = new MultipartFormDataContent();
                form.Add(new StreamContent(new FileStream(filePath, FileMode.Open)), "file", file.FileName);
                string url = "http://localhost:8080/import";
                var response = await client.PostAsync(url, form);
                if(!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, "Error importing to Go Api");
                }       

                var result = await response.Content.ReadAsStringAsync();
                var categories = JsonConvert.DeserializeObject<List<CategoryForExcelImportVM>>(result);

                // bien danh dang danh sach de luu truu 
                var category = categories.Select(c => new Category()
                {
                    CategoryName = c.CategoryName,
                    CreatedDate = c.CreatedDate,
                    UserId = c.UserId,
                }).ToList();

                _context.Categories.AddRange(category);
                await _context.SaveChangesAsync();

                return Ok(new { success = true, data = categories });
            }
        }
    }
}

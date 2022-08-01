using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using OfficeOpenXml;
using Rotativa.AspNetCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using TestingAplikasi.DAO;
using TestingAplikasi.Models;

namespace TestingAplikasi.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        HomeDAO dao;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            dao = new HomeDAO();
        }

        public IActionResult Index()
        {
            dynamic objek = new ExpandoObject();
            var data = dao.getKaryawanAll();

            objek.table = data;

            return View(objek);
        }

        public IActionResult Detail(string npp)
        {
            dynamic objek = new ExpandoObject();
            objek.npp = npp;
            objek.id = "01";

            return View(objek);
        }

        public IActionResult Tambah()
        {
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult TambahKaryawan(UserModel mdl)
        {
            if (dao.simpanKaryawan(mdl))
            {
                TempData["success"] = "Berhasil menambahkan data!";
            }
            else
            {
                TempData["error"] = "Gagal menambahkan data!";
            }

            return RedirectToAction("Index");
        }

        //Export data ke excel
        //cara menggunakan install EPPlus lewat nugget packages
        //konfigurasi di appsettings.json
        public IActionResult ExportSeminar()
        {
            byte[] result;
            string namafile = "nama_file_excel.xlsx";
            var data = dao.getKaryawanAll();

            using (var package = new ExcelPackage())
            {
                try
                {
                    var worksheet = package.Workbook.Worksheets.Add("Nama Sheet"); //Worksheet name
                    worksheet.Cells.LoadFromCollection(data, true);
                    result = package.GetAsByteArray();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            return File(result, "application/ms-excel", namafile);
        }

        //pembuatan pdf
        // install rotativa dan rotativa.aspnetcore via nugget packages
        // copy folder Rotativa ke folder wwwroot
        // Tambahkan RotativaConfiguration.Setup(env2); pada startup.cs fungsi configure
        public IActionResult CetakPdf()
        {
            string halaman = "Cetak";
            var data = dao.getKaryawanAll();

            return new ViewAsPdf(halaman, data)
            {
                PageOrientation = Rotativa.AspNetCore.Options.Orientation.Landscape,
                PageSize = Rotativa.AspNetCore.Options.Size.A4,
                PageMargins = {
                        Left = 5,
                        Right = 5,
                        Top = 5,
                        Bottom = 5
                    }
            };
        }

        // import data dari excel
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ImportData(IFormFile formFile, int id_event)
        {
            //if (formFile == null || formFile.Length <= 0)
            //{
            //    TempData["err"] = "Silahkan Upload File";
            //    return RedirectToAction("Index");
            //}

            //if (!Path.GetExtension(formFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            //{
            //    TempData["err"] = "File Harus berbentuk Xlsx";
            //    return RedirectToAction("Index");
            //}

            //var list = new List<PESERTA>();

            //using (var stream = new MemoryStream())
            //{
            //    formFile.CopyTo(stream);
            //    using (var package = new ExcelPackage(stream))
            //    {
            //        ExcelWorksheet worksheet = package.Workbook.Worksheets[0];
            //        var rowCount = worksheet.Dimension.Rows;
            //        var barcode = GenerateKode(id_event, new Random());

            //        for (int row = 2; row <= rowCount; row++)
            //        {
            //            var tempcode = row - 1;
            //            var nama_peserta = (worksheet.Cells[row, 1].Value == null) ? "" : worksheet.Cells[row, 1].Value.ToString().Trim();
            //            var peran = (worksheet.Cells[row, 2].Value == null) ? "Peserta" : worksheet.Cells[row, 2].Value.ToString().Trim();
            //            var no_sertifikat = (worksheet.Cells[row, 3].Value == null) ? "" : worksheet.Cells[row, 3].Value.ToString().Trim();
            //            var institusi = (worksheet.Cells[row, 6].Value == null) ? "" : worksheet.Cells[row, 6].Value.ToString().Trim();
            //            var email = (worksheet.Cells[row, 7].Value == null) ? "" : worksheet.Cells[row, 7].Value.ToString().Trim();
            //            DateTime tanggal = DateTime.Now;

            //            if (worksheet.Cells[row, 4].Value != null)
            //            {
            //                var tgl = worksheet.Cells[row, 4].Value.ToString();
            //                if (tgl.Contains("/") || tgl.Contains("-"))
            //                {
            //                    tanggal = DateTime.Parse(tgl);
            //                }
            //                else
            //                {
            //                    tanggal = DateTime.FromOADate(Double.Parse(tgl));
            //                }

            //            }

            //            if (!String.IsNullOrEmpty(nama_peserta) && String.IsNullOrEmpty(email))
            //            {
            //                TempData["err"] = "Error! Kolom email wajib diisi!";
            //                return RedirectToAction("Index");
            //            }

            //            if (!String.IsNullOrEmpty(nama_peserta))
            //            {
            //                list.Add(new PESERTA
            //                {
            //                    ID_EVENT = id_event,
            //                    NAMA_PESERTA = nama_peserta,
            //                    PERAN = peran,
            //                    NO_SERTIFIKAT = no_sertifikat,
            //                    TGL_DAFTAR = tanggal,
            //                    BARCODE = barcode + tempcode.ToString().PadLeft(3, '0'),
            //                    INSTITUSI = institusi,
            //                    EMAIL = email
            //                });
            //            }
            //        }

            //        var simpan = dao.SimpanPeserta(list);
            //        if (simpan.status)
            //        {
            //            TempData["suc"] = "Berhasil mengupload data";
            //        }
            //        else
            //        {
            //            TempData["err"] = "Error! " + simpan.pesan;
            //        }
            //    }
            //}

            return RedirectToAction("Index");
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

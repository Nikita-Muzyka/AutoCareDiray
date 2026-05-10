using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;

namespace AutoCareDiray.Shared.Interface
{
    public interface IPdfService
    {
        Task<Result> CreatePdfStateAsync(Vehicle vehicle, DateTime startDate, DateTime endDate); // формирования отчета пдф
        Task<Result> OpenPdfAsync(string pdfFile); // открытие файла
        Result DeletePdf(string pdfFile); // удаление пдф файла
    }
}

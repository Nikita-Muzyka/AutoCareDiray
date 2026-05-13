using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using QuestPDF.Fluent;
using QuestPDF.Elements;
using QuestPDF.Drawing;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoCareDiray.Shared.Interface;

namespace AutoCareDiray.Service.PDF
{
    public class PdfService : IPdfService
    {
        public async Task<Result> CreatePdfStateAsync(Vehicle vehicle, DateTime startDate, DateTime endDate)
        {
            try
            {
               
                var filteredRepairs = vehicle.Repairs
                    .Where(r => r.DateRepair >= startDate && r.DateRepair <= endDate)
                    .OrderBy(r => r.DateRepair)
                    .ToList();

               
                decimal totalSpent = filteredRepairs.Sum(r => r.Cost);

               
                string fileName = $"Отчет_{vehicle.StateNumber}_{DateTime.Now:ddMMyy}.pdf";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

               
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1.5f, Unit.Centimetre);
                        page.PageColor(QuestPDF.Helpers.Colors.White);

                  
                        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("sans-serif"));

                       
                        page.Header().Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("ИСТОРИЯ ОБСЛУЖИВАНИЯ АВТОМОБИЛЯ")
                                    .FontSize(20).SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);

                                column.Item().Text($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}");
                            });
                        });

                         
                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(20);

                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(info =>
                                {
                                    info.Item().Text("Информация об автомобиле").SemiBold().FontSize(16).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                                    info.Item().PaddingTop(5).Text($"Название авто: {vehicle.NameVehicle}");
                                    info.Item().Text($"VIN: {vehicle.VinCode}");
                                    info.Item().Text($"Гос. номер: {vehicle.StateNumber}");
                                    info.Item().Text($"Тип кузова: {vehicle.VehicleType}");
                                    info.Item().Text($"Коробка: {vehicle.TransmissionType}");
                                    info.Item().Text($"Текущий пробег: {vehicle.Mileage:N0} км").SemiBold();
                                });

                                if (!string.IsNullOrEmpty(vehicle.PhotoVehicle) && File.Exists(vehicle.PhotoVehicle))
                                {
                                  
                                     
                                    row.ConstantItem(150).MaxHeight(120).AlignRight().Image(vehicle.PhotoVehicle).FitArea();
                                }
                            });

                            
                            column.Item().Background(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(10).Row(row =>
                            {
                                row.RelativeItem().Text($"Отчетный период: с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}").SemiBold();
                                row.RelativeItem().AlignRight().Text($"Всего потрачено: {totalSpent:N2} руб.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Red.Darken2);
                            });

                           
                            column.Item().Text("История обслуживания:").SemiBold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);

                           
                            if (filteredRepairs.Any())
                            {
                                column.Item().Table(table =>
                                {
                                    table.ColumnsDefinition(columns =>
                                    {
                                        columns.ConstantColumn(80);  // 1. Дата
                                        columns.RelativeColumn();    // 2. Название
                                        columns.ConstantColumn(80);  // 3. Пробег
                                        columns.ConstantColumn(70);  // 4. Цена
                                        columns.ConstantColumn(100);  // 5. Где была выполнена работа
                                    });

                                    table.Header(header =>
                                    {
                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).Text("Дата").SemiBold();
                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).Text("Выполненные работы").SemiBold();
                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).Text("Пробег на момент ремонта (км)").SemiBold();
                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).AlignRight().Text("Цена (руб.)").SemiBold();
                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).AlignRight().Text("Выполнение работ").SemiBold();
                                    });

                                  
                                    foreach (var repair in filteredRepairs)
                                    {
                                   
                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).Text($"{repair.DateRepair:dd.MM.yyyy}");
                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).Text(repair.RepairType.TitleRepair ??
                                            "Без названия");
                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).Text($"{repair.CurrentMileage:N0}");
                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).AlignRight().Text($"{repair.Cost:N2}");
                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).AlignRight().Text($"{repair.Job}");
                                    }
                                });
                            }
                            else
                            {
                                column.Item().PaddingTop(10).Text("За выбранный период записи о ремонте отсутствуют.").Italic().FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
                            }
                        });

                       
                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Сгенерировано в AutoCareDiary | Страница ");
                            x.CurrentPageNumber();
                            x.Span(" из ");
                            x.TotalPages();
                        });
                    });
                }).GeneratePdf(filePath);

               
               await Launcher.Default.OpenAsync(new OpenFileRequest
               {
                   Title = "Отчет по авто",
                   File = new ReadOnlyFile(filePath)
               });

                return Result<string>.SuccessCreate(filePath);
            }
            catch (Exception ex)
            {
                
                return Result.ErrorCreate($"Ошибка создания PDF: {ex.StackTrace}");
            }
        } // формирования отчета пдф

//        public async Task<Result> CreatePdfStateAsync(Vehicle vehicle, DateTime startDate, DateTime endDate)
//        {
//            try
//            {
//                // 1. ФИЛЬТРУЕМ ремонты по выбранным датам
//                var filteredRepairs = vehicle.Repairs
//                    .Where(r => r.DateRepair >= startDate && r.DateRepair <= endDate)
//                    .OrderBy(r => r.DateRepair)
//                    .ToList();

//                decimal totalSpent = filteredRepairs.Sum(r => r.Cost);

//                // Используем CacheDirectory и добавляем время, чтобы файлы не "склеивались"
//                //string fileName = $"Отчет_{vehicle.StateNumber}_{DateTime.Now:ddMMyy_HHmmss}.pdf";
//                //string filePath = Path.Combine(FileSystem.CacheDirectory, fileName);

//                // 2. ВАЖНО: Присваиваем документ переменной, А НЕ генерируем его сразу
//                var document = Document.Create(container =>
//                {
//                    container.Page(page =>
//                    {
//                        page.Size(PageSizes.A4);
//                        page.Margin(1.5f, Unit.Centimetre);
//                        page.PageColor(QuestPDF.Helpers.Colors.White);

//                        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("sans-serif"));

//                        // === ШАПКА ДОКУМЕНТА ===
//                        page.Header().Row(row =>
//                        {
//                            row.RelativeItem().Column(column =>
//                            {
//                                column.Item().Text("ИСТОРИЯ ОБСЛУЖИВАНИЯ АВТОМОБИЛЯ")
//                                    .FontSize(20).SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);
//                                column.Item().Text($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}");
//                            });
//                        });

//                        // === ОСНОВНОЕ СОДЕРЖИМОЕ ===
//                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
//                        {
//                            column.Spacing(20);

//                            // Блок 1: Информация об автомобиле
//                            column.Item().Row(row =>
//                            {
//                                row.RelativeItem().Column(info =>
//                                {
//                                    info.Item().Text("Информация об автомобиле").SemiBold().FontSize(16).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
//                                    info.Item().PaddingTop(5).Text($"Название авто: {vehicle.NameVehicle}");
//                                    info.Item().Text($"Год выпуска: {vehicle.YearCreate.Year}");
//                                    info.Item().Text($"VIN: {vehicle.VinCode}");
//                                    info.Item().Text($"Гос. номер: {vehicle.StateNumber}");
//                                    info.Item().Text($"Тип кузова: {vehicle.VehicleType}");
//                                    info.Item().Text($"Коробка: {vehicle.TransmissionType}");
//                                    info.Item().Text($"Текущий пробег: {vehicle.Mileage:N0} км").SemiBold();
//                                });

                               
//                            });

//                            // Блок 2: Сводка по ремонту (с датами)
//                            column.Item().Background(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(10).Row(row =>
//                            {
//                                row.RelativeItem().Text($"Отчетный период: с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}").SemiBold();
//                                row.RelativeItem().AlignRight().Text($"Всего потрачено: {totalSpent:N2} руб.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Red.Darken2);
//                            });

//                            // Блок 3: Таблица ремонтов
//                            column.Item().Text("История обслуживания:").SemiBold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);

//                            if (filteredRepairs.Any())
//                            {
//                                column.Item().Table(table =>
//                                {
//                                    table.ColumnsDefinition(columns =>
//                                    {
//                                        columns.ConstantColumn(80);  // 1. Дата
//                                        columns.RelativeColumn();    // 2. Название
//                                        columns.ConstantColumn(80);  // 3. Пробег
//                                        columns.ConstantColumn(70);  // 4. Цена
//                                        columns.ConstantColumn(100); // 5. Где была выполнена работа
//                                    });

//                                    table.Header(header =>
//                                    {
//                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).Text("Дата").SemiBold();
//                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).Text("Выполненные работы").SemiBold();
//                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).Text("Пробег на момент ремонта (км)").SemiBold();
//                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).AlignRight().Text("Цена (руб.)").SemiBold();
//                                        header.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Black).Padding(2).AlignRight().Text("Выполнение работ").SemiBold();
//                                    });

//                                    foreach (var repair in filteredRepairs)
//                                    {
//                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).Text($"{repair.DateRepair:dd.MM.yyyy}");
//                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).Text(repair.RepairType.TitleRepair ?? "Без названия");
//                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).Text($"{repair.CurrentMileage:N0}");
//                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).AlignRight().Text($"{repair.Cost:N2}");
//                                        table.Cell().BorderBottom(1).BorderColor(QuestPDF.Helpers.Colors.Grey.Lighten2).Padding(4).AlignRight().Text($"{repair.Job}");
//                                    }
//                                });
//                            }
//                            else
//                            {
//                                column.Item().PaddingTop(10).Text("За выбранный период записи о ремонте отсутствуют.").Italic().FontColor(QuestPDF.Helpers.Colors.Grey.Medium);
//                            }
//                        });

//                        // === ПОДВАЛ ДОКУМЕНТА ===
//                        page.Footer().AlignCenter().Text(x =>
//                        {
//                            x.Span("Сгенерировано в AutoCareDiary | Страница ");
//                            x.CurrentPageNumber();
//                            x.Span(" из ");
//                            x.TotalPages();
//                        });
//                    });
//                }); // КОНЕЦ СОЗДАНИЯ (Убрали GeneratePdf отсюда)

//                // 3. ГЕНЕРИРУЕМ БАЙТЫ
//                byte[] pdfBytes = await Task.Run(() => document.GeneratePdf());

//                if (pdfBytes == null || pdfBytes.Length < 500)
//                {
//                    await Shell.Current.DisplayAlert("Ошибка", "Пустой файл", "ОК");
//                    return Result.ErrorCreate("Пустой файл");
//                }

//                string fileName = $"Отчет_{vehicle.StateNumber}_{DateTime.Now:ddMMyy_HHmmss}.pdf";
//                string filePath = "";

//                // ==== СПЕЦИАЛЬНЫЙ КОД ТОЛЬКО ДЛЯ ANDROID ====
//#if ANDROID
//        // Получаем прямой путь к публичной папке "Загрузки" (Downloads) на телефоне
//        string downloadsPath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryDownloads).AbsolutePath;
//        filePath = Path.Combine(downloadsPath, fileName);
//#else
//                // Для iOS или Windows оставляем стандартный кэш
//                filePath = Path.Combine(FileSystem.CacheDirectory, fileName);
//#endif
//                // ============================================

//                // 4. ЗАПИСЫВАЕМ ФАЙЛ
//                // Используем обычный синхронный метод для надежности закрытия потока
//                File.WriteAllBytes(filePath, pdfBytes);

//                // 5. ВЫВОДИМ ТАБЛИЧКУ УСПЕХА
//                await Shell.Current.DisplayAlert(
//                    "Готово!",
//                    $"Отчет успешно сохранен!\nИщите его в папке 'Загрузки' (Downloads).\nПуть: {filePath}",
//                    "Супер");

//                // 6. Пробуем открыть (если Xiaomi опять заблокирует, файл всё равно уже лежит в Загрузках!)
//                await Launcher.Default.OpenAsync(new OpenFileRequest
//                {
//                    Title = "Отчет по авто",
//                    File = new ReadOnlyFile(filePath)
//                });

//                return Result<string>.SuccessCreate(filePath);
//            }
//            catch (Exception ex)
//            {
//                await Shell.Current.DisplayAlert("КРАШ", $"Ошибка: {ex.Message}", "ОК");
//                return Result.ErrorCreate($"Ошибка: {ex.Message}");
//            }
//        }
        
        public async Task<Result> OpenPdfAsync(string pdfFile)
        {
            try
            {
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    Title = "Отчет по авто",
                    File = new ReadOnlyFile(pdfFile)
                });

                return Result.SuccessCreate();
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Ошибка при открытии файла {ex}");
            }
        } // открытие файла
        public Result DeletePdf(string pdfFile)
        {
            if (File.Exists(pdfFile) == false) return Result.ErrorCreate("PDF файла не существует");
            File.Delete(pdfFile);
            return Result.SuccessCreate();
        } // удаление пдф файла
    } 
} 

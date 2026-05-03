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
                // 1. ФИЛЬТРУЕМ ремонты по выбранным датам (и сортируем по дате)
                var filteredRepairs = vehicle.Repairs
                    .Where(r => r.DateRepair >= startDate && r.DateRepair <= endDate)
                    .OrderBy(r => r.DateRepair)
                    .ToList();

                // Считаем сумму только по ОТФИЛЬТРОВАННЫМ ремонтам
                decimal totalSpent = filteredRepairs.Sum(r => r.Cost);

                // Генерируем уникальное имя файла
                string fileName = $"Отчет_{vehicle.StateNumber}_{DateTime.Now:ddMMyy}.pdf";
                string filePath = Path.Combine(FileSystem.AppDataDirectory, fileName);

                // 2. Начинаем рисовать документ
                Document.Create(container =>
                {
                    container.Page(page =>
                    {
                        page.Size(PageSizes.A4);
                        page.Margin(1.5f, Unit.Centimetre);
                        page.PageColor(QuestPDF.Helpers.Colors.White);

                        // ВАЖНО: Убрал Fonts.Arial. На телефонах Android/iOS его часто нет по умолчанию,
                        // из-за чего QuestPDF может вылетать. Пусть использует стандартный системный шрифт.
                        page.DefaultTextStyle(x => x.FontSize(11).FontFamily("sans-serif"));

                        // === ШАПКА ДОКУМЕНТА ===
                        page.Header().Row(row =>
                        {
                            row.RelativeItem().Column(column =>
                            {
                                column.Item().Text("ИСТОРИЯ ОБСЛУЖИВАНИЯ АВТОМОБИЛЯ")
                                    .FontSize(20).SemiBold().FontColor(QuestPDF.Helpers.Colors.Blue.Darken3);

                                column.Item().Text($"Дата формирования: {DateTime.Now:dd.MM.yyyy HH:mm}");
                            });
                        });

                        // === ОСНОВНОЕ СОДЕРЖИМОЕ ===
                        page.Content().PaddingVertical(1, Unit.Centimetre).Column(column =>
                        {
                            column.Spacing(20);

                            // Блок 1: Информация об автомобиле
                            column.Item().Row(row =>
                            {
                                row.RelativeItem().Column(info =>
                                {
                                    info.Item().Text("Информация об автомобиле").SemiBold().FontSize(16).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);
                                    info.Item().PaddingTop(5).Text($"Название авто: {vehicle.NameVehicle}");
                                    info.Item().Text($"Год выпуска: {vehicle.YearCreate.Year}");
                                    info.Item().Text($"VIN: {vehicle.VinCode}");
                                    info.Item().Text($"Гос. номер: {vehicle.StateNumber}");
                                    info.Item().Text($"Тип кузова: {vehicle.VehicleType}");
                                    info.Item().Text($"Коробка: {vehicle.TransmissionType}");
                                    info.Item().Text($"Текущий пробег: {vehicle.Mileage:N0} км").SemiBold();
                                });

                                if (!string.IsNullOrEmpty(vehicle.PhotoVehicle) && File.Exists(vehicle.PhotoVehicle))
                                {
                                    // Убрали жесткий Height(100).
                                    // Дали ширину 150, ограничили максимальную высоту и сказали "Вписаться в эти рамки" (FitArea).
                                    // AlignRight прижмет фото красиво к правому краю, если оно будет узким.
                                    row.ConstantItem(150).MaxHeight(120).AlignRight().Image(vehicle.PhotoVehicle).FitArea();
                                }
                            });

                            // Блок 2: Сводка по ремонту (с датами)
                            column.Item().Background(QuestPDF.Helpers.Colors.Grey.Lighten3).Padding(10).Row(row =>
                            {
                                row.RelativeItem().Text($"Отчетный период: с {startDate:dd.MM.yyyy} по {endDate:dd.MM.yyyy}").SemiBold();
                                row.RelativeItem().AlignRight().Text($"Всего потрачено: {totalSpent:N2} руб.").SemiBold().FontColor(QuestPDF.Helpers.Colors.Red.Darken2);
                            });

                            // Блок 3: Таблица ремонтов
                            column.Item().Text("История обслуживания:").SemiBold().FontSize(14).FontColor(QuestPDF.Helpers.Colors.Blue.Darken2);

                            // Проверяем ОТФИЛЬТРОВАННЫЙ список
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

                                    // Бежим по отфильтрованному списку
                                    foreach (var repair in filteredRepairs)
                                    {
                                        // ВАЖНО: Должно быть ровно 4 ячейки (Cell), как и колонок!
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

                        // === ПОДВАЛ ДОКУМЕНТА ===
                        page.Footer().AlignCenter().Text(x =>
                        {
                            x.Span("Сгенерировано в AutoCareDiary | Страница ");
                            x.CurrentPageNumber();
                            x.Span(" из ");
                            x.TotalPages();
                        });
                    });
                }).GeneratePdf(filePath);

                // Открываем созданный PDF
                await Launcher.Default.OpenAsync(new OpenFileRequest
                {
                    Title = "Отчет по авто",
                    File = new ReadOnlyFile(filePath)
                });

                return Result<string>.SuccessCreate(filePath);
            }
            catch (Exception ex)
            {
                // Если что-то пойдет не так, мы вернем ошибку, а не уроним приложение
                return Result.ErrorCreate($"Ошибка создания PDF: {ex.Message}");
            }
        } // формирования отчета пдф
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

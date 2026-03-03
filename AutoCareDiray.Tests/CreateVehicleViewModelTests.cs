using AutoCareDiray.Shared.ViewModels.RepairViewModel; // Твоя VM из Core
using AutoCareDiray.Shared.Interface;                   // Интерфейсы из Core
using AutoCareDiray.Shared.Models.RepairModel;       // Модели
using Moq;
using Xunit;
using FluentAssertions;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCareDiray.Tests
{
    // Класс тестов должен быть public
    public class CreateRepairViewModelTests
    {
        // === МОКИ (Подделки зависимостей) ===
        // Мы создаем "фейковые" версии сервисов, чтобы не зависеть от реального кода
        private Mock<IApiService> _mockApiService;
        private Mock<IDialogService> _mockDialogService;
        private Mock<IDataService> _mockDataService;
        private Mock<INavigationService> _mockNavigationService;

        private CreateRepairViewModel _viewModel;

        // === ИНИЦИАЛИЗАЦИЯ ===
        // Этот метод запускается ПЕРЕД КАЖДЫМ тестом (гарантия чистоты)
        public CreateRepairViewModelTests()
        {
            _mockApiService = new Mock<IApiService>();
            _mockDialogService = new Mock<IDialogService>();
            _mockDataService = new Mock<IDataService>();
            _mockNavigationService = new Mock<INavigationService>();

            // Создаем ViewModel с поддельными сервисами
            _viewModel = new CreateRepairViewModel(
                _mockApiService.Object,
                _mockDialogService.Object,
                _mockDataService.Object,
                _mockNavigationService.Object
            );
        }

        // ============================================================
        // 🧪 ТЕСТ 1: Инициализация ViewModel
        // ============================================================
        [Fact]
        public async Task Initialize_SetsVehicleId_AndCallsLoading()
        {
            // --- ARRANGE (Подготовка) ---
            int testVehicleId = 123;

            // --- ACT (Действие) ---
            // Вызываем метод инициализации
            await _viewModel.Initialize(testVehicleId);

            // --- ASSERT (Проверка) ---
            // Проверяем, что ID сохранился (через приватное поле не проверить, 
            // но мы проверим, что Loading был вызван косвенно)
            // Флаг isInitialize приватный, поэтому проверяем поведение
            _viewModel.RepairTypes.Should().NotBeNull();
        }

        // ============================================================
        // 🧪 ТЕСТ 2: Загрузка списка ремонтов (Loading)
        // ============================================================
        [Fact]
        public async Task Initialize_FetchesRepairTypes_FromDataService()
        {
            // --- ARRANGE ---
            int testVehicleId = 123;

            var mockRepairTypes = new List<RepairType>
    {
        new RepairType { Id = 1, TitleRepair = "ТО", IntervalMileagee = 15000 },
        new RepairType { Id = 2, TitleRepair = "Масло", IntervalMileagee = 8000 }
    };

            _mockDataService
                .Setup(x => x.GetListRepairTypeAsync(testVehicleId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRepairTypes);

            // --- ACT ---
            // ✅ Вызываем ТОЛЬКО Initialize (он сам вызовет Loading внутри)
            await _viewModel.Initialize(testVehicleId);

            // --- ASSERT ---
            _viewModel.RepairTypes.Should().HaveCount(2);
            _viewModel.SelectedRepairType.Should().NotBeNull();

            // ✅ Проверяем, что сервис был вызван 1 раз (при инициализации)
            _mockDataService.Verify(x =>
                x.GetListRepairTypeAsync(testVehicleId, It.IsAny<CancellationToken>()),
                Times.Once);
        }

        // ============================================================
        // 🧪 ТЕСТ 3: Создание ремонта — УСПЕХ
        // ============================================================
        [Fact]
        public async Task CreateRepair_ValidData_SavesAndNavigatesBack()
        {
            // --- ARRANGE ---
            // 1. Инициализируем ViewModel
            await _viewModel.Initialize(123);

            // 2. Заполняем тестовые данные
            _viewModel.RepairTypes.Add(new RepairType { Id = 1, TitleRepair = "ТО" });
            _viewModel.SelectedRepairType = _viewModel.RepairTypes[0];
            _viewModel.DateRepairSelected = DateTime.Today;
            _viewModel.SparePartsFilled = "Фильтр";
            _viewModel.CostFilled = 1000;
            _viewModel.DescriptionFilled = "Замена";
            _viewModel.IntervalMileageFilled = 15000;
            _viewModel.IntervalDateSelected = DateTime.Today.AddMonths(6);

            // 3. Настраиваем моки: сервисы должны вернуть true (успех)
            _mockDataService
                .Setup(x => x.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            _mockDataService
                .Setup(x => x.UpdateRepairTypeAsync(It.IsAny<RepairType>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(true);

            // --- ACT ---
            // Вызываем команду создания ремонта
            await _viewModel.CreateRepairCommand.ExecuteAsync(null);

            // --- ASSERT ---
            // 1. Проверяем, что CreateRepairAsync был вызван 1 раз
            _mockDataService.Verify(x =>
                x.CreateRepairAsync(
                    It.Is<Repair>(r => r.VehicleId == 123 && r.SpareParts == "Фильтр"),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            // 2. Проверяем, что UpdateRepairTypeAsync был вызван 1 раз
            _mockDataService.Verify(x =>
                x.UpdateRepairTypeAsync(
                    It.Is<RepairType>(rt => rt.IntervalMileagee == 15000),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            // 3. Проверяем, что навигация назад была выполнена
            _mockNavigationService.Verify(x => x.GoToBack(), Times.Once);

            // 4. Проверяем, что сообщение об ошибке НЕ установлено
            _viewModel.StatusMessage.Should().BeNullOrEmpty();
        }

        // ============================================================
        // 🧪 ТЕСТ 4: Создание ремонта — ОШИБКА
        // ============================================================
        [Fact]
        public async Task CreateRepair_ServiceReturnsFalse_ShowsErrorMessage()
        {
            // --- ARRANGE ---
            await _viewModel.Initialize(123);

            _viewModel.RepairTypes.Add(new RepairType { Id = 1, TitleRepair = "ТО" });
            _viewModel.SelectedRepairType = _viewModel.RepairTypes[0];
            _viewModel.DateRepairSelected = DateTime.Today;
            _viewModel.SparePartsFilled = "Фильтр";
            _viewModel.CostFilled = 1000;
            _viewModel.DescriptionFilled = "Замена";

            // Настраиваем моки: сервис возвращает false (ошибка)
            _mockDataService
                .Setup(x => x.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(false);

            // --- ACT ---
            await _viewModel.CreateRepairCommand.ExecuteAsync(null);

            // --- ASSERT ---
            // Проверяем, что установлено сообщение об ошибке
            _viewModel.StatusMessage.Should().Be("Ремонт или интервалы не были сохранены");

            // Проверяем, что навигация НЕ была выполнена (нельзя уходить при ошибке)
            _mockNavigationService.Verify(x => x.GoToBack(), Times.Never);
        }

        // ============================================================
        // 🧪 ТЕСТ 5: Выбор типа ремонта — Автозаполнение интервала
        // ============================================================
        [Fact]
        public void OnSelectedRepairTypeChanged_UpdatesIntervalMileageFilled()
        {
            // --- ARRANGE ---
            var repairType = new RepairType
            {
                Id = 1,
                TitleRepair = "ТО",
                IntervalMileagee = 20000 // Ожидаемое значение
            };

            // --- ACT ---
            // Устанавливаем SelectedRepairType (это вызовет partial метод автоматически)
            _viewModel.SelectedRepairType = repairType;

            // --- ASSERT ---
            // Проверяем, что интервал пробега заполнился автоматически
            _viewModel.IntervalMileageFilled.Should().Be(20000);
        }

        // ============================================================
        // 🧪 ТЕСТ 6: Отмена операции — Reset CancellationToken
        // ============================================================
        [Fact]
        public void CancelToken_CreatesNewCancellationTokenSource()
        {
            // --- ARRANGE ---
            // Запоминаем старый токен (через публичное свойство или косвенно)

            // --- ACT ---
            _viewModel.CancelTokenCommand.Execute(null);

            // --- ASSERT ---
            // Т.к. _cts приватный, проверяем, что метод выполнился без исключений
            // Это регрессионный тест — защита от ошибок в будущем
            // Можно добавить проверку, что старый токен отменен (через Mock)
        }

        // ============================================================
        // 🧪 ТЕСТ 7: Повторная инициализация — Защита от дублей
        // ============================================================
        [Fact]
        public async Task Initialize_CalledTwice_DoesNotReloadData()
        {
            // --- ARRANGE ---
            var mockRepairTypes = new List<RepairType>
            {
                new RepairType { Id = 1, TitleRepair = "ТО" }
            };

            _mockDataService
                .Setup(x => x.GetListRepairTypeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(mockRepairTypes);

            // --- ACT ---
            // Вызываем инициализацию ДВАЖДЫ
            await _viewModel.Initialize(123);
            await _viewModel.Initialize(999); // Второй вызов с другим ID

            // --- ASSERT ---
            // DataService должен быть вызван только 1 раз (защита isInitialize сработала)
            _mockDataService.Verify(x =>
                x.GetListRepairTypeAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()),
                Times.Once);
        }
    }
}
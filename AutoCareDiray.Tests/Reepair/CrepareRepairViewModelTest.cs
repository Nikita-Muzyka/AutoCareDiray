namespace AutoCareDiray.Tests;

using AutoCareDiray.Shared.Extensions.StringEx;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Models.Validation;
using AutoCareDiray.Shared.Models.VehicleModel;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.ViewModels.RepairViewModel;
using FluentAssertions;
using Moq;
using System.Collections.ObjectModel;
using Xunit;

public class CreateRepairViewModelTests
{
    // Мок-объекты для зависимостей ViewModel
    private readonly Mock<IDialogService> _mockDialog = new();
    private readonly Mock<IDataService> _mockData = new();
    private readonly Mock<INavigationService> _mockNav = new();
    private readonly RepairValidation _validation = new(); // Реальный объект валидации
    private readonly Mock<IPhotoPicker> _mockPhoto = new();
    private readonly Mock<IPreferencesService> _mockPrefs = new();

    /// <summary>
    /// Фабрика для создания ViewModel с настроенными моками.
    /// Позволяет переиспользовать код во всех тестах.
    /// </summary>
    private CreateRepairViewModel CreateViewModel()
    {
        // Настраиваем мок диалога по умолчанию (для ShowOptions)
        _mockDialog.Setup(d => d.ShowDisplayAction()).ReturnsAsync("Удалить");

        return new CreateRepairViewModel(
            _mockDialog.Object,
            _mockData.Object,
            _mockNav.Object,
            _validation,
            _mockPhoto.Object,
            _mockPrefs.Object);
    }

    #region Тесты для CreateSparePart

    [Fact]
    public void CreateSparePart_WhenValidDataProvided_ShouldAddPartAndClearFields()
    {
        // ARRANGE
        var viewModel = CreateViewModel();
        const string testName = "Масляный фильтр";
        const string testArticle = "OF-12345";
        const decimal testCost = 500m;
        const string testCurrency = "₽";

        viewModel.CreateNamePart = testName;
        viewModel.CreateArticleNumberPart = testArticle;
        viewModel.CreateCostPart = testCost;
        viewModel.SelectedCurrencySignPart = testCurrency;

        // ACT
        viewModel.CreateSparePart();

        // ASSERT
        viewModel.ListSpareParts.Should().HaveCount(1);

        var addedPart = viewModel.ListSpareParts[0];
        addedPart.NamePart.Should().Be(testName);
        addedPart.ArticleNumberPart.Should().Be(testArticle);
        addedPart.CostPart.Should().Be(testCost);
        addedPart.CurrentCurrencySing.Should().Be(testCurrency);

        // Поля ввода должны очиститься (бизнес-требование)
        viewModel.CreateNamePart.Should().BeEmpty();
        viewModel.CreateArticleNumberPart.Should().BeEmpty();
        viewModel.CreateCostPart.Should().Be(0);
    }

    #endregion

    #region Тесты для ShowOptions

    [Fact]
    public async Task ShowOptions_WhenUserSelectsDelete_ShouldRemovePartFromList()
    {
        // ARRANGE
        var viewModel = CreateViewModel();
        var testSpare = new SparePart
        {
            NamePart = "Масляный фильтр",
            ArticleNumberPart = "OF-12345",
            CostPart = 500,
            CurrentCurrencySing = "₽"
        };
        viewModel.ListSpareParts.Add(testSpare);

        // ACT (мок по умолчанию возвращает "Удалить")
        await viewModel.ShowOptions(testSpare);

        // ASSERT
        viewModel.ListSpareParts.Should().BeEmpty();
    }

    [Fact]
    public async Task ShowOptions_WhenUserSelectsEdit_ShouldCopyDataToFieldsAndRemovePart()
    {
        // ARRANGE
        // Перенастраиваем мок на возврат "Редактировать"
        _mockDialog.Setup(d => d.ShowDisplayAction()).ReturnsAsync("Редактировать");
        var viewModel = CreateViewModel();

        var testSpare = new SparePart
        {
            NamePart = "Масляный фильтр",
            ArticleNumberPart = "OF-12345",
            CostPart = 500,
            CurrentCurrencySing = "₽"
        };
        viewModel.ListSpareParts.Add(testSpare);

        // ACT
        await viewModel.ShowOptions(testSpare);

        // ASSERT
        // В новом коде деталь удаляется при редактировании (чтобы добавить заново после изменений)
        viewModel.ListSpareParts.Should().BeEmpty();

        // Данные скопированы в поля ввода
        viewModel.CreateNamePart.Should().Be("Масляный фильтр");
        viewModel.CreateArticleNumberPart.Should().Be("OF-12345");
        viewModel.CreateCostPart.Should().Be(500);
    }

    #endregion

    #region Тесты для DeleteAttachPhoto

    [Fact]
    public void DeleteAttachPhoto_WhenPhotoExists_ShouldRemoveItFromCollection()
    {
        // ARRANGE
        var viewModel = CreateViewModel();
        const string photoPath = "linkOnPhoto";
        viewModel.AttachedPhotos.Add(photoPath);

        // ACT — вызываем ИМЕННО ТОТ метод, который тестируем
        viewModel.DeleteAttachPhoto(photoPath);

        // ASSERT
        viewModel.AttachedPhotos.Should().BeEmpty();
    }

    [Fact]
    public void DeleteAttachPhoto_WhenPhotoDoesNotExist_ShouldNotThrow()
    {
        // ARRANGE
        var viewModel = CreateViewModel();
        viewModel.AttachedPhotos.Add("existing_photo");

        // ACT — удаляем фото, которого нет (граничный случай)
        var act = () => viewModel.DeleteAttachPhoto("non_existing_photo");

        // ASSERT — не должно быть исключения
        act.Should().NotThrow();
        viewModel.AttachedPhotos.Should().HaveCount(1);
    }

    #endregion

    #region Тесты для CreateNewRepairType

    [Fact]
    public async Task CreateNewRepairType_WhenTitleAlreadyExists_ShouldShowToastAndNotAddType()
    {
        // ARRANGE
        var vm = CreateViewModel();
        var existingType = new RepairType { TitleRepair = "Pop" };
        vm.RepairTypes.Add(existingType);
        vm.TitleNewRepairType = "Pop";

        // ACT
        await vm.CreateNewRepairType();

        // ASSERT
        // Проверяем, что был показан тост
        _mockDialog.Verify(d => d.ShowToastAsync("Данный тип ремонта уже существует"), Times.Once);

        // Тип НЕ должен был добавиться
        vm.RepairTypes.Should().HaveCount(1);
    }

    [Fact]
    public async Task CreateNewRepairType_WhenTitleIsUnique_ShouldCreateNewType()
    {
        // ARRANGE
        var vm = CreateViewModel();
        vm.TitleNewRepairType = "Замена тормозных колодок";
        vm.SelectedCategory = "Салон авто";
        vm.IntervalMileageNewType = 30000;
        vm.IntervalMonthNewType = 12;

        // ACT
        await vm.CreateNewRepairType();

        // ASSERT
        vm.RepairTypes.Should().HaveCount(1);

        var createdType = vm.RepairTypes[0];
        createdType.TitleRepair.Should().Be("Замена тормозных колодок");
        createdType.IntervalMileage.Should().Be(30000);
        createdType.IntervalMonth.Should().Be(12);
        createdType.Category.Should().Be(RepairCategory.Salon);

        // Новый тип должен стать выбранным
        vm.SelectedRepairType.Should().Be(createdType);
    }

    #endregion

    #region Тесты для ProcessingRepair

    /// <summary>
    /// Если валидация провалилась — сервисы не должны вызываться.
    /// </summary>
    [Fact]
    public async Task ProcessingRepair_WhenValidationFails_ShouldNotCallDataService()
    {
        // ARRANGE
        var vm = CreateViewModel();

        // Заполняем поля НЕВАЛИДНЫМИ данными, чтобы реальная валидация провалилась
        vm.SelectedRepairType = null;          // ← Ошибка: тип не выбран
        vm.SelectedJob = string.Empty;         // ← Ошибка: пустая работа
        vm.MileageFilled = -100;               // ← Ошибка: отрицательный пробег
        vm.CostFilled = -500;                  // ← Ошибка: отрицательная цена

        // ACT
        await vm.ProcessingRepair();

        // ASSERT — ни один метод DataService не должен быть вызван
        _mockData.Verify(d => d.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockData.Verify(d => d.UpdateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockData.Verify(d => d.CreateRepairTypeAsync(It.IsAny<RepairType>(), It.IsAny<CancellationToken>()), Times.Never);

        // Навигация тоже не должна вызваться
        _mockNav.Verify(n => n.GoToBack(), Times.Never);

        // Должны быть установлены флаги ошибок
        vm.IsMileageError.Should().BeTrue();
        vm.IsCostError.Should().BeTrue();
        vm.IsJobError.Should().BeTrue();
        vm.IsSelectedRepairTypeError.Should().BeTrue();
    }

    /// <summary>
    /// Успешное создание нового ремонта — happy path.
    /// </summary>
    [Fact]
    public async Task ProcessingRepair_WhenCreatingNewRepair_ShouldCallCreateAndNavigateBack()
    {
        // ARRANGE
        var vm = CreateViewModel();

        // Настраиваем моки на успешные результаты
        _mockData
            .Setup(d => d.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Repair> { Success = true });

        _mockData
            .Setup(d => d.UpdateRepairTypeAsync(It.IsAny<RepairType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<RepairType> { Success = true });

        _mockData
            .Setup(d => d.GetVehicleMileageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Vehicle> { Success = true, Data = new Vehicle { Mileage = 5000 } });

        _mockData
            .Setup(d => d.UpdateVehicleMileageAsync(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result { Success = true });

        // Заполняем поля ВАЛИДНЫМИ данными
        vm.SelectedRepairType = new RepairType { Id = 1, TitleRepair = "Замена масла", IntervalMileage = 10000, IntervalMonth = 6 };
        vm.SelectedJob = "Самостоятельно";
        vm.MileageFilled = 10000;
        vm.CostFilled = 500;

        // ACT
        await vm.ProcessingRepair();

        // ASSERT
        _mockData.Verify(d => d.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockData.Verify(d => d.UpdateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockNav.Verify(n => n.GoToBack(), Times.Once);

        vm.StatusMessage.Should().BeNullOrWhiteSpace();
    }

    /// <summary>
    /// Ошибка при создании ремонта — StatusMessage должен содержать ошибку, навигация не вызывается.
    /// </summary>
    [Fact]
    public async Task ProcessingRepair_WhenCreateRepairFails_ShouldSetStatusMessageAndNotNavigate()
    {
        // ARRANGE
        var vm = CreateViewModel();
        const string errorMessage = "Не удалось создать ремонт";

        _mockData
            .Setup(d => d.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Repair> { Success = false, ErrorMessage = errorMessage });

        vm.SelectedRepairType = new RepairType { Id = 1, TitleRepair = "Замена масла", IntervalMileage = 10000, IntervalMonth = 6 };
        vm.SelectedJob = "Самостоятельно";
        vm.MileageFilled = 10000;
        vm.CostFilled = 500;

        // ACT
        await vm.ProcessingRepair();

        // ASSERT
        vm.StatusMessage.Should().Contain(errorMessage);
        _mockNav.Verify(n => n.GoToBack(), Times.Never);
    }

    /// <summary>
    /// Режим обновления ремонта (когда _isUpdateRepair = true).
    /// Вызываем LoadingUpdate() перед тестом, чтобы установить флаг.
    /// </summary>
    [Fact]
    public async Task ProcessingRepair_WhenUpdatingRepair_ShouldCallUpdateInsteadOfCreate()
    {
        // ARRANGE
        var vm = CreateViewModel();

        // Настраиваем мок для LoadingUpdate (чтобы установить _isUpdateRepair = true)
        var existingRepair = new Repair
        {
            Id = 42,
            RepairType = new RepairType { Id = 1, TitleRepair = "Замена масла", IntervalMileage = 10000, IntervalMonth = 6 },
            VehicleId = 1,
            Cost = 500,
            CurrentMileage = 10000,
            DateRepair = DateTime.UtcNow
        };

        _mockData
            .Setup(d => d.GetRepairAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Repair> { Success = true, Data = existingRepair });

        // Моки для ProcessingRepair
        _mockData
            .Setup(d => d.UpdateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Repair> { Success = true });

        _mockData
            .Setup(d => d.UpdateRepairTypeAsync(It.IsAny<RepairType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<RepairType> { Success = true });

        _mockData
            .Setup(d => d.GetVehicleMileageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Vehicle> { Success = true, Data = new Vehicle { Mileage = 5000 } });

        _mockData
            .Setup(d => d.UpdateVehicleMileageAsync(It.IsAny<int>(), It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result { Success = true });

        // Вызываем LoadingUpdate, чтобы установить _isUpdateRepair = true
        await vm.LoadingUpdate();

        // Заполняем поля для ProcessingRepair
        vm.SelectedJob = "Самостоятельно";
        vm.MileageFilled = 15000;
        vm.CostFilled = 700;

        // ACT
        await vm.ProcessingRepair();

        // ASSERT — должен вызваться UpdateRepairAsync, а не CreateRepairAsync
        _mockData.Verify(d => d.UpdateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()), Times.Once);
        _mockData.Verify(d => d.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()), Times.Never);
        _mockNav.Verify(n => n.GoToBack(), Times.Once);
    }

    /// <summary>
    /// Прикрепление фото — SavePhotosAsync должен быть вызван.
    /// </summary>
    [Fact]
    public async Task ProcessingRepair_WhenPhotosAttached_ShouldSavePhotos()
    {
        // ARRANGE
        var vm = CreateViewModel();

        // Добавляем фото в коллекцию
        vm.AttachedPhotos.Add("photo1.jpg");
        vm.AttachedPhotos.Add("photo2.jpg");

        // Настраиваем моки
        _mockPhoto
            .Setup(p => p.SavePhotosAsync(It.IsAny<ObservableCollection<string>>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<List<string>>
            {
                Success = true,
                Data = new List<string> { "saved1.jpg", "saved2.jpg" }
            });

        _mockData
            .Setup(d => d.CreateRepairAsync(It.IsAny<Repair>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Repair> { Success = true });

        _mockData
            .Setup(d => d.UpdateRepairTypeAsync(It.IsAny<RepairType>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<RepairType> { Success = true });

        _mockData
            .Setup(d => d.GetVehicleMileageAsync(It.IsAny<int>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result<Vehicle> { Success = true, Data = new Vehicle { Mileage = 5000 } });

        _mockData
            .Setup(d => d.UpdateVehicleMileageAsync(It.IsAny<int>(),It.IsAny<double>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new Result { Success = true });

        vm.SelectedRepairType = new RepairType { Id = 1, TitleRepair = "Замена масла", IntervalMileage = 10000, IntervalMonth = 6 };
        vm.SelectedJob = "Самостоятельно";
        vm.MileageFilled = 10000;
        vm.CostFilled = 500;

        // ACT
        await vm.ProcessingRepair();

        // ASSERT — SavePhotosAsync должен быть вызван ровно 1 раз
        _mockPhoto.Verify(p => p.SavePhotosAsync(It.IsAny<ObservableCollection<string>>(), It.IsAny<CancellationToken>()), Times.Once);
    }

    #endregion
}
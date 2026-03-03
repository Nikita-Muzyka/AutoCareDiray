using AutoCareDiray.ViewModels.VehicleViewModel;
using AutoCareDiray.Models.Validation;
using AutoCareDiray.Service;
using AutoCareDiray.Service.Navigation;
using AutoCareDiray.Service.Data;
using Moq;
using Xunit;
using FluentAssertions;
using System.Threading;
using System.Threading.Tasks;

namespace AutoCareDiray.Tests
{
    public class CreateVehicleViewModelTests
    {
        // Моки зависимостей
        private Mock<IApiService> _mockApiService;
        private Mock<IDialogService> _mockDialogService;
        private Mock<IDataService> _mockDataService;
        private Mock<INavigationService> _mockNavigation;
        private VehicleValidation _vehicleValidation;

        private CreateVehicleViewModel _viewModel;

        public CreateVehicleViewModelTests()
        {
            // Инициализация моков перед каждым тестом
            _mockApiService = new Mock<IApiService>();
            _mockDialogService = new Mock<IDialogService>();
            _mockDataService = new Mock<IDataService>();
            _mockNavigation = new Mock<INavigationService>();
            _vehicleValidation = new VehicleValidation(); // Реальный объект валидации

            _viewModel = new CreateVehicleViewModel(
                _mockApiService.Object,
                _mockDialogService.Object,
                _mockDataService.Object,
                _mockNavigation.Object,
                _vehicleValidation
            );
        }

        #region Тесты валидации пробега

        [Fact]
        public void ValidationMileage_ValidNumber_NoErrors()
        {
            // Arrange
            _viewModel.Mileage = "15000";

            // Act
            _vehicleValidation.ValidationMileage(_viewModel.Mileage);

            // Assert
            _viewModel.HasErrors.Should().BeFalse();
            _vehicleValidation.GetErrors(nameof(_viewModel.Mileage)).Should().BeNullOrEmpty();
        }

        [Theory]
        [InlineData("")]
        [InlineData("abc")]
        [InlineData("12.34")]
        [InlineData("-100")]
        public void ValidationMileage_InvalidInput_HasErrors(string invalidMileage)
        {
            // Arrange
            _viewModel.Mileage = invalidMileage;

            // Act
            _vehicleValidation.ValidationMileage(_viewModel.Mileage);

            // Assert
            _viewModel.HasErrors.Should().BeTrue();
            _vehicleValidation.GetErrors(nameof(_viewModel.Mileage)).Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Тесты валидации дат

        [Fact]
        public void ValidationDate_PurchaseAfterCreate_NoErrors()
        {
            // Arrange
            var create = new DateTime(2020, 1, 1);
            var purchase = new DateTime(2022, 1, 1);

            // Act
            _vehicleValidation.ValidationDate(purchase, create);

            // Assert
            _vehicleValidation.GetErrors(nameof(_viewModel.YearPurchaseSelected)).Should().BeNullOrEmpty();
        }

        [Fact]
        public void ValidationDate_PurchaseBeforeCreate_HasErrors()
        {
            // Arrange
            var create = new DateTime(2022, 1, 1);
            var purchase = new DateTime(2020, 1, 1); // Ошибка: покупка раньше создания

            // Act
            _vehicleValidation.ValidationDate(purchase, create);

            // Assert
            _vehicleValidation.GetErrors(nameof(_viewModel.YearPurchaseSelected)).Should().NotBeNullOrEmpty();
        }

        #endregion

        #region Тесты команды CreateVehicle

        [Fact]
        public async Task CreateVehicleAsync_ValidData_CallsDataServiceAndNavigatesBack()
        {
            // Arrange
            _viewModel.NameVehicle = "Toyota Camry";
            _viewModel.Mileage = "50000";
            _viewModel.YearCreateSelected = new DateTime(2020, 1, 1);
            _viewModel.YearPurchaseSelected = new DateTime(2022, 1, 1);
            _viewModel.SelectedTypeVehicle = "Автомобиль";

            // Мокаем DataService: метод CreateVehicleAsync должен выполниться успешно
            _mockDataService
                .Setup(x => x.CreateVehicleAsync(It.IsAny<Shared.Models.VehicleModel.Vehicle>(), It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);

            // Act
            await _viewModel.CreateVehicleAsync(); // Если оставили async void — используйте await Task.Run(() => _viewModel.CreateVehicle())

            // Assert
            // 1. DataService был вызван ровно 1 раз
            _mockDataService.Verify(x =>
                x.CreateVehicleAsync(
                    It.Is<Shared.Models.VehicleModel.Vehicle>(v =>
                        v.NameVehicle == "Toyota Camry" &&
                        v.Mileage == 50000),
                    It.IsAny<CancellationToken>()),
                Times.Once);

            // 2. Навигация назад была вызвана
            _mockNavigation.Verify(x => x.GoToBack(), Times.Once);

            // 3. Статус сообщения не содержит ошибок
            _viewModel.StatusMessage.Should().NotContain("ошибка", Because: "данные валидны");
        }

        [Fact]
        public async Task CreateVehicleAsync_InvalidMileage_DoesNotCallDataService()
        {
            // Arrange
            _viewModel.Mileage = "invalid"; // Некорректный пробег
            _vehicleValidation.ValidationMileage(_viewModel.Mileage);

            // Act
            await _viewModel.CreateVehicleAsync();

            // Assert
            // DataService НЕ должен быть вызван, т.к. есть ошибки валидации
            _mockDataService.Verify(x =>
                x.CreateVehicleAsync(It.IsAny<Shared.Models.VehicleModel.Vehicle>(), It.IsAny<CancellationToken>()),
                Times.Never);

            // Навигация НЕ должна произойти
            _mockNavigation.Verify(x => x.GoToBack(), Times.Never);
        }

        #endregion

        #region Тесты свойств и коллекций

        [Fact]
        public void TypeVehicle_ContainsExpectedValues()
        {
            // Arrange & Act
            var types = _viewModel.TypeVehicle;

            // Assert
            types.Should().Contain("Автомобиль", "Мотоцикл", "Грузовое ТС", "Другое");
            types.Should().HaveCount(4);
        }

        [Fact]
        public void DefaultValues_AreSetCorrectly()
        {
            // Assert
            _viewModel.DateNow.Date.Should().Be(DateTime.Today.Date);
            _viewModel.YearCreateSelected.Date.Should().Be(DateTime.Today.Date);
        }

        #endregion
    }
} 
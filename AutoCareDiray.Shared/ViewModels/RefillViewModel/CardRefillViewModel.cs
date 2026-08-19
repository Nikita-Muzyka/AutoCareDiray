using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Shared.Models.RefillModel;
using AutoCareDiray.Shared.Models.VehicleModel;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using System.IO;

namespace AutoCareDiray.Shared.ViewModels.RefillViewModel
{
    public partial class CardRefillViewModel : BaseViewModel
    {
        #region Поля и приватные данные

        private CancellationTokenSource _cts;
        private bool _isInitialized = false;
        private int _refillId = -1;

        #endregion

        #region Коллекции для UI

        // Выделяем фото в отдельную коллекцию для безопасного биндинга
        public ObservableCollection<string> AttachedPhotos { get; set; } = new();

        #endregion

        #region Observable-свойства (данные)

        [ObservableProperty] private Refill refillCard = new();
        [ObservableProperty] private string statusMessage;
        [ObservableProperty] private string mileageUnit = "км"; // Единицы измерения по умолчанию

        #endregion

        #region Observable-свойства (флаги UI)

        [ObservableProperty] private bool hasPhotos;
        [ObservableProperty] private bool hasDescription;

        #endregion

        public CardRefillViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        #region Инициализация и загрузка данных

        [RelayCommand]
        public async Task Initialize(IDictionary<string, object> query)
        {
            if (_isInitialized) return;

            // Даем странице 300мс на плавное открытие без фризов
            await Task.Delay(300);

            if (query.TryGetValue("RefillId", out var refillIdObj) && refillIdObj is int refillId)
            {
                _refillId = refillId;
                await LoadRefillDataAsync();
            }
            else
            {
                StatusMessage = "Ошибка: ID заправки не найден.";
            }
        }

        private async Task LoadRefillDataAsync()
        {
            var result = await _dataService.GetRefillAsync(_refillId, _cts.Token);

            if (result is not Result<Refill> refillResult)
            {
                StatusMessage = result.ErrorMessage ?? "Не удалось загрузить данные о заправке.";
                return;
            }

            RefillCard = refillResult.Data;

            // Запрашиваем авто, чтобы получить единицы измерения пробега (км или мили)
            var vehicleResult = await _dataService.GetVehicleAsync(RefillCard.VehicleId, _cts.Token);
            if (vehicleResult is Result<Vehicle> vehResult && vehResult.Data != null)
            {
                MileageUnit = vehResult.Data.UnitDistance ?? "км";
            }

            // Устанавливаем флаги для скрытия пустых блоков в XAML
            HasDescription = !string.IsNullOrWhiteSpace(RefillCard.Description);
            PopulatePhotos();

            _isInitialized = true;
        }

        private void PopulatePhotos()
        {
            AttachedPhotos.Clear();
            if (RefillCard.Photos != null && RefillCard.Photos.Any())
            {
                foreach (var photo in RefillCard.Photos)
                {
                    if (File.Exists(photo))
                        AttachedPhotos.Add(photo);
                }
                HasPhotos = AttachedPhotos.Any();
            }
            else
            {
                HasPhotos = false;
            }
        }

        #endregion

        #region Команды управления

        [RelayCommand]
        public async Task EditRefill()
        {
            var navigationParameter = new Dictionary<string, object>
            {
                { "RefillId", _refillId }
            };
            await _navigationService.GoNavigation("CreateRefillView", navigationParameter);
        }

        [RelayCommand]
        public void CancelToken()
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = new CancellationTokenSource();
        }

        #endregion
    }
}
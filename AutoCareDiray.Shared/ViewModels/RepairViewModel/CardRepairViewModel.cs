using AutoCareDiray.Shared.Interface;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AutoCareDiray.Shared.Models.RepairModel;
using AutoCareDiray.Shared.Service.ResultService;
using System.Collections.ObjectModel;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Threading;

namespace AutoCareDiray.Shared.ViewModels.RepairViewModel
{
    public partial class CardRepairViewModel : BaseViewModel
    {
        #region Поля и приватные данные

        private CancellationTokenSource _cts;
        private bool _isInitialized = false;
        private int _repairId = -1;

        #endregion

        #region Коллекции для UI

        public ObservableCollection<SparePart> SpareParts { get; set; } = new();
        public ObservableCollection<string> AttachedPhotos { get; set; } = new();

        #endregion

        #region Observable-свойства (данные)

        [ObservableProperty] private Repair repair;
        [ObservableProperty] private RepairType repairType;
        [ObservableProperty] private string statusMessage;

        #endregion

        #region Observable-свойства (флаги UI)

        [ObservableProperty] private bool hasSpareParts;
        [ObservableProperty] private bool hasPhotos;
        [ObservableProperty] private bool isServiceJob;

        #endregion

        public CardRepairViewModel(IDialogService dialog, IDataService data, INavigationService navigate)
            : base(dialog, data, navigate)
        {
            _cts = new CancellationTokenSource();
        }

        #region Инициализация и загрузка данных

        [RelayCommand]
        public async Task Initialize(IDictionary<string, object> query)
        {
            if (_isInitialized) return;

            // Считываем ID ремонта так же, как в CreateRepairViewModel
            if (query.TryGetValue("RepairId", out var repairIdObj) && repairIdObj is int repairId)
            {
                _repairId = repairId;
                await LoadRepairDataAsync();
            }
            else
            {
                StatusMessage = "Ошибка: ID ремонта не найден.";
            }
        }

        private async Task LoadRepairDataAsync()
        {
            var result = await _dataService.GetRepairAsync(_repairId, _cts.Token);

            // Безопасный кастинг и проверка на ошибки
            if (result is not Result<Repair> repairResult)
            {
                StatusMessage = result.ErrorMessage ?? "Не удалось загрузить данные о ремонте.";
                return;
            }

            Repair = repairResult.Data;
            RepairType = Repair.RepairType;

            // Определяем, нужно ли показывать название сервиса и комментарий механика
            IsServiceJob = Repair.Job == "Сервис";

            // Заполняем списки для UI и ставим флаги видимости
            PopulateSpareParts();
            PopulatePhotos();

            _isInitialized = true;
        }

        private void PopulateSpareParts()
        {
            SpareParts.Clear();
            if (Repair.SpareParts != null && Repair.SpareParts.Any())
            {
                foreach (var part in Repair.SpareParts)
                    SpareParts.Add(part);

                HasSpareParts = true;
            }
            else
            {
                HasSpareParts = false;
            }
        }

        private void PopulatePhotos()
        {
            AttachedPhotos.Clear();
            if (Repair.Photos != null && Repair.Photos.Any())
            {
                foreach (var photo in Repair.Photos)
                    AttachedPhotos.Add(photo);

                HasPhotos = true;
            }
            else
            {
                HasPhotos = false;
            }
        }

        #endregion

        #region Команды управления

        [RelayCommand]
        public async Task EditRepair()
        {
            // Переход на страницу редактирования текущего ремонта
            var navigationParameter = new Dictionary<string, object>
            {
                { "RepairId", _repairId }
            };
            await _navigationService.GoNavigation("CreateRepairView", navigationParameter);
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
using AutoCareDiray.Service.Dialog;
using AutoCareDiray.Shared.Interface;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Service.PhotoPicker
{
    public class PhotoPicker : IPhotoPicker
    {
        private readonly IDialogService _dialogService;
        public PhotoPicker(IDialogService dialogService)
        {
            _dialogService = dialogService;
        }
        public async Task<string> PickPhotoAsync()
        {
            string resultaction = await _dialogService.ShowDisplayAction("Выберите действие", "Отмена", "Сделать снимок", "Выбрать из галереи");

            FileResult photo = null;

            try
            {
                if (resultaction == "Сделать снимок")
                {
                    if (MediaPicker.Default.IsCaptureSupported)
                    {
                        photo = await MediaPicker.Default.CapturePhotoAsync();
                    }
                    else
                    {
                        Debug.WriteLine("Съемка не поддерживается на этом устройстве.");
                    }
                }
                else if (resultaction == "Выбрать из галереи")
                {
                    // 1. Проверяем, есть ли у нас уже разрешение
                    var status = await Permissions.CheckStatusAsync<Permissions.Photos>();

                    // 2. Если нет, запрашиваем его у пользователя (появится системное окошко)
                    if (status != PermissionStatus.Granted)
                    {
                        status = await Permissions.RequestAsync<Permissions.Photos>();
                    }

                    // 3. Если пользователь нажал "Разрешить", открываем галерею
                    if (status == PermissionStatus.Granted)
                    {
                        photo = await MediaPicker.Default.PickPhotoAsync();
                    }
                    else
                    {
                        await Shell.Current.DisplayAlert("Отказ", "Без разрешения мы не сможем загрузить фото машины", "ОК");
                        return null;
                    }
                }

                if (photo == null) return string.Empty;

                string newFileName = $"vehiclePhoto_{Guid.NewGuid()}{Path.GetExtension(photo.FileName)}";
                string locationPhoto = Path.Combine(FileSystem.AppDataDirectory, newFileName);

                using (Stream source = await photo.OpenReadAsync())
                {
                    using (FileStream locationFile = File.OpenWrite(locationPhoto))
                    {
                        await source.CopyToAsync(locationFile);
                    }
                }

                return locationPhoto;

            }
            catch (Exception ex)
            {
                await _dialogService.ShowToastAsync($"Ошибка при добавлении фото {ex}");
                return string.Empty;
            }
        }
    }
}

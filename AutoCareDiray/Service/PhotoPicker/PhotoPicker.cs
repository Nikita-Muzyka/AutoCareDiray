using AutoCareDiray.Shared.Service.ResultService;
using AutoCareDiray.Service.Dialog;
using AutoCareDiray.Shared.Interface;
using AutoCareDiray.Shared.Service.ResultService;
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
        public async Task<Result> PickPhotoAsync()
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
                    }
                }

                return photo?.FullPath != null ? Result<string>.SuccessCreate(photo.FullPath) : Result.ErrorCreate("Фото не найдено");
            }
            catch (Exception ex)
            {
                await _dialogService.ShowToastAsync($"Ошибка при добавлении фото {ex}");
                return Result.ErrorCreate($"Ошибка при добавлении фото {ex}");
            }
        } //выбрать фото

        public async Task<Result> SavePhotoAsync(string photoLink, CancellationToken token)
        {
            try
            {
                if (photoLink == null) return Result.ErrorCreate("Фото не найдено");

                string newFileName = $"vehiclePhoto_{Guid.NewGuid()}{Path.GetExtension(photoLink)}";
                string locationPhoto = Path.Combine(FileSystem.AppDataDirectory, newFileName);

                token.ThrowIfCancellationRequested();
                using (Stream source = File.OpenRead(photoLink))
                {
                    using (FileStream locationFile = File.OpenWrite(locationPhoto))
                    {
                        await source.CopyToAsync(locationFile);
                    }
                }

                return Result<string>.SuccessCreate(locationPhoto);
            }
            catch(Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при сохранении фото {ex}");
            }
        } //сохранить фото

        public async Task<Result> SavePhotosAsync(IEnumerable<string> photstringoLink, CancellationToken token) 
        {
            try
            {
                if (photstringoLink == null) return Result.ErrorCreate("Фотографии не найдены");

                var photosList = new List<string>();

                token.ThrowIfCancellationRequested();

                foreach (var photo in photstringoLink)
                {
                    string newFileName = $"PhotosRepair{Guid.NewGuid()}{Path.GetExtension(photo)}";
                    string locationPhoto = Path.Combine(FileSystem.AppDataDirectory, newFileName);

                    using (Stream source = File.OpenRead(photo))
                    {
                        using (FileStream locationFile = File.OpenWrite(locationPhoto))
                        {
                            await source.CopyToAsync(locationFile,token);
                            photosList.Add(locationPhoto);
                        }
                    }
                }

                
                return Result<List<string>>.SuccessCreate(photosList);
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Произошла ошибка при сохранении фотографий {ex}");
            }
        } //сохранить фото

        public async Task<Result> PickPhotosAsync()
        {
            try
            {
                var list = new List<string>();
                var result = await FilePicker.Default.PickMultipleAsync(new PickOptions
                {
                    PickerTitle = "Выберите фотографии",
                    FileTypes = FilePickerFileType.Images
                });

                if (result == null) return Result.ErrorCreate("Ошибка при выборе фото");

                foreach (var photo in result)
                {
                    list.Add(photo.FullPath);
                }

                return Result<List<string>>.SuccessCreate(list);
            }
            catch (Exception ex)
            {
                return Result.ErrorCreate($"Ошибка при выборе фото {ex}");
            }
        }
    }
}

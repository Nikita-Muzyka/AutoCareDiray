using AutoCareDiray.Shared.Service.ResultService;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Interface
{
    public interface IPhotoPicker
    {
        Task<Result> PickPhotoAsync(); //выбрать фото
        Task<Result> SavePhotoAsync(string photstringoLink,CancellationToken token); //сохранить фото

        Task<Result> SavePhotosAsync(IEnumerable<string> photstringoLink, CancellationToken token); //сохранить фото
        Task<Result> PickPhotosAsync(); //выбрать много фото

        Task<Result> DeletePhoto(string oldPhoto);
    }
}


# Основные Классы AutoCareDiray 

> Последнее обновление: 12 февраля 2026  

## Models
### Vehicle
| Поле            | Тип         | Обязательное?  | Описание             |
|-----------------|-------------|----------------|----------------------|
| Id              | `int`       | Да (Авто)      | Первичный ключ       |
| Name            | `string`    | Нет            | Название авто        |
| Mileage         | `int`       | Да             | Пробег (км)          |
| DatePurchase    | `DateOnly`  | Нет            | Дата покупки         |
| DateProduction  | `DateOnly`  | Нет            | Дата производства    |

## Views
- ListVehicleView.xaml
- CreateVehicleView.xaml
- CardVehicleView.xaml

## ViewModels
- BaseViewModel _Базовый класс для VM_
- ListVehicleViewModel
- CreateVehicleViewModel
- CardVehicleViewModel

## Services
- IValidationService, ValidationService
- IApiService,ApiService
- IDialogService,DialogService

## Data
- AppDBContex

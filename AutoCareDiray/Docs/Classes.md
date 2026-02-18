# Основные Классы AutoCareDiray 

> Последнее обновление: 12 февраля 2026  

## Models
### Vehicle
| Поле            | Тип         | Обязательное?  | Описание             |
|-----------------|-------------|----------------|----------------------|
| Vehicle_Id      | `int`       | Да (Авто)      | Первичный ключ       |
| Name            | `string`    | Нет            | Название авто        |
| Mileage         | `int`       | Да             | Пробег (км)          |
| DatePurchase    | `DateOnly`  | Нет            | Дата покупки         |
| DateProduction  | `DateOnly`  | Нет            | Дата производства    |

### Repair
| Поле            | Тип         | Обязательное?  | Описание             |
|-----------------|-------------|----------------|----------------------|
| Repair_Id       | `int`       | Да (Авто)      | Первичный ключ       |
| Vehicle_Id      | `int`       | Да             | Вторичный ключ       |
| DateRepair      | `DateOnly`  | Нет            | Дата ремонта         |
| Mileage         | `int`       | Нет            | Пробег (км)          |
| SpareParts      | `string`    | Нет            | Запчасти             |
| Cost            | `int`       | Нет            | Стоимости            |
| Description     | `string`    | Нет            | Описание работ       |

## Views
- ListVehicleView.xaml
- CreateVehicleView.xaml
- CardVehicleView.xaml

- CardVehicleView.xaml

## ViewModels
- BaseViewModel _Базовый класс для VM_
- ListVehicleViewModel
- CreateVehicleViewModel
- CardVehicleViewModel

- RepairVehicleViewModel

## Services
- IValidationService, ValidationService
- IApiService,ApiService
- IDialogService,DialogService
- IDataService,DataService
- INavigationService,NavigationService

## Data
- AppDBContex

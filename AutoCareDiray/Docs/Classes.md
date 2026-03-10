# Основные Классы AutoCareDiray 

> Последнее обновление: 12 февраля 2026  

## Models
### Vehicle
| Поле               | Тип         | Обязательное?  | Описание             |
|--------------------|-------------|----------------|----------------------|
| Id                 | `int`       | Да (Авто)      | Первичный ключ       |
| Name               | `string`    | Нет            | Название авто        |
| VehicleType        | `string`    | Нет            | ТИп авто             |
| Mileage            | `int`       | Да             | Пробег (км)          |
| DatePurchase       | `DateOnly`  | Нет            | Дата покупки         |
| DateProduction     | `DateOnly`  | Нет            | Дата производства    |

**Навигация**
- public List"Repair" Repait {get;set;} = new()
- public List'RepairTypes' RepairTypes {get;set;} = new()

### Repair
| Поле            | Тип         | Обязательное?  | Описание                         |
|-----------------|-------------|----------------|----------------------------------|
| Id              | `int`       | Да (Авто)      | Первичный ключ                   |
| VehicleId       | `int`       | Да             | Вторичный ключ                   |
| CurrentMileage  | `int`       | Да             | Пробег на момомент создания      |
| DateRepair      | `DateOnly`  | Нет            | Дата ремонта                     |
| RepairTypeId    | `int`       | Да             | Вторичный ключ                   |
| SpareParts      | `string`    | Нет            | Запчасти                         |
| Cost            | `int`       | Нет            | Стоимости                        |
| Description     | `string`    | Нет            | Описание работ                   |

**Навигация**
- public Vehicle Vehicle {get;set;}
- public RepairType  RepairType {get;set;}

### RepairTypes
| Поле            | Тип              | Обязательное?  | Описание             |
|-----------------|------------------|----------------|----------------------|
| Id              | `int`            | Да             | Первичный ключ       |
| TitleRepair     | `string`         | Да             | Название ремонта     |
| IntervalMileage | `int`            | Да             | Интервал пробега     |
| IntervalDate    | `DateOnly`       | Да             | Интервал даты        |
| VehicleId       | `int`            | Да             | ID Vehicle           |

**Навигация**
- public Vehicle Vehicle {get;set;}
- public List 'Repair' Repairs {get;set;} = new()


- VehicleValidation
- RepairValidation


## Views
- ListVehicleView.xaml
- CreateVehicleView.xaml
- CardVehicleView.xaml

- ListRepairView.xaml
- CreateRepairView.xaml
- CardRepairView.xaml

## ViewModels
- BaseViewModel _Базовый класс для VM_

- ListVehicleViewModel
- CreateVehicleViewModel
- CardVehicleViewModel

- ListRepairViewModel
- CreateRepairViewModel
- CardRepairViewModel

## Services
- IValidationService, ValidationService
- IApiService,ApiService
- IDialogService,DialogService
- IDataService,DataService
- INavigationService,NavigationService

## Data
- AppDBContex

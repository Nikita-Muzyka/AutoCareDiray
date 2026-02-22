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
**Навигация**
- public Repair Repait {get;set;} 

### Repair
| Поле            | Тип         | Обязательное?  | Описание             |
|-----------------|-------------|----------------|----------------------|
| Repair_Id       | `int`       | Да (Авто)      | Первичный ключ       |
| Vehicle_Id      | `int`       | Да             | Вторичный ключ       |
| DateRepair      | `DateOnly`  | Нет            | Дата ремонта         |
| TypeRepair_Id   | `int`       | Да             | Вторичный ключ       |
| SpareParts      | `string`    | Нет            | Запчасти             |
| Cost            | `int`       | Нет            | Стоимости            |
| Description     | `string`    | Нет            | Описание работ       |

### RepairTypes
| Поле            | Тип              | Обязательное?  | Описание             |
|-----------------|------------------|----------------|----------------------|
| TypeRepair_Id   | `int`            | Да             | Первичный ключ       |
| TitleRepair     | `string`         | Да             | Название ремонта     |
| IntervalMileage | `int`            | Да             | Интервал пробега     |
| IntervalDate    | `DateOnly`       | Да             | Интервал даты        |

### ListRepairTypes
| Поле            | Тип                 | Обязательное?  | Описание             |
|-----------------|---------------------|----------------|----------------------|
| ListRepair_Id   | `int`               | Да             | Первичный ключ       |
| ListRepairTypes | `List<RepairTypes>` | Да             | Список классов ремонта    |

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

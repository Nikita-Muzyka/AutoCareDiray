# Основные Классы AutoCareDiray 

> Последнее обновление: 12 февраля 2026  

## Models
### Vehicle
| Поле               | Тип         | Обязательное?  | Описание             |
|--------------------|-------------|----------------|----------------------|
| Id                 | `int`       | Да (Авто)      | Первичный ключ       |
| Name               | `string`    | нет            | Название авто        |
| VehicleType        | `string`    | Нет            | ТИп авто             |
| Mileage            | `int`       | Да             | Пробег (км)          |
| VinCode            | `string`    | Нет            | Вин код              |
| StateNumber        | `string`    | Нет            | Гос номер            |
| TransmissionType   | `string`    | Нет            | Вин код              |
| DatePurchase       | `DateOnly`  | default        | Дата покупки         |
| DateProduction     | `DateOnly`  | default        | Дата производства    |

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
| Поле                                | Тип              | Обязательное?  | Описание                      |
|-------------------------------------|------------------|----------------|-------------------------------|
| Id                                  | `int`            | Да             | Первичный ключ                |
| TitleRepair                         | `string`         | Да             | Название ремонта              |
| Category                            | `string`         | Да             | Категория для группировки     |
| IntervalMileage                     | `int`            | Да             | Интервал пробега              |
| LastServiceMileage                  | `int`            | default = 0    | Последний пробег при ремонте  |
| IntervalDate                        | `DateOnly`       | Нет            | Интервал даты                 |
| IsServiced                          | `bool`           | false          | Обслужена деталь              |
| RemoveMaintenance                   | `bool`           | false          | Удалить запись                |
| VehicleId                           | `int`            | Да             | ID Vehicle                    |

**Навигация**
- public Vehicle Vehicle {get;set;}
- public List 'Repair' Repairs {get;set;} = new()


### RepairGroup : List<RepairType>
| Поле            | Тип              | Обязательное?  | Описание             |
|-----------------|------------------|----------------|----------------------|
| Name            | `string`         | Да             | Имя группы           |
| IsExpanded      | `bool`           | falsed          | Название ремонта     |

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

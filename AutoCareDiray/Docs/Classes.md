# Основная логика классов для обновления

        private ObservableCollection<RepairGroup> CreateRepairTypeGroups()
        {
            var groups = new ObservableCollection<RepairGroup>
    {
        new RepairGroup("Регулярное ТО", new List<RepairType>
        {
            // Самое частое. Масло - раз в год или 10к, Фильтры - вместе с ним
            new("Масло в двигателе и Масляный фильтр", "Регулярное ТО", 10000, 12),
            new("Воздушный фильтр двигателя", "Регулярное ТО", 20000, 24),
            new("Салонный фильтр", "Регулярное ТО", 15000, 12),
            new("Топливный фильтр", "Регулярное ТО", 40000, 48)
        }),

        new RepairGroup("Тормозная система", new List<RepairType>
        {
            new("Тормозная жидкость", "Тормозная система", 40000, 24),
            new("Передние тормозные колодки", "Тормозная система", 30000),
            new("Задние тормозные колодки", "Тормозная система", 50000),
            new("Передние тормозные диски + колодки", "Тормозная система", 70000),
            new("Задние тормозные диски + колодки", "Тормозная система", 90000),
            new("Задние барабаны + колодки + тормозной цилиндр", "Тормозная система", 90000),
            new("Обслуживание суппортов (смазка)", "Тормозная система", 30000, 24),
            new("Обслуживание передних тормозов", "Тормозная система", 0),
            new("Обслуживание задних тормозов", "Тормозная система", 0)
        }),

        new RepairGroup("Двигатель", new List<RepairType>
        {
            // Обобщаем ремни и цепи
            new("Свечи зажигания / накаливания", "Двигатель и Зажигание", 40000, 48),
            new("Привод ГРМ (Ремень / Цепь)", "Двигатель и Зажигание", 90000, 60),
            new("Ремни навесного оборудования", "Двигатель и Зажигание", 60000, 60),
        }),

        new RepairGroup("Охлаждение и Климат", new List<RepairType>
        {
            // Разделили помпу и антифриз
            new("Охлаждающая жидкость", "Охлаждение и Климат", 60000, 36),
            new("Водяная помпа (Насос)", "Охлаждение и Климат", 90000, 60),
            new("Промывка радиаторов", "Охлаждение и Климат", 60000, 24),
            new("Обслуживание кондиционера (фреон)", "Охлаждение и Климат", 40000, 24)
        }),

        new RepairGroup("Трансмиссия (Коробка и Привод)", new List<RepairType>
        {
            // Универсальные названия
            new("Масло в коробке передач", "Трансмиссия", 60000, 48),
            new("Фильтр коробки передач", "Трансмиссия", 60000, 48,"Автоматическая"),
            new("Сброс адаптации", "Трансмиссия", 60000, 48,"Автоматическая"),
            new("Масло в редукторе / мосту", "Трансмиссия", 60000, 48),
            new("Масло в раздаточной коробке", "Трансмиссия", 60000, 48),
            new("Сцепление", "Трансмиссия", 100000)
        }),

        new RepairGroup("Подвеска и Рулевое", new List<RepairType>
        {
            new("Жидкость ГУР", "Подвеска и Рулевое", 50000, 36),
            new("Передние амортизаторы", "Подвеска и Рулевое", 80000),
            new("Задние амортизаторы", "Подвеска и Рулевое", 90000),
            new("Стойки и втулки стабилизатора", "Подвеска и Рулевое", 40000),
            new("Сайлентблоки (комплект)", "Подвеска и Рулевое", 80000),
            new("Шаровые опоры", "Подвеска и Рулевое", 70000),
            new("Рулевые наконечники и тяги", "Подвеска и Рулевое", 70000)
        }),

         new RepairGroup("Электрика", new List<RepairType>
        {
            new("Замена/Ремонт ЭБУ", "Электрика", 15000, 12),
        }),

        new RepairGroup("Шины и Колеса", new List<RepairType>
        {
            new("Сход-развал", "Шины и Колеса", 20000, 12),
            new("Балансировка колес", "Шины и Колеса", 10000, 6),
            new("Сезонная смена шин", "Шины и Колеса", 0, 6)
        }),

        new RepairGroup("Кузов и Оптика", new List<RepairType>
        {
            new("Щетки стеклоочистителя", "Кузов и Оптика", 15000, 12),
            new("Обработка кузова (Антикор)", "Кузов и Оптика", 0, 36),
            new("Замена ламп", "Кузов и Оптика")
        })
    };

            return groups;
        }


        _listRepairType.RemoveAll(c => c.IsRemoveMaintenance == true);

         var groups = _vehicle.RepairTypes
     .GroupBy(g => g.Category)
     .Select(g => new RepairGroup(g.Key,g.ToList()))
     .ToList();
 RepairGrouped = new ObservableCollection<RepairGroup>(groups);
 _listRepairType = RepairGrouped.SelectMany(c => c).ToList();



 var groups = CreateRepairTypeGroups();

RepairGrouped = new ObservableCollection<RepairGroup>(groups);

_listRepairType = RepairGrouped.SelectMany(c => c).ToList();
_isInitilized = true;

 [RelayCommand]
 public void ChangeIsServiced()
 {
     foreach (var list in _listRepairType)
     {
         list.IsServiced = !list.IsServiced;
         if(list.IsServiced == true)
         {
             list.LastServiceMileage = Mileage;
         }
     }
 }



  partial void OnTransmissionTypeChanged(string value)
 {
     foreach (var item in _listRepairType)
     {
         if (item.TransmissionType == "Автоматическая" || item.TransmissionType == "Механическая")
         {
             item.IsRemoveMaintenance = item.TransmissionType != value;
         }
     }
 }

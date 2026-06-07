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




   <Button Text="Авто обслужено" HorizontalOptions="Center" Style="{DynamicResource ButtonCustomStandartRed}"
          Command="{Binding ChangeIsServicedCommand}" Margin="0,0,0,10"/>
  <!--Изменение обслужено авто или нет-->

  <Border Style="{DynamicResource BorderForEntryCard}">
      <VerticalStackLayout Padding="15,10">
          <Label Text="КАТЕГОРИЯ ОБСЛУЖИВАНИЯ" Style="{DynamicResource LabelInfoForEntry}"/>

          <Grid>
              <Picker Title="Выберите категорию..." ItemsSource="{Binding RepairGrouped}" ItemDisplayBinding="{Binding Name}" 
                      SelectedItem="{Binding SelectedGroup}" Style="{DynamicResource PickerStandart}"/>
              
              <Label Text="▼" Style="{DynamicResource LabelArrowDown}"/>
          </Grid>
      </VerticalStackLayout>
  </Border> <!--Picker выбора категории деталей-->


    <VerticalStackLayout BindableLayout.ItemsSource="{Binding SelectedGroup}" Spacing="15" Padding="15">
      <BindableLayout.ItemTemplate>
          <DataTemplate x:DataType="models:RepairType">
              <Border Style="{DynamicResource BorderForEntryCard}" >
                  <VerticalStackLayout Padding="15">

                      <Label Text="{Binding TitleRepair}" Style="{DynamicResource LabelCustom}"/> <!--Название тип ремонта-->

                      <Grid ColumnDefinitions="*, *" ColumnSpacing="10">

                          <VerticalStackLayout Grid.Column="0" Spacing="5">
                              <Label Text="Интервал (км)" Style="{DynamicResource LabelInfoForEntry}"/>
                              <Border Style="{DynamicResource BorderForEntryImput}">
                                  <Grid ColumnDefinitions="*, Auto" Padding="5,0">
                                      <Entry Text="{Binding IntervalMileage}" Keyboard="Numeric" Style="{DynamicResource EntryImput}"/>
                                      <Label Grid.Column="1" Text="км" FontSize="11" VerticalOptions="Center" Margin="0,0,5,0"/>
                                  </Grid>
                              </Border>
                          </VerticalStackLayout> <!--пробег-->

                          <VerticalStackLayout Grid.Column="1" Spacing="5">
                              <Label Text="Интервал (мес)" Style="{DynamicResource LabelInfoForEntry}"/>
                              <Border Style="{DynamicResource BorderForEntryImput}">
                                  <Grid ColumnDefinitions="*, Auto" Padding="5,0">
                                      <Entry Text="{Binding IntervalMonth}" Keyboard="Numeric" Style="{DynamicResource EntryImput}"/>
                                      <Label Grid.Column="1" Text="мес" FontSize="11" VerticalOptions="Center" Margin="0,0,5,0"/>
                                  </Grid>
                              </Border>
                          </VerticalStackLayout> <!--месяц-->
                          
                      </Grid> <!--интервалы пробега и мессяца-->

                      <BoxView Style="{DynamicResource BoxViewCustom}" Margin="0,10,0,0"/>

                      <HorizontalStackLayout Spacing="20">
                          
                          <HorizontalStackLayout Spacing="2">
                              <CheckBox IsChecked="{Binding IsServiced}" Style="{DynamicResource CheckBoxCustom}"/>
                              <Label Text="Обслужено" Style="{DynamicResource LabelSecondaryCustom}" VerticalOptions="Center"/>
                          </HorizontalStackLayout>

                          <HorizontalStackLayout Spacing="2">
                              <CheckBox IsChecked="{Binding IsRemoveMaintenance}" Style="{DynamicResource CheckBoxCustom}"/>
                              <Label Text="Не нужно" Style="{DynamicResource LabelSecondaryCustom}" VerticalOptions="Center"/>
                          </HorizontalStackLayout>
                          
                      </HorizontalStackLayout> <!--CheckBox-->

                  </VerticalStackLayout>
              </Border>
          </DataTemplate>
      </BindableLayout.ItemTemplate> <!--ВЫбранные категории ремонта-->
      
  </VerticalStackLayout> <!--Binable для категории ремонта-->


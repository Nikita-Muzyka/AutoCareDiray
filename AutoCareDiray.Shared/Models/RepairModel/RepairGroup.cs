using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.RepairModel
{
    public class RepairGroup : List<RepairType>, INotifyPropertyChanged
    {
        public string Name { get; set; }
        private bool isExpanded = false;
        public bool IsExpanded
        {
            get => isExpanded;
            set
            {
                if (isExpanded != value)
                {
                    isExpanded = value;
                    OnPropertyChanged();
                }
            }
        }

        public RepairGroup(string name,List<RepairType> repairTypes) : base(repairTypes)
        {
            Name = name;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = null) =>
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

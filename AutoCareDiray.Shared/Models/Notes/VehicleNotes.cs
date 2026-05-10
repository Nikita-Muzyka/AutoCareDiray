using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace AutoCareDiray.Shared.Models.Notes
{
    public class VehicleNotes : INotifyPropertyChanged
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int VehicleId { get; set; }

        private string title;
        public string Title
        {
            get => title;
            set
            {
                title = value;
                OnPropertyChanged();
            }
        }

        private string content;
        public string Content 
        { 
            get => content;
            set
            {
                content = value;
                OnPropertyChanged();
            }
        }

        [Required]
        public DateTime DateCreated { get; set; }

        private DateTime dateUpdated;
        public DateTime DateUpdated
        {
            get => dateUpdated;
            set
            {
                dateUpdated = value;
                OnPropertyChanged();
            }
        }



        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName]string name = null) => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}

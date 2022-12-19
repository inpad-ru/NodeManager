using System.Runtime.CompilerServices;
using System.ComponentModel;

namespace NodeManager.Web.Models.Entities
{
    public class RevitElementProperty : INotifyPropertyChanged
    {
        string value;

        public bool IsEditable { get; set; }
        public string PropertyName { get; set; }
        public string PropertyValue
        {
            get => value;
            set
            {
                this.value = value;
                OnPropertyChanged();
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        void OnPropertyChanged([CallerMemberName] string prop = "") =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
    }
}

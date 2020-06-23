using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace MyStateApp
{
    public class MainViewModel : BindableObject
    {
        private string myState = "Open";
        public MainViewModel()
        {
            this.ChangeStateCommand = new Command(ChangeState);
        }

        public string MyState { get => myState; set { myState = value; OnPropertyChanged(); } }

        public ICommand ChangeStateCommand { get; set; }

        private void ChangeState()
        {
            if (this.MyState == "Open")
            {
                this.MyState = "Closed";
            }
            else
            {
                this.MyState = "Open";
            }
        }
    }
}

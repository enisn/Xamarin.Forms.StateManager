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
        private string currentState = "Failed";
        private string content;

        private static readonly Random random = new Random();
        public MainViewModel()
        {
            this.LoadCommand = new Command(Load);
        }

        public string CurrentState { get => currentState; set { currentState = value; OnPropertyChanged(); } }

        public string Content { get => content; set { content = value; OnPropertyChanged(); } }

        public ICommand LoadCommand { get; set; }

        private async void Load()
        {
            this.CurrentState = "Loading";

            await Task.Delay(2000);

            var tmp = random.Next(0, 100);
            if (tmp > 50)
            {
                this.CurrentState = "Failed";
            }
            else
            {
                this.CurrentState = "Loaded";
                this.Content = "This is loaded content.";
            }
        }
    }
}

using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.Data.Json;

namespace REST8.ViewModel
{
    public class JsonViewModel : ViewModelBase
    {
        public JsonViewModel()
        {
            StringUrl = "http://dev-exopoint.exoscale.ch/_list/events";
        }

        private RelayCommand getCommand;

        public ICommand GetCommand
        {
            get
            {
                if (getCommand == null)
                {
                    getCommand = new RelayCommand(() =>
                    {
                        Get();
                    });
                }
                return getCommand;
            }
        }

        public string StringUrl { get; set; }

        public IJsonValue Json
        {
            get;
            set;
        }

        private async void Get()
        {
            try
            {
                var uri = new Uri(StringUrl, UriKind.Absolute);

                var client = new HttpClient();
                var response = await client.GetAsync(uri);
                var responseString = await response.Content.ReadAsStringAsync();

                JsonObject o;
                if (JsonObject.TryParse(responseString, out o))
                {
                    Json = o;
                    RaisePropertyChanged(() => this.Json);
                }
                else
                {
                    ShowErrorMessage("unable to parse resonse");
                }
            }
            catch (Exception ex)
            {
                ShowErrorMessage(ex.Message);
            }
        }

        private async void ShowErrorMessage(string message)
        {
            var dialog = new Windows.UI.Popups.MessageDialog(message);
            await dialog.ShowAsync();
        }
    }
}

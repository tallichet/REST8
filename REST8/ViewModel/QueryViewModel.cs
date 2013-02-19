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
    public class QueryViewModel : ViewModelBase
    {
        private RelayCommand sendCommand;
        private RelayCommand clearCommand;
        private string urlString;
        private List<string> requestHeaders;
        private List<string> responseHeaders;
        private string requestBody;
        private HttpResponseMessage response;
        
        public QueryViewModel()
        {
            StringUrl = "http://dev-exopoint.exoscale.ch/_list/events";
        }

        
        public ICommand SendCommand
        {
            get
            {
                if (sendCommand == null)
                {
                    sendCommand = new RelayCommand(() =>
                    {
                        Get();
                    });
                }
                return sendCommand;
            }
        }

        public ICommand ClearCommand
        {
            get
            {
                if (clearCommand == null)
                {
                    clearCommand = new RelayCommand(() =>
                    {
                        Get();
                    });
                }
                return clearCommand;
            }
        }

        public string StringUrl { get; set; }

        public IJsonValue Json
        {
            get;
            set;
        }

        public string ResponseString
        {
            get; private set;
        }

        private async void Get()
        {
            try
            {
                var uri = new Uri(StringUrl, UriKind.Absolute);

                var client = new HttpClient();
                response = await client.GetAsync(uri);


                ResponseString = await response.Content.ReadAsStringAsync();
                this.RaisePropertyChanged(() => this.ResponseString);
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

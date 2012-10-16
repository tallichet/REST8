using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Windows.ApplicationModel.Resources;
using Windows.Data.Json;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace JsonViewer
{
    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class MainPage : JsonViewer.Common.LayoutAwarePage
    {
        ResourceLoader resources;

        public MainPage()
        {
            this.InitializeComponent();
            resources = new ResourceLoader();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        protected override void LoadState(Object navigationParameter, Dictionary<String, Object> pageState)
        {

        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        protected override void SaveState(Dictionary<String, Object> pageState)
        {

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is IStorageFile)
            {
                Open(e.Parameter as IStorageFile);
            }
            else
            {
                this.BottomAppBar.IsOpen = true;
            }

        }

        public async void Open(object sender, RoutedEventArgs e)
        {
            var picker = new FileOpenPicker();
            picker.FileTypeFilter.Add(".json");
            picker.SuggestedStartLocation = PickerLocationId.Desktop;
            picker.ViewMode = PickerViewMode.List;

            var file = await picker.PickSingleFileAsync();
            
            if (file != null)
            {
                Open(file);
            }
        }

        public async void Open(IStorageFile file) 
        {
            pageTitleFilename.Text = file.Name;

            var text = await FileIO.ReadTextAsync(file);
            IJsonValue json = null;
            JsonObject o; JsonArray a; JsonValue v;
            if (text.TrimStart()[0] == '{' && JsonObject.TryParse(text, out o))
            {
                json = o;
            }
            else if (text.TrimStart()[0] == '[' && JsonArray.TryParse(text, out a))
            {
                json = a;
            }
            else if (JsonValue.TryParse(text, out v))
            {
                json = v;
            }
            else
            {
                await new MessageDialog(resources.GetString("DialogErrorInvalidFile")).ShowAsync();
                return;
            }

            jsonViewerZone.Json = json;
        }
    }
}

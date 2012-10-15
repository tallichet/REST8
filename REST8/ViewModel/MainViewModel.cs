using GalaSoft.MvvmLight;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace REST8.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}

            MainMenu = new ObservableCollection<MenuItem>();

            // Initialise the JsonMenu
            MainMenu.Add(new MenuItem("json", new List<PageInfo>()
            {
                PageInfo.Create<JsonGet>("get")
            }));
        }

        public ObservableCollection<MenuItem> MainMenu { get; private set; }

        /// <summary>
        /// A menu group in the main view
        /// </summary>
        public class MenuItem : IEnumerable<PageInfo>
        {
            public MenuItem(string title, List<PageInfo> pages)
            {
                Title = title;
                Pages = pages;
            }

            public string Title { get; private set; }
            private List<PageInfo> Pages;

            public IEnumerator<PageInfo> GetEnumerator()
            {
                return Pages.GetEnumerator();
            }

            System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }
        }

        /// <summary>
        /// Page details for a clickable element on the menu page
        /// </summary>
        public class PageInfo
        {
            public static PageInfo Create<T>(string name) where T : Windows.UI.Xaml.Controls.Page, new()
            {
                return new PageInfo() {
                    Name = name,
                    PageType = typeof (T)
                };
            }

            public string Name { get; private set; }
            public System.Type PageType { get; private set; }
        }
    }
}
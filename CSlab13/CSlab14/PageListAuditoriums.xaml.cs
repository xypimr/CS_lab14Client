using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CSlab13.HttpRequestData;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageListAuditoriums : ContentPage
    {
        public PageListAuditoriums()
        {
            InitializeComponent();
        }
        // При открытии этой страницы инициализизуется список сборок из базы данных
        protected override void OnAppearing()
        {
            AuditoriumList.ItemsSource = GetAllAuditoriums();
            base.OnAppearing();
        }
        // Обработка кнопки добавления сборки
        private async void CreateAuditorium(object sender, EventArgs e)
        {
            Auditorium auditorium = new Auditorium();
            PageAuditorium pageAuditorium = new PageAuditorium();
            pageAuditorium.BindingContext = auditorium;
            await Navigation.PushAsync(pageAuditorium);
        }
        // Обработка нажатия элемента в списке
        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Auditorium selectedAuditorium = (Auditorium)e.SelectedItem;
            var auditoriumFromId = GetAuditoriumFromId(selectedAuditorium.Id);
            PageAuditorium pageAuditorium = new PageAuditorium();
            pageAuditorium.BindingContext = auditoriumFromId;
            await Navigation.PushAsync(pageAuditorium);
        }
    }
}
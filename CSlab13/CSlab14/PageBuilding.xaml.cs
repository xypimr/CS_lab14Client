#nullable enable
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using static CSlab13.HttpRequestData;

namespace CSlab13
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PageBuilding : ContentPage
    {
        public PageBuilding()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            Building v = (Building)this.BindingContext;
            if (v.Id != 0)
            {
                ListAuditoriumGroupsInBuilding.ItemsSource = GetBuildingFromId(v.Id).AuditoriumGroups;
            }

            base.OnAppearing();
        }

        private void SaveBuilding(object sender, EventArgs e)
        {
            var building = (Building)BindingContext;
            if (!String.IsNullOrEmpty(building.Name))
            {
                if (building.Id == 0)
                    AddBuilding(building);
                else
                {
                    var buildingUpd = GetBuildingFromId(building.Id);
                    buildingUpd.Name = building.Name;
                    ChangeBuilding(buildingUpd);
                }
            }

            this.Navigation.PopAsync();
        }

        private void DeleteBuilding(object sender, EventArgs e)
        {
            var building = (Building)BindingContext;
            HttpRequestData.DeleteBuilding(building);
            this.Navigation.PopAsync();
        }

        private async void AddAuditoriumGroup(object sender, EventArgs e)
        {
            var building = (Building)BindingContext;
            if (building.Id == 0)
            {
                AddBuilding(building);
                building = GetAllBuilding().Find(x => x.Name == building.Name);
            }
            
            var auditoriumName = await DisplayPromptAsync("Добавление аудиторий в корпус",
                "Введите название аудитории",
                keyboard: Keyboard.Text);
            Console.WriteLine(auditoriumName);
            // Сделать выход после нажания отмены 
            if (auditoriumName == "" || auditoriumName == null)
                return;
            if (building.Id != 0)
            {
                var buildingFromId = GetBuildingFromId(building.Id);
                var partL = buildingFromId.AuditoriumGroups.Find(x => x.Auditorium.Name == auditoriumName);
                if (partL != null)
                {
                    await DisplayAlert("Внимание", "Аудитория уже есть в корпусе", "Хорошо");
                    return;
                }
            }

            // Проверка есть ли деталь с которой пытаемся создать часть сборки
            var allAuditoriums = GetAllAuditoriums();
            var auditorium = allAuditoriums.Find(x => x.Name == auditoriumName);
            if (auditorium == null)
            {
                bool flag = await DisplayAlert(
                    "Ошибочка",
                    "Похоже, нет такой аудитории(\nХотите создать?",
                    "Создать",
                    "Отмена");
                // Если хотим то можно сразу создать ее
                if (flag)
                {
                    PageAuditorium pageAuditorium = new PageAuditorium();
                    Auditorium auditoriumNew = new Auditorium { Name = auditoriumName };
                    pageAuditorium.BindingContext = auditoriumNew;
                    await Navigation.PushAsync(pageAuditorium);
                    return;
                }
                else
                {
                    return;
                }
            }

            string quantity = await DisplayPromptAsync("Добавление аудитории в корпус",
                $"Введите количество аудиторий \"{auditoriumName}\" в корпусе",
                keyboard: Keyboard.Numeric);
            if (quantity == "0" || quantity == "" || !int.TryParse(quantity, out var numericValue))
                return;
            AuditoriumGroup temp = new AuditoriumGroup
            {
                Quantity = Int32.Parse(quantity),
                Auditorium = auditorium,
                AuditoriumId = auditorium.Id
            };
            building.AuditoriumGroups.Add(temp);
            ListAuditoriumGroupsInBuilding.ItemsSource = building.AuditoriumGroups;
            ChangeBuilding(building);
            await DisplayAlert("Внимание", "Аудитория добавлена", "Хорошо");
            OnAppearing();
        }

        private async void EditAuditoriumGroup(object sender, EventArgs e)
        {
            var building = (Building)BindingContext;
            var name = ((MenuItem)sender).CommandParameter.ToString();
            string quantityNew = await DisplayPromptAsync("Редактирование группы аудиторий",
                $"Введите новое количество \"{name}\"",
                keyboard: Keyboard.Numeric);
            if (quantityNew == "0" || quantityNew == "" || !int.TryParse(quantityNew, out var numericValue))
                return;
            var buildingFromId = GetBuildingFromId(building.Id);
            buildingFromId.AuditoriumGroups.Find(x => x.Auditorium.Name == name).Quantity = int.Parse(quantityNew);
            ChangeBuilding(buildingFromId);
            OnAppearing();
        }

        private void DeleteAuditoriumGroup(object sender, EventArgs e)
        {
            var building = (Building)BindingContext;
            var buildingFromId = GetBuildingFromId(building.Id);
            var name = ((MenuItem)sender).CommandParameter.ToString();
            buildingFromId.AuditoriumGroups.RemoveAll(x => x.Auditorium.Name == name);
            ChangeBuilding(buildingFromId);
            OnAppearing();
        }
    }
}
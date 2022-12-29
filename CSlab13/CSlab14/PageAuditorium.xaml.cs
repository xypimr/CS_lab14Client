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
    public partial class PageAuditorium : ContentPage
    {
        public PageAuditorium()
        {
            InitializeComponent();
        }

        private void SaveAuditorium(object sender, EventArgs e)
        {
            var auditorium = (Auditorium)BindingContext;
            if (!String.IsNullOrEmpty(auditorium.Name))
            {
                if (auditorium.Id == 0)
                    AddAuditorium(auditorium);
                else

                    ChangeAuditorium(auditorium);
            }

            this.Navigation.PopAsync();
        }

        private void DeleteAuditorium(object sender, EventArgs e)
        {
            var auditorium = (Auditorium)BindingContext;
            HttpRequestData.DeleteAuditorium(auditorium);

            this.Navigation.PopAsync();
        }
    }
}
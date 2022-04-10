using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using Java.Lang;
using Newtonsoft.Json;
using Nhom7_FoodOrderingApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.CommunityToolkit.UI.Views;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FoodOrderingApp.Views.Admin
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CategoryPage : ContentPage, INotifyPropertyChanged
    {
        ICategoryService categoryService = DependencyService.Get<ICategoryService>();

        public CategoryPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetAllCatigories();
            SelectCategoryCommand = new Command<CategoryData>((model) => ExecuteSelectCategoryCommand(model));

            //SecureStorage.SetAsync("token", "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1c2VybmFtZSI6ImFkbWluIiwicGFzc3dvcmQiOiIxMjM0NTYiLCJpYXQiOjE2NDgyMTg2ODcsImV4cCI6MTY0ODgyMzQ4N30.uoGYVcz4alkQj0wrbseBlkWh4QGXhffD0fi2OY-8xRw");
            
        }

        public Command SelectCategoryCommand { get; }

        private void ExecuteSelectCategoryCommand(CategoryData model)
        {
            idUpdate.Text = model.id.ToString();
            nameUpdate.Text = model.name;
            descriptionUpdate.Text = model.description;
            imageUpdateLink.Text = model.image;
            imageUpdate.Source = model.image;
            tabView.SelectedIndex = 2;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string name = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        protected bool SetProperty<T>(ref T field, T value, [CallerMemberName] string propertyName = null)
        {
            if (EqualityComparer<T>.Default.Equals(field, value))
                return false;
            field = value;
            OnPropertyChanged(propertyName);
            return true;

        }
        private FileResult pickResultAdd { get; set; }
        private FileResult pickResultUpdate { get; set; }
        private ObservableCollection<CategoryData> _categories;
        public ObservableCollection<CategoryData> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }

        public async void GetAllCatigories()
        {
            var result = await categoryService.getAllCategories();
            ObservableCollection<CategoryData> ct = new ObservableCollection<CategoryData>();
            if (result != null)
            {
                Categories = result.data;
            }

        }
        private async void UploadImage(object sender, EventArgs e)
        {
            var pickResultTemp = await FilePicker.PickAsync(new PickOptions
            {
                FileTypes = FilePickerFileType.Images,
                PickerTitle = "Pick an image"
            });

            if (pickResultTemp != null)
            {
                var stream = await pickResultTemp.OpenReadAsync();

                if (tabView.SelectedIndex==1)
                {
                    imageAdd.Source = ImageSource.FromStream(()=>stream);
                    pickResultAdd = pickResultTemp;

                }
                if (tabView.SelectedIndex == 2)
                {
                    imageUpdate.Source = ImageSource.FromStream(() => stream);
                    pickResultUpdate = pickResultTemp;

                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "You have to add or update image of category", "OK");
            }
        }
        private async void AddCategory(object sender, EventArgs e)
        {
            if (pickResultAdd != null)
            {
                var result = await categoryService.addCategory(nameAdd.Text, descriptionAdd.Text, pickResultAdd);
                Console.WriteLine("55" + JsonConvert.SerializeObject(result));
                if(result != null && result.status == 1)
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "Your category is added ", "OK");
                    var resultAllCate = await categoryService.getAllCategories();
                    if (resultAllCate != null && resultAllCate.status == 1)
                    {
                        Categories = resultAllCate.data;

                    }
                }
                else
                {
                    await Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Please upload your image", "OK");
            }
            
        }
        private async void UpdateCategory(object sender, EventArgs e)
        {
            var result = await categoryService.updateCategory(nameUpdate.Text, descriptionUpdate.Text, pickResultUpdate, Integer.ParseInt(idUpdate.Text));
            Console.WriteLine("73" + JsonConvert.SerializeObject(result));
            if (result != null && result.status == 1)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Your category is added ", "OK");
                var resultAllCate = await categoryService.getAllCategories();
                if (resultAllCate != null && resultAllCate.status == 1)
                {
                    Categories = resultAllCate.data;

                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");
            }
        }
        private async void DeleteCategory(object sender, EventArgs e)
        {
            Console.WriteLine(((TappedEventArgs)e).Parameter);
            var result = await categoryService.deleteCategory(Integer.ParseInt(((TappedEventArgs)e).Parameter.ToString()));
            Console.WriteLine("31" + JsonConvert.SerializeObject(result));
            if (result != null && result.status == 1)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Your category is added ", "OK");
                var resultAllCate = await categoryService.getAllCategories();
                if (resultAllCate != null && resultAllCate.status == 1)
                {
                    Categories = resultAllCate.data;

                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");
            }
        }
        private void ClickUpdateTabView(object sender, TabTappedEventArgs e)
        {
            idUpdate.Text = "";
            nameUpdate.Text = "";
            descriptionUpdate.Text = "";
            imageUpdateLink.Text = "";
            imageUpdate.Source = "";
        }
    }
}
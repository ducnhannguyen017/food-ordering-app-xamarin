using FoodOrderingApp.Models;
using FoodOrderingApp.Services.IServices;
using Java.Lang;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
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
    public partial class FoodPage : ContentPage, INotifyPropertyChanged
    {
        IFoodService foodService = DependencyService.Get<IFoodService>();
        ICategoryService categoryService = DependencyService.Get<ICategoryService>();
        public FoodPage()
        {
            InitializeComponent();
            BindingContext = this;
            GetAllFood(0);
            GetAllCatigories();
            SelectFoodCommand = new Command<FoodData>((model) => ExecuteSelectFoodCommand(model));
        }
        public Command SelectFoodCommand { get; }

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
        private ObservableCollection<FoodData> _foods;
        public ObservableCollection<FoodData> Foods
        {
            get { return _foods; }
            set { SetProperty(ref _foods, value); }
        }
        private List<KeyValuePair<string, string>> _categories;
        public List<KeyValuePair<string, string>> Categories
        {
            get { return _categories; }
            set { SetProperty(ref _categories, value); }
        }
        private int _categorySelectedIndex;
        public int CategorySelectedIndex
        {
            get { return _categorySelectedIndex; }
            set { SetProperty(ref _categorySelectedIndex, value); }
        }
        public async Task GetAllFood(int category_id)
        {
            var result = await foodService.getFoodByCategory(category_id,4);
            if (result != null)
            {
                Console.WriteLine("57"+ JsonConvert.SerializeObject(result));
                Foods = result.data.data;
            }
        }
        private void ExecuteSelectFoodCommand(FoodData model)
        {
            idUpdate.Text = model.id.ToString();
            nameUpdate.Text = model.name;
            descriptionUpdate.Text = model.description;
            priceUpdate.Text = model.price.ToString();
            categoryUpdate.Text = model.category_id.ToString();
            imageUpdateLink.Text = model.image;
            imageUpdate.Source = model.image;
            tabView.SelectedIndex = 2;
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

                if (tabView.SelectedIndex == 1)
                {
                    imageAdd.Source = ImageSource.FromStream(() => stream);
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
        private async void AddFood(object sender, EventArgs e)
        {
            if (pickResultAdd != null)
            {
                var result = await foodService.addFood(nameAdd.Text, descriptionAdd.Text, Integer.ParseInt(priceAdd.Text), pickResultAdd, Integer.ParseInt(categoryAdd.Text));
                Console.WriteLine("55" + JsonConvert.SerializeObject(result));
                if (result != null && result.status == 1)
                {
                    Console.WriteLine("112"+ JsonConvert.SerializeObject(result));
                    await Application.Current.MainPage.DisplayAlert("Notification", "Food is deleted ", "OK");
                    await GetAllFood(0);
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
        private async void UpdateFood(object sender, EventArgs e)
        {
            var result = await foodService.updateFood(nameUpdate.Text, descriptionUpdate.Text, Integer.ParseInt(priceUpdate.Text), pickResultUpdate, Integer.ParseInt(categoryUpdate.Text), Integer.ParseInt(idUpdate.Text));
            Console.WriteLine("73" + JsonConvert.SerializeObject(result));
            if (result != null && result.status == 1)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Your category is updated ", "OK");
                await GetAllFood(0);
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Connection Error", "OK");
            }
        }
        private async void DeleteFood(object sender, EventArgs e)
        {
            Console.WriteLine(((TappedEventArgs)e).Parameter);
            var result = await foodService.deleteFood(Integer.ParseInt(((TappedEventArgs)e).Parameter.ToString()));
            Console.WriteLine("31" + JsonConvert.SerializeObject(result));
            if (result != null && result.status == 1)
            {
                await Application.Current.MainPage.DisplayAlert("Notification", "Your category is added ", "OK");
                await GetAllFood(0);
                CategorySelectedIndex = -1;
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
        async void GetAllCatigories()
        {
            var result = await categoryService.getAllCategories();
            Dictionary<string, string> keyValuePair = new Dictionary<string, string>();
            if (result != null)
            {
                foreach (var category in result.data)
                {
                    keyValuePair.Add(category.id.ToString(), category.name);
                }
                Categories = keyValuePair.ToList();
            }

        }

        private async void Picker_SelectedIndexChanged(object sender, EventArgs e)
        {
            Picker picker = (Picker)sender;
            if (picker.SelectedIndex == -1)
            {

            }
            else
            {
                CategorySelectedIndex = picker.SelectedIndex;
                List<string> allKeys = (from kvp in Categories select kvp.Key).Distinct().ToList();
                int selectedIndex = picker.SelectedIndex;
                var sellected = (string)allKeys[selectedIndex];
                await GetAllFood(Integer.ParseInt(sellected));
            }
        }
    }
}
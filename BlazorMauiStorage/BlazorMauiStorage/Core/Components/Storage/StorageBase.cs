//using CommunityToolkit.Mvvm.ComponentModel;
//using CommunityToolkit.Mvvm.Input;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System.Windows.Input;

namespace BlazorMauiStorage.Core.Components.Storage
{
    //[ObservableObject]
    public partial class StorageBase : ComponentBase
    {
        [Inject]
        protected ISettingService settingService { get; set; }

        //[ObservableProperty]
        protected bool sendNotifications;

        protected override void OnInitialized()
        {
            LoadData();
        }

        //public StorageBase(ISettingService settingService)
        //{
        //    save = new AsyncRelayCommand(SaveSettings);
        //    this.settingService = settingService;
        //}     

        //[ObservableProperty]
        //public ICommand save;

        public async Task LoadData()
        {
            sendNotifications = await settingService.Get<bool>(nameof(sendNotifications), false);
        }

		public async Task SaveSettings()
		{
			await settingService.Save(nameof(sendNotifications), sendNotifications);

            //await Shell.Current.DisplayAlert("Saved!", "Settings has been saved", "OK");
        }
    }
}

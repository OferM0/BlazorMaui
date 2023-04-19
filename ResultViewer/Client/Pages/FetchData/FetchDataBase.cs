using Microsoft.AspNetCore.Components;
using ResultViewer.Client.Core.Entities.Local;

namespace ResultViewer.Client.Pages.FetchData
{
    public class FetchDataBase : ComponentBase
    {
        [Inject]
        public WeatherForecastService ForecastService { get; set; }
        public WeatherForecast[] Forecasts { get; set; }

        protected async override Task OnInitializedAsync()
        {
            Forecasts = await ForecastService.GetForecastAsync(DateTime.Now);
        }
    }
}

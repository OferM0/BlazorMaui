using Microsoft.AspNetCore.Components;
using ChartJs.Blazor.PieChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Util;
using ChartJS.Data;
using System.Data;
using ChartJs.Blazor.Common.Enums;

namespace ChartJS.Core.Components.PieChart
{
    public class PieChartBase : ComponentBase
    {
        public PieConfig _config;

        [Inject]
        public WeatherForecastService ForecastService { get; set; }
        private WeatherForecast[] forecasts;

        protected override async Task OnInitializedAsync()
        {
            forecasts = await ForecastService.GetForecastAsync(DateTime.Now);

            var temperatureCounts = forecasts
                .GroupBy(f => f.Date.ToString("ddd"))
                .Select(g => new { Day = g.Key, Temperature = g.Average(f => f.TemperatureC) })
                .ToList();

            _config = new PieConfig
            {
                Options = new PieOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "Temperature by Day"
                    }
                }
            };

            foreach (string day in temperatureCounts.Select(s => s.Day).ToList())
            {
                _config.Data.Labels.Add(day);
            }

            PieDataset<int> dataset = new PieDataset<int>(temperatureCounts.Select(s => (int)Math.Round(s.Temperature)).ToList())
            {
                BackgroundColor = new[]
                {
                    ColorUtil.ColorHexString(255, 99, 132), // Red
                    ColorUtil.ColorHexString(255, 205, 86), // Yellow
                    ColorUtil.ColorHexString(75, 192, 192), // Green
                    ColorUtil.ColorHexString(54, 162, 235), // Blue
                    ColorUtil.ColorHexString(153, 102, 255), // Purple
                    ColorUtil.ColorHexString(255, 159, 64) // Orange
                }
            };

            _config.Data.Datasets.Add(dataset);

            //_config = new PieConfig
            //{
            //    Options = new PieOptions
            //    {
            //        Responsive = true,
            //        Title = new OptionsTitle
            //        {
            //            Display = true,
            //            Text = "ChartJs.Blazor Pie Chart"
            //        }
            //    }
            //};

            //foreach (string color in new[] { "Red", "Yellow", "Green", "Blue" })
            //{
            //    _config.Data.Labels.Add(color);
            //}

            //PieDataset<int> dataset = new PieDataset<int>(new[] { 6, 5, 3, 7 })
            //{
            //    BackgroundColor = new[]
            //    {
            //    ColorUtil.ColorHexString(255, 99, 132), // Slice 1 aka "Red"
            //    ColorUtil.ColorHexString(255, 205, 86), // Slice 2 aka "Yellow"
            //    ColorUtil.ColorHexString(75, 192, 192), // Slice 3 aka "Green"
            //    ColorUtil.ColorHexString(54, 162, 235), // Slice 4 aka "Blue"
            //}
            //};
            //_config.Data.Datasets.Add(dataset);
        }
    }
}

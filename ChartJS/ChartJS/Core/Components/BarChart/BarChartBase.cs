using Microsoft.AspNetCore.Components;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Util;
using ChartJs.Blazor.Common.Axes.Ticks;
using ChartJs.Blazor.Common.Axes;

namespace ChartJS.Core.Components.BarChart
{
    public class BarChartBase : ComponentBase
    {
        public BarConfig _config;

        protected override void OnInitialized()
        {
            _config = new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "ChartJs.Blazor Bar Chart"
                    },
                    Scales = new BarScales
                    {
                        YAxes = new[]
                        {
                        new LinearCartesianAxis
                        {
                            Ticks = new LinearCartesianTicks
                            {
                                BeginAtZero = true
                            }
                        }
                    }
                    }
                }
            };

            foreach (string month in new[] { "January", "February", "March", "April", "May", "June", "July" })
            {
                _config.Data.Labels.Add(month);
            }

            BarDataset<int> dataset = new BarDataset<int>(new[] { 65, 59, 80, 81, 56, 55, 40 })
            {
                Label = "Sales",
                BackgroundColor = "rgba(75, 192, 192, 0.2)",
                BorderColor = "rgb(75, 192, 192)",
                BorderWidth = 1
            };

            _config.Data.Datasets.Add(dataset);
        }
    }
}

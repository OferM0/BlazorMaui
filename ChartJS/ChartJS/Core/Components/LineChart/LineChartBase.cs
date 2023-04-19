using Microsoft.AspNetCore.Components;
using ChartJs.Blazor.LineChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Util;
using ChartJs.Blazor.Common.Axes.Ticks;
using ChartJs.Blazor.Common.Axes;

namespace ChartJS.Core.Components.LineChart
{
    public class LineChartBase : ComponentBase
    {
        public LineConfig _config;

        protected override void OnInitialized()
        {
            _config = new LineConfig
            {
                Options = new LineOptions
                {
                    Responsive = true,
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "ChartJs.Blazor Line Chart"
                    },
                    Scales = new Scales
                    {
                        YAxes = new List<CartesianAxis>
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

            var dataset = new LineDataset<int>(new[] { 65, 59, 80, 81, 56, 55, 40 })
            {
                Label = "Sales",
                Fill = false,
                BorderColor = ColorUtil.ColorHexString(75, 192, 192),
                LineTension = 0.1f
            };

            _config.Data.Datasets.Add(dataset);
        }
    }
}

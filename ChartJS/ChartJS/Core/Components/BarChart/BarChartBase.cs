using Microsoft.AspNetCore.Components;
using ChartJs.Blazor.BarChart;
using ChartJs.Blazor.Common;
using ChartJs.Blazor.Util;
using ChartJs.Blazor.Common.Axes.Ticks;
using ChartJs.Blazor.Common.Axes;
using ChartJS.Data;
using System;
using Microsoft.AspNetCore.Components.Web;
using ChartJs.Blazor.Common.Handlers;
using ChartJs.Blazor.Interop;
using Newtonsoft.Json.Linq;

namespace ChartJS.Core.Components.BarChart
{
    public class BarChartBase : ComponentBase
    {
        public BarConfig _config;
        private List<SuperHero> _heroes { get; set; }
        [Inject]
        private ISuperHeroService _superHeroService { get; set; }

        public string Text { get; set; } = "";

        protected override async Task OnInitializedAsync()
        {
            _config = new BarConfig
            {
                Options = new BarOptions
                {
                    Responsive = true,
                    OnClick = new DelegateHandler<ChartMouseEvent>(OnClickHandler),
                    Title = new OptionsTitle
                    {
                        Display = true,
                        Text = "SuperHeroes by Place"
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

            var heroes = await _superHeroService.GetAllSuperHeroes();
            _heroes = heroes;
            var groupedHeroes = heroes.GroupBy(h => h.Place).Select(g => new { Place = g.Key, Count = g.Count() });

            var data = new List<int>();

            var colors = new[]
            {
                "rgba(75, 192, 192)",
                "rgba(153, 102, 255)",
                "rgba(255, 159, 64)",
                "rgba(255, 99, 132)",
                "rgba(54, 162, 235,0.2)",
                "rgba(255, 206, 86)",
            };

            var borderColors = new[]
            {
                "rgba(75, 192, 192,0.2)",
                "rgba(153, 102, 255,0.2)",
                "rgba(255, 159, 64,0.2)",
                "rgba(255, 99, 132,0.2)",
                "rgba(54, 162, 235,0.2)",
                "rgba(255, 206, 86,0.2)",
            };

            foreach (var gh in groupedHeroes)
            {
                _config.Data.Labels.Add(gh.Place);
                data.Add(gh.Count);
            }

            var random = new Random(Environment.TickCount);

            var dataset = new BarDataset<int>(data)
            {
                Label = "SuperHeroes by Cities",
                BackgroundColor = colors,
                BorderColor = borderColors, // set a darker border color
                BorderWidth = 1
            };

            _config.Data.Datasets.Add(dataset);

            //_config = new BarConfig
            //{
            //    Options = new BarOptions
            //    {
            //        Responsive = true,
            //        Title = new OptionsTitle
            //        {
            //            Display = true,
            //            Text = "ChartJs.Blazor Bar Chart"
            //        },
            //        Scales = new BarScales
            //        {
            //            YAxes = new[]
            //            {
            //            new LinearCartesianAxis
            //            {
            //                Ticks = new LinearCartesianTicks
            //                {
            //                    BeginAtZero = true
            //                }
            //            }
            //        }
            //        }
            //    }
            //};

            //foreach (string month in new[] { "January", "February", "March", "April", "May", "June", "July" })
            //{
            //    _config.Data.Labels.Add(month);
            //}

            //BarDataset<int> dataset = new BarDataset<int>(new[] { 65, 59, 80, 81, 56, 55, 40 })
            //{
            //    Label = "Sales",
            //    BackgroundColor = "rgba(75, 192, 192, 0.2)",
            //    BorderColor = "rgb(75, 192, 192)",
            //    BorderWidth = 1
            //};

            //_config.Data.Datasets.Add(dataset);
        }

        public void OnClickHandler(JObject mouseEvent, JArray activeElements)
        {
            foreach (JObject elem in activeElements)
            {
                foreach (JProperty prop in elem.GetValue("_model"))
                {
                    if (prop.Name.Equals("label"))
                    {
                        string cityName = prop.Value.ToString();
                        int count = _heroes.Count(h => h.Place == cityName);
                        Text = $"{cityName}: {count} heroes";
                        StateHasChanged();
                    }
                }
            }
        }
    }
}

﻿using Microsoft.Extensions.Logging;
using ChartJS.Data;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace ChartJS;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
			});

		builder.Services.AddMauiBlazorWebView();

#if DEBUG
		builder.Services.AddBlazorWebViewDeveloperTools();
		builder.Logging.AddDebug();
#endif

		builder.Services.AddSingleton<WeatherForecastService>();
        builder.Services.AddSingleton<ISuperHeroService, SuperHeroService>();

		//for configuration of appsettings.json
		using var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("ChartJS.appsettings.json");
		var config = new ConfigurationBuilder().AddJsonStream(stream).Build();
		builder.Configuration.AddConfiguration(config);

        return builder.Build();
	}
}

using System;

namespace BlazorMauiStorage
{
	public interface ISettingService
	{
		Task<T> Get<T> (string key, T defaultValue);
		Task Save<T> (string key, T value);
	}
}

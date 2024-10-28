
namespace maui_mvvm.Services;

public class PreferencesStorage : IPreferencesStorage
{
    public int Get(string key, int defaultValue) => Get(key, defaultValue);

    public void Set(string key, int value) => Set(key, value);
}

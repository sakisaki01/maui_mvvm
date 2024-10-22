
namespace maui_mvvm.Services;

public interface IPreferencesStorage //将不可测试的的改为接口
{
    void Set(string key, int value);

    int Get(string key, int defaultValue);
}

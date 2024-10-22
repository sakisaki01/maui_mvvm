
using maui_mvvm.Models;
using System.Linq.Expressions;

namespace maui_mvvm.Services;

public class PoetryStorage : IPoetryStorage
{
    public bool IsIiitialized => 
        Preferences.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;
    //如果左边=右边，返回true 代表已经初始化 否则返回false

    public Task InitializeAsyne()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Poetry>> GetPoetriesAsync(Expression<Func<Poetry, bool>> where, int skip, int take)
    {
        throw new NotImplementedException();
    }

    public Task<Poetry> GetPoetryAsync(int id)
    {
        throw new NotImplementedException();
    }

}

//保存常量
public static class PoetryStorageConstant
{
    public const int Version = 1; //正确的版本号

    //偏好存储存的版本（用户版本）
    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version); //= PoetryStorageConstant.1(Version)

}
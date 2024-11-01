﻿
using maui_mvvm.Models;
using SQLite;
using System.Linq.Expressions;

namespace maui_mvvm.Services;

public class PoetryStorage : IPoetryStorage
{
    //数据库的名字
    public const string DbName = "poetrydb.sqlite3";
    //数据库特殊地址
    public static readonly string PoetryDbPath =
        Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder
                .LocalApplicationData), DbName);

    //打开数据库连接
    private SQLiteAsyncConnection? _connection;

    public SQLiteAsyncConnection Connection =>
        _connection ??= new SQLiteAsyncConnection(PoetryDbPath);


    private readonly IPreferencesStorage _preferencesStorage;

    public PoetryStorage(IPreferencesStorage preferencesStorage)
    {
        _preferencesStorage = preferencesStorage;
    }


    public bool IsIiitialized =>
        _preferencesStorage.Get(PoetryStorageConstant.VersionKey, 0) == PoetryStorageConstant.Version;
    //如果左边=右边，返回true 代表已经初始化 否则返回false

    public async Task InitializeAsync()
    {
        //打开目标文件   利用using 在执行完自动关闭
        await using var dbFileStream =
             new FileStream(PoetryDbPath, FileMode.OpenOrCreate);
        //打开资源
        await using var dbAssetStream =
            typeof(PoetryStorage).Assembly.GetManifestResourceStream(DbName);
        //复制文件
        await dbAssetStream.CopyToAsync(dbFileStream);
        //存储版本号
        _preferencesStorage.Set(PoetryStorageConstant.VersionKey,
            PoetryStorageConstant.Version);

    }

    public async Task<IEnumerable<Poetry>> GetPoetriesAsync
        (Expression<Func<Poetry, bool>> where, int skip, int take) =>
        await Connection.Table<Poetry>().Where(where).Skip(skip).Take(take)
            .ToListAsync();

    public async Task<Poetry> GetPoetryAsync(int id) =>
        await Connection.Table<Poetry>().FirstOrDefaultAsync(p => p.Id == id);

    public async Task ClosePoetryAsync() => await Connection.CloseAsync();
}

//保存常量
public static class PoetryStorageConstant
{
    public const int Version = 1; //正确的版本号

    //偏好存储存的版本（用户版本）
    public const string VersionKey = nameof(PoetryStorageConstant) + "." + nameof(Version); //= PoetryStorageConstant.1(Version)

}
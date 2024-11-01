﻿using maui_mvvm.Models;
using maui_mvvm.Services;
using Moq;
using System.Linq.Expressions;
namespace maui_mvvm.UnitTest.Services;

public class PoetryStorageTest : IDisposable
{
    //在单元测试之前清理文件
    public PoetryStorageTest() => File.Delete(PoetryStorage.PoetryDbPath);
    //析构函数 垃圾清理器
    public void Dispose() => File.Delete(PoetryStorage.PoetryDbPath);


    [Fact]  //专用于xUnit
    public void IsIiitialized_Default()
    {
        //用mock造一个假的实例
        //因为我们的 IPoetryStorage 没有实现(如果要实现，需要依赖maui的api 但是如果使用了maui ，就不可测试了 所以用了一个假的）
        var preferenceStorageMock = new Mock<IPreferencesStorage>();
        preferenceStorageMock.Setup(p => p.Get(PoetryStorageConstant.VersionKey, 0))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.True(poetryStorage.IsIiitialized); //断定 一定是true
    }
    [Fact]
    public async Task TestInitializeAsyne_Default()
    {
        var preferenceStorageMock = new Mock<IPreferencesStorage>();
        //没有返回值 不需要set
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);

        //先证明文件不存在
        Assert.False(File.Exists(PoetryStorage.PoetryDbPath));
        //初始化
        await poetryStorage.InitializeAsync();
        //初始化后文件应该存在
        Assert.True(File.Exists(PoetryStorage.PoetryDbPath));
        //验证mock的调用是否正常
        preferenceStorageMock.Verify(p => p.Set(PoetryStorageConstant.VersionKey,
            PoetryStorageConstant.Version), Times.Once);
    }
    [Fact]
    public async Task GetPoetryAsync_Default()
    {
        //先初始化
        //var preferenceStorageMock = new Mock<IPreferencesStorage>();
        //var mockPreferenceStorage = preferenceStorageMock.Object;
        //var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        //await poetryStorage.InitializeAsync();
        var poetryStorage = await GetInitializedPoetryStorage();
        //读诗词
        var poetry = await poetryStorage.GetPoetryAsync(10001);
        //判断结果
        Assert.Equal("临江仙 · 夜归临皋", poetry.Name);
        //关闭数据库
        await poetryStorage.ClosePoetryAsync();
    }
    [Fact]
    public async Task GetPoetresAsync_Default()
    {
        //先初始化
        var poetryStorage = await GetInitializedPoetryStorage();
        var poetries = await poetryStorage.GetPoetriesAsync(
            Expression.Lambda<Func<Poetry, bool>>
            (Expression.Constant(true), Expression.Parameter(typeof(Poetry), "p")), 0, int.MaxValue);
        //中间那段到 ,0,int.MaxValue 之前 代表：connection.Table<Poetry>().where(p => True);
        Assert.Equal(30, poetries.Count());
        //关闭数据库
        await poetryStorage.ClosePoetryAsync();

    }

    //初始化
    public static async Task<PoetryStorage> GetInitializedPoetryStorage()
    {
        var preferenceStorageMock = new Mock<IPreferencesStorage>();
        var mockPreferenceStorage = preferenceStorageMock.Object;
        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        await poetryStorage.InitializeAsync();
        return poetryStorage;
    }
}
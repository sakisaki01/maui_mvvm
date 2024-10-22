using maui_mvvm.Services;
using Moq;
namespace maui_mvvm.UnitTest.Services;

public class PoetryStorageTest :IDisposable
{
    //在单元测试之前清理文件
    public PoetryStorageTest() {
        File.Delete(PoetryStorage.PoetryDbPath);
    }
    //析构函数 垃圾清理器
    public void Dispose(){
        File.Delete(PoetryStorage.PoetryDbPath);
    }

    [Fact]  //专用于xUnit
    public void IsIiitialized_Default()
    {
        //用mock造一个假的实例
        //因为我们的 IPoetryStorage 没有实现(如果要实现，需要依赖maui的api 但是如果使用了maui ，就不可测试了 所以用了一个假的）
        var preferenceStorageMock = new Mock<IPreferencesStorage>();
        preferenceStorageMock.Setup(p=>p.Get(PoetryStorageConstant.VersionKey,0))
            .Returns(PoetryStorageConstant.Version);
        var mockPreferenceStorage = preferenceStorageMock.Object;

        var poetryStorage = new PoetryStorage(mockPreferenceStorage);
        Assert.True(poetryStorage.IsIiitialized); //断定 一定是true
    }
    [Fact]
    public async Task TestInitializeAsyne_Default() {
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
    }
}

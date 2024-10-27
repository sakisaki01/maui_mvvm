

using maui_mvvm.Models;
using maui_mvvm.Services;
using maui_mvvm.UnitTest.Services;
using maui_mvvm.ViewModels;
using Moq;
using System.Linq.Expressions;

namespace maui_mvvm.UnitTest.ViewModels;

public class ResultPageViewModelTest : IDisposable{
    public ResultPageViewModelTest() => File.Delete(PoetryStorage.PoetryDbPath);

    public void Dispose() => File.Delete(PoetryStorage.PoetryDbPath); 

    [Fact]
    public async Task Poetries_Default()
    {
        var where = Expression.Lambda<Func<Poetry, bool>>(
            Expression.Constant(true),
            Expression.Parameter(typeof(Poetry),"p"));

        //var pList = new List<Poetry>
        //{
        //    new Poetry {Id = 1 } ,new Poetry {Id = 2} ,new Poetry {Id = 3}
        //};
        //LING  语言集合查询 就相当于第一个
        //var poetriesWithIdGt1 = pList.Where(p => p.Id > 1);


        var poetryStorage = await PoetryStorageTest.GetInitializedPoetryStorage();
        var resultPageViewModel = new ResultPageViewModel(poetryStorage);

        resultPageViewModel.where = where;

        var StatusList = new List<string>();
        resultPageViewModel.PropertyChanged += (sender, args) =>
        {
            if (args.PropertyName == nameof(ResultPageViewModel.Status)){
                StatusList.Add(resultPageViewModel.Status);
            }
        };

        await resultPageViewModel.NavigatedToCommandFunction();
        Assert.Equal(ResultPageViewModel.PageSize, resultPageViewModel.Poetries.Count);

        Assert.Equal(2, StatusList.Count());
        Assert.Equal(ResultPageViewModel.Loading , StatusList[0]);
        Assert.Equal(string.Empty, StatusList[1]);



        await poetryStorage.ClosePoetryAsync();
    }

    private void ResultPageViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        throw new NotImplementedException();
    }
}


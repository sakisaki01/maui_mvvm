using CommunityToolkit.Mvvm.ComponentModel;
using maui_mvvm.Models;
using maui_mvvm.Services;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using TheSalLab.MauiInfiniteScrolling;

namespace maui_mvvm.ViewModels;

public partial class ResultPageViewModel : ObservableObject {

    private Expression<Func<Poetry, bool>>? _where;

    public Expression<Func<Poetry, bool>> where
    {
        get => _where;
        set => SetProperty(ref _where, value);
    }

    [ObservableProperty]
    private string _status;

    public MauiInfiniteScrollCollection<Poetry> Poetries{ get; }

    public ResultPageViewModel(IPoetryStorage poetryStorage) { 
        Poetries = new MauiInfiniteScrollCollection<Poetry>{
            OnCanLoadMore = () =>true, //true  永远能继续滚  false 就停止
            OnLoadMore = async () => { //上面为true 就能加载内容
                Status = Loading;

                return await poetryStorage.GetPoetriesAsync(where, Poetries.Count, PageSize); }
        };
    }

    private const int PageSize = 20;

    public const string Loading = "正在载入";

    public const string NoResult = "没有满足条件的结果";

    public const string NoMoreResult = "没有更多结果";


}

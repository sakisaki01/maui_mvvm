using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using maui_mvvm.Models;
using maui_mvvm.Services;
using System.Data;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using TheSalLab.MauiInfiniteScrolling;

namespace maui_mvvm.ViewModels;

public partial class ResultPageViewModel : ObservableObject {

    private Expression<Func<Poetry, bool>>? _where;

    public Expression<Func<Poetry, bool>> where
    {
        get => _where;
        set => _canLoadMore = SetProperty(ref _where, value);   
        //SetProperty 如果在_where 变化了才会返回true
    }

    [ObservableProperty]
    private string _status;

    public MauiInfiniteScrollCollection<Poetry> Poetries{ get; }

    public ResultPageViewModel(IPoetryStorage poetryStorage) { 

        //调试用
        where = Expression.Lambda<Func<Poetry, bool>>(
            Expression.Constant(true),
            Expression.Parameter(typeof(Poetry), "p"));
        
        //

        Poetries = new MauiInfiniteScrollCollection<Poetry>{
            OnCanLoadMore = () =>_canLoadMore, //  true  永远能继续滚  false 就停止 
            OnLoadMore = async () => { //上面为true 就能加载内容
                Status = Loading;

                var poetries =  (await poetryStorage.GetPoetriesAsync
                (where, Poetries.Count, PageSize)).ToList();
                Status = string.Empty;
                //要重复拿东西，所以先变成list

                if(poetries.Count <PageSize) { 
                    Status = NoMoreResult;
                    _canLoadMore = false;}  //不足一页

                if(poetries.Count == 0 && Poetries.Count == 0) {
                    Status = NoResult;
                    _canLoadMore = false;}

                return poetries;
            }
        };
    }

    private RelayCommand _navigatedToCommand;

    public RelayCommand NavigatedToCommand =>
        _navigatedToCommand ??= new RelayCommand(async() => {
            //Poetries.Clear();
            //await Poetries.LoadMoreAsync();
            await NavigatedToCommandFunction();
        });
    //从command里面剥离出方法，用于调试Test
    public async Task NavigatedToCommandFunction()
    {
        Poetries.Clear();
        await Poetries.LoadMoreAsync();
    }

    //成员变量
    private bool _canLoadMore;

    public const int PageSize = 20;

    public const string Loading = "正在载入";

    public const string NoResult = "没有满足条件的结果";

    public const string NoMoreResult = "没有更多结果";


}

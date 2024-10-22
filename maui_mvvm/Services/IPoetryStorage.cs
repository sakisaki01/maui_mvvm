

using System.Linq.Expressions;
using maui_mvvm.Models;

namespace maui_mvvm.Services;

public interface IPoetryStorage
{
    bool IsIiitialized { get; }

    Task InitializeAsyne();

    Task<Poetry> GetPoetryAsync(int id);  

    Task<IEnumerable<Poetry>> GetPoetriesAsync(
        Expression<Func<Poetry, bool>> where, int skip, int take);
        
}

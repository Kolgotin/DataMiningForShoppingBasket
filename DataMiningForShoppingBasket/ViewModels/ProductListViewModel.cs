using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using DynamicData;
using DynamicData.Binding;

namespace DataMiningForShoppingBasket.ViewModels;

public sealed class ProductListViewModel : NotifyPropertyChangedImplementation, IAsyncInitialized, IDisposable
{
    #region Fields
    private readonly IDbManager _dbManager;
    private readonly INotifier<Products, int> _productsNotifier;
    private readonly CompositeDisposable _cleanup = new();
    private readonly ReadOnlyObservableCollection<ProductViewModel> _productList;
    private string _searchString;
    private ICommand _doubleClickElementCommand;

    #endregion

    #region Properties
    public ICommand ClearSearchCommand { get; }

    public ICommand DoubleClickElementCommand
    {
        get => _doubleClickElementCommand;
        set => SetProperty(ref _doubleClickElementCommand, value);
    }

    //todo: добавить фильтрацию по наличию на складе
    public ReadOnlyObservableCollection<ProductViewModel> ProductsList => _productList;

    public string SearchString
    {
        get => _searchString;
        set => SetProperty(ref _searchString, value);
    }

    #endregion

    public ProductListViewModel()
    {
        _dbManager = DbManager.GetInstance();
        _productsNotifier = DefaultNotifier<Products, int>.GetInstance();

        SearchString = string.Empty;
        ClearSearchCommand = new MyCommand(ExecuteClearSearch);

        _cleanup.Add(_productsNotifier.Changes
                .Filter(this.WhenValueChanged(x => x.SearchString)
                    .Select(SearchFilter))
                .SortBy(x => x.Id)
                .Transform(x => new ProductViewModel(x))
                .Bind(out _productList)
                .Subscribe());
    }

    public async Task InitializeAsync()
    {
        var productList = await _dbManager.GetListAsync<Products>();
        productList.ForEach(_productsNotifier.NotifyAdd);
    }

    private static Func<Products, bool> SearchFilter(string searchStr)
    {
        var searchStrLower = searchStr.ToLower();
        return x => x.ProductName.ToLower().Contains(searchStrLower) ||
                    x.Id.ToString().Contains(searchStrLower);
    }

    private void ExecuteClearSearch()
    {
        SearchString = string.Empty;
    }
    
    public void Dispose()
    {
        _cleanup.Dispose();
    }
}
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
    private bool _inStockOnly;

    #endregion

    #region Properties
    public ICommand ClearSearchCommand { get; }

    public ICommand DoubleClickElementCommand
    {
        get => _doubleClickElementCommand;
        set => SetProperty(ref _doubleClickElementCommand, value);
    }

    public ReadOnlyObservableCollection<ProductViewModel> ProductsList => _productList;

    public string SearchString
    {
        get => _searchString;
        set => SetProperty(ref _searchString, value);
    }

    public bool InStockOnly
    {
        get => _inStockOnly;
        set => SetProperty(ref _inStockOnly, value);
    }

    #endregion

    public ProductListViewModel()
    {
        _dbManager = DbManager.GetInstance();
        _productsNotifier = DefaultNotifier<Products, int>.GetInstance();

        SearchString = string.Empty;
        InStockOnly = true;
        ClearSearchCommand = new MyCommand(ExecuteClearSearch);

        _cleanup.Add(_productsNotifier.Changes
                .Transform(x => new ProductViewModel(x))
                .Filter(this.WhenValueChanged(x => x.SearchString)
                    .Select(SearchFilter))
                .Filter(this.WhenValueChanged(vm => vm.InStockOnly)
                    .Select(StockFilter))
                .SortBy(x => x.Id)
                .Bind(out _productList)
                .Subscribe());
    }

    public async Task InitializeAsync()
    {
        var productList = await _dbManager.GetListAsync<Products>();
        productList.ForEach(_productsNotifier.NotifyAdd);
    }

    private static Func<ProductViewModel, bool> SearchFilter(string searchStr)
    {
        var searchStrLower = searchStr.ToLower();
        return x => x.ProductName.ToLower().Contains(searchStrLower) ||
                    x.Id.ToString().Contains(searchStrLower);
    }

    private static Func<ProductViewModel, bool> StockFilter(bool inStockOnly)
        => x => !inStockOnly || x.WarehouseQuantity > 0;

    private void ExecuteClearSearch()
    {
        SearchString = string.Empty;
    }
    
    public void Dispose()
    {
        _cleanup.Dispose();
    }
}
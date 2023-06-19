using System;
using System.Collections.ObjectModel;
using System.Reactive.Disposables;
using System.Threading.Tasks;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using DynamicData;

namespace DataMiningForShoppingBasket.ViewModels
{
    public sealed class FocusProductListViewModel : NotifyPropertyChangedImplementation, IAsyncInitialized, IDisposable
    {
        private readonly IDbManager _dbManager;
        private readonly INotifier<FocusProducts, int> _focusProductsINotifier;
        private readonly ReadOnlyObservableCollection<FocusProductViewModel> _focusProductList;
        private readonly CompositeDisposable _cleanup = new();
        private IAsyncCommand _doubleClickElementCommand;

        public FocusProductListViewModel()
        {
            _dbManager = DbManager.GetInstance();
            _focusProductsINotifier = DefaultNotifier<FocusProducts, int>.GetInstance();

            _cleanup.Add(_focusProductsINotifier.Changes
                    .Transform(x => new FocusProductViewModel(x))
                    .Bind(out _focusProductList)
                    .Subscribe());
        }

        public ReadOnlyObservableCollection<FocusProductViewModel> FocusProductList => _focusProductList;

        public IAsyncCommand DoubleClickElementCommand
        {
            get => _doubleClickElementCommand;
            set => SetProperty(ref _doubleClickElementCommand, value);
        }

        //! придумать как вызывать этот метод более корректно и удалить класс AsyncInitializedCreator
        public async Task InitializeAsync()
        {
            var focusProductsList = await _dbManager.GetListAsync<FocusProducts>();
            focusProductsList.ForEach(_focusProductsINotifier.NotifyAdd);
        }

        public void Dispose()
        {
            _cleanup?.Dispose();
        }
    }
}

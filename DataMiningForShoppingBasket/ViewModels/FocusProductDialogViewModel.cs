using DataMiningForShoppingBasket.Commands;
using DataMiningForShoppingBasket.Common;
using DataMiningForShoppingBasket.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace DataMiningForShoppingBasket.ViewModels
{
    public class FocusProductDialogViewModel : NotifyPropertyChangedImplementation
    {
        private const int TimeZoneFinishHourCorrection = 24;
        private const int TimeDateSecondsCorrection = -1;

        private readonly FocusProducts _focusProduct;
        private readonly IDbManager _dbManager;

        public FocusProductDialogViewModel(FocusProducts focusProduct = null)
        {
            _dbManager = DbManager.GetInstance();

            ProductList = _dbManager.GetListAsync<Products>().Result
                .OrderBy(x=>x.ProductName).ToList();
            SaveCommand = new MyAsyncCommand<Window>(SaveExecuteAsync);

            _focusProduct = focusProduct ?? new FocusProducts
            {
                StartDate = DateTime.Today.ToUniversalTime(),
                FinishDate = DateTime.Today.ToUniversalTime()
            };
            Description = _focusProduct.Description;
            StartDate = _focusProduct.StartDate.ToLocalTime();
            FinishDate = _focusProduct.FinishDate.ToLocalTime();
            ProductId= _focusProduct.ProductId;
            DiscountCost = _focusProduct.DiscountCost;
        }

        #region Properties

        public ICommand SaveCommand { get; }

        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime FinishDate { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountCost { get; set; }

        public IReadOnlyCollection<Products> ProductList { get; }

        #endregion

        private async Task SaveExecuteAsync(Window window)
        {
            try
            {
                _focusProduct.Description = Description ?? string.Empty;
                _focusProduct.StartDate = StartDate.Date.ToUniversalTime();
                _focusProduct.FinishDate = FinishDate.Date.AddHours(TimeZoneFinishHourCorrection)
                    .AddSeconds(TimeDateSecondsCorrection).ToUniversalTime();
                _focusProduct.ProductId = ProductId;
                _focusProduct.DiscountCost = DiscountCost;
                await _dbManager.SaveAndNotifyHavingIdEntityAsync<FocusProducts, int>(_focusProduct);

                window.DialogResult = true;
            }
            catch (Exception e)
            {
                MessageWriter.ShowMessage(e.Message);
                window.DialogResult = false;
            }
            finally
            {
                window.Close();
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataMiningForShopingBasket.Events
{
    public interface IChangeWindowCaller
    {
        object DataContext { get; set; }
        IChangeWindowCallerDataContext CustomDataContext { get; set; }
    }

    public interface IChangeWindowCallerDataContext
    {
        event ChangeWindowEventHandler ChangeWindowCalled;
    }

    public delegate void ChangeWindowEventHandler(object sender, IChangeWindowCaller e);
}

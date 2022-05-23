using System.Windows;

namespace DataMiningForShoppingBasket.CommonClasses
{
    public static class MessageWriter
    {
        public static void ShowMessage(string message)
        {
            MessageBox.Show(message);
        }

        public static bool ShowMessage(string message, string caption)
        {
            var answer = MessageBox.Show(message, caption, MessageBoxButton.YesNo);
            return answer == MessageBoxResult.Yes;

        }
    }
}

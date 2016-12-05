using System;

namespace Drcom_Dialer.ViewModel
{
    /// <summary>
    /// 所有ViewModel继承
    /// </summary>
    public class ViewModelBase:NotifyProperty
    {
        public ViewModelBase()
        {

        }


    }

    /// <summary>
    /// 导航
    /// 存在内部的页面也可以导航
    /// </summary>
    interface INagivate
    {
        void NavigateTo(Type viewModel, object parameter);
    }

    /// <summary>
    /// 可以被导航
    /// </summary>
    interface INagivateable
    {
        /// <summary>
        /// 跳转到
        /// </summary>
        /// <param name="parameter"></param>
        void OnNavigateTo(object parameter);
        /// <summary>
        /// 跳转出
        /// </summary>
        /// <param name="parameter"></param>
        void OnNavigateFrom(object parameter);
    }
}
using System.Windows.Input;
using MvvmCross.Commands;
using MvvmCross.Navigation;
using MvvmCross.ViewModels;

namespace SpecialTestApp.Core.ViewModels
{
    public abstract class BaseViewModel<T> : MvxViewModel<T>
    {
        protected IMvxNavigationService NavigationService { get; }
        public ICommand BackCommand => new MvxCommand(() => NavigationService.Close(this));

        protected BaseViewModel(IMvxNavigationService navigationService)
        {
            NavigationService = navigationService;
        }
    }

    public abstract class BaseViewModel : BaseViewModel<object>
    {
        protected BaseViewModel(IMvxNavigationService navigationService) : base(navigationService)
        {

        }
    }
}

namespace StateBuilder.Library.Infrastructure.Views
{
    public interface IViewController<TViewModel>
        where TViewModel : new()//ViewModelBase, new()
    {
        TViewModel ViewModel { get; }
        void Run();
    }
}
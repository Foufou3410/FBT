using WpfApplication1.Model.Initializer;

namespace WpfApplication1.ViewModel.InitializerViewModel
{
    public interface IInitializerViewModel
    {
        #region Public Properties

        IInitializer Initializer { get; }
        string Name { get; }

        #endregion Public Properties
    }
}

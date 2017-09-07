using FBT.Model.Initializer;

namespace FBT.ViewModel.InitializerViewModel
{
    public interface IInitializerViewModel
    {
        #region Public Properties

        IInitializer Initializer { get; }
        string Name { get; }

        #endregion Public Properties
    }
}

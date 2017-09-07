using FBT.Model;
using FBT.Model.Initializer;

namespace FBT.ViewModel.InitializerViewModel
{
    public class HardCodeInitializerViewModel : IInitializerViewModel
    {
        #region Private Fields

        private IInitializer initializer;

        #endregion Private Fields

        #region Public Constructors

        public HardCodeInitializerViewModel()
        {
            initializer = new HardCodeInitializer();
        }

        #endregion Public Constructors

        #region Public Properties

        public IInitializer Initializer
        {
            get
            {
                return initializer;
            }
        }

        public string Name
        {
            get
            {
                return "HardCode";
            }
        }
        #endregion Public Properties
    }
}

using System.Collections.ObjectModel;
using System.Linq;

namespace Engine
{
    public struct Origin
    {
        #region Properties

        public string Algorithm { get; }
        public ReadOnlyCollection<Operand> Operands { get; }

        #endregion

        #region Constructor

        public Origin(string algorithm, params Operand[] operands)
        {
            Algorithm = algorithm;
            Operands = operands.ToList().AsReadOnly();
        }

        #endregion
    }
}

using System;

namespace Engine
{
    internal class Origin
    {
        #region Properties

        public Operand Left { get; }
        public Operand Right { get; }
        public Operations Operation { get; }

        public bool Binary
        {
            get
            {
                switch (Operation)
                {
                    case Operations.Add:
                    case Operations.Divide:
                    case Operations.Remainder:
                    case Operations.Subtract:
                    case Operations.Max:
                    case Operations.Min:
                    case Operations.Multiply:
                        return true;
                    case Operations.Ceiling:
                    case Operations.Floor:
                    case Operations.Round:
                        return false;
                    default:
                        throw new NotImplementedException();
                }
            }
        }

        #endregion

        #region Constructor

        public Origin(Operand left, Operand right, Operations operation)
        {
            Left = left;
            Right = right;
            Operation = operation;
        }

        #endregion
    }
}

using System;
using System.Linq;

namespace Engine
{
    // Ported & slightly modified to allow multi character variables from https://stackoverflow.com/a/26941309
    internal class RecursiveDescentParser
    {
        #region Constants

        private const char EOL = char.MinValue;

        #endregion

        #region Fields

        private readonly string
            _input;

        private int
            _curPos;

        #endregion

        #region Properties

        private char CurrentToken
        {
            get
            {
                if (_curPos < 0 || string.IsNullOrEmpty(_input) || _curPos > _input.Length - 1 || _input.ElementAt(_curPos) == 0)
                    return EOL;

                return _input.ElementAt(_curPos);
            }
        }

        #endregion

        #region Constructor

        public RecursiveDescentParser(string input)
        {
            _input = input;
            _curPos = -1;
        }

        #endregion

        #region Methods

        public string Parse()
        {
            NextToken();
            var result = Expression();
            if(CurrentToken != EOL)
                throw new InvalidOperationException($"Found unexpected character: {CurrentToken}");
            return result.Text;
        }

        private void NextToken()
        {
            if(_curPos >= _input.Length)
                throw new InvalidOperationException(@"Unexpected end of input!");

            do{ ++_curPos; }
            while (CurrentToken != EOL && CurrentToken == ' ');
        }

        private Result Expression()
        {
            var left = Term();

            var op = CurrentToken;
            if (op != '+' && op != '-')
                return left;

            NextToken();

            var right = Term();

            if (op == '-' && (right.Op == '-' || right.Op == '+'))
                right = EncloseInParentheses(right);

            return new Result(left.Text + " " + op + " " + right.Text, op);
        }

        private Result Term()
        {
            var left = Factor();

            var op = CurrentToken;
            if (op != '*' && op != '/')
                return left;

            NextToken();

            var right = Factor();

            if (left.Op == '+' || left.Op == '-')
                left = EncloseInParentheses(left);
            if (right.Op == '+' || right.Op == '-' || (op == '/' && (right.Op == '/' || right.Op == '*')))
                right = EncloseInParentheses(right);

            return new Result(left.Text + " " + op + " " + right.Text, op);
        }

        private Result Factor()
        {
            if (CurrentToken == '(')
                return Parenthesis();
            if (char.IsLetter(CurrentToken) || char.IsDigit(CurrentToken))
                return Variable();

            throw new InvalidOperationException($"Expected variable or '(', found {CurrentToken} at position {_curPos}");
        }

        private Result Parenthesis()
        {
            NextToken();
            var result = Expression();
            if(CurrentToken != ')')
                throw new InvalidOperationException($"Expected ')', found {CurrentToken} at position {_curPos}");
            NextToken();

            return result;
        }

        private Result Variable()
        {
            var resultText = $"{CurrentToken}";
            NextToken();
            while (char.IsLetter(CurrentToken) || char.IsDigit(CurrentToken))
            {
                resultText += CurrentToken;
                NextToken();
            }

            var result = new Result(resultText, ' ');
            return result;
        }

        private static Result EncloseInParentheses(Result result)
        {
            return new Result($"({result.Text})", result.Op);
        }

        #endregion

        #region Nested Classes/Structs/...

        private struct Result
        {
            #region Properties

            public string Text { get; }
            public char Op { get; }

            #endregion

            #region Constructor

            public Result(string text, char op)
            {
                Text = text;
                Op = op;
            }

            #endregion
        }

        #endregion
    }
}

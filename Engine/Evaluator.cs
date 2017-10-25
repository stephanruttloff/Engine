using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace Engine
{
    public static class Evaluator
    {
        #region Interface

        /// <summary>
        /// Evaluate an algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <param name="variables">Variables that are used by the alghorithm.</param>
        /// <returns></returns>
        public static double Evaluate(string algorithm, params Operand[] variables)
        {
            return Evaluate(algorithm, null, variables);
        }

        /// <summary>
        /// Evaluate an algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <param name="context">Context of the evaluation e.g. object that has constants defined that are used by the algorithm.</param>
        /// <param name="variables">Variables that are used by the alghorithm.</param>
        /// <returns></returns>
        public static double Evaluate(string algorithm, object context, params Operand[] variables)
        {
            var injectedFormula = InjectValues(algorithm, context, variables);
            var dt = new DataTable();
            var result = dt.Compute(injectedFormula, string.Empty);

            return double.Parse(result.ToString());
        }

        /// <summary>
        /// Inject values into the algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <param name="variables">Variables that are used by the alghorithm.</param>
        /// <returns></returns>
        public static string InjectValues(string algorithm, params Operand[] variables)
        {
            return InjectValues(algorithm, null, variables);
        }

        /// <summary>
        /// Inject values into the algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <param name="context">Context of the evaluation e.g. object that has constants defined that are used by the algorithm.</param>
        /// <param name="variables">Variables that are used by the alghorithm.</param>
        /// <returns></returns>
        public static string InjectValues(string algorithm, object context, params Operand[] variables)
        {
            var ids = GetIdsFromFormula(algorithm);
            var constants = GetConstantOperators(context);
            var operands = GetOperandsFromIds(ids, constants, variables);

            string MatchEvaluator(Match match)
            {
                var id = match.Groups[1].Value;
                var operand = operands.Single(x => Equals(x.Id, id));
                return operand.Value.ToString(CultureInfo.InvariantCulture);
            }

            var injectedFormula =
                Regex
                    .Replace(algorithm, Operand.Regex_Op_Id_Placeholder, MatchEvaluator)
                    .Replace(',', '.');

            return injectedFormula;
        }

        /// <summary>
        /// Inject variable and/or constant names into the algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <param name="variables">Variables that are used by the alghorithm.</param>
        /// <returns></returns>
        public static string GetFormula(string algorithm, params Operand[] variables)
        {
            return GetFormula(algorithm, null, variables);
        }

        /// <summary>
        /// Inject variable and/or constant names into the algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <param name="context">Context of the evaluation e.g. object that has constants defined that are used by the algorithm.</param>
        /// <param name="variables">Variables that are used by the alghorithm.</param>
        /// <returns></returns>
        public static string GetFormula(string algorithm, object context, params Operand[] variables)
        {
            var ids = GetIdsFromFormula(algorithm);
            var constants = GetConstantOperators(context);
            var operands = GetOperandsFromIds(ids, constants, variables);

            string MatchEvaluator(Match match)
            {
                var id = match.Groups[1].Value;
                var operand = operands.Single(x => Equals(x.Id, id));
                return operand.Name;
            }

            var readableFormula = Regex.Replace(algorithm, Operand.Regex_Op_Id_Placeholder, MatchEvaluator);

            return readableFormula;
        }

        /// <summary>
        /// Strip unnecessary/duplicate parentheses from a formula
        /// </summary>
        /// <param name="formula">The formula string (NOT the algorithm).</param>
        /// <returns></returns>
        public static string Simplify(string formula)
        {
            var parser = new RecursiveDescentParser(formula);
            return parser.Parse();
        }

        /// <summary>
        /// Recover the algorithm from an operand's origin tree
        /// </summary>
        /// <param name="operand">The operand to get the origin algorithm from.</param>
        /// <param name="involvedOperands">Operands that are used by the origin algorithm.</param>
        /// <returns></returns>
        public static string RecoverAlgorithm(Operand operand, out IReadOnlyCollection<Operand> involvedOperands)
        {
            if (!operand.HasOrigin)
            {
                involvedOperands = Enumerable.Empty<Operand>().ToList().AsReadOnly();
                return operand.ToString();
            }

            var ids = GetIdsFromFormula(operand.Origin.Algorithm);
            var operands = operand.Origin.Operands.ToList();
            var algorithm = operand.Origin.Algorithm;
            foreach (var id in ids)
            {
                var originOperand = operands.Single(x => Equals(x.Id, id));
                algorithm = algorithm.Replace($"[[{id}]]", RecoverAlgorithm(originOperand, out var involvedChildOperands));
                operands.AddRange(involvedChildOperands);
            }

            ids = GetIdsFromFormula(algorithm);
            operands = GetOperandsFromIds(ids, operands).ToList();
            involvedOperands = operands.AsReadOnly();
            return algorithm;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Get constants from a context object (properties/fields that have been marked with the Constant attribute)
        /// </summary>
        /// <param name="context">The context object.</param>
        /// <returns></returns>
        private static Operand[] GetConstantOperators(object context)
        {
            if (context == null)
                return Enumerable.Empty<Operand>().ToArray();

            var type = context.GetType();
            var properties = type.GetProperties();
            var propertyConstants = properties.Where(x=>Attribute.IsDefined(x, typeof(Constant))).Select(x=>x.GetValue(context)).OfType<Operand>();
            var fields = type.GetFields();
            var fieldConstants = fields.Where(x => Attribute.IsDefined(x, typeof(Constant))).Select(x => x.GetValue(context)).OfType<Operand>();

            return propertyConstants.Concat(fieldConstants).ToArray();
        }

        /// <summary>
        /// Filter operands by ids
        /// </summary>
        /// <param name="ids">Whitelist of IDs to get operands from.</param>
        /// <param name="constants">Constant operands.</param>
        /// <param name="variables">Variable operands.</param>
        /// <returns></returns>
        private static Operand[] GetOperandsFromIds(IReadOnlyCollection<string> ids,
            IReadOnlyCollection<Operand> constants, params Operand[] variables)
        {
            var constantOperands =
                from c in constants
                from id in ids
                where Equals(c.Id, id)
                select c;

            var variableOperants =
                from v in variables
                from id in ids
                where Equals(v.Id, id)
                select v;

            return constantOperands.Concat(variableOperants).ToArray();
        }

        /// <summary>
        /// Get all ids from an algorithm
        /// </summary>
        /// <param name="algorithm">The algorithm string.</param>
        /// <returns></returns>
        private static string[] GetIdsFromFormula(string algorithm)
        {
            var matches = Regex.Matches(algorithm, Operand.Regex_Op_Id);
            var ids = matches.Cast<Match>().Select(x=>x.Value);

            return ids.Distinct().ToArray();
        }

        #endregion
    }
}

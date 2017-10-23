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

        public static double Evaluate(string formula, params Operand[] variables)
        {
            return Evaluate(formula, null, variables);
        }

        public static double Evaluate(string formula, object context, params Operand[] variables)
        {
            var injectedFormula = InjectValues(formula, context, variables);
            var dt = new DataTable();
            var result = dt.Compute(injectedFormula, string.Empty);

            return double.Parse(result.ToString());
        }

        public static string InjectValues(string formula, params Operand[] variables)
        {
            return InjectValues(formula, null, variables);
        }

        public static string InjectValues(string formula, object context, params Operand[] variables)
        {
            var ids = GetIdsFromFormula(formula);
            var constants = GetConstantOperators(context);
            var operands = GetOperandsFromIds(ids, constants, variables);
            MatchEvaluator evaluator = match =>
            {
                var id = match.Groups[1].Value;
                var operand = operands.Single(x => Equals(x.Id, id));
                return operand.Value.ToString(CultureInfo.InvariantCulture);
            };
            var injectedFormula =
                Regex
                    .Replace(formula, Operand.Regex_Op_Id_Placeholder, evaluator)
                    .Replace(',', '.');

            return injectedFormula;
        }

        public static string GetFormula(string formula, params Operand[] variables)
        {
            return GetFormula(formula, null, variables);
        }

        public static string GetFormula(string formula, object context, params Operand[] variables)
        {
            var ids = GetIdsFromFormula(formula);
            var constants = GetConstantOperators(context);
            var operands = GetOperandsFromIds(ids, constants, variables);
            MatchEvaluator evaluator = match =>
            {
                var id = match.Groups[1].Value;
                var operand = operands.Single(x => Equals(x.Id, id));
                return operand.Name;
            };
            var readableFormula = Regex.Replace(formula, Operand.Regex_Op_Id_Placeholder, evaluator);

            return readableFormula;
        }

        #endregion

        #region Methods

        private static Operand[] GetConstantOperators(object context)
        {
            if (context == null)
                return Enumerable.Empty<Operand>().ToArray();

            Type type = context.GetType();
            var properties = type.GetProperties();
            var propertyConstants = properties.Where(x=>Attribute.IsDefined(x, typeof(Constant))).Select(x=>x.GetValue(context)).OfType<Operand>();
            var fields = type.GetFields();
            var fieldConstants = fields.Where(x => Attribute.IsDefined(x, typeof(Constant))).Select(x => x.GetValue(context)).OfType<Operand>();

            return propertyConstants.Concat(fieldConstants).ToArray();
        }

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

        private static string[] GetIdsFromFormula(string formula)
        {
            var matches = Regex.Matches(formula, Operand.Regex_Op_Id);
            var ids = matches.Cast<Match>().Select(x=>x.Value);

            return ids.Distinct().ToArray();
        }

        #endregion
    }
}

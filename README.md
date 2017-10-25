# Engine

Very basic compute engine that allows you to recover the algorithm as a human readable string.

## Example

```c-sharp
var simple = (c.A + 2) / ((c.B - var1) * var2) + c.C;
var algorithm = Evaluator.RecoverAlgorithm(simple, out var operands);
var injected = Evaluator.InjectValues(algorithm, null, operands.ToArray());
var formula = Evaluator.GetFormula(algorithm, null, operands.ToArray());
var simplified = Evaluator.Simplify(formula);
var simplifiedInjected = Evaluator.Simplify(injected);
Console.WriteLine("Operands:");
Console.WriteLine("=========");
Console.WriteLine(@"Algorithm:  " + algorithm);
Console.WriteLine(@"Formula:    " + formula);
Console.WriteLine(@"Simplified: " + simplified);
Console.WriteLine(@"Injected:   " + injected);
Console.WriteLine(@"Simplified: " + simplifiedInjected);
Console.WriteLine(@"Result:     " + simple.Value);
```

!()[demo_result.png]

See Demo project for examples on how to use this.
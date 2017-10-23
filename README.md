# Engine

Very basic compute engine that allows you to recover the algorithm as a human readable string.

## Simple Example

```c#
var var1 = new Operand(0.3);
var var2 = var1 + 2;
var fSimple = var1 + var2;

Console.WriteLine(Evaluator.GetFormula(fSimple, var1, var2));
//> 0.3 + (0.3 + 2)
Console.WriteLine(Evaluator.InjectValues(fSimple, var1, var2));
//> 0.3 + 2.3
Console.WriteLine(Evaluator.Evaluate(fSimple, var1, var2));
//> 2.6
```

## Complex Example

```c#
class Constants
{
  [Constant]
  public Operand A = new Operand(@"A", 100);
  [Constant]
  public Operand B = new Operand(@"B", 200);
  [Constant]
  public Operand C = new Operand(@"C", 300);
}

...

var c = new Constants();
var var1 = new Operand(0.3);
var var2 = var1 + 2;

var fComplex = $"{c.A} + ({c.B} / {c.C}) + (0.3 / {var1}) * {var2}";

Console.WriteLine(Evaluator.GetFormula(fComplex, c, var1, var2));
//> A + (B / C) + (0.3 / 0.3) * (0.3  + 2)
Console.WriteLine(Evaluator.InjectValues(fComplex, c, var1, var2));
//> 100 + (200 / 300) + (0.3 / 0.3) * 2.3
Console.WriteLine(Evaluator.Evaluate(fComplex, c, var1, var2));
//> 102.96...
```
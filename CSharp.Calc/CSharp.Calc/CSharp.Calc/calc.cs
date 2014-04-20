using System;
using System.Globalization;

namespace CSharp.Calc
{
    public class Calc
    {
        public string StatementParser(string str)
        {
            var result = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (i < str.Length && str[i] == 'c')
                {
                    if (i < str.Length - 2 && str[i + 1] == 'o' && str[i + 2] == 's')
                    {
                        var openBracket = 1;
                        var closedBracket = 0;
                        i = i + 4;
                        var tmp = "";
                        while (i < str.Length && openBracket != closedBracket)
                        {
                            if (str[i] == '(')
                                openBracket++;
                            if (str[i] == ')')
                                closedBracket++;
                            if (openBracket != closedBracket)
                                tmp = tmp + str[i];
                            i++;
                        }
                        tmp = StatementParser(tmp);
                        tmp = ToPolish(tmp);
                        var resultWithin = Calculate(tmp);
                        result = result + '~' + resultWithin.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                        result = result + str[i];
                }
                if (i < str.Length && str[i] == 's')
                {
                    if (i < str.Length - 2 && str[i + 1] == 'i' && str[i + 2] == 'n')
                    {
                        var openBracket = 1;
                        var closedBracket = 0;
                        i = i + 4;
                        var tmp = "";
                        while (i < str.Length && openBracket != closedBracket)
                        {
                            if (str[i] == '(')
                                openBracket++;
                            if (str[i] == ')')
                                closedBracket++;
                            if (openBracket != closedBracket)
                                tmp = tmp + str[i];
                            i++;
                        }
                        tmp = StatementParser(tmp);
                        tmp = ToPolish(tmp);
                        var resultWithin = Calculate(tmp);
                        result = result + '!' + resultWithin.ToString(CultureInfo.InvariantCulture);
                    }
                    else
                        result = result + str[i];
                }
                if (i < str.Length && str[i] != 'c' && str[i] != 's')
                    result = result + str[i];
            }
            return result;
        }
        public double SimpleOp(char op, double x, double y)
        {
            double answ = 0;
            switch (op)
            {
                case '+':
                    answ = y + x;
                    break;
                case '-':
                    answ = y - x;
                    break;
                case '/':
                {
                    if (Math.Abs(x) <= double.Epsilon)
                        throw new Exception("division by zero");
                    Console.WriteLine(x);
                    answ = y/x;
                    break;
                }
                case '*':
                    answ = y*x;
                    break;
                case '^':
                    answ = Math.Pow(y, x);
                    break;
                case '~':
                    answ = Math.Cos(x);
                    break;
                case '!':
                    answ = Math.Sin(x);
                    break;
            }
            return answ;
        }
        public int SymbolPriority(char x)
        {
            var ans = 0;
            switch (x)
            {
                case '+':
                    ans = 2;
                    break;
                case '-':
                    ans = 2;
                    break;
                case '*':
                    ans = 3;
                    break;
                case '/':
                    ans = 3;
                    break;
                case '^':
                    ans = 3;
                    break;
                case '(':
                    ans = 1;
                    break;
                //cosine
                case '~':
                    ans = 4;
                    break;
                //sine
                case '!':
                    ans = 4;
                    break;
            }
            return ans;
        }
        public string ToPolish(string str)
        {
            var inputed = new Stack<char>();
            var polishInverted = "";
            var flag = false;
            foreach (var x in str)
            {
                if (Char.IsDigit(x) || Char.IsLetter(x) || x == '.' || x == ',')
                {
                    var y = x;
                    if (x == '.')
                        y = ',';
                    polishInverted = polishInverted + y;
                    flag = true;
                }
                else
                {
                    if (flag)
                    {
                        polishInverted = polishInverted + '@';
                        flag = false;
                    }
                    if (x != '(' && x != ')' && (inputed.Empty() || SymbolPriority(x) > SymbolPriority(inputed.Top.Data)))
                    {
                        inputed.Push(new StackNode<char>(x));
                        continue;
                    }
                    if (x != '(' && x != ')' && !inputed.Empty() && SymbolPriority(inputed.Top.Data) >= SymbolPriority(x))
                    {
                        while (inputed.Size > 0 && SymbolPriority(inputed.Top.Data) >= SymbolPriority(x))
                        {
                            polishInverted = polishInverted + inputed.Top.Data + '@';
                            inputed.Pop();
                        }
                        inputed.Push(new StackNode<char>(x));
                        continue;
                    }
                    if (x == '(')
                        inputed.Push(new StackNode<char>(x));
                    if (x == ')')
                    {
                        while (!inputed.Empty() && inputed.Top.Data != '(')
                        {
                            polishInverted = polishInverted + inputed.Top.Data + '@';
                            inputed.Pop();
                        }
                        inputed.Pop();
                    }
                }
            }
            if (flag)
                polishInverted = polishInverted + '@';
            while (!inputed.Empty())
            {
                polishInverted = polishInverted + inputed.Top.Data + '@';
                inputed.Pop();
            }
            //Console.WriteLine(polishInverted);
            return polishInverted;
        }
        public double Calculate(string polishInverted)
        {
            var calcStack = new Stack<double>();
            for (var i = 0; i < polishInverted.Length; i++)
            {
                var x = polishInverted[i];
                if (Char.IsDigit(x))
                {
                    var j = i + 1;
                    var number = x.ToString(CultureInfo.InvariantCulture);
                    while (j < polishInverted.Length && polishInverted[j] != '@')
                    {
                        number = number + polishInverted[j].ToString(CultureInfo.InvariantCulture);
                        j++;
                    }
                    i = j;
                    //Console.WriteLine(number);
                    calcStack.Push(new StackNode<double>(Convert.ToDouble(number)));
                }
                else
                {
                    if (x == '@')
                        continue;
                    if (x == '~')
                    {
                        var y = calcStack.Top.Data;
                        calcStack.Pop();
                        calcStack.Push(new StackNode<double>(SimpleOp(x, y, y)));
                    }
                    if (x == '!')
                    {
                        var y = calcStack.Top.Data;
                        calcStack.Pop();
                        calcStack.Push(new StackNode<double>(SimpleOp(x, y, y)));
                    }
                    if (x != '~' && x != '!')
                    {
                        var y = calcStack.Top.Data;
                        calcStack.Pop();
                        var z = calcStack.Top.Data;
                        calcStack.Pop();
                        calcStack.Push(new StackNode<double>(SimpleOp(x, y, z)));
                    }
                }
            }
            return calcStack.Top.Data;
        }
    }
}

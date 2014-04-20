using System;
using System.Globalization;

namespace CSharp.Calc
{
    class Shell
    {
        private int _quantatyFunc;
        private Vector<string> _formulas;

        private void AgregateFormulas()
        {
            Console.Write("f"+(_formulas.Size+1)+"=");
            var str = Console.ReadLine();
            if(str == null) throw new Exception("bad usage");
            var result = "";
            for (var i = 0; i < str.Length; i++)
            {
                if (str[i] == 'f')
                {
                    i++;
                    var number = "";
                    while (i < str.Length && Char.IsDigit(str[i]))
                    {
                        number += str[i];
                        i++;
                    }
                    i--;
                    var index = Convert.ToInt32(number) - 1;
                    result += _formulas.Data[index];
                }
                else
                {
                    result += str[i];
                }
            }
            //Console.WriteLine(result);
            _formulas.Push(result);
            Console.WriteLine("saved as f"+_formulas.Size+"="+result);          
        }
        private string Substitute(int formulaId)
        {
            var tmp = _formulas.Data[formulaId];
            var parameters = new Vector<char>();
            var subs = new Vector<double>();
            for (var i = 0; i < tmp.Length; i++)
            {
                if (Char.IsLetter(tmp[i]) && (i + 1 >= tmp.Length || !Char.IsLetter(tmp[i + 1])) &&
                    (i - 1 < 0 || !Char.IsLetter(tmp[i - 1])))
                {
                    if (parameters.Search(tmp[i]) == -1)
                        parameters.Push(tmp[i]);
                }
            }
            for (var i = 0; i < parameters.Size; i++)
            {
                Console.Write("@" + parameters.Data[i].ToString(CultureInfo.InvariantCulture) + "=");
                var rawInput = Console.ReadLine();
                if (rawInput == null) throw new Exception("bad alloc");
                var input = "";
                foreach (var x in rawInput)
                {
                    if (x == '.')
                        input = input + ',';
                    else
                    {
                        input = input + x;
                    }
                }
                subs.Push(Convert.ToDouble(input));
            }
            var res = "";
            for (var i = 0; i < tmp.Length; i++)
            {
                if (Char.IsLetter(tmp[i]) && (i + 1 >= tmp.Length || !Char.IsLetter(tmp[i + 1])) &&
                    (i - 1 < 0 || !Char.IsLetter(tmp[i - 1])))
                {
                    res = res + subs.Data[parameters.Search(tmp[i])].ToString(CultureInfo.InvariantCulture);
                }
                else
                {
                    res = res + tmp[i];
                }
            }
            //Console.WriteLine(res);
            return res;
        }
        public void Main()
        {
            var test = new Calc();
            _quantatyFunc = 0;
            _formulas = new Vector<string>();
            Console.WriteLine("type 'exit' to quit");
            while (true)
            {
                Console.WriteLine();
                try
                {
                    Console.Write("input:");
                    var str = Console.ReadLine();
                    switch (str)
                    {
                        case "new func":
                        {
                            _quantatyFunc++;
                            //Console.Write("name:");
                            //var name = Console.ReadLine();
                            Console.Write('f' + _quantatyFunc.ToString(CultureInfo.InvariantCulture) + '=');
                            var func = Console.ReadLine();
                            _formulas.Push(func);
                        }
                            break;
                        case "agr":
                            AgregateFormulas();
                            break;
                        default:
                            if (str == "exit")
                                return;
                            if (!string.IsNullOrEmpty(str) && str[0] == '@')
                            {
                                var temp = "";
                                for (var i = 2; i < str.Length; i++)
                                    temp = temp + str[i];
                                temp = Substitute(Convert.ToInt32(temp) - 1);
                                temp = test.StatementParser(temp);
                                temp = test.ToPolish(temp);
                                Console.WriteLine("output:" + test.Calculate(temp));
                            }
                            else
                            {
                                var parsed = test.StatementParser(str);
                                var inputInPolish = test.ToPolish(parsed);
                                Console.WriteLine("output:" + test.Calculate(inputInPolish));
                            }
                            break;
                    }
                }
                catch (Exception e)
                {
                    if (e.Message == "division by zero")
                        Console.WriteLine("error:" + e.Message);
                    else 
                        Console.WriteLine("error:bad usage");
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Calculator2
{
    public class Calculator
    {
        private const string SquareRoot = "sqr";

        private string _errorMessage = "";

        /// <summary>
        /// Uses for list expression 
        /// </summary>
        private class ExpressionItem
        {
            public bool IsOperand;
            public string GetValue;
            public string GetSign => GetValue;

            public ExpressionItem()
            {
                IsOperand = false;
                GetValue = "";
            }
        }

        private readonly int[,] _mapPrioritiesForSigns = new int[,]
        {
            {4, 1, 1, 1, 1, 1, 5, 1, 7, 1, 1},
            {2, 2, 2, 1, 1, 1, 2, 1, 7, 1, 1},
            {2, 2, 2, 1, 1, 1, 2, 1, 7, 1, 1},
            {2, 2, 2, 2, 2, 1, 2, 1, 7, 1, 1},
            {2, 2, 2, 2, 2, 1, 2, 1, 7, 1, 1},
            {5, 1, 1, 1, 1, 1, 3, 1, 7, 1, 1},
            {2, 6, 6, 2, 2, 1, 6, 6, 7, 1, 1},
            {2, 2, 2, 2, 2, 1, 2, 1, 7, 1, 1},
            {2, 2, 2, 2, 2, 1, 2, 1, 7, 1, 2},
        };

        private int GetXForSignOnArrow(string sign)
        {
            string[] indexesForSignOnArrow = new string[11] { "|", "+", "-", "*", "/", "(", ")", "#", "!", "^", SquareRoot };

            return GetCoordinateInCharArray(indexesForSignOnArrow, sign);
        }

        private int GetYForSignInWaitingStation(string sign)
        {
            string[] indexesForSignOnArrow = new string[9] { "|", "+", "-", "*", "/", "(", "#", "^", SquareRoot };

            return GetCoordinateInCharArray(indexesForSignOnArrow, sign);
        }

        private static int GetCoordinateInCharArray(string[] array, string item)
        {
            for (int i = 0; i < array.Length; i++)
            {
                if (item.ToLower() == array[i])
                {
                    return i;
                }
            }

            return -1;
        }

        /// <summary>
        /// If true RPN is done.
        /// </summary>
        private bool PrioritizationForSign(Stack<string> waitingStation, List<string> RPN, string signOnArrow)
        {
            bool impossibleCase = waitingStation.Peek() == ")";

            if (impossibleCase)
            {
                IncorrectExpression(RPN, waitingStation);
                return false;
            }

            int X = GetXForSignOnArrow(signOnArrow.ToString());
            int Y = GetYForSignInWaitingStation(waitingStation.Peek().ToString());

            int priority = _mapPrioritiesForSigns[Y, X];

            return PutUpSignsByPriority(RPN, waitingStation, priority, signOnArrow);
        }

        private bool PutUpSignsByPriority(List<string> RPN, Stack<string> waitingStation, int priority, string signOnArrow)
        {
            if (priority == 1)
            {
                ToWaitingStation(waitingStation, signOnArrow);
            }
            else if (priority == 2)
            {
                RedirectFromWaitingStation(RPN, waitingStation, signOnArrow);
            }
            else if (priority == 3)
            {
                HijackingFromWaitingStation(waitingStation);
            }
            else if (priority == 4)
            {
                return true;
            }
            else if (priority == 5)
            {
                return IncorrectExpression(RPN, waitingStation);
            }
            else if (priority == 6)
            {
                GetCouple(RPN, waitingStation, signOnArrow);
            }
            else if (priority == 7)
            {
                RPN.Add(signOnArrow.ToString());
            }

            return false;
        }

        private void RedirectFromWaitingStation(List<string> RPN, Stack<string> waitingStation, string signOnArrow)
        {
            RPN.Add(waitingStation.Pop().ToString());
            PrioritizationForSign(waitingStation, RPN, signOnArrow.ToString());
        }

        private void HijackingFromWaitingStation(Stack<string> waitingStation)
        {
            waitingStation.Pop();
        }

        private void ToWaitingStation(Stack<string> waitingStation, string signOnArrow)
        {
            waitingStation.Push(signOnArrow);
        }

        private void GetCouple(List<string> RPN, Stack<string> waitingStation, string signOnArrow)
        {
            RPN.Add(waitingStation.Pop().ToString());

            bool whetherToGetCouple = waitingStation.Count > 0 && waitingStation.Peek() != "(" && waitingStation.Peek() != "|";

            if (whetherToGetCouple)
            {
                RPN.Add(waitingStation.Pop().ToString());
            }

            PrioritizationForSign(waitingStation, RPN, signOnArrow.ToString());
        }

        private bool IncorrectExpression(List<string> RPN, Stack<string> waitingStation)
        {
            _errorMessage = "Не правильно виставлений вираз";
            return false;
        }

        private string DesignationHiddenUnits(string expression)
        {
            do
            {
                expression = expression.Replace("/-(", "/#(");
                expression = expression.Replace("*-(", "*#(");
                expression = expression.Replace("--(", "-#(");
                expression = expression.Replace("+-(", "+#(");
                expression = expression.Replace("(-(", "(#(");
                expression = expression.Replace("^-(", "^#(");

            } while (expression.Contains("/-(") || expression.Contains("*-(") || expression.Contains("--(") || expression.Contains("+-(") || expression.Contains("(-(") || expression.Contains("^-("));

            if (expression[0] == '-' && expression[1] == '(')
            {
                expression = expression.Remove(0, 1);
                expression = "#" + expression;
            }

            return expression;
        }

        /// <summary>
        /// String expression to list expressionItem
        /// </summary>
        private List<ExpressionItem> ExpressionToList(string expression)
        {
            List<ExpressionItem> listExpression = new List<ExpressionItem>();

            expression = DesignationHiddenUnits(expression);

            expression += '|';

            int i = 0;
            while (i < expression.Length)
            {
                ExpressionItem expressionItem;

                if (IsNumber(expression, i))
                {
                    expressionItem = MoveEntireNumberIntoExpressionItem(expression, ref i);
                }
                else
                {
                    expressionItem = MoveEntireSignIntoExpressionItem(expression, ref i);
                }

                if (_errorMessage != "")
                {
                    return listExpression;
                }

                listExpression.Add(expressionItem);
            }

            return listExpression;
        }

        private ExpressionItem MoveEntireSignIntoExpressionItem(string expression, ref int index)
        {
            ExpressionItem expressionItem = new ExpressionItem();

            bool isContinuationOfSign;

            if (char.IsLetter(expression[index]) == false)
            {
                MoveSignIntoExpressionItem(expressionItem, expression, ref index);
                return expressionItem;
            }

            do
            {
                MoveSignIntoExpressionItem(expressionItem, expression, ref index);

                isContinuationOfSign = index < expression.Length && char.IsLetter(expression[index]);

            } while (isContinuationOfSign);

            IsCorrectSign(expressionItem.GetValue);

            return expressionItem;
        }

        private void MoveSignIntoExpressionItem(ExpressionItem expressionItem, string expression, ref int index)
        {
            expressionItem.GetValue += expression[index];
            index++;
        }

        private void IsCorrectSign(string sign)
        {
            if (GetXForSignOnArrow(sign) == -1)
            {
                _errorMessage = $"Не визначено \"{sign}\"";
            }
        }

        private ExpressionItem MoveEntireNumberIntoExpressionItem(string expression, ref int index)
        {
            ExpressionItem expressionItem = new ExpressionItem();

            expressionItem.IsOperand = true;

            bool isContinuationOfNumber;

            do
            {
                expressionItem.GetValue += expression[index];
                index++;
                isContinuationOfNumber = expression[index] == '.' || char.IsDigit(expression[index]);

            } while (index < expression.Length && isContinuationOfNumber);

            return expressionItem;
        }

        /// <summary>
        /// Checks if the value in the expression is number 
        /// </summary>
        private bool IsNumber(string expression, int index)
        {
            if (char.IsDigit(expression[index]))
            {
                return true;
            }

            int previousI = index - 1;
            int nextI = index + 1;

            if (IsNegativeNumberAfterMathSign(expression, index))
            {
                return true;
            }

            if (index == 0 && IsNegativeNumber(expression, index))
            {
                return true;
            }

            return false;
        }

        private bool IsNegativeNumberAfterMathSign(string expression, int index)
        {
            int previousI = index - 1;
            int nextI = index + 1;

            bool isOutOfbBounds = (index > 0 && index < expression.Length - 1) == false;

            if (isOutOfbBounds)
            {
                return false;
            }

            if (IsCorrectMathSign(expression[previousI]) && IsNegativeNumber(expression, index))
            {
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if there can be a negative number after the sign
        /// </summary>
        /// <param name="sign">Math sign</param>
        private bool IsCorrectMathSign(char sign)
        {
            if (sign != ')' && sign != '!' && false == char.IsDigit(sign))
            {
                return true;
            }

            return false;
        }

        private bool IsNegativeNumber(string expression, int index)
        {
            int nextI = index + 1;

            if (expression[index] == '-' && char.IsDigit(expression[nextI]))
            {
                return true;
            }

            return false;
        }

        private List<string> TransformationInRPN(string expression)
        {
            List<string> RPN = new List<string>();

            Stack<string> waitingStation = new Stack<string>();

            waitingStation.Push("|");

            List<ExpressionItem> listExpression = ExpressionToList(expression);

            if (TransformationInRPN(listExpression, waitingStation, RPN) || _errorMessage != "")
            {
                return RPN;
            }

            MoveStackToList(waitingStation, RPN);

            RPN.RemoveAll((str) => "|" == str);

            return RPN;
        }

        /// <summary>
        /// if true RPN is finish
        /// </summary>
        private bool TransformationInRPN(List<ExpressionItem> listExpression, Stack<string> waitingStation, List<string> RPN)
        {
            foreach (ExpressionItem EItem in listExpression)
            {
                if (EItem.IsOperand)
                {
                    RPN.Add(EItem.GetValue);
                    continue;
                }

                if (PrioritizationForSign(waitingStation, RPN, EItem.GetSign))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// carry over the balance from waitingStation to RPN
        /// </summary>
        private static void MoveStackToList(Stack<string> stack, List<string> list)
        {
            while (stack.Count > 0)
            {
                list.Add(stack.Pop().ToString());
            }
        }

        public string CalculateRPN(List<string> RPN)
        {
            Stack<double> computingStack = new Stack<double>();

            bool isNumber;

            for (int i = 0; i < RPN.Count; i++)
            {
                isNumber = char.IsDigit(RPN[i][^1]);

                if (isNumber)
                {
                    NumberIntoStack(RPN, computingStack, i);
                    continue;
                }

                CalculateFromStack(RPN, computingStack, i);

                if (_errorMessage != "")
                {
                    return "error";
                }
            }

            return computingStack.Pop().ToString();
        }

        private void CalculateFromStack(List<string> RPN, Stack<double> computingStack, int index)
        {
            double firstNumber;
            double secondNumber = 0;

            string sign = RPN[index];

            if (sign == "#" || sign == "!" || sign.ToLower() == SquareRoot)
            {
                firstNumber = computingStack.Pop();
            }
            else
            {
                secondNumber = computingStack.Pop();
                firstNumber = computingStack.Pop();
            }

            ActionOnSign(computingStack, firstNumber, secondNumber, sign);
        }

        /// <summary>
        /// If true it is fail
        /// </summary>
        private void ActionOnSign(Stack<double> computingStack, double firstNumber, double secondNumber, string sign)
        {
            if (sign == "#")
            {
                computingStack.Push(-1.0 * firstNumber);
            }
            else if (sign == "-")
            {
                computingStack.Push(firstNumber - secondNumber);
            }
            else if (sign == "+")
            {
                computingStack.Push(firstNumber + secondNumber);
            }
            else if (sign == "*")
            {
                computingStack.Push(firstNumber * secondNumber);
            }
            else if (sign == "/")
            {
                computingStack.Push(firstNumber / secondNumber);
            }
            else if (sign == "!")
            {
                Factorial(computingStack, factorial: firstNumber);
            }
            else if (sign == "^")
            {
                computingStack.Push(Math.Pow(firstNumber, secondNumber));
            }
            else if (sign.ToLower() == SquareRoot)
            {
                CalculateSquareRoot(computingStack, firstNumber);
            }
        }

        private void CalculateSquareRoot(Stack<double> computingStack, double d)
        {
            if (d < 0)
            {
                _errorMessage = "Під коренем не може бути відємного числа";
                return;
            }

            computingStack.Push(Math.Sqrt(d));
        }

        private void Factorial(Stack<double> computingStack, double factorial)
        {
            bool cannotBeFactorial = (factorial < 0) || ((factorial == Math.Truncate(factorial)) == false);

            if (cannotBeFactorial)
            {
                _errorMessage = "Factorial може бути тільки цілим числом більшим за нуль або 0";
                return;
            }

            computingStack.Push(Factorial(factorial));
        }

        private double Factorial(double factorial)
        {
            if (factorial == 0)
            {
                return 1;
            }

            return Factorial(factorial - 1) * factorial;
        }

        private void NumberIntoStack(List<string> RPN, Stack<double> computingStack, int index)
        {
            RPN[index] = RPN[index].Replace(".", ",");
            computingStack.Push(double.Parse(RPN[index]));
        }

        public string Calculate(string expression, ref string errorMessage)
        {
            _errorMessage = "";

            expression = expression.Replace(" ", "");

            List<string>? RPN;

            try
            {
                RPN = TransformationInRPN(expression);

                if (_errorMessage != "")
                {
                    return _errorMessage;
                }

                string answer = CalculateRPN(RPN);

                errorMessage = _errorMessage;

                return answer;
            }
            catch (Exception)
            {
                return "error";
            }
        }
    }
}







/*
       1 2 3 4 5 6 7 8 9 10 11
       | + — * / ( ) # ! ^  Sqr вагони на стрілці  
     | 4 1 1 1 1 1 5 1 7 1  1
     + 2 2 2 1 1 1 2 1 7 1  1
     — 2 2 2 1 1 1 2 1 7 1  1
     * 2 2 2 2 2 1 2 1 7 1  1
     / 2 2 2 2 2 1 2 1 7 1  1
     ( 5 1 1 1 1 1 3 1 7 1  1
     # 2 6 6 2 2 1 6 5 7 1  1
     ^ 2 2 2 2 2 1 2 1 7 1  1
   Sqr 2 2 2 2 2 1 2 1 7 2  2

     ^ вагони в waiting station 

    1. Вагон на стрілці відправляється в waiting station.
    
    2. Останій вагон що находиться в waiting station направляється в RPN

    3. Вагон, котрий находиться на стрілці, і останій вагон, котрий відправився в waiting station, удаляються.
    
    4. Зупинка. Завершення перетворення вираза в RPN.

    5. Зупинка. Виникла помилка.

    6. Вагон виштовхується з waiting station забираючи і попередній вагон якщо не пусто і попередній не має бути | і (. 

    7. Вагон на стрілці одразу відправляти в RPN

    # == -1*

*/















namespace Calculator2.Tests
{
    public class UnitTest
    {
        private Calculator _calculator = new Calculator();
        private string _errorMessage = "";

        /// <summary>
        /// Пet the answer from the math expression
        /// </summary>
        private string Ans(string expression)
        {

            return _calculator.Calculate(expression, ref _errorMessage);
        }

        [Fact]
        public void Test1()
        {
            // Arrange
            Calculator calculator = new Calculator();

            // Act 
            string answer = calculator.Calculate("-(6-9)", ref _errorMessage);

            // Assert
            Assert.Equal("3", answer);
        }

        [Fact]
        public void Test0()
        {
            Assert.Equal("Не визначено \"dfdf\"", Ans("dfdf"));
        }

        [Fact]
        public void Test2()
        {
            Assert.Equal("error", Ans("--(6-9)"));
        }

        [Fact]
        public void Test3()
        {
            Assert.Equal("error", Ans("--9"));
        }

        [Fact]
        public void Test4()
        {
            Assert.Equal("33", Ans("-(2*3/6+3)--(-(5-6-(6*6)))"));
        }

        [Fact]
        public void Test5()
        {
            Assert.Equal("-1188", Ans("(-(2*3/6+3)--(-(5-6-(6*6))))*-(-(-(-(-(9*(8/2))))))"));
        }

        [Fact]
        public void Test6()
        {
            Assert.Equal("5483", Ans("(-(2*3/6+3)--(-(5-6-(6*6))))*-(-(-(-(-(9*(8/2))))))--(8-(-(0*8)))/9*-(9)+6777-98"));
        }

        [Fact]
        public void Test7()
        {
            Assert.Equal("-1017", Ans("(-(2*3/6+3)--(-(5-6-(6*6))))*-(-(-(-(-(9*(8/2))))))--(9+9)+-(0-9)*-(9+8)/-(9-8)"));
        }

        [Fact]
        public void Test8()
        {
            Assert.Equal("135", Ans("-(9+9)+-(0-9)*-(9+8)/-(9-8)"));
        }

        [Fact]
        public void Test9()
        {
            string expression = "135,52941";

            Assert.Equal(expression, Ans("-(9+9)+-(0-9)*-(9+8)/-(9-8)--(9/-((9+8)/-(9-8)))")[..expression.Length]);
        }

        [Fact]
        public void Test10()
        {
            Assert.Equal("-81", Ans("-(-(-(-(-(-9)))))*-9"));
        }

        [Fact]
        public void Test11()
        {
            string expression = "-0,008395061728";

            Assert.Equal(expression, Ans("6.8/-9/90")[..expression.Length]);
        }

        [Fact]
        public void Test12()
        {
            string expression = "-57,00839506";

            Assert.Equal(expression, Ans("(6.8 / -9 / 90) - (9 - 8) - (8 * 7)")[..expression.Length]);
        }

        [Fact]
        public void Test13()
        {
            string expression = "-30,0721649";

            Assert.Equal(expression, Ans("15/(7-(1+1))*3-(2+(1+1))*15/(7-(200+1))*3-(2+(1+1))*(15/(7-(1+1))*3-(2+(1+1))+15/(7-(1+1))*3-(2+(1+1)))")[..expression.Length]);
        }

        [Fact]
        public void Test14()
        {
            Assert.Equal("81", Ans("-90.0/-9+-9*-9+(-90/9)"));
        }

        [Fact]
        public void Test15()
        {
            string expression = "321,63725";

            Assert.Equal(expression, Ans("-(9+9)+-(0-9)*-(9+8)/-(9-8)--(9-8)/-(6+0)*-(9+8)/-(9-8)-(9+9)--(0-9)/-(9+8)-(9+9)+-(0-9)/-(9+8)-(9+9)--(0-9)*-(9+8)-(9+9)+-(0-9)+-(9+8)-(9+9)+-(0-9)/-(9+8)-(9+9)--(0-9)*-(9+8)")[..expression.Length]);
        }

        [Fact]
        public void Test16()
        {
            Assert.Equal("40312", Ans("(2+3!)!-(6+2!)"));
        }
    }
}
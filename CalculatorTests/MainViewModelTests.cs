using Calculator.WpfApp.Net.ViewModels;
using NUnit.Framework;

namespace CalculatorTests
{
    [TestFixture]
    public class MainViewModelTests
    {
        private const string DefaultOperator = "+";
        private const string PseudoRandomNumber = "5";

        [TestCase("0", "0,")]
        [TestCase("123", "123,")]
        [TestCase("123+", "123+0,")]
        [TestCase("234+12", "234+12,")]
        public void AddDot_AfterRecreateInitialScreenValue_ScreenValueShouldEqualToExpectedValue(string initialScreenValue, string expectedValue)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);
            viewModel.AddDot(",");

            // Assert
            Assert.That(viewModel.ScreenValue, Is.EqualTo(expectedValue));
        }

        [TestCase("0", $"0{DefaultOperator}")]
        [TestCase("123,", $"123{DefaultOperator}")] // if "," is last elemnt it will be overrided
        public void AddOperation_AfterRecreateInitialScreenValue_ScreenValueShouldEqualToExpectedValue(string initialScreenValue, string expectedValue)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);
            viewModel.AddOperation(DefaultOperator);

            // Assert
            Assert.That(viewModel.ScreenValue, Is.EqualTo(expectedValue));
        }

        [TestCase("0", $"{PseudoRandomNumber}")]
        [TestCase("0000", $"{PseudoRandomNumber}")]
        [TestCase("1,", $"1,{PseudoRandomNumber}")]
        [TestCase("1+", $"1+{PseudoRandomNumber}")]
        [TestCase("15/0=C", $"{PseudoRandomNumber}")] // the Calculator return symbol of infinity
        [TestCase("0/0=C", $"{PseudoRandomNumber}")] // the Calculator return "NaN"
        public void AddNumber_AfterRecreateInitialScreenValue_ScreenValueShouldEqualToExpectedValue(string initialScreenValue, string expectedValue)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);
            viewModel.AddNumber(PseudoRandomNumber);

            // Assert
            Assert.That(viewModel.ScreenValue, Is.EqualTo(expectedValue));
        }

        [TestCase("1")]
        [TestCase("1,")]
        [TestCase("1+")]
        [TestCase("15/0=")] // the Calculator return symbol of infinity
        [TestCase("0/0=")] // the Calculator return "NaN"
        public void ClearScreen_AfterRecreateInitialScreenValue_ScreenValueShouldDisplayZero(string initialScreenValue)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);
            viewModel.ClearScreen();

            // Assert
            Assert.That(viewModel.ScreenValue, Is.EqualTo("0"));
        }

        [TestCase("0", true)]
        [TestCase("0,", false)]
        [TestCase("0,12", false)]
        [TestCase("123+", true)]
        [TestCase("234+0", true)]
        [TestCase("234+0,", false)]
        [TestCase("234+1,23", false)]
        [TestCase("234+123=", false)]
        [TestCase("15/0=", false)] // the Calculator return symbol of infinity
        [TestCase("0/0=", false)] // the Calculator return "NaN"
        [TestCase("0/0=C", true)] // C is clearing command, after that all of botton should be avaliable
        public void CanAddDot_AfterRecreateInitialScreenValue_ShouldReturnExpectedResult(string initialScreenValue, bool expectedResult)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);

            // Assert
            Assert.That(viewModel.CanAddDot(), Is.EqualTo(expectedResult));
        }

        [TestCase("0", true)]
        [TestCase("0,", true)]
        [TestCase("123+", true)]
        [TestCase("123+23=", false)]
        [TestCase("15/0=", false)] // the Calculator return symbol of infinity
        [TestCase("0/0=", false)] // the Calculator return "NaN"
        [TestCase("0/0=C", true)] // C is clearing command, after that all of botton should be avaliable
        public void CanAddNumber_AfterRecreateInitialScreenValue_ShouldReturnExpectedResult(string initialScreenValue, bool expectedResult)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);

            // Assert
            Assert.That(viewModel.CanAddNumber(), Is.EqualTo(expectedResult));
        }

        [TestCase("0", true)]
        [TestCase("0,", true)] // if "," is last elemnt it will be overrided
        [TestCase("123+", false)]
        [TestCase("123+23=", true)]
        [TestCase("15/0=", false)] // the Calculator return symbol of infinity
        [TestCase("0/0=", false)] // the Calculator return "NaN"
        [TestCase("0/0=C", true)] // C is clearing command, after that all of botton should be avaliable
        public void CanAddOperation_AfterRecreateInitialScreenValue_ShouldReturnExpectedResult(string initialScreenValue, bool expectedResult)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);

            // Assert
            Assert.That(viewModel.CanAddOperation(), Is.EqualTo(expectedResult));
        }

        [TestCase("0")]
        [TestCase("0,")]
        [TestCase("123+")]
        [TestCase("15/0=")] // the Calculator return symbol of infinity
        [TestCase("0/0=")] // the Calculator return "NaN"
        [TestCase("0/0=C")] // C is clearing command, after that all of botton should be avaliable
        public void ClearScrenCommandCanExecute_AfterRecreateInitialScreenValue_ShouldAlwaysReturnTrue(string initialScreenValue)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);

            // Assert
            Assert.That(viewModel.ClearScrenCommand.CanExecute("C"), Is.EqualTo(true));
        }

        [TestCase("0", true)]
        [TestCase("0,", true)]
        [TestCase("123+", false)]
        [TestCase("123+23=", true)]
        [TestCase("15/0=", false)] // the Calculator return symbol of infinity
        [TestCase("0/0=", false)] // the Calculator return "NaN"
        [TestCase("0/0=C", true)] // C is clearing command, after that all of botton should be avaliable
        public void CanGetResult_AfterRecreateInitialScreenValue_ShouldReturnExpectedResult(string initialScreenValue, bool expectedResult)
        {
            // Arrange
            var viewModel = new MainViewModel();

            // Act
            RecreateInitialValueStepByStep(viewModel, initialScreenValue);

            // Assert
            Assert.That(viewModel.CanGetResult(), Is.EqualTo(expectedResult));
        }

        private void RecreateInitialValueStepByStep(MainViewModel viewModel, string initialValue)
        {
            foreach (var item in initialValue.Select(c => c.ToString()))
            {
                switch (item)
                {
                    case "+":
                    case "-":
                    case "*":
                    case "/":
                        viewModel.AddOperation(item);
                        break;

                    case ",":
                        viewModel.AddDot(item);
                        break;

                    case "=":
                        viewModel.GetResult();
                        break;

                    case "C":
                        viewModel.ClearScreen();
                        break;

                    default:
                        viewModel.AddNumber(item);
                        break;
                }
            }
        }
    }
}
using Calculator.WpfApp.Net.Commands;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;

namespace Calculator.WpfApp.Net.ViewModels
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private string? _screenValue;
        private List<string> _avaliableOperations = new() { "+", "-", "*", "/" };
        private DataTable _dataTable = new();
        private bool _isLastSignAnOperation;
        private bool _dotAlreadyExistsInCurrentNumber;
        private bool _isInvalidResult;
        private bool _CurrentScreenValueIsResult;

        public MainViewModel()
        {
            ScreenValue = "0";
            AddNumberCommand = new Command(AddNumber, CanAddNumber);
            AddDotCommand = new Command(AddDot, CanAddDot);
            AddOperationCommand = new Command(AddOperation, CanAddOperation);
            ClearScrenCommand = new Command(ClearScreen);
            GetResultCommand = new Command(GetResult, CanGetResult);
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        public ICommand GetResultCommand { get; set; }
        public ICommand AddNumberCommand { get; set; }
        public ICommand AddDotCommand { get; set; }
        public ICommand AddOperationCommand { get; set; }
        public ICommand ClearScrenCommand { get; set; }

        public string ScreenValue
        {
            get { return _screenValue; }
            set
            {
                _screenValue = value;
                OnPropertyChanged();
            }
        }

        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        internal bool CanAddOperation(object obj = null) => !_isLastSignAnOperation && !_isInvalidResult;
        internal bool CanGetResult(object obj = null) => !_isLastSignAnOperation && !_isInvalidResult;
        internal bool CanAddDot(object obj = null) => !_dotAlreadyExistsInCurrentNumber && !_isInvalidResult && !_CurrentScreenValueIsResult;
        internal bool CanAddNumber(object obj = null) => !_isInvalidResult && !_CurrentScreenValueIsResult;

        internal void GetResult(object obj = null)
        {
            var result = _dataTable.Compute(ScreenValue.Replace(",", "."), "");
            ScreenValue = result.ToString();
            _isLastSignAnOperation = false;

            _isInvalidResult = ScreenValue == null || !char.IsDigit(ScreenValue.Last());
            _dotAlreadyExistsInCurrentNumber = ScreenValue!.Contains(',');
            _CurrentScreenValueIsResult = true;
        }

        internal void ClearScreen(object obj = null)
        {
            ScreenValue = "0";
            _isLastSignAnOperation = false;
            _dotAlreadyExistsInCurrentNumber = false;
            _isInvalidResult = false;
            _CurrentScreenValueIsResult = false;
        }

        internal void AddOperation(object obj)
        {
            var operation = obj as string;

            if (ScreenValue.Substring(ScreenValue.Length - 1) == ",")
                ScreenValue = ScreenValue.Remove(ScreenValue.Length - 1);

            ScreenValue += operation;
            _isLastSignAnOperation = true;
            _dotAlreadyExistsInCurrentNumber = false;
            _CurrentScreenValueIsResult = false;
        }

        internal void AddNumber(object obj)
        {
            var number = obj as string;

            if (ScreenValue == "0")
                ScreenValue = string.Empty;

            ScreenValue += number;

            _isLastSignAnOperation = false;
        }

        internal void AddDot(object obj)
        {
            var dot = obj as string;

            if (_avaliableOperations.Contains(ScreenValue.Substring(ScreenValue.Length - 1)))
                dot = "0,";

            ScreenValue += dot;

            _isLastSignAnOperation = false;
            _dotAlreadyExistsInCurrentNumber = true;
        }
    }
}

using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using PasswordGenerator.Generation;


namespace PasswordGenerator
{
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private static readonly object InstanceSyncRoot = new object();
        private static volatile MainWindow _instance;
        public static MainWindow Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (InstanceSyncRoot)
                    {
                        if (_instance == null)
                            _instance = new MainWindow();
                    }
                }

                return _instance;
            }
        }



        public event PropertyChangedEventHandler PropertyChanged;



        private readonly StringGenerator _stringGenerator;



        private int _minLength;
        public int MinLength
        {
            get
            {
                return _minLength;
            }
            set
            {
                if (value > MaxLength)
                    value = MaxLength;

                _minLength = value;
                OnPropertyChanged(nameof(MinLength));

                UpdateGenerationAccess();
            }
        }
        private int _maxLength;
        public int MaxLength
        {
            get
            {
                return _maxLength;
            }
            set
            {
                if (value < MinLength)
                    value = MinLength;

                _maxLength = value;
                OnPropertyChanged(nameof(MaxLength));

                UpdateGenerationAccess();
            }
        }
        private int _length;
        public int Length
        {
            get
            {
                return _length;
            }
            set
            {
                if (value < MinLength)
                    value = MinLength;
                if (value > MaxLength)
                    value = MaxLength;

                _length = value;
                OnPropertyChanged(nameof(Length));

                UpdateGenerationAccess();
            }
        }
        private bool _includeLowerCaseCharacters;
        public bool IncludeLowerCaseCharacters
        {
            get
            {
                return _includeLowerCaseCharacters;
            }
            set
            {
                _includeLowerCaseCharacters = value;
                OnPropertyChanged(nameof(IncludeLowerCaseCharacters));

                UpdateGenerationAccess();
            }
        }
        private bool _includeUpperCaseCharacters;
        public bool IncludeUpperCaseCharacters
        {
            get
            {
                return _includeUpperCaseCharacters;
            }
            set
            {
                _includeUpperCaseCharacters = value;
                OnPropertyChanged(nameof(IncludeUpperCaseCharacters));

                UpdateGenerationAccess();
            }
        }
        private bool _includeDigits;
        public bool IncludeDigits
        {
            get
            {
                return _includeDigits;
            }
            set
            {
                _includeDigits = value;
                OnPropertyChanged(nameof(IncludeDigits));

                UpdateGenerationAccess();
            }
        }
        private bool _includeSpecialCharacters;
        public bool IncludeSpecialCharacters
        {
            get
            {
                return _includeSpecialCharacters;
            }
            set
            {
                _includeSpecialCharacters = value;
                OnPropertyChanged(nameof(IncludeSpecialCharacters));

                UpdateGenerationAccess();
            }
        }
        private bool _generationEnabled;
        public bool GenerationEnabled
        {
            get
            {
                return _generationEnabled;
            }
            set
            {
                _generationEnabled = value;
                OnPropertyChanged(nameof(GenerationEnabled));
            }
        }



        private MainWindow()
        {
            InitializeComponent();
            DataContext = this;

            _minLength = int.MinValue;
            _maxLength = int.MaxValue;
            _length = 0;

            MinLength = 4;
            MaxLength = 30;
            Length = MinLength;
            IncludeLowerCaseCharacters = true;
            IncludeUpperCaseCharacters = false;
            IncludeDigits = false;
            IncludeSpecialCharacters = false;

            _stringGenerator = new StringGenerator(
                MaxLength);
        }



        private void UpdateGenerationAccess()
        {
            if (Length < 1
                || Length < MinLength
                || Length > MaxLength)
            {
                GenerationEnabled = false;

                return;
            }

            if (!(IncludeLowerCaseCharacters
                 || IncludeUpperCaseCharacters
                 || IncludeDigits
                 || IncludeSpecialCharacters))
            {
                GenerationEnabled = false;

                return;
            }

            GenerationEnabled = true;
        }



        protected virtual void OnPropertyChanged(
            [CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this,
                new PropertyChangedEventArgs(propertyName));
        }


        private void GeneratePasswordButton_Click(object sender, RoutedEventArgs e)
        {
            IsEnabled = false;

            try
            {
                var alphabets = PasswordGenerationAlphabet.None;

                if (IncludeLowerCaseCharacters)
                    alphabets |= PasswordGenerationAlphabet.LowerCaseCharacters;
                if (IncludeUpperCaseCharacters)
                    alphabets |= PasswordGenerationAlphabet.UpperCaseCharacters;
                if (IncludeDigits)
                    alphabets |= PasswordGenerationAlphabet.Digits;
                if (IncludeSpecialCharacters)
                    alphabets |= PasswordGenerationAlphabet.SpecialCharacters;

                PasswordTextBox.Text = _stringGenerator.
                    GeneratePassword(Length, alphabets);
            }
            finally
            {
                IsEnabled = true;
            }
        }
    }
}

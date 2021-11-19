using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Input;

namespace PasswordGenerator.Controls
{
    public partial class IntegerUpDown : UserControl
    {
        public event EventHandler<DependencyPropertyChangedEventArgs> ValueChanged;



        public static readonly DependencyProperty MinimumProperty = DependencyProperty.Register(
            "Minimum", typeof(int), typeof(IntegerUpDown),
            new UIPropertyMetadata(int.MinValue, MinimumPropertyChangedCallback));
        public static readonly DependencyProperty MaximumProperty = DependencyProperty.Register(
            "Maximum", typeof(int), typeof(IntegerUpDown),
            new UIPropertyMetadata(int.MaxValue, MaximumPropertyChangedCallback));
        public static readonly DependencyProperty StepProperty = DependencyProperty.Register(
            "Step", typeof(int), typeof(IntegerUpDown),
            new UIPropertyMetadata(1));
        public static readonly DependencyProperty ValueProperty = DependencyProperty.Register(
            "Value", typeof(int), typeof(IntegerUpDown),
            new UIPropertyMetadata(0, ValuePropertyChangedCallback));



        private TextBox _contentPresenter;
        private RepeatButton _upButton;
        private RepeatButton _downButton;

        private BindingExpression _contentPresenterTextBinding;


        public int Minimum
        {
            get
            {
                return (int)GetValue(MinimumProperty);
            }
            set
            {
                SetValue(MinimumProperty, value);
            }
        }
        public int Maximum
        {
            get
            {
                return (int)GetValue(MaximumProperty);
            }
            set
            {
                SetValue(MaximumProperty, value);
            }
        }
        public int Step
        {
            get
            {
                return (int)GetValue(StepProperty);
            }
            set
            {
                SetValue(StepProperty, value);
            }
        }
        public int Value
        {
            get
            {
                return (int)GetValue(ValueProperty);
            }
            set
            {
                SetCurrentValue(ValueProperty, value);
            }
        }



        public IntegerUpDown()
        {
            InitializeComponent();
        }



        private static void MinimumPropertyChangedCallback(
            DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var target = source as IntegerUpDown;

            target?.OnMinimumChanged(e);
        }

        private static void MaximumPropertyChangedCallback(
            DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var target = source as IntegerUpDown;

            target?.OnMaximumChanged(e);
        }

        private static void ValuePropertyChangedCallback(
            DependencyObject source, DependencyPropertyChangedEventArgs e)
        {
            var target = source as IntegerUpDown;

            target?.OnValueChanged(e);
        }



        private void OnMinimumChanged(
            DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue > Maximum)
                Minimum = Maximum;

            ValueChanged?.Invoke(this,
                new DependencyPropertyChangedEventArgs(ValueProperty, e.OldValue, Minimum));

            //ValueProperty.OverrideMetadata(typeof(IntegerUpDown),
            //    new UIPropertyMetadata(Minimum, ValuePropertyChangedCallback));

            if (Value < Minimum)
                Value = Minimum;
        }

        private void OnMaximumChanged(
            DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < Minimum)
                Maximum = Minimum;

            ValueChanged?.Invoke(this,
                new DependencyPropertyChangedEventArgs(ValueProperty, e.OldValue, Maximum));

            if (Value > Maximum)
                Value = Maximum;
        }

        private void OnValueChanged(
            DependencyPropertyChangedEventArgs e)
        {
            if ((int)e.NewValue < Minimum)
                Value = Minimum;
            if ((int)e.NewValue > Maximum)
                Value = Maximum;

            ValueChanged?.Invoke(this,
                new DependencyPropertyChangedEventArgs(ValueProperty, e.OldValue, Value));
        }



        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _contentPresenterTextBinding = null;

            if (_contentPresenter != null)
            {
                _contentPresenter.PreviewTextInput -= ContentPresenter_PreviewTextInput;
                _contentPresenter.IsKeyboardFocusedChanged -= ContentPresenter_IsKeyboardFocusedChanged;
                _contentPresenter.KeyUp -= ContentPresenter_KeyUp;

                DataObject.RemovePastingHandler(_contentPresenter, ContentPresenter_PastingHandler);

            }
            if (_upButton != null)
            {
                _upButton.Click -= UpButton_Click;
            }
            if (_downButton != null)
            {
                _downButton.Click -= DownButton_Click;
            }

            _contentPresenter = Template.FindName("PART_ContentPresenter", this) as TextBox;
            _upButton = Template.FindName("PART_UpButton", this) as RepeatButton;
            _downButton = Template.FindName("PART_DownButton", this) as RepeatButton;

            if (_contentPresenter != null)
            {
                _contentPresenterTextBinding = _contentPresenter
                    .GetBindingExpression(TextBox.TextProperty);

                _contentPresenter.PreviewTextInput += ContentPresenter_PreviewTextInput;
                _contentPresenter.IsKeyboardFocusedChanged += ContentPresenter_IsKeyboardFocusedChanged;
                _contentPresenter.KeyUp += ContentPresenter_KeyUp;

                DataObject.AddPastingHandler(_contentPresenter, ContentPresenter_PastingHandler);

            }
            if (_upButton != null)
            {
                _upButton.Click += UpButton_Click;
            }
            if (_downButton != null)
            {
                _downButton.Click += DownButton_Click;
            }
        }



        private void ContentPresenter_PastingHandler(object sender, DataObjectPastingEventArgs e)
        {
            if (!e.SourceDataObject.GetDataPresent(
                DataFormats.Text, true))
            {
                e.CancelCommand();
                e.Handled = true;

                return;
            }

            var text = e.SourceDataObject.GetData(
                DataFormats.Text, true) as string;

            if (string.IsNullOrEmpty(text))
            {
                e.CancelCommand();
                e.Handled = true;

                return;
            }

            var presenter = sender as TextBox;

            if (presenter == null)
            {
                e.CancelCommand();
                e.Handled = true;

                return;
            }

            var startIndex = 0;

            if (text[0] == '-')
            {
                if (presenter.CaretIndex != 0
                    || (presenter.Text.Length != 0 && presenter.Text[0] == '-'))
                {
                    e.CancelCommand();
                    e.Handled = true;

                    return;
                }

                startIndex = 1;
            }
            for (var i = startIndex; i < text.Length; ++i)
            {
                if (char.IsDigit(text[i]))
                    continue;

                e.CancelCommand();
                e.Handled = true;

                return;
            }
        }


        private void ContentPresenter_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            var presenter = sender as TextBox;

            if (presenter == null)
                return;

            if (string.IsNullOrEmpty(e.Text))
            {
                e.Handled = true;

                return;
            }

            if (e.Text[0] == '-')
            {
                if (presenter.CaretIndex != 0
                    || (presenter.Text.Length != 0 && presenter.Text[0] == '-'))
                {
                    e.Handled = true;
                }

                return;
            }
            if (char.IsDigit(e.Text[0]))
            {
                return;
            }

            e.Handled = true;
        }

        private void ContentPresenter_IsKeyboardFocusedChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (_contentPresenter.Text.Length == 0
                || (_contentPresenter.Text.Length == 1 && _contentPresenter.Text[0] == '-'))
            {
                _contentPresenter.Text = "0";
            }

            _contentPresenterTextBinding?
                .UpdateSource();
        }

        private void ContentPresenter_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Keyboard.ClearFocus();

                e.Handled = true;
            }
            if ((Keyboard.Modifiers & ModifierKeys.Control) == ModifierKeys.Control
                && e.Key == Key.V)
            {
                e.Handled = true;
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            Value += Step;
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            Value -= Step;
        }
    }
}

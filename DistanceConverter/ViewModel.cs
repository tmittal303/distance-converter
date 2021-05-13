using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DistanceConverter
{
    public class ViewModel : INotifyPropertyChanged
    {
        const double MIN_VALUE_CHANGE_LIMIT = 0.05;
        const double MAX_VALUE_CHANGE_LIMIT = 0.95;

        const double MAX_VALUE_CHANGE_FACTOR = 1.50;
        const double MAX_VALUE_CHANGE_FACTOR_ = 4;
        const double MIN_VALUE_CHANGE_FACTOR = 0.50;

        const int INPUT_ROUND_DIGITS = 4;

        const int FEET_YARD = 3;
        const int INCH_FEET = 12;
        const int INCH_YARD = 36;

        const int INCH = 0;
        const int FEET = 1;
        const int YARD = 2;

        const int SINGULAR_LIMIT = 1;

        const int TICK_FRE_FACTOR = 1;
        const int TICK_FRE_POWER = 10;

        private int fromDistanceUnit = INCH;
        public int FromDistanceUnit
        {
            get { return fromDistanceUnit; }
            set
            {
                fromDistanceUnit = value; Calc();
                changed();
            }
        }

        private int toDistanceUnit = FEET;
        public int ToDistanceUnit
        {
            get { return toDistanceUnit; }
            set
            {
                toDistanceUnit = value; Calc();
                changed();
            }
        }

        private double convertToValue = 4.1667; // Default Output
        public double ConvertToValue
        {
            get { return convertToValue; }
            set { convertToValue = value; changed(); }
        }

        private double tickFrequency = 1;
        public double TickFrequency
        {
            get { return tickFrequency; }
            set { tickFrequency = value; changed(); }
        }

        private double toolTipPrec = 0;
        public double ToolTipPrec
        {
            get { return toolTipPrec; }
            set { toolTipPrec = value; changed(); }
        }

        private double minimum = 1;
        public double Minimum
        {
            get { return minimum; }
            set { minimum = value; changed(); }
        }

        private double maximum = 100;
        public double Maximum
        {
            get { return maximum; }
            set { maximum = value; changed(); }
        }

        private bool inputValueFlag = true, textboxSliderFlag = false;
        private double inputValue = 50; // Default Input
        public double InputValue
        {
            get { return inputValue; }
            set
            {
                inputValue = value;
                Calc();
                if (textboxSliderFlag)
                {
                    inputValueFlag = false;
                    TexboxValue = value;
                }
                else
                {
                    inputValueFlag = false;
                    SliderValue = value;
                }
            }
        }

        public double TexboxValue
        {
            get
            {
                return InputValue;
            }
            set
            {
                if (Double.TryParse(value.ToString(), out inputValue))
                {
                    if (Maximum < value)
                    {
                        Maximum = value * MAX_VALUE_CHANGE_FACTOR;
                    }
                    else if (Minimum > value)
                    {
                        Minimum = value * MIN_VALUE_CHANGE_FACTOR;
                    }
                    if (inputValueFlag)
                    {
                        textboxSliderFlag = false;
                        // To find number of decimal places in value
                        TickFrequency = TICK_FRE_FACTOR / Math.Pow(TICK_FRE_POWER, BitConverter.GetBytes(decimal.GetBits(Convert.ToDecimal(value))[3])[2]);
                        ToolTipPrec = BitConverter.GetBytes(decimal.GetBits(Convert.ToDecimal(TickFrequency))[3])[2];
                        InputValue = Math.Round(value, INPUT_ROUND_DIGITS);
                    }
                    inputValueFlag = true;
                    if (Maximum > value * MAX_VALUE_CHANGE_FACTOR_)
                    {
                        Maximum = value * MAX_VALUE_CHANGE_FACTOR;
                    }
                    changed();
                }
            }
        }

        public double SliderValue
        {
            get { return InputValue; }
            set
            {
                if (Double.TryParse(value.ToString(), out inputValue))
                {
                    if (Maximum < value)
                    {
                        Maximum = value * MAX_VALUE_CHANGE_FACTOR;
                    }
                    else if (Minimum > value)
                    {
                        Minimum = value * MIN_VALUE_CHANGE_FACTOR;
                    }
                    if (inputValueFlag)
                    {
                        textboxSliderFlag = true;
                        InputValue = Math.Round(value, INPUT_ROUND_DIGITS);
                    }
                    if (Maximum * MAX_VALUE_CHANGE_LIMIT < value)
                    {
                        Maximum = value * MAX_VALUE_CHANGE_FACTOR;
                    }
                    else if (Maximum * MIN_VALUE_CHANGE_LIMIT > value)
                    {
                        Minimum = value * MIN_VALUE_CHANGE_FACTOR;
                    }
                    if (Maximum > value * MAX_VALUE_CHANGE_FACTOR_)
                    {
                        Maximum = value * MAX_VALUE_CHANGE_FACTOR;
                    }
                    changed();
                }
            }
        }

        private string distanceFrom = "Inch";
        public string DistanceFrom
        {
            get { return distanceFrom; }
            set { distanceFrom = value; changed(); }
        }

        private string distanceTo = "Feet";
        public string DistanceTo
        {
            get { return distanceTo; }
            set { distanceTo = value; changed(); }
        }

        public void Calc()
        {
            if (FromDistanceUnit == ToDistanceUnit)
            {
                ConvertToValue = InputValue;
            }
            if (FromDistanceUnit == INCH) //from distance unit is 'Inch'
            {
                if (inputValue <= SINGULAR_LIMIT)
                    DistanceFrom = "Inch";
                else
                    DistanceFrom = "Inches";
                switch (ToDistanceUnit)
                {
                    case FEET: // to distance is 'Feet'
                        ConvertToValue = Math.Round(InputValue / INCH_FEET, INPUT_ROUND_DIGITS);
                        break;
                    case YARD: // to distance is 'Yard'
                        ConvertToValue = Math.Round(InputValue / INCH_YARD, INPUT_ROUND_DIGITS);
                        break;
                }
            }
            else if (FromDistanceUnit == FEET) //from distance unit is 'Feet'
            {
                if (inputValue <= SINGULAR_LIMIT)
                    DistanceFrom = "Feet";
                else
                    DistanceFrom = "Feets";
                switch (ToDistanceUnit)
                {
                    case INCH: // to distance is 'Inch'
                        ConvertToValue = Math.Round(InputValue * INCH_FEET, INPUT_ROUND_DIGITS);
                        break;
                    case YARD: // to distance is 'Yard'
                        ConvertToValue = Math.Round(InputValue / FEET_YARD, INPUT_ROUND_DIGITS);
                        break;
                }
            }
            else if (FromDistanceUnit == YARD) //from distance unit is 'Yard'
            {
                if (inputValue <= SINGULAR_LIMIT)
                    DistanceFrom = "Yard";
                else
                    DistanceFrom = "Yards";
                switch (ToDistanceUnit)
                {
                    case INCH: // to distance is 'Inch'
                        ConvertToValue = Math.Round(InputValue * INCH_YARD, INPUT_ROUND_DIGITS);
                        break;
                    case FEET: // to distance is 'Feet'
                        ConvertToValue = Math.Round(InputValue * FEET_YARD, INPUT_ROUND_DIGITS);
                        break;
                }
            }
            switch (ToDistanceUnit)
            {
                case INCH: // to distance is 'Inch'
                    if (inputValue <= SINGULAR_LIMIT)
                        DistanceTo = "Inch";
                    else
                        DistanceTo = "Inches";
                    break;
                case FEET: // to distance is 'Feet'
                    if (inputValue <= SINGULAR_LIMIT)
                        DistanceTo = "Feet";
                    else
                        DistanceTo = "Feets";
                    break;
                case YARD: // to distance is 'Yard'
                    if (inputValue <= SINGULAR_LIMIT)
                        DistanceTo = "Yard";
                    else
                        DistanceTo = "Yards";
                    break;
            }

        }

        #region PropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        private void changed([CallerMemberName] string property = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
        #endregion
    }
}

using System.ComponentModel.DataAnnotations;

namespace SweetsAndSnacks.Utilities
{
    public class MinNumericValueAttribute:ValidationAttribute
    {
        public int MinValue { get; set; }

        public override bool IsValid(object? value)
        {
            if (value == null)
            {
                return false;
            }

            int val = (int)value;

            if(val < MinValue)
            {
                return false;
            }
            else
            {
                return true;
            }

        }
    }
}

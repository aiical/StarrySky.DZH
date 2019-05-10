using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StarrySky.DZH.Util.CustomAttribute
{
    [AttributeUsage(AttributeTargets.All)]
    public class RangeAttribute : Attribute
    {
        private int _minLength;

        private int _maxLength;

        public RangeAttribute(int minLength, int maxLength)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }
    }

}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LineStatusClient.Common
{
    public static class EnumExtensions
    {
        public static string GetDescription(this Enum value)
        {
            var field = value.GetType().GetField(value.ToString());

            return field?
                .GetCustomAttribute<DescriptionAttribute>()?
                .Description
                ?? value.ToString();
        }
    }
}

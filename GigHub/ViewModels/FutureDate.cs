using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GigHub.ViewModels
{
    public class FutureDate: ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            bool isValid = DateTime.TryParseExact(Convert.ToString(value),
                "dd MMM yy",
                CultureInfo.CurrentCulture,
                DateTimeStyles.None,
                out DateTime datetime
                );

            return (isValid && datetime > DateTime.Now);
        }
    }
}
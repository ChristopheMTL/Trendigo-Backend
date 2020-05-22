using FluentValidation;
using FluentValidation.Results;
using IMS.Common.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Common.Core.Validator
{
    public class AddressValidator : AbstractValidator<Address>
    {
        public override ValidationResult Validate(Address instance) 
        {
            //TODO - Implement regex for fields
            //RuleFor(address => address.Zip).Matches("");
            return instance == null ? 
                new ValidationResult(new[] { new ValidationFailure("Address", "Address cannot be null") })
                : base.Validate(instance);
        }
    }
}

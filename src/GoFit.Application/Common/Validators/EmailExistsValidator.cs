using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFit.Application.Common.Validators;
public class EmailExistsValidator : AbstractValidator<string>
{
    public EmailExistsValidator()
    {
        
    }
}

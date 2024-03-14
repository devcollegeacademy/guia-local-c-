using FluentValidation;
using FluentValidation.Results;
using guialocal.Models;

namespace guialocal.Middlewares
{
    public class CustomerCreateValidator : AbstractValidator<Customer>
    {
        public CustomerCreateValidator()
        {            
            RuleFor(x => x.Title).NotEmpty().WithMessage("O campo 'Title' deve ser informado.");
            RuleFor(x => x.ZipCode).NotEmpty().WithMessage("O campo 'ZipCode' deve ser informado.");
            RuleFor(x => x.Number).NotEmpty().WithMessage("O campo 'Number' deve ser informado.");
            RuleFor(x => x.Address).NotEmpty().WithMessage("O campo 'Address' deve ser informado.");
            RuleFor(x => x.Email).Equal(x => x.Email).WithMessage("O 'E-mail' não pode ser alterado.");
            RuleFor(x => x.Active).NotNull().WithMessage("O campo 'Active' deve ser informado.");
        }
    }

    public class CustomerReadByFilterValidator : AbstractValidator<string?>
    {
        public ValidationResult ValidateOrNull(string? title)
        {
            if (title == null)
                return new ValidationResult();

            return Validate(title);
        }
    }

    public class CustomerReadOneValidator : AbstractValidator<string>
    {
        public CustomerReadOneValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("'Email' deve ser informado.");
        }
    }

    public class CustomerUpdateValidator : AbstractValidator<(string Email, Customer Customer)>
    {
        public CustomerUpdateValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("'E-mail' deve ser informado.");
            RuleFor(x => x.Customer.Title).NotEmpty().WithMessage("O campo 'Title' deve ser informado.");
            RuleFor(x => x.Customer.ZipCode).NotEmpty().WithMessage("O campo 'ZipCode' deve ser informado.");
            RuleFor(x => x.Customer.Number).NotEmpty().WithMessage("O campo 'Number' deve ser informado.");
            RuleFor(x => x.Customer.Address).NotEmpty().WithMessage("O campo 'Address' deve ser informado.");
            RuleFor(x => x.Customer.Email).Equal(x => x.Email).WithMessage("O 'E-mail' não pode ser alterado.");
            RuleFor(x => x.Customer.Active).NotNull().WithMessage("O campo 'Active' deve ser informado.");
        }
    }


    public class CustomerDeleteValidator : AbstractValidator<string>
    {
        public CustomerDeleteValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("'Email' deve ser informado.");
        }
    }

    public class CustomerGetAddressValidator : AbstractValidator<string>
    {
        public CustomerGetAddressValidator()
        {
            RuleFor(x => x).NotEmpty().WithMessage("'CEP' deve ser informado.");
        }
    }
}

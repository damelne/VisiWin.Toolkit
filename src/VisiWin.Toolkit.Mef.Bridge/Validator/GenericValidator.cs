using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.ComponentModel.Composition;
using System.Threading;
using System.Threading.Tasks;

namespace AST.FastCtrl.HMI.MefAdapter.Validator
{
    [Export(typeof(IValidator<>))]
    internal class GenericValidator<T> : IValidator<T> where T : class
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IValidator<T> _innerValidator;

        [ImportingConstructor]
        public GenericValidator(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
            _innerValidator = _serviceProvider.GetRequiredService<IValidator<T>>();
        }

        public bool CanValidateInstancesOfType(Type type)
        {
            return _innerValidator.CanValidateInstancesOfType(type);
        }

        public IValidatorDescriptor CreateDescriptor()
        {
            return _innerValidator.CreateDescriptor();
        }

        public ValidationResult Validate(T instance)
        {
            return _innerValidator.Validate(instance);
        }

        public ValidationResult Validate(IValidationContext context)
        {
            return _innerValidator.Validate(context);
        }

        public Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellation = default)
        {
            return _innerValidator.ValidateAsync(instance, cancellation);
        }

        public Task<ValidationResult> ValidateAsync(IValidationContext context, CancellationToken cancellation = default)
        {
            return _innerValidator.ValidateAsync(context, cancellation);
        }
    }
}
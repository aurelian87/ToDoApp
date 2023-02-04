using FluentValidation;
using ToDoApp.Shared.Models;
using ToDoApp.Shared.ResourceFiles;

namespace ToDoApp.Shared.ModelValidators
{
    public class ToDoModelValidator : AbstractValidator<ToDoModel>
    {
        public ToDoModelValidator()
        {
            RuleFor(x => x.Title).NotEmpty().WithMessage(Resource.er_ToDoModel_TitleRequired);
            RuleFor(x => x.Description).NotEmpty().WithMessage(Resource.er_ToDoModel_DescriptionRequired);
            RuleFor(x => x.DueDate).Must(BeAValidDate).WithMessage(Resource.er_ToDoModel_DueDateRequired);
            RuleFor(x => x.DueDate).GreaterThanOrEqualTo(DateTime.Today).WithMessage(Resource.er_ToDoModel_DueDateInvalid);
        }

        private bool BeAValidDate(DateTime date)
        {
            //if (date == default(DateTime))
            //    return true;
            return date != default(DateTime);
        }
        private bool BeAValidDate2(DateTime date)
        {
            var test = date.ToString();
            return test.All(char.IsDigit);
        }
    }
}
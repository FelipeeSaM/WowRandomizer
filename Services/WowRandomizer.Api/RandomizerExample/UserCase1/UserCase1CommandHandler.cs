namespace WowRandomizer.Api.RandomizerExample.UserCase1
{
    public record UserCase1Command(int Id, string Something) : ICommand<UserCase1Result>;

    public record UserCase1Result(string Result);

    public class UserCase1CommandValidator : AbstractValidator<UserCase1Command>
    {
        public UserCase1CommandValidator()
        {
            // only for examples
            RuleFor(x => x.Id).GreaterThan(0);
            RuleFor(x => x.Something).NotEmpty();
        }
    }

    public class UserCase1CommandHandler
        : ICommandHandler<UserCase1Command, UserCase1Result>
    {
        public async Task<UserCase1Result> Handle(UserCase1Command request, CancellationToken cancellationToken)
        {
            // logics

            return new UserCase1Result("return something");
        }
    }
}

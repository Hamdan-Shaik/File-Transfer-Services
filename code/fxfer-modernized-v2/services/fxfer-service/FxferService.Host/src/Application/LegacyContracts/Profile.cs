using System.Linq.Expressions;

namespace FxferService.Application.LegacyContracts;

public abstract class Profile
{
    protected Profile()
    {
        this.Configure();
    }

    protected abstract void Configure();

    protected MappingExpression<TSource, TDestination> CreateMap<TSource, TDestination>()
        => new();

    protected sealed class MappingExpression<TSource, TDestination>
    {
        public MappingExpression<TSource, TDestination> ForMember<TMember>(
            Expression<Func<TDestination, TMember>> destinationMember,
            Action<MemberOptions<TSource, TDestination, TMember>> memberOptions)
        {
            _ = destinationMember;
            memberOptions(new MemberOptions<TSource, TDestination, TMember>());
            return this;
        }

        public MappingExpression<TSource, TDestination> ForAllMembers(Action<MemberConfigurationExpression> memberOptions)
        {
            memberOptions(new MemberConfigurationExpression());
            return this;
        }

        public MappingExpression<TSource, TDestination> As<TDestinationSubtype>() where TDestinationSubtype : TDestination
            => this;
    }

    protected sealed class MemberOptions<TSource, TDestination, TMember>
    {
        public void MapFrom(Expression<Func<TSource, TMember>> sourceMember)
        {
            _ = sourceMember;
        }
    }

    protected sealed class MemberConfigurationExpression
    {
        public void Condition(Func<ResolutionContext, bool> predicate)
        {
            _ = predicate;
        }
    }

    protected readonly struct ResolutionContext
    {
        public bool IsSourceValueNull { get; init; }
    }
}


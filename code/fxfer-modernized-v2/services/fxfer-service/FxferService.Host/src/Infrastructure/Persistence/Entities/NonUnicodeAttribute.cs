using System;

namespace FxferService.Infrastructure.Persistence.Entities;

[AttributeUsage(AttributeTargets.Property)]
public sealed class NonUnicodeAttribute : Attribute
{
}

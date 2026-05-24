using System.Numerics;
using System.Reflection;

namespace carp.Shared.Utils;

using System;

// Base classes --------------------------------------------------------------------------------------------------------

[AttributeUsage(AttributeTargets.Property)]
internal class ValidationMethod : Attribute
{
    private Type Validator { get; }

    public ValidationMethod(Type validatorType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validatorType))
        {
            throw new ArgumentException($"{validatorType.Name} must implement IValidator");
        }

        Validator = validatorType;
    }

    public string? Check(object? value)
    {
        var validator = (IValidator?)Activator.CreateInstance(Validator);
        if (validator == null)
            return "Nie udało się zawołać metody walidacyjnej";
        return validator.Validate(value);
    }
}

internal interface IValidator
{
    public string? Validate(object? value);
}

// Validator classes ---------------------------------------------------------------------------------------------------

internal sealed class ValidatorOptional : IValidator
{
    public string? Validate(object? value)
    {
        return null;
    }
}

internal sealed class ValidatorNotNull : IValidator
{
    public string? Validate(object? value)
    {
        if (value is string s && string.IsNullOrWhiteSpace(s))
            return "Wartość tekstowa jest pusta";
        return value != null ? null : "Wartość jest null";
    }
}

internal sealed class ValidatorGreaterThan0<T> : IValidator where T : INumber<T>
{
    public string? Validate(object? value)
    {
        if (value is T v)
            return v > T.Zero ? null : $"Oczekiwano wartości > 0 ale otrzymano {v}";
        if (value != null)
            return "Wartość innego typu";
        return "Wartość jest null";
    }
}

// The actual validation methods ---------------------------------------------------------------------------------------

public static class Validation
{
    public static string? Check<T>(T dto, bool skipIdFields = false) where T : class
    {
        var properties = dto.GetType().GetProperties();
        foreach (var prop in properties)
        {
            if (skipIdFields && prop.Name.EndsWith("Id"))
                continue;

            var attr = prop.GetCustomAttribute(typeof(ValidationMethod), true);
            if (attr is not null && attr is ValidationMethod v)
            {
                var result = v.Check(prop.GetValue(dto));
                if (result != null)
                    return $"{Names.GetName<T>(prop.Name)}: {result}";
            }
        }
        return null;
    }
}
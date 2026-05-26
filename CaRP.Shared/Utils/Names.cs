using System.Linq.Expressions;
using System.Reflection;

namespace carp.Shared.Utils;

[AttributeUsage(AttributeTargets.Property)]
internal class ActualName(string name) : Attribute
{
    public string Name { get; } = name;
}

public class Names
{
    // Pobiera nazwę pola dla danej właściwości
    // Używać w stylu: Names.GetName<typ>(x => x.Pole); lub Names.GetName((typ x) => x.Pole);
    public static string GetName<T>(Expression<Func<T, object>> expression)
    {
        var attribute = GetAttribute(expression);
        return attribute == null ? "???" : ((ActualName)attribute).Name;
    }

    // To samo, ale na nazwie pola, proszę nie wpisywać na stałe stringów, bo to niekulturalnie
    public static string GetName<T>(string propertyName)
    {
        var attribute = GetAttribute<T>(propertyName);
        return attribute == null ? "???" : ((ActualName)attribute).Name;
    }

    // tutaj już magia C#'owa, niepotrzebne do szczęścia na froncie
    private static Attribute? GetAttribute<T>(string propName)
    {
        var property = typeof(T).GetProperty(propName);
        if (property == null)
            return null;
        return property.GetCustomAttribute(typeof(ActualName), true);
    }
    private static Attribute? GetAttribute<T>(Expression<Func<T, object>> expression)
    {
        try
        {
            MemberExpression memberExpr;

            // Sprawdzamy, czy wyrażenie zawiera konwersję na 'object' (typy wartościowe jak int, decimal, DateOnly)
            if (expression.Body is UnaryExpression unaryExpr)
            {
                memberExpr = (MemberExpression)unaryExpr.Operand;
            }
            else
            {
                // Dla typów referencyjnych (np. string)
                memberExpr = (MemberExpression)expression.Body;
            }

            return GetAttribute<T>(memberExpr.Member.Name);
        }
        catch
        {
            return null;
        }
    }
}
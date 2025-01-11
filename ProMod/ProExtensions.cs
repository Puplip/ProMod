using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ProMod;

public class ProDisplayAttribute : Attribute
{
	public readonly string Name;
	public ProDisplayAttribute(string name)
	{
		Name = name;
	}
}

public static class ProExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsLower(this char c)
    {
		return c >= 'a' && c <= 'z';
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsUpper(this char c)
    {
        return c >= 'A' && c <= 'Z';
    }

	[MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsDigit(this char c)
    {
        return c >= '0' && c <= '9';
    }
    public static string ProDisplay(this Enum enumValue)
	{
		return enumValue.GetType().GetMember(enumValue.ToString()).FirstOrDefault(null!)?.GetCustomAttributes<ProDisplayAttribute>().FirstOrDefault()?.Name ?? enumValue.ToString();
    }

    private enum CharType
    {
        Lower,
        Upper,
        Digit,
        Space
    };


    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static CharType GetCharType(this char c)
    {
		if (c.IsUpper())
		{
			return CharType.Upper;
		}
		else if (c.IsLower())
		{
			return CharType.Lower;
		} else if(c.IsDigit())
		{
			return CharType.Digit;
		} else
		{
			return CharType.Space;
		}
    }

    public static string CamelToSpaces(this string input)
    {
        string ret = "";

        input.Replace('_', ' ').Trim();

        if (input.Length < 2)
        {
            return input;
        }

        CharType lastChar = CharType.Space;
        CharType currentChar = input[0].GetCharType();

        for (int i = 0; i < input.Length; i++)
        {
            //next CharType or space if past the end
            CharType nextChar = i < input.Length - 1 ? input[i + 1].GetCharType() : CharType.Space;

            //just write it after a space
            if (lastChar == CharType.Space)
            {
                //only write non-space and try to capitalize
                ret += input[i].ToString().Trim().ToUpper();
                //space before first Upper case if last wasn't upper or next is lower
            }
            else if (currentChar == CharType.Upper && (lastChar != CharType.Upper || nextChar == CharType.Lower))
            {
                ret += " " + input[i];
                //space before first digit
            }
            else if (currentChar == CharType.Digit && lastChar != CharType.Digit)
            {
                ret += " " + input[i];
                //space before first lower if last was digit
            }
            else if (currentChar == CharType.Lower && lastChar == CharType.Digit)
            {
                ret += " " + input[i].ToString().ToUpper();
                //add normally
            }
            else
            {
                ret += input[i];
            }

            //shift types
            lastChar = currentChar;
            currentChar = nextChar;
        }

        return ret;
    }
    public static string NameWithSpaces(this Enum enumValue)
    {
		return enumValue.ToString().CamelToSpaces();
    }
	[MethodImpl(MethodImplOptions.AggressiveInlining)]
	public static float MinMax(this float f,float min,float max)
	{
		float t = f < min ? min : f;
		return t > max ? max : t;
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static string DebugPrintMember<T,U>(this T obj, Expression<Func<T,U>> expression, Func<U,string> formatPredicate = null)
    {
		if(expression is MemberExpression memberExpression && memberExpression.Member is MemberInfo memberInfo)
		{
            object value = memberInfo.MemberType switch {
                MemberTypes.Property => (memberInfo as PropertyInfo).GetValue(obj),
                MemberTypes.Field => (memberInfo as FieldInfo).GetValue(obj),
                _ => throw new Exception("Please only use this on fields and properties.")
            };

            return $"{memberInfo.Name}: {(formatPredicate != null ? formatPredicate((U)value) : value)}";

        }

        throw new Exception("Please only use this on fields and properties.");
    }
}

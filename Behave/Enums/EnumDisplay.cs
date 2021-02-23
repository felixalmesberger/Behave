using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Behave.Enums
{
  /// <summary>
  /// A Helper class providing display names of enums
  /// </summary>
  public static class EnumDisplay
  {
    /// <summary>
    /// Gets the display name
    /// </summary>
    /// <param name="value">enum value</param>
    /// <returns></returns>
    public static string GetDisplayName<TEnum>(TEnum value)
    {
      var field = typeof(TEnum).GetField(value.ToString());
      return ExtractDisplayName(field);
    }

    public static IList<EnumDisplayBag<TEnum>> GetDisplayBags<TEnum>()
      where TEnum : struct, IConvertible // Almost Enum
    {
      return EnumerateDisplayBags<TEnum>().ToList();
    }

    public static IEnumerable<EnumDisplayBag<TEnum>> EnumerateDisplayBags<TEnum>()
      where TEnum : struct, IConvertible // Almost Enum
    {
      var fields = typeof(TEnum).GetFields()
        .Where(x => x.FieldType == typeof(TEnum));

      foreach (var field in fields)
      {
        var name = ExtractDisplayName(field);
        var value = (TEnum) field.GetValue(null);

        yield return new EnumDisplayBag<TEnum>(value, name);
      }
    }

    private static string ExtractDisplayName(FieldInfo fieldInfo)
      => fieldInfo.GetCustomAttribute<DisplayNameAttribute>()
        ?.DisplayName ?? fieldInfo.Name;
  }
}
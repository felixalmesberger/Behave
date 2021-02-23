using System;

namespace Behave.Enums
{
  /// <summary>
  /// Encapsulates the enum value and display name
  /// </summary>
  /// <typeparam name="TEnum">Type of Enum</typeparam>
  public class EnumDisplayBag<TEnum>
    where TEnum : struct, IConvertible
  {
    public TEnum Value { get; }
    public string DisplayName { get; }

    internal EnumDisplayBag(TEnum value, string displayName)
    {
      this.Value = value;
      this.DisplayName = displayName;
    }

    public static implicit operator TEnum(EnumDisplayBag<TEnum> bag)
      => bag.Value;
  }
}
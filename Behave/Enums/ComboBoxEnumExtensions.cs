namespace Behave.Enums
{
  using System;
  using System.Linq;
  using System.Text;
  using System.Threading.Tasks;
  using System.Windows.Forms;

  public static class ComboBoxEnumExtensions
  {
    public static void PopulateWithEnum<TEnum>(this ComboBox combo)
      where TEnum : struct, IConvertible // Almost Enum
    {
      if (!typeof(TEnum).IsEnum)
        throw new ArgumentException("Type is not an enum");

      var items = EnumDisplay.EnumerateDisplayBags<TEnum>();

      combo.DisplayMember = nameof(EnumDisplayBag<TEnum>.DisplayName);
      combo.ValueMember = nameof(EnumDisplayBag<TEnum>.Value);

      combo.DataSource = items.ToArray();
    }
  }
}

  
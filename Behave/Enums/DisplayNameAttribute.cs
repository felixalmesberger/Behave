using System;
using System.Globalization;
using System.Resources;

namespace Behave.Enums
{
  /// <summary>
  /// Provide a DisplayName for the EnumValue
  /// </summary>
  [AttributeUsage(AttributeTargets.Enum | AttributeTargets.Field)]
  public class DisplayNameAttribute : Attribute
  {

    public string DisplayName { get; }

    /// <summary>
    /// Display Name of the Enum entry
    /// </summary>
    /// <param name="displayName"></param>
    public DisplayNameAttribute(string displayName)
    {
      this.DisplayName = displayName
                         ?? throw new ArgumentNullException(nameof(displayName));
    }

    /// <summary>
    /// Localized Display Name of Enum entry
    /// </summary>
    /// <param name="resourceType">Type of Resource being used</param>
    /// <param name="resourceId">Name of the Strings resource</param>
    public DisplayNameAttribute(Type resourceType, string resourceId)
    {
      if (resourceType is null)
        throw new ArgumentNullException(nameof(resourceId));

      if (resourceId is null)
        throw new ArgumentNullException(nameof(resourceId));

      this.DisplayName = this.GetLocalizedString(resourceType, resourceId);
    }

    private string GetLocalizedString(Type resourceType, string resourceIdentifier)
    {
      var baseName = resourceType.FullName;
      var resMan = new ResourceManager(baseName, resourceType.Assembly);

      try
      {
        return resMan.GetString(resourceIdentifier, CultureInfo.CurrentCulture);
      }
      catch
      {
        return resourceIdentifier;
      }
    }

    public override string ToString() => this.DisplayName;
  }
}
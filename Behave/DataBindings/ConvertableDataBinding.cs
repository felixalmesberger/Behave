using System;
using System.Windows.Forms;

namespace Behave.Validation
{
  public class ConvertableDataBinding : Binding
  {
    #region constructors

    public ConvertableDataBinding(string propertyName, object dataSource, string dataMember) 
      : base(propertyName, dataSource, dataMember)
    {
    }

    public ConvertableDataBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled) 
      : base(propertyName, dataSource, dataMember, formattingEnabled)
    {
    }

    public ConvertableDataBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode)
      : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode)
    {
    }

    public ConvertableDataBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue) 
      : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue)
    {
    }

    public ConvertableDataBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString) 
      : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString)
    {
    }

    public ConvertableDataBinding(string propertyName, object dataSource, string dataMember, bool formattingEnabled, DataSourceUpdateMode dataSourceUpdateMode, object nullValue, string formatString, IFormatProvider formatInfo) 
      : base(propertyName, dataSource, dataMember, formattingEnabled, dataSourceUpdateMode, nullValue, formatString, formatInfo)
    {
    }

    #endregion

    #region properties

    protected override void OnFormat(ConvertEventArgs cevent)
    {
      base.OnFormat(cevent);
    }

    protected override void OnParse(ConvertEventArgs cevent)
    {
      base.OnParse(cevent);
    }

    #endregion
  }
}
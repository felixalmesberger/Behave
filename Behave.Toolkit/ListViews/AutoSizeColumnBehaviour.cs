using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace Behave.ListViews
{
  public class AutoSizeColumnBehaviour : Behaviour<ListView>
  {
    protected override void OnAttached()
    {
      base.OnAttached();
    }

    public static void AutoSizeColumns(ListView listView)
    {
      //Prevents flickering
      listView.BeginUpdate();

      var maxWidthPerColumn = (int)((double)listView.Width / 4.0);

      var columnSize = new Dictionary<int, int>();

      //Auto size using header
      listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);

      //Grab column size based on header
      foreach (ColumnHeader colHeader in listView.Columns)
        columnSize.Add(colHeader.Index, colHeader.Width);

      //Auto size using data
      listView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent);

      //Grab comumn size based on data and set max width
      foreach (ColumnHeader colHeader in listView.Columns)
      {
        int nColWidth;
        if (columnSize.TryGetValue(colHeader.Index, out nColWidth))
          colHeader.Width = Math.Min(Math.Max(nColWidth, colHeader.Width), maxWidthPerColumn);
        else
          //Default to 50
          colHeader.Width = Math.Max(50, colHeader.Width);
      }

      listView.EndUpdate();
    }
  }
}
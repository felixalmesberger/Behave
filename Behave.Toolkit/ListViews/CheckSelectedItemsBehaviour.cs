using System;
using System.Windows.Forms;

namespace Behave.ListViews
{
  public class CheckSelectedItemsBehaviour : Behaviour<ListView>
  {
    protected override void OnAttached()
    {
      this.Control.SelectedIndexChanged += this.ListViewOnSelectedIndexChanged;
    }

    protected override void OnDetaching()
    {
      this.Control.SelectedIndexChanged -= this.ListViewOnSelectedIndexChanged;
    }

    private void ListViewOnSelectedIndexChanged(object sender, EventArgs e)
    {
      if (!this.Control.CheckBoxes)
        return;

      var selectedItems = this.Control
        .SelectedItems;

      for (var i = 0; i < this.Control.Items.Count; i++)
      {
        var item = this.Control.Items[i];
        var isSelected = selectedItems.Contains(item);
        var isChecked = this.Control.CheckedItems.Contains(item);

        if (isSelected != isChecked)
          item.Checked = isChecked;
      }
    }
  }
}
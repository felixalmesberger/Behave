using System;
using System.Windows.Forms;

namespace Behave.Forms
{
  public class UseParentIconBehaviour : Behaviour<Form>
  {
    protected override void OnAttached()
    {
      this.Control.ParentChanged += this.FormParentChanged;
    }

    protected override void OnDetaching()
    {
      this.Control.ParentChanged -= this.FormParentChanged;
    }

    private void FormParentChanged(object sender, EventArgs e)
    {
      if (!(this.Control.Parent is Form parentForm))
        return;

      this.Control.Icon = parentForm.Icon;
    }
  }
}
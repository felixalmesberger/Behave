using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Behave.TextBoxes
{
  public class PlaceholderBehaviour : Behaviour<TextBox>
  {

    #region fields

    private string placeholder;

    #endregion

    #region properties

    public string Placeholder
    {
      get => this.placeholder;
      set
      {
        this.placeholder = value;
        this.ApplyPlaceholder();
      }
    }

    #endregion

    protected override void OnAttached()
    {
      this.Control.HandleCreated += this.ControlHandleCreated;
    }

    protected override void OnDetaching()
    {
      this.Control.HandleCreated -= this.ControlHandleCreated;
    }


    private void ControlHandleCreated(object sender, EventArgs e)
    {
      this.ApplyPlaceholder();
    }

    
    private void ApplyPlaceholder()
    {
      if (this.Control is null)
        return;

      if (!this.Control.IsHandleCreated)
        return;

      NativeMethods.SendMessage(this.Control.Handle, NativeMethods.EM_SETCUEBANNER, 0, this.Placeholder);
    }

    private static class NativeMethods
    {
      public const int EM_SETCUEBANNER = 0x1501;

      [DllImport("user32.dll", CharSet = CharSet.Auto)]
      public static extern long SendMessage(IntPtr hWnd, int msg, int wParam, [MarshalAs(UnmanagedType.LPWStr)]string lParam);
    }
  }
}
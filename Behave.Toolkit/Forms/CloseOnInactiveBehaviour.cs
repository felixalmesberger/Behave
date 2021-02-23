using System;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Behave.Forms
{
  public class CloseOnInactiveBehaviour : Behaviour<Form>
  {

    private WindowProcListener listener;

    protected override void OnAttached()
    {
      this.listener = new WindowProcListener(this.Control);
      this.listener.NewWindowMessage += this.NewWindowMessage;
    }

    private void NewWindowMessage(object sender, NewWindowMessageEventArgs e)
    {
      if (e.Message.Msg != NativeMethods.WM_ACTIVATE)
        return;

      if ((int)e.Message.WParam != NativeMethods.WA_INACTIVE)
        return;

      this.Control.Close();
    }

    public override void OnDispose()
    {
      this.listener.NewWindowMessage -= this.NewWindowMessage;
      this.listener.Dispose();
    }


    private static class NativeMethods
    {
      public const int WM_ACTIVATE = 0x0006;
      public const int WM_NCACTIVATE = 0x086;

      public const int WA_INACTIVE = 0;
    }
  }

  public class WindowProcListener : NativeWindow
  {

    public event EventHandler<NewWindowMessageEventArgs> NewWindowMessage;

    private readonly Control control;

    public WindowProcListener(Control control)
    {
      this.control = control ?? throw new ArgumentNullException(nameof(control));

      this.control.HandleCreated += this.ControlHandleCreated;
      this.control.HandleDestroyed += this.ControlHandleDestroyed;

      if (this.control.IsHandleCreated)
        this.ControlHandleCreated();
    }

    private void ControlHandleCreated(object sender = null, EventArgs e = null)
    {
      this.AssignHandle(this.control.Handle);
    }

    private void ControlHandleDestroyed(object sender, EventArgs e)
    {
      this.Dispose();
    }

    public void Dispose()
    {
      this.ReleaseHandle();
      this.control.HandleCreated -= this.ControlHandleCreated;
      this.control.HandleDestroyed -= this.ControlHandleDestroyed;
    }

    protected override void WndProc(ref Message m)
    {
      this.NewWindowMessage?.Invoke(this, new NewWindowMessageEventArgs(m));
      base.WndProc(ref m);
    }
  }

  public class NewWindowMessageEventArgs
  {
    public NewWindowMessageEventArgs(Message message)
    {
      this.Message = message;
    }

    public Message Message { get; }
  }

}
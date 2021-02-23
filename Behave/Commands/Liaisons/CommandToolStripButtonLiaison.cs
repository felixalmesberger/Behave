using System;
using System.Windows.Forms;

namespace Behave.Commands
{
  /// <summary>
  ///     A liaison connecting a ToolStripButton to a command
  /// </summary>
  public class CommandToolStripButtonLiaison : CommandComponentLiaison
  {
    private readonly ToolStripButton button;

    public CommandToolStripButtonLiaison(ICommand command, ToolStripButton button)
      : base(command, button)
    {
      this.button = button ?? throw new ArgumentNullException(nameof(button));
      this.AttachToButton();

      this.Init();
    }

    private void AttachToButton()
    {
      this.button.Click -= this.ButtonOnClick;
      this.button.Click += this.ButtonOnClick;
    }

    private void DetachFromButton()
    {
      this.button.Click -= this.ButtonOnClick;
    }

    private void ButtonOnClick(object sender, EventArgs e)
    {
      if (!this.Command.CanExecute)
        return;

      this.Command.Execute();
    }

    public override void SetComponentEnabled(bool canExecute)
    {
      this.button.Enabled = canExecute;
    }

    public override void Dispose()
    {
      base.Dispose();
      this.DetachFromButton();
    }
  }
}
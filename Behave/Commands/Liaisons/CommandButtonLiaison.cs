using System;
using System.Windows.Forms;

namespace Behave.Commands
{
  /// <summary>
  /// Describes a liaison between a button to a command
  /// The command will be executed by clicking on a button
  /// The enabled status of the button will be synced with
  /// the CanExecute property of the command.
  /// </summary>
  public class CommandButtonLiaison : CommandComponentLiaison
  {

    private readonly Button button;

    public CommandButtonLiaison(ICommand command, Button button)
      : base(command, button)
    {
      this.button = button ?? throw new ArgumentNullException(nameof(button));
      this.AttachToButton();

      this.Init();
    }

    private void AttachToButton()
    {
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
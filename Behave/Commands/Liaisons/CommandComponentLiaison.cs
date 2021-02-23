using System;
using System.ComponentModel;

namespace Behave.Commands
{
  /// <summary>
  ///     A base class to provide common logic
  ///     needed for a liaison between a component
  ///     and a command
  /// </summary>
  public class CommandComponentLiaison : IDisposable
  {

    #region properties

    public ICommand Command { get; }
    public IComponent Component { get; }

    #endregion

    protected CommandComponentLiaison(ICommand command, IComponent component)
    {
      this.Component = component;
      this.Command = command;
      this.Command.CanExecuteChanged += this.CommandCanExecuteChanged;
    }

    protected void Init()
    {
      this.Command.RefreshCanExecute();
      this.SetComponentEnabled(this.Command.CanExecute);
    }

    protected void CommandCanExecuteChanged(object sender, EventArgs e)
      => this.SetComponentEnabled(this.Command.CanExecute);

    public virtual void SetComponentEnabled(bool canExecute) { }

    public virtual void Dispose()
    {
      if (this.Command is IDisposable disposable)
        disposable.Dispose();
    }
  }
}
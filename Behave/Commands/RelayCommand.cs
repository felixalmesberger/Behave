using System;

namespace Behave.Commands
{
  /// <summary>
  /// Command implementation, with Execute Action in constructor
  /// </summary>
  public class RelayCommand : ICommand
  {

    private readonly Action action;

    public RelayCommand(Action action)
    {
      this.action = action ?? throw new ArgumentNullException(nameof(action));
    }

    public void Execute()
      => this.action();

    public bool CanExecute { get; } = true;
    public event EventHandler CanExecuteChanged;
    public void RefreshCanExecute()
    {
    }
  }
}
using System;

namespace Behave.Commands
{
  ///<summary>
  ///     An interface that allows an application author to define a method to be invoked.
  ///</summary>
  public interface ICommand
  {
    /// <summary>
    /// Defines the method that should be executed when the command is executed.
    /// </summary>
    void Execute();

    /// <summary>
    /// Returns whether the command can be executed.
    /// </summary>
    bool CanExecute { get; }
    
    /// <summary>
    /// Raised when the ability of the command to execute has changed.
    /// </summary>
    event EventHandler CanExecuteChanged;

    /// <summary>
    /// Explicitly re evaluate the CanExecute property
    /// </summary>
    void RefreshCanExecute();
  }
}
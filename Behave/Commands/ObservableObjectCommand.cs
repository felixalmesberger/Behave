using System;
using System.ComponentModel;
using System.Reflection;
using System.Threading.Tasks;

namespace Behave.Commands
{
  /// <summary>
  /// Creates an Command from any object implementing
  /// the INotifyPropertyChanged interface. It will
  /// use a Method and a bool property called CanMethod.
  /// </summary>
  public class ObservableObjectCommand : ICommand, IDisposable
  {

    public event EventHandler CanExecuteChanged;

    #region fields

    private readonly INotifyPropertyChanged instance;
    private readonly MethodInfo executeMethod;
    private readonly PropertyInfo canExecuteProperty;
    private readonly string actionName;
    private bool canExecute;

    #endregion

    #region constructor

    public ObservableObjectCommand(INotifyPropertyChanged instance, string actionName)
    {
      this.actionName = actionName ?? throw new ArgumentNullException(nameof(actionName));
      this.instance = instance ?? throw new ArgumentNullException(nameof(instance));

      this.executeMethod = instance
                          .GetType()
                          .GetMethod(actionName);

      if (this.executeMethod is null)
        throw new MissingMethodException(SR.ActionMethodNotFound(actionName, instance));

      this.canExecuteProperty = instance
                               .GetType()
                               .GetProperty($"Can{actionName}");

      if (this.canExecuteProperty is null)
        throw new MissingMethodException(SR.CanExecuteMethodNotFound(actionName, instance));


      this.instance.PropertyChanged += this.PropertyChanged;
      this.RefreshCanExecute();
    }

    private void PropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == $"Can{this.actionName}")
        this.RefreshCanExecute();
    }

    #endregion

    public void Execute()
    {
      try
      {
        if (this.CanExecute)
          this.InvokeExecutionMethod();
      }
      catch (Exception ex)
      {
        throw new TargetInvocationException(SR.ExecutionException(this), ex);
      }
    }

    public bool CanExecute
    {
      get => this.canExecute;
      set
      {
        if (this.canExecute == value)
          return;
        this.canExecute = value;
        this.CanExecuteChanged?.Invoke(this, EventArgs.Empty);
      }
    }

    public void RefreshCanExecute()
    {
      try
      {
        this.CanExecute = (bool)this.canExecuteProperty.GetValue(this.instance);
      }
      catch
      {
        //log.Debug("Could not retrieve can execute value from object {this.instance}");
        this.CanExecute = false;
      }
    }

    public void Dispose()
    {
      this.instance.PropertyChanged -= this.PropertyChanged;
    }

    public async Task InvokeAsync(MethodInfo methodInfo, object obj)
    {
      var awaitable = methodInfo.Invoke(obj, new object[] { }) as Task;

      if (awaitable is null)
        return;

      try
      {
        await awaitable;
      }
      catch (Exception ex)
      {

      }
    }

    public async  void InvokeExecutionMethod()
    {
      if (this.executeMethod.ReturnType == typeof(Task))
        await this.InvokeAsync(this.executeMethod, this.instance);
      else
        this.executeMethod.Invoke(this.instance, new object[] { });
    }

    private static class SR
    {
      public static string CanExecuteMethodNotFound(string actionName, object instance)
        => $"No public bool property with name Can{actionName} in type {instance.GetType()} was found.";

      public static string ActionMethodNotFound(string actionName, object instance)
        => $"No public void method with name {actionName} in type {instance.GetType()} was found.";

      public static string ExecutionException(ObservableObjectCommand command)
        => $"Invoking the action of Command {command.executeMethod?.Name} threw an exception.";
    }
  }
}
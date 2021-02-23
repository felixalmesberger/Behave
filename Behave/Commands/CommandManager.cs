using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.Remoting;
using System.Windows.Forms;

namespace Behave.Commands
{
  /// <summary>
  /// A windows form component, that can be used
  /// to connect Windows Forms UI Controls to Commands
  /// </summary>
  public class CommandManager : Component
  {

    #region fields

    private readonly IList<CommandComponentLiaison> liaisons = new List<CommandComponentLiaison>();

    #endregion

    #region constructor

    public CommandManager()
    {
    }

    public CommandManager(IContainer container)
      : this()
    {
      container?.Add(this);
    }

    #endregion

    public IList<string> AvailableCommands { get; set; }

    private Type ExtractType(object commandContext)
    {
      if (commandContext is null)
        return null;

      if (this.CommandContext is Type type)
        return type;

      if (this.CommandContext is BindingSource bindingSource)
        return this.ExtractType(bindingSource.DataSource);

      return commandContext?.GetType();
    }

    private IEnumerable<string> EnumerateAvailableCommands()
    {
      var type = this.ExtractType(this.CommandContext);
      if (type is null)
        return Enumerable.Empty<string>();

      return type.GetProperties(BindingFlags.Instance)
                 .Where(x => typeof(ICommand).IsAssignableFrom(x.PropertyType))
                 .Select(x => x.Name);
    }

    private void RefreshAvailableCommands()
    {
      this.AvailableCommands = this.EnumerateAvailableCommands().ToList();
    }

    private object commandContext;

    public object CommandContext
    {
      get => this.commandContext;
      set
      {
        this.commandContext = value;
        this.RefreshAvailableCommands();
      }
    }

    /// <summary>
    /// Connect a component to an object, that is
    /// implementing the INotifyPropertyChanged interface.
    /// The command will be internally created by using a method 
    /// of the instance called name and a CanExecute defined by
    /// a public bool property of the instance called CanName.
    /// </summary>
    public void Connect(IComponent component, INotifyPropertyChanged instance, string name)
      => this.Connect(component, new ObservableObjectCommand(instance, name));

    /// <summary>
    ///     Connect a component to an Action,
    ///     It can always be executed.
    /// </summary>
    /// <param name="component"></param>
    /// <param name="action"></param>
    public void Connect(IComponent component, Action action)
      => this.Connect(component, new RelayCommand(action));

    /// <summary>
    /// Connect a component to a command
    /// </summary>
    public void Connect(IComponent component, ICommand command)
    {
      CommandComponentLiaison liaison = null;

      switch (component)
      {
        case Button button:
          liaison = new CommandButtonLiaison(command, button);
          break;
        case ToolStripButton toolStripButton:
          liaison = new CommandToolStripButtonLiaison(command, toolStripButton);
          break;
        case ToolStripMenuItem toolStripMenuItem:
          liaison = new CommandToolStripMenuButtonLiaison(command, toolStripMenuItem);
          break;
        default:
          throw new Exception($"The type {component.GetType()} is not supported.");
      }

      var existing = this.liaisons.FirstOrDefault(x => object.ReferenceEquals(x.Component, component));
      if (existing != null)
      {
        this.liaisons.Remove(existing);
        existing.Dispose();
      }

      this.liaisons.Add(liaison);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        var disposables = this.liaisons.OfType<IDisposable>();
        foreach (var disposable in disposables)
          disposable.Dispose();
      }
      base.Dispose(disposing);
    }
  }
}

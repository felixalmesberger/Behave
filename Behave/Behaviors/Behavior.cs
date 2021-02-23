using System;
using System.Windows.Forms;

namespace Behave
{

  public interface IBehaviour : IDisposable
  {
    Control Control { get; set; }
  }

  public abstract class Behaviour<TControl> : IBehaviour
    where TControl : Control
  {
    #region fields

    private TControl control;

    #endregion

    #region properties

    public TControl Control
    {
      get => this.control;
      set
      {
        if (!(this.Control is null))
          this.OnDetaching();

        this.control = value;

        if (this.Control is null)
          return;

        this.control.Disposed += this.ControlDisposed;
        this.OnAttached();
      }
    }

    Control IBehaviour.Control
    {
      get => this.Control;
      set => this.Control = (TControl)value;
    }

    #endregion

    private void ControlDisposed(object sender, EventArgs e)
    {
      this.Dispose();
    }

    #region lifecycle

    protected virtual void OnAttached() { }
    protected virtual void OnDetaching() { }
    public virtual void OnDispose() { }

    #endregion
    public void Dispose()
    {
      this.OnDispose();
    }
  }
}
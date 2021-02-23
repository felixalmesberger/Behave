using System;
using System.Net.Mime;
using System.Windows.Forms;

namespace Behave.TextBoxes
{
  public class TextBoxThrottleBehaviour : Behaviour<TextBox>
  {

    #region events

    public event EventHandler TextChanged;
    private void OnTextChanged()
      => this.TextChanged?.Invoke(this, EventArgs.Empty);

    #endregion

    #region members

    private readonly Timer throttleTimer;
    private Control control;
    private string lastReportedValue;

    #endregion

    #region constructor

    public TextBoxThrottleBehaviour()
    {
      this.throttleTimer = new Timer
      {
        Enabled = false,
      };
      this.throttleTimer.Tick += this.ThrottleTimerEllapsed;

      //Default values
      this.ThrottleTimeMs = 300;
    }

    private void ThrottleTimerEllapsed(object sender, EventArgs e)
    {
      this.throttleTimer.Stop();

      if (this.lastReportedValue == this.Text)
        return;

      this.lastReportedValue = this.Text;

      this.OnTextChanged();
    }

    #endregion

    #region properties

    public int ThrottleTimeMs
    {
      get => this.throttleTimer.Interval;
      set => this.throttleTimer.Interval = value;
    }

    public string Text => this.Control?.Text ?? "";

    #endregion

    protected override void OnAttached()
    {
      this.control.TextChanged += this.ControlTextChanged;
    }

    protected override void OnDetaching()
    {
      this.control.TextChanged -= this.ControlTextChanged;
    }

    private void ControlTextChanged(object sender, EventArgs e)
      => this.RestartThrottleTimer();
    
    private void RestartThrottleTimer()
    {
      this.throttleTimer.Stop();
      this.throttleTimer.Start();
    }
  }
}
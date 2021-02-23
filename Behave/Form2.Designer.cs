namespace Behave
{
  partial class Form2
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.components = new System.ComponentModel.Container();
      Behave.Forms.CloseOnInactiveBehaviour closeOnInactiveBehaviour1 = new Behave.Forms.CloseOnInactiveBehaviour();
      this.behaviourProvider1 = new Behave.BehaviourProvider(this.components);
      ((System.ComponentModel.ISupportInitialize)(this.behaviourProvider1)).BeginInit();
      this.SuspendLayout();
      // 
      // Form2
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      closeOnInactiveBehaviour1.Control = this;
      this.behaviourProvider1.GetBehaviours(this).AddRange(new Behave.IBehaviour[] {
            closeOnInactiveBehaviour1});
      this.ClientSize = new System.Drawing.Size(800, 450);
      this.Name = "Form2";
      this.Text = "Form2";
      ((System.ComponentModel.ISupportInitialize)(this.behaviourProvider1)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private BehaviourProvider behaviourProvider1;
  }
}
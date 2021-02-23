using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Windows.Forms;

namespace Behave
{
  [ProvideProperty("Behaviours", typeof(Control))]
  public class BehaviourProvider : Component, IExtenderProvider, ISupportInitialize
  {

    private readonly Dictionary<Control, BehaviourCollection> behaviours = new Dictionary<Control, BehaviourCollection>();

    public BehaviourProvider()
    {

    }

    public BehaviourProvider(IContainer container)
    {
      container?.Add(this);
    }


    [Category("Collections")]
    [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
    [Editor(typeof(BehaviourCollectionEditor), typeof(UITypeEditor))]
    public BehaviourCollection GetBehaviours(Control control)
    {
      if (!this.behaviours.ContainsKey(control))
        this.behaviours.Add(control, new BehaviourCollection(control));

      return this.behaviours[control];
    }

    public void SetBehaviours(Control control, BehaviourCollection behaviours)
    {
      if (this.behaviours.ContainsKey(control))
        this.behaviours[control] = behaviours;

      this.behaviours.Add(control, behaviours);

    }

    public bool CanExtend(object extendee)
    {
      return extendee is Control;
    }

    public void BeginInit()
    {
    }

    public void EndInit()
    {
      foreach (var behaviourControlBinding in this.behaviours)
        foreach (IBehaviour behaviour in behaviourControlBinding.Value)
          behaviour.Control = behaviourControlBinding.Key;
    }
  }
}
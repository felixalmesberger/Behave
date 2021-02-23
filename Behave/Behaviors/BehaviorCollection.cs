using System;
using System.Collections;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace Behave
{
  public class BehaviourCollection : CollectionBase
  {

    private readonly Control control;

    public BehaviourCollection(Control control)
    {
      this.control = control ?? throw new ArgumentNullException(nameof(control));
    }

    public IBehaviour Add(IBehaviour item)
    {
      item.Control = control;
      this.InnerList.Add(item);
      return item;
    }

    public void Remove(IBehaviour item)
    {
      item.Control = null;
      this.InnerList.Remove(item);
    }

    public bool Contains(IBehaviour item)
    {
      return this.InnerList.Contains(item);
    }

    public IBehaviour this[int i]
    {
      get => (IBehaviour)this.InnerList[i];
      set => this.InnerList[i] = value;
    }

    public void AddRange(IBehaviour[] items)
    {
      foreach (var item in items)
        item.Control = this.control;

      this.InnerList.AddRange(items);
    }

    public IBehaviour[] GetValues()
    {
      var si = new IBehaviour[this.InnerList.Count];
      this.InnerList.CopyTo(0, si, 0, this.InnerList.Count);
      return si;
    }

    protected override void OnInsert(int index, object value)
    {
      base.OnInsert(index, value);
    }
  }
}
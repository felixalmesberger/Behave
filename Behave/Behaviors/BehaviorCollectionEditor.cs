using System;
using System.ComponentModel.Design;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace Behave
{
  public class BehaviourCollectionEditor : CollectionEditor
  {
    public BehaviourCollectionEditor(Type type) : base(type)
    {
    }

    protected override Type[] CreateNewItemTypes()
    {
      var result = BehaviourRepository.Instance.GetMatching(this.Context.Instance);

      return result;
    }

    // CreateNewItemTypes is cached by the collection editor
    // In our case we dont want that behaviour because the
    // selection depends on the selected control
    // I have not come up with a nicer solution than using
    // reflection to clear the newItemTypesCache every time
    // the form is created. This leads to a refreshing of the
    // cache by calling CreateNewItemTypes

    protected override CollectionForm CreateCollectionForm()
    {
      this.ClearNewItemTypes();
      return base.CreateCollectionForm();
    }

    private void ClearNewItemTypes()
    {
      var newItemTypesField =
        typeof(CollectionEditor).GetField("newItemTypes", BindingFlags.Instance | BindingFlags.NonPublic);

      newItemTypesField?.SetValue(this, null);
    }

    protected override object CreateInstance(Type itemType)
    {
      if (!(Activator.CreateInstance(itemType) is IBehaviour behaviour))
        return base.CreateInstance(itemType);

      behaviour.Control = this.Context.Instance as Control;
      return behaviour;
    }
  }
}
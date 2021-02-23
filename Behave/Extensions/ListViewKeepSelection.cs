using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace Behave.Extensions
{
  public abstract class KeepSelectionScope<T> : IDisposable
    where T : Control
  {

    protected T Control { get; }

    private object initialSelected;

    protected abstract object GetSelected();
    protected abstract void SetSelected(object value);

    protected KeepSelectionScope(T control)
    {
      this.Control = control ?? throw new ArgumentNullException(nameof(control));
      this.Initialize();
    }

    private void Initialize()
    {
      this.initialSelected = this.GetSelected();
    }

    public void Dispose()
    {
      this.SetSelected(this.initialSelected);
    }
  }

  public class ListControlKeepSelectionScope : KeepSelectionScope<ListControl>
  {
    public ListControlKeepSelectionScope(ListControl control)
      : base(control)
    {
    }

    protected override object GetSelected()
    {
      return this.Control.SelectedValue;
    }

    protected override void SetSelected(object value)
    {
      this.Control.SelectedValue = value;
    }
  }

  public class ListBoxKeepSelectionScope : KeepSelectionScope<ListBox>
  {
    public ListBoxKeepSelectionScope(ListBox control)
      : base(control)
    {
    }

    protected override object GetSelected()
    {
      return this.Control.SelectedItem;
    }

    protected override void SetSelected(object value)
    {
      this.Control.SelectedItem = value;
    }
  }

  public class ComboBoxKeepSelectionScope : KeepSelectionScope<ComboBox>
  {
    public ComboBoxKeepSelectionScope(ComboBox control)
      : base(control)
    {
    }

    protected override object GetSelected()
    {
      return this.Control.DataSource is null
        ? this.Control.SelectedItem
        : this.Control.SelectedValue;
    }

    protected override void SetSelected(object value)
    {
      if (this.Control.DataSource is null)
        this.Control.SelectedItem = value;
      else this.Control.SelectedValue = value;
    }
  }

  public class TreeViewKeepSelectionScope : KeepSelectionScope<TreeView>
  {
    public TreeViewKeepSelectionScope(TreeView control) : base(control)
    {
    }

    protected override object GetSelected()
    {
      return this.Control.SelectedNode;
    }

    protected override void SetSelected(object value)
    {
      if (value is not TreeNode node)
        return;

      if (node?.Tag is { } tag)
      {
        var nodeMatchingTag = this.Control.Nodes.IncludeChildren().SingleOrDefault(x => x.Tag == tag);

        if (nodeMatchingTag is not null)
          this.Control.SelectedNode = nodeMatchingTag;

        return;
      }

      this.Control.SelectedNode = node;
    }
  }

  public class ListViewKeepSelectionScope : KeepSelectionScope<ListView>
  {
    public ListViewKeepSelectionScope(ListView control) : base(control)
    {
    }

    protected override object GetSelected()
    {
      return this.Control.SelectedItems;
    }

    protected override void SetSelected(object value)
    {
      if (value is not ListView.SelectedListViewItemCollection selectedItems)
        return;


      this.Control.SelectedIndices.Clear();
      foreach (ListViewItem item in selectedItems)
        this.SelectItem(item);

    }

    private void SelectItem(ListViewItem item)
    {
      if (item?.Tag is { } tag)
      {
        var nodeMatchingTag = this.Control
                                            .Items
                                            .OfType<ListViewItem>()
                                            .SingleOrDefault(x => x.Tag == tag);

        if (nodeMatchingTag is not null)
          this.Control.SelectedIndices.Add(nodeMatchingTag.Index);

        return;
      }
    }
  }

  public static class KeepSelectionScopeExtensions
  {
    public static IDisposable KeepSelection(this ListBox me)
      => new ListBoxKeepSelectionScope(me);
  }

  public static class TreeNodeCollectionExtensions
  {
    public static IEnumerable<TreeNode> IncludeChildren(this TreeNodeCollection me)
    {
      var stack = new Stack<TreeNode>();

      foreach (TreeNode child in me)
        stack.Push(child);

      while (stack.Any())
      {
        var node = stack.Pop();
        yield return node;

        foreach (TreeNode child in node.Nodes)
          stack.Push(child);
      }
    }
  }

  public static class ControlExtensions
  {
    public static IEnumerable<Control> IncludeChildren(this IEnumerable<Control> me)
    {
      var stack = new Stack<Control>();

      foreach (Control child in me)
        stack.Push(child);

      while (stack.Any())
      {
        var control = stack.Pop();
        yield return control;

        foreach (Control child in control.Controls)
          stack.Push(child);
      }
    }
  }
}
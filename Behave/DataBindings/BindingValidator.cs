namespace Behave.Validation
{
  // (C) 2020 Infomatik - Felix Almesberger

  using System;
  using System.Collections.Generic;
  using System.ComponentModel;
  using System.ComponentModel.DataAnnotations;
  using System.ComponentModel.Design;
  using System.Linq;
  using System.Windows.Forms;

  namespace Infomatik.UI.Controls.Behaviours
  {
    /// <summary>
    /// A component that validates Bindings on a given control.
    /// For this the DataSource if the Binding must implement the
    /// IValidatableObject interface. Errors will be visualized using
    /// an error provider
    /// </summary>
    public class BindingValidator : Component, ISupportInitialize
    {

      #region fields

      private IList<Binding> bindings = new List<Binding>();

      #endregion

      #region constructors

      public BindingValidator()
      {
      }

      public BindingValidator(IContainer container)
      {
        container?.Add(this);
      }

      #endregion

      #region properties

      /// <summary>
      /// Error provider that will be used to show errors
      /// </summary>
      public ErrorProvider ErrorProvider { get; set; }

      /// <summary>
      /// Service Container that will be used in
      /// the ValidationContext for Validation
      /// </summary>
      public IServiceProvider ServiceProvider { get; set; }

      public override ISite Site
      {
        set
        {
          base.Site = value;
          if (value == null)
            return;

          if (!(value.GetService(typeof(IDesignerHost)) is IDesignerHost host))
            return;

          var baseComp = host.RootComponent;

          if (baseComp is ContainerControl containerControl)
            this.ContainerControl = containerControl;
        }
      }

      /// <summary>
      /// The parent control of the controls that will be validated
      /// </summary>
      public ContainerControl ContainerControl { get; set; }

      #endregion

      private void AttachToBindings()
      {
        if (this.ContainerControl is null)
          return;

        this.bindings = BindingCollector.GetAllBindings(this.ContainerControl);

        foreach (var binding in this.bindings)
        {
          binding.FormattingEnabled = true;
          binding.BindingComplete += this.BindingComplete;
        }
      }

      private void BindingComplete(object sender, BindingCompleteEventArgs e)
      {
        var subject = this.GetBindingDataSource(e.Binding.DataSource);

        var hasBindingError = e.Exception != null;
        if (hasBindingError)
        {
          this.ShowBindingError(e);
        }
        else
        {
          var items = new Dictionary<object, object>();
          var context = new ValidationContext(subject, this.ServiceProvider, items);

          this.ValidateBindings(subject as IValidatableObject, context);
        }
      }

      private object GetBindingDataSource(object value)
      {
        if (value is BindingSource bindingSource)
          return bindingSource.DataSource;
        else
          return value;
      }

      private void ShowBindingError(BindingCompleteEventArgs e)
      {
        this.ErrorProvider?.SetError(e.Binding.Control, e.ErrorText);
      }

      private void ValidateBindings(IValidatableObject subject, ValidationContext context)
      {
        if (subject is null)
          return;

        var errors = subject.Validate(context).ToList();

        var bindingsOfSameSubject = this.bindings.Where(x => this.GetBindingDataSource(x.DataSource) == subject);

        foreach (var binding in bindingsOfSameSubject)
        {
          var boundProperty = binding.BindingMemberInfo.BindingMember;

          var errorMessage = errors
                            .FirstOrDefault(x => x.MemberNames.Contains(boundProperty))
                           ?.ErrorMessage;

          this.ErrorProvider?.SetError(binding.Control, errorMessage);
        }
      }

      public void BeginInit()
      {
      }

      public void EndInit()
      {
        this.AttachToBindings();

        if (this.TryFindErrorProvider(out var errorProvider) && this.ErrorProvider is null)
          this.ErrorProvider = errorProvider;
      }

      private bool TryFindErrorProvider(out ErrorProvider errorProvider)
      {
        errorProvider = this.Container?.Components?.OfType<ErrorProvider>()?.FirstOrDefault();
        return errorProvider != null;
      }

      /// <summary>
      /// Collects all bindings of all controls hosted in a parent control
      /// </summary>
      private class BindingCollector
      {

        public static IList<Binding> GetAllBindings(Control control)
          => new BindingCollector(control).Bindings;

        private readonly IList<Control> visitedControls = new List<Control>();
        private IList<Binding> Bindings { get; } = new List<Binding>();

        private BindingCollector(Control control)
        {
          this.VisitControl(control);
        }

        private void VisitControl(Control control)
        {
          if (this.visitedControls.Contains(control))
            return;

          this.visitedControls.Add(control);

          foreach (Binding binding in control.DataBindings)
            this.Bindings.Add(binding);

          foreach (Control subControl in control.Controls)
            this.VisitControl(subControl);
        }
      }
    }
  }
}
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading;

namespace Behave.Observables
{
  public class ObservableObject : INotifyPropertyChanged
  {
    public event PropertyChangedEventHandler PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      this.PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }

  public class SynchronizedObservableObject
  {
    public SynchronizationContext SynchronizationContext { get; set; } = SynchronizationContext.Current;
  }
}
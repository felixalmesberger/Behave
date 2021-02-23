using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Behave
{
  public class BehaviourRepository
  {
    public static BehaviourRepository Instance { get; } = new BehaviourRepository();

    private IList<Type> behaviourTypes;

    public Type[] GetMatching(object instance)
    {
      this.Initialize();

      return this.behaviourTypes
                 .Where(x => this.MatchesControl(x, instance))
                 .ToArray();
    }

    private bool MatchesControl(Type behaviourType, object control)
    {
      if (behaviourType is null)
        return false;

      if (!behaviourType.IsGenericType || behaviourType.GetGenericTypeDefinition() != typeof(Behaviour<>))
        return this.MatchesControl(behaviourType.BaseType, control);
      
      var controlConstrain = behaviourType.GenericTypeArguments.FirstOrDefault();

      if (controlConstrain is null)
        return false;

      return controlConstrain.IsInstanceOfType(control);
    }

    public void Register(Type type)
    {
      this.behaviourTypes.Add(type);
    }

    private void Initialize()
    {
      if (this.behaviourTypes != null)
        return;

      var assemblies = new List<Assembly>();

      var executingAsm = Assembly.GetExecutingAssembly();
      var referenced = executingAsm
                      .GetReferencedAssemblies()
                      .Select(Assembly.Load)
                      .ToList();
      assemblies.Add(executingAsm);
      assemblies.AddRange(referenced);

      var types = assemblies.SelectMany(x => x.GetTypes()).ToList();

      this.behaviourTypes = types.Where(x => typeof(IBehaviour)
                                 .IsAssignableFrom(x))
                                 .Where(x => !x.IsInterface)
                                 .Where(x => !x.IsAbstract)
                                 .Distinct()
                                 .ToList();
    }
  }
}
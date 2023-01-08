using System.Reflection;
using AutoMapper;

namespace ManualTools.Common;
public class MappingProfile : Profile
{
  public MappingProfile()
  {
    ApplyMappingsToAssembly(Assembly.GetExecutingAssembly());
    ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
    ApplyMappingsFrom2Assembly(Assembly.GetExecutingAssembly());
  }

  private void ApplyMappingsToAssembly(Assembly assembly)
  {
    var mapToType = typeof(IMapTo<>);

    var mappingMethodName = nameof(IMapTo<object>.Mapping);

    bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapToType;

    var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

    var argumentTypes = new Type[] { typeof(Profile) };

    foreach (var type in types)
    {
      var instance = Activator.CreateInstance(type);

      var methodInfo = type.GetMethod(mappingMethodName);

      if (methodInfo != null)
      {
        methodInfo.Invoke(instance, new object[] { this });
      }
      else
      {
        var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

        if (interfaces.Count > 0)
        {
          foreach (var @interface in interfaces)
          {
            var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

            interfaceMethodInfo?.Invoke(instance, new object[] { this });
          }
        }
      }
    }
  }
  private void ApplyMappingsFromAssembly(Assembly assembly)
  {
    var mapFromType = typeof(IMapFrom<>);

    var mappingMethodName = nameof(IMapFrom<object>.Mapping);

    bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

    var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

    var argumentTypes = new Type[] { typeof(Profile) };

    foreach (var type in types)
    {
      var instance = Activator.CreateInstance(type);

      var methodInfo = type.GetMethod(mappingMethodName);

      if (methodInfo != null)
      {
        methodInfo.Invoke(instance, new object[] { this });
      }
      else
      {
        var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

        if (interfaces.Count > 0)
        {
          foreach (var @interface in interfaces)
          {
            var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

            interfaceMethodInfo?.Invoke(instance, new object[] { this });
          }
        }
      }
    }
  }

  private void ApplyMappingsFrom2Assembly(Assembly assembly)
  {
    var mapFromType = typeof(IMapFrom<,>);

    var mappingMethodName = nameof(IMapFrom<object, object>.Mapping);

    bool HasInterface(Type t) => t.IsGenericType && t.GetGenericTypeDefinition() == mapFromType;

    var types = assembly.GetExportedTypes().Where(t => t.GetInterfaces().Any(HasInterface)).ToList();

    var argumentTypes = new Type[] { typeof(Profile) };

    foreach (var type in types)
    {
      var instance = Activator.CreateInstance(type);

      var methodInfo = type.GetMethod(mappingMethodName);

      if (methodInfo != null)
      {
        methodInfo.Invoke(instance, new object[] { this });
      }
      else
      {
        var interfaces = type.GetInterfaces().Where(HasInterface).ToList();

        if (interfaces.Count > 0)
        {
          foreach (var @interface in interfaces)
          {
            var interfaceMethodInfo = @interface.GetMethod(mappingMethodName, argumentTypes);

            interfaceMethodInfo?.Invoke(instance, new object[] { this });
          }
        }
      }
    }
  }
}


public interface IMapFrom<T>
{
  void Mapping(Profile profile)
  {
    Configure(profile.CreateMap(typeof(T), GetType()));
  }
  void Configure(IMappingExpression mappingExpression);
}


public interface IMapFrom<Source, Destination>
{
  void Mapping(Profile profile)
  {
    Configure(profile.CreateMap<Source, Destination>());
  }
  void Configure(IMappingExpression<Source, Destination> mappingExpression);
}


public interface IMapTo<T>
{
  void Mapping(Profile profile)
  {
    Configure(profile.CreateMap(GetType(), typeof(T)));
  }

  void Configure(IMappingExpression mappingExpression);
}


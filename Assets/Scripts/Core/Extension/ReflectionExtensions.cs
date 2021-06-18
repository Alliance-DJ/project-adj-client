using System;
using System.Collections.Generic;
using System.Reflection;

public static class ReflectionExtensions
{
    private static readonly Dictionary<Type, Dictionary<string, NodeTypeInfo>> cache = new Dictionary<Type, Dictionary<string, NodeTypeInfo>>();

    public static NodeTypeInfo GetNodeTypeInfo(this Type type, string name)
    {
        if (cache.TryGetValueDicDic(type, name, out var info))
        {
            return info;
        }

        var reflectionProperty = type.GetPublicProperty(name);
        if (reflectionProperty != null)
        {
            info = new PropertyNode { Type = reflectionProperty.PropertyType, Property = reflectionProperty };
            cache.AddDicDic(type, name, info);
            return info;
        }

        var reflectionField = type.GetPublicField(name);
        if (reflectionField != null)
        {
            info = new FieldNode { Type = reflectionField.FieldType, Field = reflectionField };
            cache.AddDicDic(type, name, info);
            return info;
        }

        return null;
    }
}

public abstract class NodeTypeInfo
{
    public Type Type { get; set; }

    public abstract object GetValue(object parentObject);

    public virtual void SetValue(object parentObject, object value)
    {
        throw new InvalidOperationException($"Data node of type '{Type}' is read-only.");
    }
}

public class FieldNode : NodeTypeInfo
{
    public FieldInfo Field { private get; set; }

    public override object GetValue(object parentObject)
    {
        if (parentObject == null)
        {
            return Type.IsValueType ? Activator.CreateInstance(Type) : null;
        }

        if (Field != null)
        {
            return Field.GetValue(parentObject);
        }

        return null;
    }

    public override void SetValue(object parentObject, object value)
    {
        if (parentObject == null || Field == null) return;

        Field.SetValue(parentObject, value);
    }
}

public class PropertyNode : NodeTypeInfo
{
    public PropertyInfo Property { private get; set; }

    public override object GetValue(object parentObject)
    {
        if (parentObject == null)
        {
            return Type.IsValueType ? Activator.CreateInstance(Type) : null;
        }

        if (Property != null)
        {
            return Property.GetValue(parentObject, null);
        }

        return null;
    }

    public override void SetValue(object parentObject, object value)
    {
        if (parentObject == null || Property == null) return;

        if (Property.CanWrite)
        {
            Property.SetValue(parentObject, value, null);
        }
        else
        {
            throw new InvalidOperationException($"Property '{Property.Name}' is read-only.");
        }
    }
}

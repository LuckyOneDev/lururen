using Lururen.Client.EntityComponentSystem;
using Lururen.Client.EntityComponentSystem.Generic;
using System.Reflection;

namespace Lururen.Client.Base
{
    public class ComponentBuilder
    {
        public Type TargetType { get; set; }
        public List<(PropertyInfo field, object? value)>? FieldValues { get; set; } = default;
        public Component Build(Entity entity)
        {
            var target = Activator.CreateInstance(TargetType, entity) as Component;
            FieldValues?.ForEach(fieldSetter =>
            {
                fieldSetter.field.SetValue(target, fieldSetter.value);
            });
            return target!;
        }
    }

    public class Prefab
    {
        public List<ComponentBuilder> Components { get; set; } = new();

        public static Prefab FromEntity(Entity ent)
        {
            Prefab pref = new();
            ent.Components.ForEach(component =>
            {
                var compBuilder = new ComponentBuilder();
                compBuilder.TargetType = component.GetType();
                compBuilder.FieldValues = compBuilder.TargetType.GetProperties().Select(field =>
                {
                    return (field, field.GetValue(component));
                }).ToList();
                pref.Components.Add(compBuilder);
            });
            return pref;
        }
    }
}
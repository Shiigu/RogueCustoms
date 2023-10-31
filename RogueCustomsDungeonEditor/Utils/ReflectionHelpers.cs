using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RogueCustomsDungeonEditor.Utils
{
    public static class ReflectionHelpers
    {
        public static IEnumerable<TDestination> Convert<TSource, TDestination>(this IEnumerable<TSource> sourceList)
        where TSource : class, new()
        where TDestination : class, new()
        {
            return sourceList.Select(item => item.Convert<TSource, TDestination>());
        }

        public static TDestination Convert<TSource, TDestination>(this TSource source)
            where TSource : class, new()
            where TDestination : class, new()
        {
            var target = new TDestination();
            CopyProperties(source, target);
            return target;
        }

        private static void CopyProperties<TSource, TDestination>(TSource source, TDestination target)
        {
            Type sourceType = source.GetType();
            Type targetType = target.GetType();

            PropertyInfo[] sourceProperties = sourceType.GetProperties();
            PropertyInfo[] targetProperties = targetType.GetProperties();

            foreach (var sourceProperty in sourceProperties)
            {
                try
                {
                    var targetProperty = Array.Find(targetProperties, p => p.Name == sourceProperty.Name);

                    if (targetProperty != null)
                    {
                        if (targetProperty.PropertyType == sourceProperty.PropertyType && targetProperty.GetSetMethod() != null)
                        {
                            // Try to copy properties with the same name, type, and a set method
                            object value = sourceProperty.GetValue(source, null);
                            targetProperty.SetValue(target, value, null);
                        }
                        else if (targetProperty.PropertyType != sourceProperty.PropertyType)
                        {
                            if ((sourceProperty.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(sourceProperty.PropertyType))
                            && (targetProperty.PropertyType != typeof(string) && typeof(IEnumerable).IsAssignableFrom(targetProperty.PropertyType)))
                            {

                            }
                            else
                            {
                                // Handle copying inner properties with different type names but matching values
                                object sourceValue = sourceProperty.GetValue(source, null);
                                object targetValue = targetProperty.GetValue(target, null);

                                if (sourceValue != null && targetValue == null)
                                {
                                    // Create a new instance of the target property's type
                                    targetValue = Activator.CreateInstance(targetProperty.PropertyType);
                                    targetProperty.SetValue(target, targetValue, null);
                                }

                                if (sourceValue != null)
                                {
                                    // Recursively copy inner properties
                                    CopyProperties(sourceValue, targetValue);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    // Continue. Copies can be expected to fail.
                }
            }
        }
    }
}

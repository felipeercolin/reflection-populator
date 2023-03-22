using System.Collections;
using System.Reflection;

namespace ConsoleReflectionPopulator.Acao
{
    public static class ReflectionPopulator
    {
        public static T? CreateObject<T>(object obj)
        {
            if (obj == null) return default;

            var type = obj.GetType();
            var properties = type.GetProperties();
            var item = (T)Activator.CreateInstance(typeof(T), true);
            var typeT = item.GetType();

            foreach (var property in properties)
            {
                try
                {
                    var pt = property.PropertyType;

                    var prop = type.GetProperty(property.Name);
                    var proT = typeT.GetProperty(property.Name);

                    if (prop == null || proT == null) continue;

                    var valueStr = prop.GetValue(obj, null);

                    if (valueStr == null) continue;

                    var convertTo = Nullable.GetUnderlyingType(proT.PropertyType) ?? proT.PropertyType;

                    if (pt.IsClass && pt != typeof(string) && !IsGenericType(pt, typeof(ICollection<>)) &&
                        !IsGenericType(pt, typeof(IList<>)) && !IsGenericType(pt, typeof(IEnumerable<>)))
                        proT.SetValue(item, CreateObjectWithElementType(valueStr, proT.PropertyType), null);
                    else if (IsGenericType(pt, typeof(ICollection<>)) || IsGenericType(pt, typeof(IList<>)))
                        proT.SetValue(item, PopulateCollection((IEnumerable)valueStr, proT.PropertyType), null);
                    else if (IsGenericType(pt, typeof(IEnumerable<>)) && pt != typeof(string))
                        proT.SetValue(item, PopulateEnumerable((IEnumerable)valueStr, proT.PropertyType), null);
                    else proT.SetValue(item, Convert.ChangeType(valueStr, proT.PropertyType), null);
                }
                catch (Exception erro)
                {

                }
            }
            return item;
        }

        private static bool IsGenericType(Type type, Type genericType)
            => (type.IsGenericType && type.GetGenericTypeDefinition() == genericType) ||
           (type.IsConstructedGenericType && type.GetGenericTypeDefinition() == genericType) ||
           type.GetInterfaces().Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == genericType);

        private static object PopulateCollection(IEnumerable source, Type targetType)
        {
            Type elementType = targetType.GetGenericArguments()[0];
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var item in source)
                list.Add(CreateObjectWithElementType(item, elementType));

            return list;
        }

        private static object PopulateEnumerable(IEnumerable source, Type targetType)
        {
            Type elementType = targetType.GetGenericArguments()[0];
            IList list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType));

            foreach (var item in source)
                list.Add(CreateObjectWithElementType(item, elementType));

            return list.Cast<object>().AsEnumerable();
        }

        private static object CreateObjectWithElementType(object source, Type targetType)
        {
            var method = typeof(ReflectionPopulator).GetMethod("CreateObject", BindingFlags.Static | BindingFlags.Public);
            var genericMethod = method.MakeGenericMethod(targetType);
            return genericMethod.Invoke(null, new[] { source });
        }
    }
}
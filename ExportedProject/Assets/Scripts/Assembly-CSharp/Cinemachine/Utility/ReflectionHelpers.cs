using System;
using System.Linq.Expressions;
using System.Reflection;

namespace Cinemachine.Utility
{
	[DocumentationSorting(0f, DocumentationSortingAttribute.Level.Undoc)]
	public static class ReflectionHelpers
	{
		public static void CopyFields(object src, object dst, BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
		{
		}

		public static T AccessInternalField<T>(this Type type, object obj, string memberName)
		{
			return default;
		}

		public static object GetParentObject(string path, object obj)
		{
			return null;
		}

		public static string GetFieldPath<TType, TValue>(Expression<Func<TType, TValue>> expr)
		{
			return null;
		}
	}
}

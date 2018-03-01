using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web;

namespace Trawick.Rest.Extensions
{
	public static class ModelExtensions
	{

		//// If input string is empty it does not update
		//public static bool TryUpdateProperty<TSource>(this TSource source, Expression<Func<TSource, string>> selector, string input)
		//{
		//	if (!string.IsNullOrEmpty(input))
		//	{
		//		var expr = (MemberExpression)selector.Body;
		//		var prop = (PropertyInfo)expr.Member;
		//		prop.SetValue(source, input, null);
		//		return true;
		//	}
		//	return false;
		//}
		//// if (member.TryUpdateProperty(x => x.address1, "new address")) { }

		public static bool TryUpdateProperty<TSource, TKey>(this TSource source, Expression<Func<TSource, TKey>> selector, TKey input)
		{
			//Convert.ToString(input)
			if (!input.IsDefaultValue())
			{
				var expr = (MemberExpression)selector.Body;
				var prop = (PropertyInfo)expr.Member;
				prop.SetValue(source, input, null);
				return true;
			}
			return false;
		}


		private static bool IsDefaultValue(this object param)
		{
			return param == param.GetType().Default();
		}

	}


	public static class TypeExtensions
	{
		public static object Default(this Type type)
		{
			if (type.GetTypeInfo().IsValueType)
			{
				return Activator.CreateInstance(type);
			}
			return null;
		}
	}
}

// https://stackoverflow.com/questions/11178864/pass-property-itself-to-function-as-parameter-in-c-sharp
// https://www.codeproject.com/Articles/1079028/Build-Lambda-Expressions-Dynamically


//private void TryUpdateProperty<T>(string input, T target, Expression<Func<T, string>> outExpr)
//{
//	if (!string.IsNullOrEmpty(input))
//	{
//		var expr = (MemberExpression)outExpr.Body;
//		var prop = (PropertyInfo)expr.Member;
//		prop.SetValue(target, input, null);
//	}
//}






//public static IEnumerable<TReturn> GetAll<TReturn>(Expression<Func<TEntity, TReturn>> selectExp, string orderColumnName, bool descending, params Expression<Func<TEntity, object>>[] includeExps)
//{
//	var entityType = typeof(TEntity);
//	var prop = entityType.GetProperty(orderColumnName);
//	var param = Expression.Parameter(entityType, "i");
//	var orderExp = Expression.Lambda(Expression.MakeMemberAccess(param, prop), param);

//	// get the original GetAll method overload
//	var method = this.GetType().GetMethods().Where(m => m.Name == "GetAll" && m.GetGenericArguments().Length == 2);
//	var actualMethod = method.First().MakeGenericMethod(typeof(TReturn), prop.PropertyType);
//	return (IEnumerable<TReturn>)actualMethod.Invoke(this, new object[] { selectExp, orderExp, descending, includeExps });
//}
//// _service.GetAll(i => new { i.Name, i.Email }, sortColumn, sortDescending, null);
//// https://stackoverflow.com/questions/30768271/resolving-generic-lambda-expression-type

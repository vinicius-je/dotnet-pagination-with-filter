using StockApi.Domain.Entities.Commons;
using StockApi.Domain.Interfaces.Queries;
using System.Linq.Expressions;
using System.Reflection;

namespace StockApi.Infrastructure.Repositories.Commons
{
    public class BaseFilterQuery<TBaseEntity> :
        IFilterQuery<TBaseEntity>
        where TBaseEntity : BaseEntity
    {
        public IQueryable<TBaseEntity> Filter(IQueryable<TBaseEntity> query, Dictionary<string, string> filters)
        {
            foreach (var filter in filters)
            {
                // Product Property
                var property = typeof(TBaseEntity).GetProperty(filter.Key);

                if (property == null || filter.Value == null)
                {
                    // Go to next filter in Dictionary
                    continue;
                }

                // Lambda parameter
                var parameter = Expression.Parameter(typeof(TBaseEntity), "x");
                // Lamda pamaeter + property (x.Property)
                var propertyAccess = Expression.Property(parameter, property);
                // Convert Type Value for the Type in used in Entity
                object convertedValue = ConvertTypeValue(filter, property);
                // Constant Expression
                var constant = Expression.Constant(convertedValue);
                // Define condition filter in query
                Expression condition = SetQueryConditionFilter(property, propertyAccess, constant);
                // Creates the full lamda expression, ex: x => x.Property.Contains(value)
                var lambda = Expression.Lambda<Func<TBaseEntity, bool>>(condition, parameter);
                // Set the lambda in the Where condition
                query = query.Where(lambda);
            }

            return query;
        }

        private static Expression SetQueryConditionFilter(PropertyInfo property, MemberExpression propertyAccess, ConstantExpression constant)
        {
            Expression condition;

            if (property.PropertyType == typeof(string))
            {
                // Set the Contains string method in the lamda expression
                var containsMethod = typeof(string).GetMethod("Contains", new[] { typeof(string) })!;
                // Ex: x.Property.Contains(value)
                condition = Expression.Call(propertyAccess, containsMethod, constant);
            }
            else
            {
                // Set the Equals method in the lamda expression
                condition = Expression.Equal(propertyAccess, constant);
            }

            return condition;
        }

        private static object ConvertTypeValue(KeyValuePair<string, string> filter, PropertyInfo property)
        {
            object? convertedValue = null;

            if (property.PropertyType.IsEnum)
            {
                convertedValue = Enum.Parse(property.PropertyType, filter.Value, true);
            }
            else
            {
                convertedValue = Convert.ChangeType(filter.Value, property.PropertyType);
            }

            return convertedValue;
        }
    }
}

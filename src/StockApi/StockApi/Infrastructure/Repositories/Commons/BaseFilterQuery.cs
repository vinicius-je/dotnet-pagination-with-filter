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
        /// <summary>
        /// Set the Filter Expression of the Query
        /// </summary>
        /// <param name="query">The base query to be executed.</param>
        /// <param name="filters">An object containing the filtering criteria for the query.</param>
        /// <returns>Returns a IQueryable of Entity to execute query.</returns>
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

        /// <summary>
        /// Set the query condition based on property type.
        /// </summary>
        /// <param name="property">Entity property type.</param>
        /// <param name="propertyAccess">Entity property.</param>
        /// <param name="constant">Constante expression</param>
        /// <returns>Expression configured to condition based on property type.</returns>
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
            else if (property.PropertyType == typeof(DateTimeOffset))
            {
                DateTimeOffset startDate = DateTimeOffset.Parse(constant.Value!.ToString()!).Date;
                DateTimeOffset endDate = startDate.AddDays(1);

                // Set DateTime query based on interval of time
                condition = Expression.AndAlso(
                    Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(startDate)),
                    Expression.LessThan(propertyAccess, Expression.Constant(endDate))
                );
            }
            else
            {
                // Set the Equals method in the lamda expression
                condition = Expression.Equal(propertyAccess, constant);
            }

            return condition;
        }

        /// <summary>
        /// Convert value type based on entity property type.
        /// </summary>
        /// <param name="filters">An object containing the filtering criteria for the query.</param>
        /// <param name="property">PropertyInfo of entity.</param>
        /// <returns>Object type of entity property.</returns>
        private static object ConvertTypeValue(KeyValuePair<string, string> filter, PropertyInfo property)
        {
            if (property.PropertyType.IsEnum)
            {
                return Enum.Parse(property.PropertyType, filter.Value, true);
            }

            if (property.PropertyType == typeof(DateTimeOffset))
            {
                return DateTimeOffset.Parse(filter.Value);
            }

            if (property.PropertyType == typeof(Guid))
            {
                return Guid.Parse(filter.Value);
            }

            return Convert.ChangeType(filter.Value, property.PropertyType);
        }
    }
}

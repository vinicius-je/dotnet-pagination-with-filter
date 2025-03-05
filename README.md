
<h1 align="center">
Pagination with Filter
</h1>

## Details:
 - Simple Pagination
 - Pagination With Filter
 - Multi Property filter
 - Custom Page Size
 - Limit of 250 Records Per Page

## Query example:

`https://localhost:51026/Product?Name=phone&pageNumber=1&pageSize=25`

## Response example:

```json
{
  "statusCode": 200,
  "message": "Success",
  "data": {
    "pageNumber": 1,
    "pageSize": 25,
    "totalRecords": 2,
    "totalPages": 1,
    "hasNextPage": false,
    "hasPreviousPage": false,
    "data": [
      {
        "name": "iPhone 15 Pro",
        "description": "Smartphone Apple com chip A17 Pro",
        "price": 8999.99,
        "category": "Electronics",
        "quantity": 30,
        "id": "9b83dc5b-d71d-4ce2-998e-aa134dba2948"
      },
      {
        "name": "Smartphone Samsung Galaxy S23",
        "description": "Celular Android com câmera de 50MP",
        "price": 5999.9,
        "category": "Electronics",
        "quantity": 25,
        "id": "7845ab05-f649-4348-b9f9-e266233e0537"
      }
    ]
  }
}
```

## Web Api Response

This class represents a generic API response object. It is used to standardize the structure of responses returned by the API, encapsulating the status code, message, and optional data.

### Properties

- **`StatusCode`** (`int`):  
  The HTTP status code of the response. Typically, it will be a value like `200` for success or `400` for client error.

- **`Message`** (`string`):  
  A string message providing additional information about the response. This could be an error message, success message, or other relevant information.

- **`Data`** (`T?`):  
  The data returned by the API in the response. This is a generic type `T`, allowing the response to return any type of data. The property is nullable (`T?`) and will be omitted from the serialized JSON when it is `null` (using the `[JsonIgnore]` attribute).


```csharp
using System.Text.Json.Serialization;

namespace StockApi.Response
{
    public class ApiResponse<T>
    {
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T? Data { get; set; }

        public ApiResponse(int statusCode, string message)
        {
            StatusCode = statusCode;
            Message = message;
        }

        public ApiResponse(int statusCode, string message, T? data)
        {
            StatusCode = statusCode;
            Message = message;
            Data = data;
        }
    }
}
```

## Pagination Response Object

This class represents a generic API response for pagination data. It is used to standardize the structure of responses returned by the API, encapsulating the following details:

- **`pageNumber`** (*int*): The current page number.
- **`pageSize`** (*int*): The number of records per page.
- **`totalRecords`** (*int*): The total number of records in the database.
- **`totalPages`** (*int*): The total number of pages available.
- **`hasNextPage`** (*boolean*): Indicates whether there is a next page.
- **`hasPreviousPage`** (*boolean*): Indicates whether there is a previous page.
- **`data`** (*List*): The list of records for the current page.

```csharp
namespace StockApi.Response
{
    public class PaginationResponse<T>
    {
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
        public int TotalRecords { get; init; }
        public int TotalPages { get; init; }
        public bool HasNextPage => PageNumber < TotalPages;
        public bool HasPreviousPage => PageNumber > 1;
        public List<T> Data { get; init; }

        public PaginationResponse(List<T> data, int pageNumber, int pageSize, int totalRecords)
        {
            Data = data;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalRecords = totalRecords;
            TotalPages = (int)Math.Ceiling((decimal)totalRecords / (decimal)pageSize);
        }
    }
}
```

## Filter Query

This class receives a query of the base entity type and a dictionary containing property names as keys and their corresponding values as query parameters.

For each item in the dictionary, the class extracts the corresponding property from the entity, determines the actual value type, and creates a lambda expression. The lambda expression is constructed using the property, value, and a parameter, and is then returned as a WHERE condition for the query

```csharp
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
```

## Pagination Query

This class accepts the page number, page size, and a dictionary with the filter condition as optional parameters.

If the dictionary contains data, the class executes the filtering logic using the previous functions.

Then, based on the result of the conditional logic (optional), the total number of records is calculated. After that, the data is retrieved from the database, taking into account how many rows should be skipped and how many records need to be retrieved. The result is returned as a Pagination Response.

```csharp
namespace StockApi.Infrastructure.Repositories.Commons
{
    public class BasePaginationQuery<TBaseEntity, TDto> :
        BaseFilterQuery<TBaseEntity>,
        IPaginationQuery<TDto>,
        IFilterQuery<TBaseEntity>
        where TBaseEntity : BaseEntity
        where TDto : BaseDto, new()

    {
        protected readonly AppDbContext _context;
        protected readonly DbSet<TBaseEntity> _dbSet;

        public BasePaginationQuery(AppDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<TBaseEntity>();
        }

        public async Task<PaginationResponse<TDto>> Pagination(int pageNumber, int pageSize, Dictionary<string, string>? filters)
        {
            var query = _dbSet.AsQueryable();

            if (filters is not null)
            {
                query = this.Filter(query, filters);
            }

            var totalRecords = await query.CountAsync();

            var products = await query
                .AsNoTracking()
                .Select(MapToProductDto())
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return new PaginationResponse<TDto>(products, pageNumber, pageSize, totalRecords);
        }

        private Expression<Func<TBaseEntity, TDto>> MapToProductDto()
        {
            return x => (TDto)new TDto().ConvertToDto(x);
        }
    }
}
```

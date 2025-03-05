
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

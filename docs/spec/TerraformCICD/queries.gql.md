# Queries

The [query type](http://spec.graphql.org/June2018/#sec-Type-System) defines GraphQL operations that retrieve data from the server.

Each of the following queries will have filtering, sorting, and pagination in accordance with the documented queries in the
[fetch records](#fetch-records) section.
 
 For example:

 ```graphql
 <Typename>(        
    """
    Filter the records.
    """
    filter: <Typename>FilterInput,

    """
    Sort the records.
    """
    sort: [<Typename>SortInput!],

    """
    Returns the first _n_ elements from the list.
    """
    first: Int,

    """
    Returns the elements in the list that come after the specified cursor.
    """
    after: String,

    """
    Returns the elements in the list that come before the specified cursor.
    """
    before: String,

    """
    Returns the last _n_ elements from the list.
    """
    last: Int
): <Typename>Connection!
```

## Fetch records

These are queries for retrieving various records in the Terraform CICD.

> The parameters (params) have been omitted for brevity, please see the `<Typename>` query example in the [queries](#queries) section
> for an example of what the params will look like.

```graphql
extend type Query {
    # Add your queries here.
}
```

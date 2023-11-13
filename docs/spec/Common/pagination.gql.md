# Pagination

For the Terraform CICD service we are using cursor-based pagination (a.k.a keyset pagination) rather than
offset pagination for the reason that it performs better for large datasets.

More information on cursor-based sorting can be found here: [Cursor Pagination](https://jsonapi.org/profiles/ethanresnick/cursor-pagination/) as well as this GraphQL relay spec: [connections](https://relay.dev/graphql/connections.htm).

**Note:** For pagination, this will require the database to have a secondary sort on the `key` field. This helps when sorting by a non-unique field.

## `PageInfo` Object

```graphql
"""
Information about pagination in a connection.
"""
type PageInfo {
    """
    When paginating forwards, the cursor to continue.
    """
    endCursor: String

    """
    When paginating forwards, are there more items?
    """
    hasNextPage: Boolean!

    """
    When paginating backwards, are there more items?
    """
    hasPreviousPage: Boolean!

    """
    When paginating backwards, the cursor to continue.
    """
    startCursor: String
}
```

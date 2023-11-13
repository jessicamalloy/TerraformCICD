# Connections

This follows the pagination best practices in GraphQL. For information regarding _connections_ please refer to this link: [connections](https://relay.dev/graphql/connections.htm)

## Example of the _connection_ types

> `<Typename>` would be replaced by the type name as referenced in the [`entities`](entities.gql.md) file.

```graphql
"""
The connection type for <Typename>.
"""
type <Typename>Connection {
    """
    A list of edges.
    """
    edges: [<Typename>Edge]

    """
    Contains pagination information.
    """
    pageInfo: PageInfo!

    """
    The total number of nodes returned.
    """
    totalCount: Int!
}
```

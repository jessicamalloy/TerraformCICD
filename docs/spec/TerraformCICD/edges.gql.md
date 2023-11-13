# Edges

This follows the pagination best practices in GraphQL. For information regarding _edges_ please refer to this link: [Edge Types](https://relay.dev/graphql/connections.htm#sec-Edge-Types)

## Example of the _edge_ types

> `<Typename>` would be replaced by the type name as referenced in the [`entities`](entities.gql.md) file.

```graphql
type <Typename>Edge {
    """
    Contains information about the _actual_ `<Typename>`.
    """
    node: <Typename>

    """
    Represents the position of the `<Typename>s` inside the list for use in pagination.
    """
    cursor: String!
}
```

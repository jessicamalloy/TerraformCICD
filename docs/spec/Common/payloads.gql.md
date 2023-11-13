# Payloads

## `DeletePayload` type

```graphql
"""
Describes the delete payload response type.
"""
type DeletePayload {
    """
    The ID of the object to delete.
    """
    id: UUID!

    """
    A message about the deletion of the object
    """
    message: String
}
```

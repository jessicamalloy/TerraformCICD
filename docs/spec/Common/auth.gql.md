# Authorization & Authentication

For authorization & authentication we will be initially using Microsoft AAD. Keeping in mind that we
may, at some point, be using a federated service for authenticating across several auth providers.

## Additional Resources

1. [HotChocolate Authorization](https://chillicream.com/docs/hotchocolate/security/authorization)

## `AuthRoles`

|Role|Description|
|---|---|
|Administrator|PowerUser + the deletion of **Add your description here**|
|PowerUser|ReadWrite + create **Add your description here**|
|ReadWrite|ReadOnly + create and update **Add your description here**|
|ReadOnly|Read only access **Add your description here**.|

## `AuthPolicy` ⟶ `AuthRole` Mapping

|Policy|Roles|
|---|---|
|Read|Administrator, PowerUser, ReadWrite, ReadOnly|
|ReadWrite|Administrator, PowerUser, ReadWrite|
|Admin|Administrator, PowerUser|

## `AuthPolicy` Enum

This enum comes default in `HotChocolate`.

Noting it here as something to reference when looking at the `@authorize` directive
usage in this spec.

```graphql
"""
Enum for determining if the authorization should be resolved before or after
the execution.
"""
enum ApplyPolicy {
    """
    Resolve before.
    """
    BEFORE_RESOLVER

    """
    Resolve after.
    """
    AFTER_RESOLVER
}
```

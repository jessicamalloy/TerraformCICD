# Directives

Directives, preceded by an `@`, are a way for describing runtime execution and type validation behavior in GraphQL.
They can be applied to the schema field, enum, object, etc.

## `@entity`

```graphql
"""
Defines if the field is an entity.

Adds the following fields:

* id: UUID! @key
* createdBy: String!
* createdAt: DateTime!
* updatedBy: String
* updatedAt: DateTime
"""
directive @entity on OBJECT
```

## `@unique`

```graphql
"""
Defines if the field is unique.
"""
directive @unique on FIELD_DEFINITION
```

## `@key`

```graphql
"""
Marks the field as being a `key`.
"""
directive @key on FIELD_DEFINITION
```

## `@oneOf`

```graphql
"""
Define that _only_ one of the fields defined in the input can be selected.
"""
directive @oneOf on INPUT_OBJECT | OBJECT
```

## `@optional`

```graphql
"""
Marks the input field as being optional. For more information regarding the
usage of this directive see this link: https://github.com/graphql/graphql-spec/issues/872#issuecomment-851594472
"""
directive @optional on INPUT_FIELD_DEFINITION
```

## `@authorize`

```graphql
"""
This defines the policies that a object or field is authorized and
authenticated against.
"""
directive @authorize(
    """
    The name of the authorization policy that determines access to the annotated resource.
    """
    policy: String

    """
    Roles that are allowed to access the annotated resource.
    """
    roles: [String!]

    """
    Defines when when the resolver shall be executed.

    By default authentication is handled before the resolver is executed.
    """
    apply: ApplyPolicy! = BEFORE_RESOLVER
) repeatable on SCHEMA | OBJECT | FIELD_DEFINITION
```

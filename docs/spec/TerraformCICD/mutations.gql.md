# Mutations

The mutation type defines GraphQL operations that change data on the server, such as
creating, updating, or deleting an object(s). It is analogous to performing `HTTP` verbs
such as `POST`, `PUT`, and `DELETE`.

## Create

These are operations to _create_ records that are defined in the mutation type.

> :point_right: This is equivalent to a RESTful APIs `POST` request.

```graphql
extend type Mutation {
    # Add your create mutations here.
}
```

## Add

These are operations to _add_ (link) records to an existing record.

```graphql
extend type Mutation {
    # Add your add mutations here.
}
```

## Update

These are operations to _update_ records that are defined in the mutation type.

> :point_right: This is equivalent to a RESTful APIs `PUT` request.

```graphql
extend type Mutation {
    # Add your update mutations here.
}
```

## Delete

These are operations to _delete_ records that are defined in the mutation type.

> :point_right: This is equivalent to a RESTful APIs `DELETE` request.

```graphql
extend type Mutation {
    # Add your delete mutations here.
}
```

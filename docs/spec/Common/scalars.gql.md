# Scalars

Scalars are primitive values, such as, `Int`, `Float`, `String`, `Boolean`, or `ID`.

We can also define our own scalars as seen below.

## `UUID` Scalar

```graphql
"""
The `UUID` scalar type represents the universally unique identifier as a string in RFC4122 format.
"""
scalar UUID
```

## `DateTime` Scalar

```graphql
"""
The `DateTime` scalar type represents date and time as a string in RFC3339 format.
For example: "1985-04-12T23:20:50.52Z" represents 20 minutes and 50.52 seconds after the 23rd hour of April 12th, 1985 in UTC.
"""
scalar DateTime
```

## `URL` Scalar

```graphql
"""
The `URL` scalar type represents the uniform resource locator (URL) as a string in RFC1738 format.
"""
scalar URL
```

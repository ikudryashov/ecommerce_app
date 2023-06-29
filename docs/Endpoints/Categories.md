## GET /api/categories
- Returns a list of categories
- Access: public

```json
[
    {
        "id": "29daecd2-986e-d2ae-09e4-4f50b6bf3fcb",
        "name": "Baby",
        "description": "Atque unde rerum distinctio velit quidem iure ipsum."
    },
    {
        "id": "2bbdbf1e-3efe-250e-92e1-3b76cb0e5f46",
        "name": "Grocery",
        "description": "Fuga libero aperiam delectus mollitia sed inventore."
    }
]
```

## GET /api/categories/{name}

- Returns a category with specified name
- Access: public
- Validation: category name should not be null or whitespace
- Returns **BAD REQUEST** if category name validation fails
- Returns **NOT FOUND** if a category with provided name does not exist

```json
{
    "id": "d2d25974-3212-45cc-45f9-9292ce3d1298",
    "name": "Home",
    "description": "Reprehenderit dolore enim sunt iure dolorum."
}
```



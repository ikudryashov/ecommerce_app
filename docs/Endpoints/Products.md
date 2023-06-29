## GET /api/{category}/products/
- Returns a paged list of products in specified category, 1st page with 15 records if not specified otherwise. 
Also returns the total count of records in the result and flags indicating if previous and next pages 
in the paged list exist.
- Accepts optional parameters:
    - **searchTerm**: name of product to search for
    - **sortColumn**: a column the result will be sorted by: name, price or color
    - **sortOrder**: determines how the result will be sorted: "asc" or "desc"
    - **page**: the number of the page in the paged list
    - **pageSize**: how many records will be displayed on each page
- Access: public
- Validation: category name should not be null or whitespace
- Returns **BAD REQUEST** if category name validation fails
- Returns **NOT FOUND** if a category with given name does not exist

```http request
https://localhost:{port}/api/home/products?searchTerm=Gloves&sortColumn=price&sortOrder=asc&pageSize=2&page=1
```

```json
{
    "items": [
        {
            "id": "a1edb552-5dc4-a107-7160-0e0e332b8048",
            "categoryId": "d2d25974-3212-45cc-45f9-9292ce3d1298",
            "name": "Rustic Fresh Gloves",
            "categoryName": "Home",
            "description": "Qui sunt suscipit praesentium inventore a expedita alias.",
            "price": 4.54688544783130498,
            "color": "Blue"
        },
        {
            "id": "6d8e9b54-a0bf-70e7-08d9-15b8090abcee",
            "categoryId": "d2d25974-3212-45cc-45f9-9292ce3d1298",
            "name": "Awesome Plastic Gloves",
            "categoryName": "Home",
            "description": "Consequuntur inventore corrupti omnis perferendis.",
            "price": 27.0641033584550929,
            "color": "White"
        }
    ],
    "page": 1,
    "pageSize": 2,
    "totalCount": 87,
    "nextPageExists": true,
    "prevPageExists": false
}
```

## GET /api/products/{id}
- Returns a product with specified ID
- Access: public
- Validation: product ID should not be null or whitespace
- Returns **BAD REQUEST** if product ID validation fails
- Returns **NOT FOUND** if a product with given ID does not exist

```json
{
    "id": "c43d9900-53f1-9d57-5e90-6f37ab9fb0d4",
    "categoryId": "d2d25974-3212-45cc-45f9-9292ce3d1298",
    "name": "Sleek Plastic Chicken",
    "categoryName": "Home",
    "description": "Ipsum ea et temporibus dolore numquam molestiae fugit.",
    "price": 521.576957132935876,
    "color": "Yellow"
}
```
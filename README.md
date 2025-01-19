# README

## Zo Online Store

### Project Overview

Zo Online Store is a front-end and back-end separated e-commerce website developed using the **ASP.NET Core 8** framework. The project adopts the **MVC architecture** and leverages **Entity Framework Core** for database operations and management. It is under active development and maintenance, aiming to deliver a simple yet efficient online shopping experience.

---

### Features

- **User Management:**
  - User registration, login, and authentication.
  - JWT-based token authentication.
  - Token validation for active sessions.

- **Product Management:**
  - Add, update, and query product categories.
  - Supports dynamic queries using parent-child category relationships.
  - Implements pagination for optimized large data loading.

- **Order and Inventory Management:**
  - Create, query, and manage customer orders.
  - Manage detailed order items for each order.
  - Support inventory tracking and management.

---

### Database Design

The database consists of 7 core tables:

1. **Categories** (Product Categories Table):
   - Supports parent-child category relationships for organizing categories.

2. **Products** (Products Table):
   - Stores information about all the products in the store.

3. **Orders** (Orders Table):
   - Manages user order data.

4. **OrderItems** (Order Items Table):
   - Tracks individual product items within an order.

5. **Inventories** (Inventory Table):
   - Manages stock levels for products.

6. **Users** (Users Table):
   - Stores basic user information such as email, username, and roles.

7. **Tokens** (Tokens Table):
   - Stores JWT tokens and their states.

8. **ProductImages** (Product Image Table):
   - Stores the image paths associated with each product.
   - Fields:
     - ID (Primary Key)
     - ProductID (Foreign Key referencing Products)
     - ImagePath (String: Path to the product image)
     - Caption (Optional: Short description for the image)
     - IsPrimary (Boolean: Indicates if the image is the main image for the product)
     - IsThumbnailImage (Boolean: Indicates if the image is a thumbnail)
---

### Project Structure

- **Shopping Cart**:
  - Contains the main implementation code, including controllers, models, DTOs, and database context.

- **Shopping Cart Test**:
  - Contains unit tests to ensure functionality works as expected.

---

### Technology Stack

- **Backend Framework:**
  - ASP.NET Core 8

- **Database Management:**
  - Entity Framework Core

- **Authentication:**
  - JSON Web Token (JWT)

- **Dependency Injection:**
  - IOptions pattern for dynamic parameter configuration.

---

### LINQ Queries in Use

The project extensively uses **LINQ** queries for efficient database operations. For example:

#### 1. **Paginated Category Search**
```csharp
var query = _context.Categories.AsQueryable();

if (dto.ID.HasValue)
{
    query = query.Where(c => c.ID == dto.ID || c.ParentID == dto.ID);
}

if (!string.IsNullOrEmpty(dto.Name))
{
    query = query.Where(c => c.Name.Contains(dto.Name));
}

int totalRecord = await query.CountAsync();

var result = await query
    .Skip((dto.Page - 1) * dto.PageSize)
    .Take(dto.PageSize)
    .ToListAsync();

return Ok(new
{
    TotalRecord = totalRecord,
    Page = dto.Page,
    PageSize = dto.PageSize,
    Data = result
});
```

---

### API Design

#### User Management API

1. **User Registration**
   - **POST** `/api/users/register`
   - **Description:** Accepts user information (e.g., email, username, password) to register a new user.
   - **Input Example:**
     ```json
     {
       "username": "test_user",
       "email": "test@example.com",
       "password": "password123"
     }
     ```
   - **Output:**
     ```json
     {
       "message": "User registered successfully."
     }
     ```

2. **User Login**
   - **POST** `/api/users/login`
   - **Description:** Users log in with email and password, returning a JWT token.

3. **Token Validation**
   - **POST** `/api/users/auth`
   - **Description:** Validates the user's token.

#### Category Management API

1. **Get Category List**
   - **GET** `/api/category/categories`
   - **Description:** Retrieves all categories, supporting pagination and parent-child relationship queries.
   - **Input Example:**
     ```json
     {
       "id": 1,
       "name": "Electronics",
       "page": 1,
       "pageSize": 10
     }
     ```
   - **Output:**
     ```json
     {
       "totalRecord": 15,
       "page": 1,
       "pageSize": 10,
       "data": [
         {
           "id": 1,
           "name": "Electronics",
           "parentId": null
         },
         {
           "id": 2,
           "name": "Smartphone",
           "parentId": 1
         }
       ]
     }
     ```

2. **Add Category**
   - **POST** `/api/category/addcategory`
   - **Description:** Adds a new category to the database.

#### Product Management API

1. **Get Product List**
   - **GET** `/api/products`
   - Supports filtering by category and pagination.

2. **Add Product**
   - **POST** `/api/products/add`
   - Adds a new product to the store.

---

### Development & Maintenance

This project is actively being developed with plans to include:

- **Payment Management:**
  - Support for order payment and refund functionalities.

- **User Roles:**
  - Role-based access control for administrators and general users.

- **Notification System:**
  - Notify users about order status updates.

---

### Contact Information

- **Project Lead:** Zoe
- **Email:** [email@example.com]

If you need additional details or have suggestions for the project, feel free to reach out!


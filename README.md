<!DOCTYPE html>
<html lang="en">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
    <title>Restaurant Management System - README</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            line-height: 1.6;
            margin: 40px;
            color: #333;
            background-color: #f8f9fa;
        }
        h1, h2, h3 {
            color: #007bff;
        }
        h1 {
            text-align: center;
        }
        pre {
            background: #333;
            color: #fff;
            padding: 10px;
            border-radius: 5px;
            overflow-x: auto;
        }
        code {
            color: #ffc107;
        }
        a {
            color: #007bff;
            text-decoration: none;
        }
        a:hover {
            text-decoration: underline;
        }
        .container {
            max-width: 900px;
            margin: auto;
            padding: 20px;
            background: white;
            border-radius: 12px;
            box-shadow: 0 8px 16px rgba(0, 0, 0, 0.1);
        }
        ul {
            list-style: none;
            padding: 0;
        }
        li {
            margin-bottom: 5px;
        }
    </style>
</head>
<body>

<div class="container">
    <h1>Restaurant Management System</h1>

    <h2>Project Overview</h2>
    <p>The Restaurant Management System is a web application built using ASP.NET Core MVC that allows restaurant owners to manage customers, orders, and menu items efficiently. The system supports CRUD operations, order tracking, and dynamic sorting for improved usability.</p>
    
    <h2>Features</h2>
    <ul>
        <li>Customer Management: Add, edit, delete, and view customer details.</li>
        <li>Order Management: Create, update, delete, and track orders.</li>
        <li>Menu Management: Manage menu items with descriptions and pricing.</li>
        <li>Sorting and Search: Sort tables dynamically and search customers by phone number.</li>
        <li>Modern UI: Fully responsive and styled with Bootstrap.</li>
    </ul>

    <h2>Controllers and Views</h2>
    
    <h3>Customer Controller</h3>
    <ul>
        <li>List.cshtml - Displays all customers</li>
        <li>Details.cshtml - Shows customer details</li>
        <li>AddCustomer.cshtml - Form to add a new customer</li>
        <li>Edit.cshtml - Form to update a customer</li>
        <li>DeleteCustomer.cshtml - Confirms customer deletion</li>
    </ul>

    <h3>Order Controller</h3>
    <ul>
        <li>List.cshtml - Displays all orders</li>
        <li>Details.cshtml - Shows order details</li>
        <li>Add.cshtml - Form to create a new order</li>
        <li>Edit.cshtml - Form to update an order</li>
        <li>Delete.cshtml - Confirms order deletion</li>
    </ul>

    <h3>Menu Controller</h3>
    <ul>
        <li>List.cshtml - Displays all menu items</li>
        <li>Details.cshtml - Shows menu item details</li>
        <li>Add.cshtml - Form to add a new menu item</li>
        <li>Edit.cshtml - Form to update a menu item</li>
        <li>Delete.cshtml - Confirms menu item deletion</li>
    </ul>

    <h2>Technologies Used</h2>
    <ul>
        <li>ASP.NET Core MVC</li>
        <li>Entity Framework Core</li>
        <li>SQL Server</li>
        <li>Bootstrap 5</li>
        <li>JavaScript & jQuery</li>
    </ul>

    <h2>API Endpoints</h2>
    
    <h3>Customer API</h3>
    <ul>
        <li>GET /api/Customers/List</li>
        <li>GET /api/Customers/Find/{id}</li>
        <li>POST /api/Customers/Add</li>
        <li>PUT /api/Customers/Update/{id}</li>
        <li>DELETE /api/Customers/Delete/{id}</li>
        <li>GET /api/Customers/SearchByPhone/{phone}</li>
    </ul>

    <h3>Order API</h3>
    <ul>
        <li>GET /api/Orders/List</li>
        <li>GET /api/Orders/Find/{id}</li>
        <li>POST /api/Orders/Add</li>
        <li>PUT /api/Orders/Update/{id}</li>
        <li>DELETE /api/Orders/Delete/{id}</li>
    </ul>

    <h3>Menu API</h3>
    <ul>
        <li>GET /api/MenuItems/List</li>
        <li>GET /api/MenuItems/Find/{id}</li>
        <li>POST /api/MenuItems/Add</li>
        <li>PUT /api/MenuItems/Update/{id}</li>
        <li>DELETE /api/MenuItems/Delete/{id}</li>
    </ul>

    <h2>Project Setup</h2>

    <h3>Prerequisites</h3>
    <ul>
        <li>.NET 6 SDK</li>
        <li>SQL Server</li>
        <li>Visual Studio 2022</li>
        <li>Entity Framework Core</li>
    </ul>

</div>

</body>
</html>

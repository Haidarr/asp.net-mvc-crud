# MyProject - Asp.Net MVC CRUD
A basic web application written in C# and VueJS. 

Its main feature is to create users with different roles and assign each user a project/task.

Admin role can Add/Delete/Modify users and also Add/Delete/Modify tasks and assign them to users. Staff role is to edit profile, view their tasks. 


## Getting Started

Clone the repository:
```
https://github.com/Haidarr/asp.net-mvc-crud.git
```

Open the solution in Visual Studio 2019.


## Prepare your DB
- Open pgAdmin (PostgreSQL). 
- Create your database and name it 'myproject'.
- Set your database username and password as mentioned in `db/myproject-tables.sql`.
- Copy all the myproject-tables from `db/myproject-tables.sql` and run it in pgAdmin. 


## Use MyProject
The predefined  username and password are as follows:
- username: admin
- password: 123456

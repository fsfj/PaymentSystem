# PaymentSystem
This is a sample project for payment system

I use JWTAuthentication for the authentication and to get the token.
I made a UseInMemoryDatabase to store the data.
I created CustomExceptionMiddleware class to perform try catch when running API so that i dont need to actually right try catch in every API.
I created an Encryptor class just to encrypt the authentication password when comparing to the database.
As for unit testing, I create xUnit test to test the API if its authorized\unauthorized and check the ouput of the Interface if the output is correct.


Tools Needed.
* Visual Studio 2019
* Postman (to test the project) if you don't have it installed you can download here : https://app.getpostman.com/app/download/win64

1) get the repository here : https://github.com/errolian22/PaymentSystem.git
2) open the repository in visual studio 2019
3) run the project.

I made a UseInMemoryDatabase to store the data.
the sample username and password is

User 1
username : usertest1
password : password1

User 2
username : usertest2
password : password2

To test the api it should be in postman, I created a basic authentication with JWTAuthentication to return a token.

to authenticate paste the link https://localhost/api/account/authenticate (the localhost is base on your workstation)
set the  HTTP verbs to POST, then add the Headers
Key : Content-Type
Value : application/json

then go to Body then input { "username": "usertest1", "password":"password1"} or { "username": "usertest2", "password":"password2" } depends on what user you wanted to use.
then click Send button, in the response  there should be a Token. copy the formatted text token.

example token : eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InVzZXJ0ZXN0MiIsIm5iZiI6MTYxMTc1NjgxOCwiZXhwIjoxNjExNzYwNDE4LCJpYXQiOjE2MTE3NTY4MTh9.h4NVUi4LuMLRrYAm2r7i-qynd0ypa5B9J6VqPcoessk

Go to the https://localhost/api/payments/GetPayments  set the  HTTP verbs to GET then add the Headers
Example.
Key : Authorization
Value : Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJ1bmlxdWVfbmFtZSI6InVzZXJ0ZXN0MiIsIm5iZiI6MTYxMTc1NjgxOCwiZXhwIjoxNjExNzYwNDE4LCJpYXQiOjE2MTE3NTY4MTh9.h4NVUi4LuMLRrYAm2r7i-qynd0ypa5B9J6VqPcoessk

then click Send there should be a JSON output in the body.






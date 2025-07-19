## Burbder Dinner API

Table Content (up to date)

- [Bubder Dinner API](#burbder-dinner-api)
    - [Auth](#auth)
        - [Register](#register)
            - [Register Resquest](#register-request)
            - [Register Response](#register-response)
        - [Login](#login)
            - [Login Resquest](#login-request)
            - [Login Response](#login-response)

## Auth
## Register
```js
POST {{host}}/auth/register
```

```json
{
  "firstName": "Amichai",
  "LastName": "Mantinband", 
  "email": "amichai@mantinband.com",
  "passowrd": "454546541f"
}
```
## Register Response 

```js
200 OK
```
```json
{
  "id": "1917ba0-a4b5-793b-a915-1caeceb5843e",
  "firstName": "Amichai",
  "LastName": "Mantinband", 
  "email": "amichai@mantinband.com"
}
```
## Login

```js
Login Response

```

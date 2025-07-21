## Burbder Dinner API

Table Content (up to date)

- [Bubder Dinner API](#burbder-dinner-api)
    - [Auth](#auth)
        - [Register](#register)
            - [Register Resquest](#register-request)
            - [Register Response](#register-response)
        - [Login](#login)****
            - [Login Resquest](#login-request)
            - [Login Response](#login-response)

## Auth

### Register

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

### Register Response

```js
200
OK
```

```json
{
  "id": "1917ba0-a4b5-793b-a915-1caeceb5843e",
  "firstName": "Amichai",
  "LastName": "Mantinband",
  "email": "amichai@mantinband.com"
}
```

#### Hedears
```c#
Response.Headers["Authorization"] = $"Bearer eyahqyd....";
Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions{})
```

### Login

```js
POST {{host}}/auth/login
```

### Login Resquest

```json
{
  "email": "amichai@mantinband.com",
  "password": "cptopass"
}

```
### Login Response

```json
{
  "id": "1917ba0-a4b5-793b-a915-1caeceb5843e",
  "firstName": "edy",
  "LastName": "lopes",
  "email": "amichai@mantinband.com"
}

```
#### Hedears
```c#
Response.Headers["Authorization"] = $"Bearer eyahqyd...."
Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions{})
```



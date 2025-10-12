## Burbder Dinner API

-   [Bubder Dinner API](#burbder-dinner-api)
    -   [Auth](#auth)
        -   [Register](#register)
            -   [Register Resquest](#register-request)
            -   [Register Response](#registe r-response)
        -   [Login](#login)
            -   [Login Resquest](#login-request)
            -   [Login Response](#login-response)

## Auth

### Register

```js
POST {{host}}/auth/register
```

```json
{
    "firstName": "Mauro",
    "LastName": "Mantinband",
    "email": "mauto@mantinband.com",
    "passowrd": "454546541f"
}
```

### Register Response

```js
OK 200;
```

```json
{
    "id": "1917ba0-a4b5-793b-a915-1caeceb5843e",
    "firstName": "Mauro",
    "LastName": "Mantinband",
    "email": "mauro@mantinband.com"
    "Roles": ["User"]
}
```

### Hedears

```c#
Response.Headers["Authorization"] = $"Bearer eyahqyd....";
Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions{})
```

### Log

```js
POST {{host}}/auth/login
```

## Login Resquest

```json
{
    "email": "amichai@mantinband.com",
    "password": "cptopass"
}
```

## Login Response

```json
{
    "id": "1917ba0-a4b5-793b-a915-1caeceb5843e",
    "firstName": "edy",
    "LastName": "lopes",
    "email": "ednei@hotmail.com",
    "Roles": ["User"]
}
```

### Hedears

```cs
Response.Headers["Authorization"]
 Response.Cookies.Append("refreshToken", authResult.RefreshToken, new CookieOptions{})
```

🌐 2. **Middleware de Tratamento Global de Exceções**

> Middleware intercepta erros e retorna respostas amigáveis e padronizadas à API

**Exceções específicas de domínio**, como:

-   `BusinessRuleValidationException`
-   `RefreshTokenLimitExceededException`
-   `RefreshTokenRequiredException`
    ```cs
    public class BusinessRuleValidationException : DomainException
    {
        public BusinessRuleValidationException(string message)
            : base(message) { }
    }
    ```
    ## **Fluxo execução Pipeline Behaviors MediatR**

| Etapa | Componente              | Responsabilidade                                           |
| ----- | ----------------------- | ---------------------------------------------------------- |
| 1️⃣    | **LoggingBehavior**     | Marca início e fim da requisição (tempo, sucesso, exceção) |
| 2️⃣    | **ValidationBehavior**  | Valida o comando (FluentValidation) — fail-fast            |
| 3️⃣    | **TransactionBehavior** | Abre transação, executa handler, comita/rollback           |
| 4️⃣    | **RequestHandler**      | Executa a lógica de negócio real                           |

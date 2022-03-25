# Accounts

📋 **Autenticar usuario**

- Acceso: **Publico**
- Metodo: **POST**
- Endpoint : "**API_URL/users/login**"
- Body:

```json
{
  "email" : "johndoe@example.com",
  "password" : "somePassword"
}
```

- Ejemplo respuesta:

```json
{
    "id": 1,
    "name": "John",
    "email": "johndoe@example.com",
    "password": "$2a$11$veow2TD8fGvQmbjTCpLYL.pPJKd37OWx5NcuwylCtCI4C6IkQ1zS6",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2MTk2MzAzNDEsImV4cCI6MTYxOTYzNzU0MSwiaWF0IjoxNjE5NjMwMzQxfQ.aQ72xFAGolUoQvtYqFTfrBOiDVHcxeZhF5X5No-L4aE",
    "role": "spectator"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **401**  Unauthorized
    

---

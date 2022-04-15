# Accounts

📋 **Autenticar usuario**

- Acceso: **Público**
- Método: **POST**
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
    "firstname": "John",
    "lastname": "Doe",
    "email": "johndoe@example.com",
    "role": "spectator",
    "password": "$2a$11$veow2TD8fGvQmbjTCpLYL.pPJKd37OWx5NcuwylCtCI4C6IkQ1zS6",         
    "token":      "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2MTk2MzAzNDEsImV4cCI6MTYxOTYzNzU0MSwiaWF0IjoxNjE5NjMwMzQxfQ.aQ72xFAGolUoQvtYqFTfrBOiDVHcxeZhF5X5No-L4aE"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **401**  Unauthorized
    
---

📋 **Alta de usuario**

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/users**"
- Body:

```json
{
  "firstname" : "John",
  "lastname" : "Doe",
  "role" : "spectator"
  "email" : "johndoe@example.com",
  "password" : "somePassword"
}
```

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "User has been registered"
}
```

- Códigos de respuesta:
    
    → **201** OK
    
    → **403** Forbidden

---

📋 **Baja de usuario**

- Acceso: **Privado - Admin**
- Método: **DELETE**
- Endpoint : "**API_URL/users/{user_id}**"

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "User has been deleted"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

---

📋 **Modificación de usuario**

- Acceso: **Privado - Admin**
- Método: **UPDATE**
- Endpoint : "**API_URL/users/{user_id}**"
- Body:

```json
{
  "firstname" : "John",
  "lastname" : "Doe",
  "role" : "seller"
  "email" : "johndoe@example.com",
  "password" : "somePassword"
}
```
- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "User has been updated"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

--- 

📋 **Obtener usuarios**

- Acceso: **Privado - Admin**
- Método: **GET**
- Endpoint : "**API_URL/users**"
- params admitidos:
	| Nombre Param | Tipo | Requerido Opcional | Ejemplo |
	| --- | --- | --- | --- |
	| role | string | Opcional | API_URL/users?role=spectator |
	
- Ejemplo respuesta:

```json
[
  {
  "firstname" : "John",
  "lastname" : "Doe",
  "role" : "seller"
  "email" : "johndoe@example.com"
  },
  {
  "firstname" : "Sarah",
  "lastname" : "Connor",
  "role" : "spectator"
  "email" : "johndoe@example.com"
  }
]
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden
	
---


# Accounts

### 📋 Autenticar usuario

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
    "firstname": "Lucas",
    "lastname": "Castro",
    "email": "lucas@example.com",
    "password": "$2a$11$S6cYKpMo4ucbAW1L9Ir79uesYzIizbREknjSC8NOqo4JJV8z3pqZq",
    "token": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJpZCI6IjEiLCJuYmYiOjE2NTIxNDAxMDEsImV4cCI6MTY1MjE0NzMwMSwiaWF0IjoxNjUyMTQwMTAxfQ.Ydd3PhBJOxb5wqJs5kTrLOsrQweTY4JYB88oUBdQO34",
    "role": "ADMIN",
    "activeAccount": false
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **401**  Unauthorized
    
---

### 📋 Alta de usuario

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/users**"
- Body:

```json
{
  "firstname" : "Spectator",
  "lastname" : "Test",
  "role" : "SPECTATOR",
  "email" : "spectator@example.com",
  "password" : "spectator1"
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

### 📋 Baja de usuario

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

### 📋 Modificación de usuario

- Acceso para admin: **Privado - Admin**
- Acceso para usuario spectator: **Privado - spectator**
- Método: **UPDATE**
- Endpoint : "**API_URL/users/{user_id}**"
- Body:

#### autorización para admin

```json
{
  "firstname" : "John",
  "lastname" : "Doe",
  "role" : "seller"
  "email" : "johndoe@example.com",
  "password" : "somePassword"
}
```

#### autorización para spectator

```json
{
  "firstname" : "John",
  "lastname" : "Doe",
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

### 📋 Obtener usuario

- Acceso: **Privado - Admin**
- Método: **GET**
- Endpoint : "**API_URL/users/{user_id}**"
- Ejemplo respuesta:

```json
{
  "firstname" : "John",
  "lastname" : "Doe",
  "role" : "seller"
  "email" : "johndoe@example.com"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden
  
---

### 📋 Obtener usuarios

- Acceso: **Privado - Admin**
- Método: **GET**
- Endpoint : "**API_URL/users**"
- Params admitidos:
	| Nombre Param | Tipo | Valores |Requerido/Opcional | Ejemplo | 
	| --- | --- | --- | --- | --- |
	| role | string | {admin, spectator, seller, supervisor} |Opcional | API_URL/users?role=spectator |
	
- Ejemplo respuesta:

```json
[
  {
    "id" : 1,
    "firstname" : "John",
    "lastname" : "Doe",
    "role" : "seller"
    "email" : "johndoe@example.com"
  },
  {
    "id" : 2,
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


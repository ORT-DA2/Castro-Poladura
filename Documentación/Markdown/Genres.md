# Genres

### 📋 Alta Genero

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/genres**"
- Body:

```json
{
  "name" : "pop-rock",
  "category" : "music"
}
```

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Genre added"
}
```

- Códigos de respuesta:
    
    → **201** OK
    
    → **403** Forbidden

---

### 📋 Baja Genero

- Acceso: **Privado - Admin**
- Método: **DELETE**
- Endpoint : "**API_URL/genres/{genre_id}**"
- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Genre deleted"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

--- 

### 📋 Modificación de Genero

- Acceso: **Privado - Admin**
- Método: **UPDATE**
- Endpoint : "**API_URL/genres/{genre_id}**"
- Body:

```json
{
  "name" : "suspense",
  "category" : "movies"
}
```

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Genre added"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

---

### 📋 Obtener Genero

- Acceso: **Privado - Admin**
- Método: **GET**
- Endpoint : "**API_URL/genres/{genre_id}**"
- Ejemplo respuesta:

```json
{
    "id" : 1,
    "name" : "suspense",
    "category" : "movies"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

--- 

### 📋 Obtener Generos

- Acceso: **Privado - Admin**
- Método: **GET**
- Endpoint : "**API_URL/genres**"
- Params admitidos : 
	| Nombre Param | Tipo | Requerido/Opcional | Ejemplo |
	| --- | --- | --- | --- |
	| role | string | Opcional | API_URL/users?role=spectator |
- Ejemplo respuesta:

```json
[
    {
        "id" : 1,
  	"name" : "suspense",
        "category" : "movies"
    },
    {
	"id" : 2,
  	"name" : "pop-rock",
  	"category" : "music"
    },
    {
        "id" : 1,
  	"name" : "soul",
  	"category" : "music"
    }
]
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

--- 

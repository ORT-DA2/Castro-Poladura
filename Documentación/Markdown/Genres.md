# Genres

### 📋 Alta Genero

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/genres**"
- Body:

```json
{
    "genreName":"Pop"
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
    "message": "Genre removed successfully",
    "resultCode": 0
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
    "genreName":"Pop"
}
```

- Ejemplo respuesta:

```json
{
    "message": "Genre updated successfully",
    "resultCode": 0
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
    "genreName": "pop"
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
	| category | string | Opcional | API_URL/genres?category=music |
- Ejemplo respuesta:

```json
[
    {
        "id" : 1,
        "genreName" : "movies"
    },
    {
	"id" : 2,
  	"genreName" : "music"
    },
    {
        "id" : 1,
  	"genreName" : "soul"
    }
]
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

--- 

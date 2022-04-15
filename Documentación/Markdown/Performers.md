# Performers

### Alta de Performer

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/performers**"
- Body:

```json
{
  "name" : "Coldplay",
  "type" : "band",
  "year" : "1996",
  "members" : ["Chris Martin","Guy Berryman","Phil Harvey","Will Champion","Jon Buckland"],
  "genre" : "{genre_id}"
}
```

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Performer has been registered"
}
```

- Códigos de respuesta:
    
    → **201** OK
    
    → **403** Forbidden

---

### 📋 Baja de Performer

- Acceso: **Privado - Admin**
- Método: **DELETE**
- Endpoint : "**API_URL/performers/{performer_id}**"

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Performer has been deleted"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

---

### 📋 Modificación de Performer

- Acceso: **Privado - Admin**
- Método: **UPDATE**
- Endpoint : "**API_URL/performers/{performer_id}**"
- Body:

```json
{
  "name" : "Coldplay",
  "type" : "band",
  "year" : "1996",
  "members" : ["Chris Martin","Guy Berryman","Phil Harvey","Will Champion","Jon Buckland"],
  "genre" : "{genre_id}"
}
```
- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Performer has been updated"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

--- 

### 📋 Obtener Performer

- Acceso: **Público**
- Método: **GET**
- Endpoint : "**API_URL/performers/{performer_id}**"
- Ejemplo respuesta:

```json
{
  "id" : 1,
  "name" : "Coldplay",
  "type" : "band",
  "year" : "1996",
  "members" : ["Chris Martin","Guy Berryman","Phil Harvey","Will Champion","Jon Buckland"],
  "genre" : {
    		"id" : 1,
   		"name" : "pop-rock",
    		"category" : "music"
	     }
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden
  
---

### 📋 Obtener Performers

- Acceso: **Público**
- Método: **GET**
- Endpoint : "**API_URL/performers**"
- Params admitidos:
	| Nombre Param | Tipo | Requerido/Opcional | Ejemplo |
	| --- | --- | --- | --- |
	| type | string | Opcional | API_URL/performers?type=band |
	| year | string | Opcional | API_URL/performers?year=1996 |
	| genre | number | Opcional | API_URL/performers?genre=1 |
- Ejemplo respuesta:

```json
[
  {
  	"id" : 1,
  	"name" : "Coldplay",
  	"type" : "band",
  	"year" : "1996",
  	"members" : ["Chris Martin","Guy Berryman","Phil Harvey","Will Champion","Jon Buckland"],
	"genre" : {
    		      "id" : 1,
   		      "name" : "pop-rock",
    		      "category" : "music"
		   }
   },
   {
  	 "id" : 2,
  	 "name" : "Machine Gun Kelly",
  	 "type" : "solist",
  	 "year" : "2011",
	 "genre" : {
    		       "id" : 1,
   			"name" : "pop-rock",
    			"category" : "music"
	            }
   },
]
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden
  
---

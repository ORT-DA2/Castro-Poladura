# Performers

### Alta de Performer

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/performers**"
- Body:

```json
{
  "name" : "Coldplay",
  "type" : "Band",
  "year" : "1996",
  "members" : ["Chris Martin","Guy Berryman","Phil Harvey","Will Champion","Jon 			Buckland"],
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
  "type" : "Band",
  "year" : "1996",
  "members" : ["Chris Martin","Guy Berryman","Phil Harvey","Will Champion","Jon 			Buckland"],
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



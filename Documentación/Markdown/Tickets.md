# Tickets

### 📋 Compra ticket

- Acceso para usuario vendedor: **Privado - seller**
- Acceso para usuario espectador: **Privado - spectator**
- Método: **POST**
- Endpoint : "**API_URL/tickets?event={event_id}**"
- Params admitidos:
	| Nombre Param | Tipo | Valores |Requerido/Opcional | Ejemplo | 
	| --- | --- | --- | --- | --- |
	| event | number | --- | Requerido | API_URL/tickets?event={event_id} |
- Body:

####Rol seller
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
- Ejemplo respuesta:
```json
{
    "message": "User updated successfully",
    "resultCode": 0
}
```

- Códigos de respuesta:
    
    → **201** Created
    
    → **403** Forbidden
	
	→ **400** Bad Request
	
---

### 📋 Obtener ticket

- Acceso: **Público**
- Método: **GET**
- Endpoint : "**API_URL/tickets?code={ticket_guid}**"
- Params admitidos:
	| Nombre Param | Tipo | Valores |Requerido/Opcional | Ejemplo | 
	| --- | --- | --- | --- | --- |
	| code | number | --- | Requerido | API_URL/tickets?code={ticket_guid} |
	
- Ejemplo respuesta:
```json
{
	"code" : "p3q59gjnfjgo4uqfjDXNCLKMQP31foiqnvjdanv"
	"status" : "purchased",
	"purchasedDate" : "21-05-2022",
	"event" : "Music Of The Spheres World Tour"
}
```

---

### 📋 Obtener tickets

- Acceso: **Privado - spectator**
- Método: **GET**
- Endpoint : "**API_URL/tickets**"

- Ejemplo respuesta:
```json
[
	{
		"id" : 1,
		"code" : "p3q59gjnfjgo4uqfjDXNCLKMQP31foiqnvjdanv",
		"status" : "purchased",
		"purchasedDate" : "21-05-2022",
		"event" : 
				{	"id" : 1,
					"date" : "2022-04-01",
					"type" : "concert",
					"availableTickets" : "20023",
					"price" : "127.97"
					"tourName" : "Music Of The Spheres World Tour",
					"performer" : {
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
				}	
	},
	{
		"id" : 2,
		"code" : "1270f7sdf897adfhlajbvlaaotoaweyoi2",
		"status" : "used",
		"purchasedDate" : "16-03-2019",
		"event" : {	
				 	"id" : 2,
					"date" : "2023-04-01",
					"availableTickets" : "1800",
					"type" : "concert",
					"price" : "90.97"
					"tourName" : "Future Nostalgia",
					"performer" : {
							"id" : 1,
							"name" : "Dua Lipa",
							"type" : "solist",
							"year" : "2016",
							"genre" : {
										"id" : 1,
										 "name" : "pop",
										"category" : "music"
								   	   }
					}
				  }
	}
]
	
```

---

### 📋 Modificar Ticket

- Acceso para usuario acomodador: **Privado - supervisor**
- Método: **UPDATE**
- Endpoint : "**API_URL/tickets**"
- Body:

####Rol seller
```json
{
	"code" : "p3q59gjnfjgo4uqfjDXNCLKMQP31foiqnvjdanv"
	"status" : "used",
}
```

- Códigos de respuesta:
    
    → **201** Created
    
    → **403** Forbidden
	
	→ **400** Bad Request
	
---

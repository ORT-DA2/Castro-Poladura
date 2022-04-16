# Concerts

### 📋 Alta de concierto

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/concerts**"
- Body:

```json
{
  "concertDate" : "2022-04-01",
  "availableTickets" : "20023",
  "price" : "127.97"
  "tourName" : "Music Of The Spheres World Tour",
  "performer" : "{performer_id}"
}
```

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Concert has been registered"
}
```

- Códigos de respuesta:
    
    → **201** OK
    
    → **403** Forbidden

---

### 📋 Modificación de concierto

- Acceso: **Privado - Admin**
- Método: **UPDATE**
- Endpoint : "**API_URL/concerts/{concert_id}**"
- Body:

```json
{
  "concertDate" : "2022-04-01",
  "availableTickets" : "20023",
  "price" : "127.97"
  "tourName" : "Music Of The Spheres World Tour",
  "performer" : "{performer_id}"
}
```

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Concert has been udpated"
}
```

- Códigos de respuesta:
    
    → **201** OK
    
    → **403** Forbidden

---

### 📋 Baja de concierto

- Acceso: **Privado - Admin**
- Método: **DELETE**
- Endpoint : "**API_URL/concerts/{concert_id}**"

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Concert has been deleted"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden

---

### 📋 Obtener conciertos

- Acceso: **Público**
- Método: **GET**
- Endpoint : "**API_URL/concerts**"
- Params admitidos:
	| Nombre Param | Tipo | values |Requerido/Opcional | Ejemplo | 
	| --- | --- | --- | --- | --- |
	| sort | string | {oldest, newest} | Opcional | API_URL/concerts?role=oldest |
	| startDate | string | --- | Opcional | API_URL/concerts?startDate=2022-04-01 |
	| endDate | string | --- | Opcional | API_URL/concerts?endDate=2023-06-16 |
	| performerName | string | --- | Opcional | API_URL/concerts?performerName=Coldplay |
- Ejemplo respuesta:
	```json
[
	{	"id" : 1,
  		"concertDate" : "2022-04-01",
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
	},
	{	"id" : 2,
  		"concertDate" : "2023-04-01",
		"availableTickets" : "1800",
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
]
```

- Códigos de respuesta:
    
    → **200** OK
	
---

### 📋 Obtener concierto

- Acceso: **Público**
- Método: **GET**
- Endpoint : "**API_URL/concerts/{concert_id}**"
- Ejemplo respuesta:
	```json

{	"id" : 1,
  	"concertDate" : "2022-04-01",
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
```

- Códigos de respuesta:
    
    → **200** OK

---
	
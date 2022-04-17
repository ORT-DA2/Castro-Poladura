# Concerts

### 📋 Alta de evento

- Acceso: **Privado - Admin**
- Método: **POST**
- Endpoint : "**API_URL/events?type=concert**"
- Params admitidos:
	| Nombre Param | Tipo | Valores |Requerido/Opcional | Ejemplo | 
	| --- | --- | --- | --- | --- |
	| type | string | {concert} | Requerido | API_URL/events?type=concert |
- Body:

```json
{
  "date" : "2022-04-01",
  "availableTickets" : "20023",
  "price" : "127.97"
  "type" : "concert"
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

    → **400** Bad Request
---

### 📋 Modificación de evento

- Acceso: **Privado - Admin**
- Método: **UPDATE**
- Endpoint : "**API_URL/events/{event_id}**"
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
  "message" : "Event has been udpated"
}
```

- Códigos de respuesta:
    
    → **201** OK
    
    → **403** Forbidden
    
    → **400** Bad Request

---

### 📋 Baja de concierto

- Acceso: **Privado - Admin**
- Método: **DELETE**
- Endpoint : "**API_URL/events/{event_id}**"

- Ejemplo respuesta:

```json
{
  "code" : 0,
  "message" : "Event has been deleted"
}
```

- Códigos de respuesta:
    
    → **200** OK
    
    → **403** Forbidden
    
    → **400** Bad Request

---

### 📋 Obtener conciertos

- Acceso: **Público**
- Método: **GET**
- Endpoint : "**API_URL/events?type=concert**"
- Params admitidos:
	| Nombre Param | Tipo | values |Requerido/Opcional | Ejemplo | 
	| --- | --- | --- | --- | --- |
	| type | string | {concert} | Requerido | API_URL/events?type=concert |
	| sort | string | {oldest, newest} | Opcional | API_URL/events?type=concert&sort=oldest |
	| startDate | string | --- | Opcional | API_URL/events?type=concert&startDate=2022-04-01 |
	| endDate | string | --- | Opcional | API_URL/events?type=concert&endDate=2023-06-16 |
	| performerName | string | --- | Opcional | API_URL/events?type=concert&performerName=Coldplay |
- Ejemplo respuesta:
```json
		[
		    {
		    	"id" : 1,
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
		      {	
	 		"id" : 2,
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
- Endpoint : "**API_URL/events/{event_id}**"
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
	

# API-Docs

## Endpoints

- [Accounts](Documentaci%C3%B3n/Markdown/Accounts.md)
- [Genres](Documentaci%C3%B3n/Markdown/Genres.md)
- [Performers](Documentaci%C3%B3n/Markdown/Performers.md)
- [Events](Documentaci%C3%B3n/Markdown/Events.md)
- [Tickets](Documentaci%C3%B3n/Markdown/Tickets.md)

## Accesos

| Acceso | Descripción |
| --- | --- |
| Publico  | El recurso puede ser accedido de manera pública |
| Privado | Se requiere que el usuario esté autenticado |
| Privado-Restringido | Debe estar autenticado y debe tener autorización del tipo de usuario exigido, estos se describen más abajo |

## Tipos de usuario (Roles)

| Tipo usuario | Descripción |
| --- | --- |
| admin | Usuario administrador |
| seller | Usuario vendedor |
| supervisor | Usuario acomodador |
| spectator | Usuario espectador |

## Respuesta de error API

La API maneja la siguiente estructura para describir errores ocurridos: (*Esta respuesta es a modo de ejemplo*)

```json
{
    "statusCode": 400,
    "statusDescription": "BadRequest",
    "message": "Business error described"
}
```
- **statusCode**: El código Http correspondiente asociado al resultado ocasionado, dentro de estos los posibles son:
- **statusDescription:** Nombre de código de error Http.
- **message:** Mensaje que describe el error ocurrido.

Códigos http utilizados

| Código | Descripción |
| --- | --- |
| 200 | OK |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

## Respuesta de operación realizada por la api (Para aquellos endpoints de creación, borrado y actualización)

La API maneja la siguiente estructura para describir resultados de operaciones que realiza (*Esta respuesta es a modo de ejemplo*)

```json
{
    "message": "User has been registered",
    "code": 0
}
```

Códigos de resultados de operaciones realizadas por la api

| Código | Descripción |
| --- | --- |
| 0 | SUCCESS |
| 1 | ERROR |

## **Autenticación**

La autenticación que debe ser provista por un “*Bearer Token*”.

<aside>
💡 Una vez que el usuario haya iniciado sesión, cada solicitud posterior incluirá el JWT, lo que permitirá al usuario acceder a rutas, servicios y recursos permitidos con ese token.

</aside>

<figure>
    <img src="Documentaci%C3%B3n/Markdown/JwtFlow.png" width="600" height="200"
         alt="Jwt flow">
</figure>


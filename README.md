# API-Docs

## Accesos

| Acceso | Descripción |
| --- | --- |
| Publico  | El recurso puede ser accedido de manera publica |
| Privado | Debe estar autenticado y debe tener autorización la del tipo de usuario exigido, estos se describen mas abajo |
| Privado-Restringido | Se requiere que el usuario este autenticado |

## Tipos de usuario (Roles)

| Tipo usuario | Descripción |
| --- | --- |
| admin | Usuario administrador |
| seller | Usuario vendedor |
| supervisor | Usuario acomodador |
| spectator | Usuario espectador |

## Respuesta de error API

La API maneja la siguiente estructura para describir errores al cliente: (*Esta respuesta es a modo de ejemplo*)

```json
{
    "statusCode": 400,
    "statusDescription": "BadRequest",
    "message": "Business error described"
}
```

- **statusCode**: El codigo Http correspondiente asociado al resultado ocacionado, dentro de estos los posibles son:
- **statusDescription:** Nombre de código de error Http.
- **message:** Mensaje que describe el error ocurrido.

→ Códigos http utilizados

| Código | Descripción |
| --- | --- |
| 200 | OK |
| 201 | Created |
| 400 | Bad Request |
| 401 | Unauthorized |
| 403 | Forbidden |
| 404 | Not Found |
| 500 | Internal Server Error |

## **Autenticación**

La autenticación que debe ser provista por un “*Bearer Token*”.

<aside>
💡 Una vez que el usuario haya iniciado sesión, cada solicitud posterior incluirá el JWT, lo que permitirá al usuario acceder a rutas, servicios y recursos permitidos con ese token.

</aside>

![JwtFlow](Documentaci%C3%B3n/Markdown/JwtFlow.png)

## Endpoints

[Accounts](Documentaci%C3%B3n/Markdown/Accounts.md)

---

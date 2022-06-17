Descripción de los resources de la API 

**-> URL base:** http://localhost:5000/api

**Conciertos**: 



|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.001.png)|
| :- | - |
|Descripción |Registrar concierto |
|Parametros |- |
|Respuestas |200 - 401 - 403 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.002.png)|


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.003.png)|
| :- | - |
|Descripción |obtener conciertos |
|Parametros |<p>- - - - - - - </p><p>type: string newest: boolean startDate: string endDate: string artistName: string tourName: string performerId </p>|
|Respuestas |200  |
|Header |- |
|Body |- |
Endpoint +  ![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.004.png)Metodo  



|Descripción |obtener concierto por id |
| - | - |
|Parametros |- |
|Respuestas |200 |
|Header |- |
|Body |- |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.005.png)|
| :- | - |
|Descripción |Actualizar concierto |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.006.png)|


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.007.png)|
| :- | - |
|Descripción |Borrar concierto |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body ||


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.008.png)|
| :- | - |
|Descripción |obtener conciertos |
|Parametros |<p>- </p><p>type: string </p>|


||- - - - - - |newest: boolean startDate: string endDate: string artistName: string tourName: string performerId |
| :- | - | :- |
|Respuestas |200  ||
|Header |- ||
|Body |- ||
**Generos** 



|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.009.png)|
| :- | - |
|Descripción |Registrar genero |
|Parametros |- |
|Respuestas |200 - 401 - 403 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.010.png)|


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.011.png)|
| :- | - |
|Descripción |obtener generos |
|Parametros |- |
|Respuestas |200 |


|Header |- |
| - | - |
|Body |- |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.012.png)|
| :- | - |
|Descripción |Borrar concierto |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body ||


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.013.png)|
| :- | - |
|Descripción |obtener genero por id |
|Parametros |- |
|Respuestas |200 |
|Header Endpoint + |- |
|BMoedtyo do |- |
|Descripción |Actualizar genero |
|Parametros |- |
|Respuestas |200 – 403 - 401 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.014.png)|
**Performers** 



|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.015.png)|
| :- | - |
|Descripción |Registrar performer |
|Parametros |- |
|Respuestas |200 - 401 - 403 |
|Header |Authorization |


|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.016.png)|
| - | - |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.017.png)|
| :- | - |
|Descripción |obtener performers |
|Parametros |- |
|Respuestas |200 |
|Header |- |
|Body |- |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.018.png)|
| :- | - |
|Descripción |Actualizar performer |
|Parametros |- |
|Respuestas |200 – 403 - 401 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.019.png)|
|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.020.png)|
|Descripción |Borrar performer |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body ||


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.021.png)|
| :- | - |
|Descripción |obtener performer por id |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |- |
|Body |- |
**Tickets** 



|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.022.png)|
| :- | - |
|Descripción |Comprar ticket |
|Parametros |<p>- </p><p>eventId: string </p>|
|Respuestas |200 - 401 - 403 |
|Header |Authorization (si es usuario registrado solo mandar Bearer Token) |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.023.png)|


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.018.png)|
| :- | - |
|Descripción |Actualizar Ticket status |
|Parametros |- |
|Respuestas |200 – 403 - 401 |
|Header |Authorization  |
Body  ![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.024.png)



|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.025.png)|
| :- | - |
|Descripción |Borrar ticket |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body ||


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.026.png)|
| :- | - |
|Descripción |obtener ticket por id |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body |- |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.027.png)|
| :- | - |
|Descripción |obtener tickets |
|Parametros |- |


|Respuestas |200 – 401 - 403 |
| - | - |
|Header |Authorization |
|Body |- |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.028.png)|
| :- | - |
|Descripción |obtener codigo de ticket |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body |- |
**Users** 



|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.029.png)|
| :- | - |
|Descripción |Login usuario |
|Parametros |- |
|Respuestas |200 - 401 - 403 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.030.png)|


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.031.png)|
| :- | - |
|Descripción |obtener usuarios |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |


|Body |- |
| - | - |
|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.029.png)|
|Descripción |Registrar usuario |
|Parametros |- |
|Respuestas |200 - 401 - 403 |
|Header |Authorization |
|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.032.png)|


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.033.png)|
| :- | - |
|Descripción |obtener usuario por id |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body |- |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.034.png)|
| :- | - |
|Descripción |Actualizar usuario |
|Parametros |- |
|Respuestas |200 – 403 - 401 |
|Header |Authorization  |


|Body |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.035.png)|
| - | - |


|Endpoint + Metodo |![](Aspose.Words.7b16d730-b25e-4705-8aed-bd3ebd15db21.036.png)|
| :- | - |
|Descripción |Borrar usuario |
|Parametros |- |
|Respuestas |200 – 401 - 403 |
|Header |Authorization |
|Body ||


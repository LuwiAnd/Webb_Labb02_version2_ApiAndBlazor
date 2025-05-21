# API-specifikation för Labb 2 i Webbutveckling med .Net - av Ludwig Andersson

Version 1.0.0.
Av Ludwig Andersson.
Mejladress: Ludwig.Andersson@iths.se.
Syftet med dessa API:er är att skapa en enkel webbshop, där användare kan skapa konton, administrera sin egen information och handla, samtidigt som adminitratörer ska kunna administrera både användarkonton och produkter.



## Admin

### GET /admin/test
Detta API testar om du är inloggad som admin eller inte. Syftet med detta API var att testa att autentiseringen fungerade under uppbyggnaden av detta projekt.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response
Om du är autentiserad som admin:
200 Success
"Du är admin!"

Annars:
401 Unauthorized





## Auth

### POST /auth/login
Loggar in användare och genererar ett JWT-token.

#### Parametrar
Inga parametrar.

#### Request
Request body, exempel:
{
  "Email": "kalle@anka.se",
  "Password": "a"
}

#### Response
200 Success:  
{  
  "Token": "string",  
  "Role": "string",  
  "Email": "string"  
}

401: Error: response status is 401




## Cart

### GET /cart
Hämtar användarens kundvagn.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body används.

#### Response
200	
Lista över produkter i kundvagn. Exempel:
[
  {
    "ProductID": 1,
    "ProductName": "Mjölk",
    "Price": 15.9,
    "Quantity": 1,
    "StockQuantity": 1
  },
  {
    "ProductID": 2,
    "ProductName": "Vatten",
    "Price": 0.9,
    "Quantity": 2,
    "StockQuantity": 100
  }
]

401	
Ej inloggad

403	
Forbidden




### POST /cart/add
Lägger till en produkt i kundvagnen.  

#### Parametrar
Inga parametrar.

#### Request

ProductID = ID för produkten som ska läggas till i kundvagnen.
Quantity = Antalet av den aktuella produkten som ska läggas till i kundvagnen.
{
  "ProductID": int,
  "Quantity": int
}


#### Response
200	
Produkt tillagd i kundvagnen

204	
No Content

400	
Felaktig data eller lagerbrist

401	
Ej inloggad

403	
Forbidden




### POST /cart/checkout
Genomför en beställning. Detta innebär att en order sparas i databasen för användaren och att lagersaldot minskar för motsvarande produkter.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response
200	
Beställningen genomfördes

204	
No Content

400	
Fel, t.ex. otillräckligt lager

401	
Ej inloggad

403	
Forbidden





### DELETE /cart/clear
Tömmer användarens kundvagn. Tar bort alla produkter från pågående kundvagn (unhandled order).

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response





### DELETE /cart/item/{productId}
Tar bort en produkt från användarens kundvagn.

#### Parametrar
productId: int. Id för den produkt som ska tas bort.

#### Request
Ingen request body behövs.

#### Response
204	
Produkten togs bort

401	
Unauthorized

403	
Forbidden

404	
Produkten eller kundvagnen hittades inte





### PUT /cart/update
Uppdaterar antalet av en produkt i kundvagnen.

#### Request
{
  "ProductID": int,
  "Quantity": int
}

#### Response
Ingen response body.

204	
Antal uppdaterat

400	
Felaktiga uppgifter eller lagerbrist

401	
Unauthorized

403	
Forbidden

404	
Produkten eller kundvagnen hittades inte








## Orders

### GET /orders
Hämtar alla ordrar.

#### Parametrar
Inga parametrar.


#### Request
Ingen Request body behövs.

#### Response
200	
Lista med ordrar. Exempel:
[
  {
    "OrderID": 2,
    "UserID": 2,
    "Email": "user@anka.se",
    "OrderDate": "2025-04-05T10:25:05.830972",
    "OrderStatus": "shipped",
    "TotalAmount": 6014.9,
    "Items": [
      {
        "ProductID": 1,
        "Quantity": 1,
        "Price": 15.9
      }
    ]
  },
  {
    "OrderID": 3,
    "UserID": 4,
    "Email": "u",
    "OrderDate": "2025-04-05T10:25:06.0023493",
    "OrderStatus": "shipped",
    "TotalAmount": 6014.9,
    "Items": [
      {
        "ProductID": 1,
        "Quantity": 1,
        "Price": 15.9
      }
    ]
  }
]

401	
Ej inloggad

403	
Inte behörig





### GET /orders/{id}
Hämtar en order med ett specifikt ID.

#### Parametrar
id: int. Id för den order som ska hämtas.

#### Request
Ingen request body behövs.

#### Response
200
Order hittades. Exempel:
{
  "OrderID": 2,
  "UserID": 2,
  "Email": "",
  "OrderDate": "2025-04-05T10:25:05.830972",
  "OrderStatus": "shipped",
  "TotalAmount": 6014.9,
  "Items": [
    {
      "ProductID": 1,
      "Quantity": 1,
      "Price": 15.9
    }
  ]
}

401	
Unauthorized

403	
Forbidden

404	
Order hittades inte





### GET /orders/mine
Hämtar ordrar för den inloggade användaren.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response
200
Ordrar hittades. Exempel:
{
  "OrderID": 2,
  "UserID": 2,
  "Email": "",
  "OrderDate": "2025-04-05T10:25:05.830972",
  "OrderStatus": "shipped",
  "TotalAmount": 6014.9,
  "Items": [
    {
      "ProductID": 1,
      "Quantity": 1,
      "Price": 15.9
    }
  ]
}

401	
Ej inloggad

403	
Forbidden







## Products

### GET /products
Hämtar alla produkter.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response
200	
Lista med produkter. Exempel:
[
  {
    "ID": 0,
    "Number": 0,
    "Name": "string",
    "Description": "string",
    "Price": 0,
    "Category": "string",
    "Status": 0,
    "StockQuantity": 0
  }
]





### POST /products
Lägger till en ny produkt.

#### Parametrar
Inga parametrar.

#### Request
Exempel:
{
  "Number": int,
  "Name": "string",
  "Description": "string",
  "Price": decimal,
  "Category": "string",
  "Status": int, (motsvarar en enum i databasen för om produkten är tillgänglig/utgått/...)
  "StockQuantity": int
}

#### Response
201	
Returnerar den skapade produkten. Exempel:
{
  "ID": 1,
  "Number": 1001,
  "Name": "string",
  "Description": "string",
  "Price": 15.9,
  "Category": "string",
  "Status": 2,
  "StockQuantity": 5
}





### DELETE /products/{id}
Tar bort en specifik produkt baserat på dess id.

#### Parametrar
id: int. Id för produkten som ska tas bort.

#### Request
Ingen request body behövs.

#### Response
204	
Borttaget

401	
Unauthorized

403	
Forbidden

404	
Produkten hittades inte





### GET /products/{id}
Hämtar en specifik produkt baserat på dess id.

#### Parametrar
id: int. Id för produkten som ska hämtas.

#### Request
Ingen request body behövs.

#### Response
200	
Hittad produkt





### PUT /products/{id}
Uppdaterar en specifik produkt.

#### Parametrar
id: int. Id för produkten som ska uppdateras.

#### Request
Exempel:
{
  "Name": "string",
  "Description": "string",
  "Price": decimal,
  "Category": "string",
  "Status": int, (motsvarar en enum i databasen för om produkten är tillgänglig/utgått/...)
  "StockQuantity": int,
  "ProductNumber": int
}

#### Response

204	
Produkt uppdaterad

400	
Felaktig data

401	
Unauthorized

403	
Forbidden

404	
Produkt hittades inte





### DELETE /products/by-number/{number}
Tar bort en specifik produkt baserat på produktnummer.

#### Parametrar
number: int. Produktnummer för produkten som ska tas bort.

#### Request
Ingen request body behövs.

#### Response

204	
Borttagen

No links
401	
Unauthorized

No links
403	
Forbidden

No links
404	
Produkten hittades inte





### GET /products/by-number/{number}
Hämtar en specifik produkt baserat på produktnummer.

#### Parametrar
number: int. Produktnummer för produkten som ska hämtas.

#### Request
Ingen request body behövs.

#### Response
200	
Hittad produkt. Exempel:
{
  "ID": int,
  "Number": int,
  "Name": "string",
  "Description": "string",
  "Price": decimal,
  "Category": "string",
  "Status": int, (motsvarar en enum i backend)
  "StockQuantity": int
}





### GET /products/search
Hämtar alla produkter som uppfyller sökkriterier.

#### Parametrar
Name: string. Produktens namn, eller del av produktens namn.
ProductNumber: int. Produktens produktnummer.

#### Request
Ingen request body behövs.

#### Response

200	
Lista över matchande produkter. Exempel:
[
  {
    "ID": 1,
    "Number": 1001,
    "Name": "string",
    "Description": "string",
    "Price": decimal,
    "Category": "string",
    "Status": int, (motsvarar en enum i backend)
    "StockQuantity": int
  }
]








## Register

### POST /register
Tillgänglig när man inte är inloggad. Alla nyregistrerade användare får rollen "user". Endast en admin kan gå in och ändra rollen till "admin".

#### Parametrar
Inga parametrar.

#### Request
Exempel:
{
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Password": "string"
}

#### Response
201	
Ny användare skapad. Exempel:
{
  "UserID": 2004,
  "FirstName": "asdf",
  "LastName": "asdf",
  "Email": "asdf@asdf.se",
  "PhoneNumber": "2345",
  "HomeAddress": "f2",
  "Role": "user"
}

400
Ogiltig data








## Test

### GET /test/role
Detta är ett API för att testa vilken roll man är inloggad som. Det användes för att testa om det gick att logga in som admin under skapandet av detta projekt.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response
200	
Success. Exempel:
"Du är inloggad som: admin."

401	
Unauthorized








## Users

### GET /users
Hämtar alla användare. Endast admin får anropa.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response
200	
Success. Exempel:
[
  {
    "UserID": 1,
    "FirstName": "Kalle",
    "LastName": "Anka",
    "Email": "kalle@anka.se",
    "PhoneNumber": "0700000000",
    "HomeAddress": "Testgatan 1",
    "Role": "admin"
  },
  {
    "UserID": 2,
    "FirstName": "Kalle",
    "LastName": "Anka",
    "Email": "user@anka.se",
    "PhoneNumber": "0701112233",
    "HomeAddress": "Ankeborg 3",
    "Role": "user"
  }
]

401	
Ej inloggad

403	
Inte behörig (kräver admin)





### POST /users
Skapar en ny användare.

#### Parametrar
Inga parametrar.

#### Request
{
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string",
  "Password": "string"
}

#### Responnse

201	
Skapad användare. Exempel:
{
  "UserID": 0,
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string",
  "Password": "string",
  "PasswordHash": "string"
}

400	
Ogiltig data

401	
Ej inloggad

403	
Inte behörig





### PUT /users
Uppdaterar en användare.

#### Parametrar
Inga parametrar.

#### Request
{
  "Id": 0,
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string",
  "Password": "string"
}

#### Response

200	
Uppdaterad användare. Exempel:
{
  "UserID": 0,
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string",
  "Password": "string",
  "PasswordHash": "string"
}

401	
Unauthorized

403	
Forbidden

404	
Användaren hittades inte





### DELETE /users/{id}
Tar bort en användare baserat på id.

#### Parametrar
id: int. Användarens id.

#### Request
Ingen request body behövs.

#### Response

204	
Användare borttagen

401	
Ej inloggad

403	
Inte behörig

404	
Användare hittades inte





### GET /users/{id}
Hämtar en specifik användare baserat på id.

#### Parametrar
id: int. Användarens id.

#### Request
Ingen request body behövs.

#### Response

200	
Hittad användare. Exempel:
{
  "UserID": 0,
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string",
  "Password": "string",
  "PasswordHash": "string"
}

401	
Ej inloggad

403	
Inte behörig (kräver admin)

404	
Användare hittades inte





### GET /users/me
Hämtar den inloggade användarens uppgifter.

#### Parametrar
Inga parametrar.

#### Request
Ingen request body behövs.

#### Response

200	
Användarinfo. Exempel:
{
  "UserID": 0,
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string"
}

401	
Ej inloggad

403	
Forbidden





### PUT /users/me
Uppdaterar den inloggade användarens uppgifter.

#### Parametrar
Inga parametrar.

#### Request
{
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Password": "string"
}

#### Response

204	
Uppdatering lyckades

400	
Ogiltig data

401	
Ej inloggad

403	
Forbidden





### GET /users/search
Hämtar en användare baserat på dess exakta mejladress.

#### Parametrar
Email: sträng. Användarens hela och rättstavade mejladress.

#### Request
Ingen request body behövs.

#### Response

200	
Användare hittad. Exempel:

{
  "UserID": 0,
  "FirstName": "string",
  "LastName": "string",
  "Email": "string",
  "PhoneNumber": "string",
  "HomeAddress": "string",
  "Role": "string"
}

401	
Unauthorized

403	
Forbidden

404	
Användare hittades inte





### GET /users/search/fragment
Hämtar alla användare vars mejladress innehåller den sträng man skickar in.

#### Parametrar
Query-parameter: 
partialEmail: string. Söksträng för att hitta alla användare vars mejladress innehåller söksträngen.

#### Request
Ingen request body behövs.

#### Response

200	
Lista med användare. Exempel:
[
    {
        "UserID": 0,
        "FirstName": "string",
        "LastName": "string",
        "Email": "string",
        "PhoneNumber": "string",
        "HomeAddress": "string",
        "Role": "string"
    },
    {
        "UserID": 1,
        "FirstName": "string",
        "LastName": "string",
        "Email": "string",
        "PhoneNumber": "string",
        "HomeAddress": "string",
        "Role": "string"
    }
]


401	
Unauthorized

403	
Forbidden

# Slut på dokument.
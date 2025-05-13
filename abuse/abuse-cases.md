# Abuse Cases for ExpenseSystem

Dette dokument beskriver potentielle misbrugscases for ExpenseSystem, identificerer mulige angrebsvektorer og definerer foranstaltninger.

## 1. Uautoriseret adgang til udgiftsdata

### Angriber
En ondsindet aktør, der kan være:
- En ekstern hacker, der forsøger at få adgang til systemet
- En insider med begrænset adgang, der forsøger at få udvidet adgang
- En tidligere medarbejder, hvis konti ikke er blevet deaktiveret korrekt

### Mål/Asset
- Følsomme udgiftsdata, herunder fakturabeløb, leverandøroplysninger og interne budgetnumre
- Personlige oplysninger for medarbejdere, der indsender udgifter
- Økonomiske godkendelsesprocesser

### Forsvar/Mitigation
- Implementer robust autentifikation med multi-faktor godkendelse
- Etabler detaljeret adgangskontrol baseret på roller
- Gennemfør regelmæssig brugerkontorevision og automatisk deaktivering
- Log og overvåg alle adgangsforsøg, især mislykkede forsøg
- Krypter følsomme data både i hvile og under transmission

## 2. Manipulation af udgiftsbeløb

### Angriber
- Intern medarbejder, der forsøger at indsende forfalsket udgift
- Ondsindet aktør, der har opnået adgang til en gyldig brugers konto
- Teknisk angriber, der udnytter inputvalidering eller beregningslogik

### Mål
- Økonomisk gevinst gennem forfalskning af udgiftsbeløb
- Omgåelse af godkendelsesgrænser for udgifter
- Manipulation af finansielle registreringer

### Forsvar
- Implementer stærk inputvalidering af alle beløbsfelter
- Kræv kvitteringer som dokumentation for udgifter over et vist beløb
- Etabler flere godkendelsesniveauer for store udgifter
- Anvend hashing eller digital signatur på udgiftsposter efter godkendelse
- Implementer automatisk flagning af usædvanlige mønstre eller beløb

## 3. SQL-injektion angreb ved login

### Angriber
En teknisk kompetent ondsindet aktør, der forsøger at udnytte svagheder i webgrænsefladen.

### Mål
- Uautoriseret adgang til databasen
- Eksfiltrering af brugerdata og udgiftsdata
- Mulig mulighed for at ændre databaseindhold

### Forsvar
- Implementer parametriserede forespørgsler i alle databaseinteraktioner
- Anvend ORM (Object-Relational Mapping) framework med indbygget beskyttelse
- Begræns databasebrugerens rettigheder efter princippet om mindst muligt privilegium
- Implementer Web Application Firewall (WAF) for at detektere og blokere injektionsforsøg
- Udfør regelmæssig sikkerhedsscanning og penetrationstest

## 4. Cross-Site Scripting (XSS) i kommentarfelter

### Angriber
Ondsindet bruger, der indsætter skadelig JavaScript-kode i kommentarfelter til udgifter.

### Mål
- Stjæle sessionscookies fra andre brugere
- Udføre handlinger på vegne af administratorer
- Indsamle følsomme oplysninger fra brugerens browser

### Forsvar
- Implementer streng inputvalidering og output-encoding
- Anvend Content Security Policy (CSP) headers
- Brug HttpOnly og Secure flag på alle cookies
- Fjern eller begræns HTML i brugerinput
- Indfør automatisk scanning for potentielle XSS-mønstre

## 5. Denial of Service (DoS) angreb

### Angriber
Ekstern aktør, der forsøger at gøre systemet utilgængeligt.

### Mål
- Forhindre legitime brugere i at få adgang til systemet
- Afbryde kritiske forretningsprocesser
- Skade organisationens omdømme

### Forsvar
- Implementer rate-limiting på alle API-endpoints
- Anvend Cloud-baseret DoS-beskyttelse
- Konfigurer timeout-grænser korrekt
- Design systemet til at kunne skalere under høj belastning
- Etabler en incident response plan specifikt for DoS-angreb
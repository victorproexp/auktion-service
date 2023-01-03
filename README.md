# auktion-service

Guide til at bruge auktionshuset

1. Opret en vare i projektet vareAPI i løsningen vare-service*
2. Opret en kunde i projektet kundeAPI i løsningen kunde-service*
3. Opret en auktion i projektet auktionAPI i løsningen auktion-service*
4. Opret et bud i projektet budAPI i løsningen bud-service
5. Buddet findes nu i auktiondatabasen, hvis det blev godkendt i BudHandler.cs**.

/* data findes måske allerede i MongoDB-databaserne.
/** Se auktion-service/auktionAPI/Services/BudHandler.

# Overvejelser

Make references explicit
Store a URI instead of a foreign key (ID) and dereference the URI to look up the associated resource (fx /vare/3, works well in RESTful APIs).

Internal implementation
AuktionService and BudHandler separated to simplify testing. 

Contexts boundaries
Separate kunde-service due to PII and GDPR
Aggregates Vare and Auktion could be implemented in the same microservice as vare-service “looks like a thin wrapper around database CRUD operations” and could be a sign of “weak cohesion and tighter coupling” (Newman 2021).

Rejection
A request sent by bud-service to change the internal state of Auktion can be rejected by auktion-service.

Coupling
Domain coupling between bud-service and auktion-service as temporal coupling has been avoided through the use of asynchronous communication by RabbitMQ as opposed to synchronous blocking network calls. 

Data integrity workaround 
Copy name of vare into auktion when auktion is made.

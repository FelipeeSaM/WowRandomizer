# WowRandomizer - Planejamento de Arquitetura

## 📋 Visão Geral do Projeto

Sistema de geração aleatória de personagens do World of Warcraft baseado em facções, raças, classes, profissões e sub-profissões com restrições específicas.

---

## 🏗️ Arquitetura Geral

### Tipo de Arquitetura
- **Vertical Slice Architecture**: Cada funcionalidade (feature) será organizada verticalmente
- **Event-Driven**: Comunicação assíncrona entre serviços via eventos
- **CQRS**: Separação de comandos (escrita) e queries (leitura)

### Estrutura de Microserviços

```
┌─────────────────────────────────────────────────────────────────┐
│                        Cliente (UI)                              │
└────────────────────────┬────────────────────────────────────────┘
                         │
                         ▼
┌─────────────────────────────────────────────────────────────────┐
│                    YARP API Gateway                              │
│  - Roteamento                                                    │
│  - Rate Limiting                                                 │
│  - Load Balancing                                                │
└────────────────────────┬────────────────────────────────────────┘
                         │
         ┌───────────────┼───────────────┐
         ▼               ▼               ▼
┌─────────────┐  ┌─────────────┐  ┌─────────────┐
│  Character  │  │   Game      │  │  Generator  │
│  Service    │  │   Data      │  │  Service    │
│             │  │   Service   │  │             │
└──────┬──────┘  └──────┬──────┘  └──────┬──────┘
       │                │                │
       └────────────────┼────────────────┘
                        ▼
              ┌──────────────────┐
              │   RabbitMQ       │
              │   (MassTransit)  │
              └──────────────────┘
                        │
         ┌──────────────┼──────────────┐
         ▼              ▼              ▼
┌─────────────┐  ┌──────────┐  ┌──────────┐
│ SQL Server  │  │  Redis   │  │   ELK    │
│             │  │  Cache   │  │  Stack   │
└─────────────┘  └──────────┘  └──────────┘
```

---

## 📦 Microserviços

### 1. **WowRandomizer.ApiGateway**
- **Responsabilidade**: Gateway de entrada para toda a aplicação
- **Tecnologias**: YARP, HealthChecks
- **Funcionalidades**:
  - Roteamento de requisições
  - Autenticação/Autorização (futuro)
  - Rate Limiting
  - Load Balancing

### 2. **WowRandomizer.GameData.Service**
- **Responsabilidade**: Gerenciar dados estáticos do jogo (raças, classes, facções, profissões)
- **Tecnologias**: Carter, MediatR, EF Core, SQL Server, Redis
- **Endpoints**:
  - GET `/api/factions` - Listar facções
  - GET `/api/races` - Listar raças
  - GET `/api/classes` - Listar classes
  - GET `/api/professions` - Listar profissões
  - GET `/api/subprofessions` - Listar sub-profissões
  - GET `/api/races/{id}/classes` - Classes disponíveis para uma raça
  - POST `/api/factions` - Criar facção (admin)
  - POST `/api/races` - Criar raça (admin)
  - etc.

### 3. **WowRandomizer.Generator.Service**
- **Responsabilidade**: Gerar personagens aleatórios com base nas regras
- **Tecnologias**: Carter, MediatR, Redis
- **Endpoints**:
  - POST `/api/generator/random` - Gerar personagem completamente aleatório
  - POST `/api/generator/custom` - Gerar personagem com parâmetros fixos
  - GET `/api/generator/history/{userId}` - Histórico de personagens gerados

### 4. **WowRandomizer.Character.Service**
- **Responsabilidade**: Persistir e gerenciar personagens gerados
- **Tecnologias**: Carter, MediatR, EF Core, SQL Server
- **Endpoints**:
  - GET `/api/characters` - Listar personagens salvos
  - GET `/api/characters/{id}` - Obter personagem específico
  - POST `/api/characters` - Salvar personagem gerado
  - DELETE `/api/characters/{id}` - Deletar personagem

---

## 🗂️ Estrutura de Pastas (Vertical Slice Architecture)

```
WowRandomizer/
│
├── src/
│   ├── ApiGateway/
│   │   └── WowRandomizer.ApiGateway/
│   │       ├── Program.cs
│   │       ├── appsettings.json
│   │       ├── yarp.json
│   │       └── HealthChecks/
│   │
│   ├── Services/
│   │   ├── GameData/
│   │   │   └── WowRandomizer.GameData.Service/
│   │   │       ├── Program.cs
│   │   │       ├── Database/
│   │   │       │   ├── AppDbContext.cs
│   │   │       │   └── Migrations/
│   │   │       ├── Features/
│   │   │       │   ├── Factions/
│   │   │       │   │   ├── CreateFaction.cs
│   │   │       │   │   ├── GetFactions.cs
│   │   │       │   │   ├── GetFactionById.cs
│   │   │       │   │   ├── Faction.cs (Entity)
│   │   │       │   │   └── FactionEndpoints.cs (Carter)
│   │   │       │   ├── Races/
│   │   │       │   │   ├── CreateRace.cs
│   │   │       │   │   ├── GetRaces.cs
│   │   │       │   │   ├── GetRaceById.cs
│   │   │       │   │   ├── GetClassesByRace.cs
│   │   │       │   │   ├── Race.cs (Entity)
│   │   │       │   │   └── RaceEndpoints.cs (Carter)
│   │   │       │   ├── Classes/
│   │   │       │   │   ├── CreateClass.cs
│   │   │       │   │   ├── GetClasses.cs
│   │   │       │   │   ├── Class.cs (Entity)
│   │   │       │   │   └── ClassEndpoints.cs (Carter)
│   │   │       │   ├── Professions/
│   │   │       │   │   └── ...
│   │   │       │   └── SubProfessions/
│   │   │       │       └── ...
│   │   │       ├── Common/
│   │   │       │   ├── Behaviors/
│   │   │       │   │   ├── ValidationBehavior.cs
│   │   │       │   │   └── LoggingBehavior.cs
│   │   │       │   ├── Extensions/
│   │   │       │   └── Mappings/
│   │   │       └── Infrastructure/
│   │   │           ├── Caching/
│   │   │           └── Events/
│   │   │
│   │   ├── Generator/
│   │   │   └── WowRandomizer.Generator.Service/
│   │   │       ├── Program.cs
│   │   │       ├── Features/
│   │   │       │   ├── GenerateRandom/
│   │   │       │   │   ├── GenerateRandomCharacter.cs
│   │   │       │   │   ├── GenerateRandomCharacterValidator.cs
│   │   │       │   │   └── GeneratorEndpoints.cs
│   │   │       │   └── GenerateCustom/
│   │   │       │       ├── GenerateCustomCharacter.cs
│   │   │       │       ├── GenerateCustomCharacterValidator.cs
│   │   │       │       └── CustomGeneratorEndpoints.cs
│   │   │       ├── Services/
│   │   │       │   ├── IRandomizerService.cs
│   │   │       │   └── RandomizerService.cs
│   │   │       └── Common/
│   │   │
│   │   └── Character/
│   │       └── WowRandomizer.Character.Service/
│   │           ├── Program.cs
│   │           ├── Database/
│   │           ├── Features/
│   │           │   ├── CreateCharacter/
│   │           │   ├── GetCharacters/
│   │           │   └── DeleteCharacter/
│   │           └── Common/
│   │
│   └── Shared/
│       └── WowRandomizer.Shared/
│           ├── Events/
│           │   ├── CharacterGenerated.cs
│           │   ├── CharacterSaved.cs
│           │   └── FactionCreated.cs
│           ├── DTOs/
│           │   ├── CharacterDto.cs
│           │   ├── FactionDto.cs
│           │   ├── RaceDto.cs
│           │   └── ClassDto.cs
│           ├── Enums/
│           │   ├── Gender.cs
│           │   └── ProfessionType.cs
│           └── Common/
│
├── docker/
│   ├── docker-compose.yml
│   ├── docker-compose.override.yml
│   └── .env
│
└── tests/
    ├── WowRandomizer.GameData.Tests/
    ├── WowRandomizer.Generator.Tests/
    └── WowRandomizer.Character.Tests/
```

---

## 🔄 Fluxo de Dados

### Caso de Uso 1: Gerar Personagem Aleatório

```
1. Cliente → API Gateway → Generator Service
   POST /api/generator/random

2. Generator Service:
   - Valida request (FluentValidation)
   - Consulta dados do GameData Service (com cache Redis)
   - Aplica lógica de randomização
   - Publica evento "CharacterGenerated" no RabbitMQ
   - Retorna personagem para cliente

3. Character Service (Consumidor):
   - Escuta evento "CharacterGenerated"
   - Salva personagem no banco de dados
   - Atualiza cache
```

### Caso de Uso 2: Gerar Personagem com Parâmetros Fixos

```
1. Cliente → API Gateway → Generator Service
   POST /api/generator/custom
   Body: { faction: "Alliance", class: "Warrior" }

2. Generator Service:
   - Valida parâmetros
   - Consulta raças válidas para Aliança + Guerreiro
   - Randomiza raça compatível
   - Randomiza profissões (2 max)
   - Randomiza sub-profissões (2 max)
   - Randomiza gênero
   - Publica evento
   - Retorna personagem
```

---

## 🗃️ Modelo de Dados

### GameData Service - SQL Server

```sql
-- Facções
Factions
- Id (int, PK)
- Name (nvarchar)
- Description (nvarchar)
- CreatedAt (datetime2)

-- Raças
Races
- Id (int, PK)
- Name (nvarchar)
- FactionId (int, FK)
- Description (nvarchar)
- CreatedAt (datetime2)

-- Classes
Classes
- Id (int, PK)
- Name (nvarchar)
- Description (nvarchar)
- CreatedAt (datetime2)

-- Relacionamento Many-to-Many
RaceClasses
- RaceId (int, FK)
- ClassId (int, FK)
- (PK composta)

-- Profissões
Professions
- Id (int, PK)
- Name (nvarchar)
- Type (int) -- 0: Profession, 1: SubProfession
- Description (nvarchar)
- CreatedAt (datetime2)
```

### Character Service - SQL Server

```sql
-- Personagens Gerados
Characters
- Id (guid, PK)
- FactionId (int)
- FactionName (nvarchar)
- RaceId (int)
- RaceName (nvarchar)
- ClassId (int)
- ClassName (nvarchar)
- Gender (int) -- 0: Male, 1: Female
- Profession1Id (int, nullable)
- Profession1Name (nvarchar, nullable)
- Profession2Id (int, nullable)
- Profession2Name (nvarchar, nullable)
- SubProfession1Id (int, nullable)
- SubProfession1Name (nvarchar, nullable)
- SubProfession2Id (int, nullable)
- SubProfession2Name (nvarchar, nullable)
- GeneratedAt (datetime2)
- SavedAt (datetime2)
```

---

## 📚 Stack Tecnológica Detalhada

### Backend
- **.NET 10**: Framework principal
- **Carter**: Minimal API endpoints
- **MediatR**: CQRS pattern
- **FluentValidation**: Validação de requests
- **Mapster**: Mapeamento de objetos
- **Entity Framework Core**: ORM
- **Polly**: Resiliência e retry policies
- **MassTransit**: Abstração sobre RabbitMQ
- **Serilog**: Logging estruturado

### Infraestrutura
- **SQL Server**: Banco de dados relacional
- **Redis**: Cache distribuído
- **RabbitMQ**: Message broker
- **YARP**: API Gateway
- **Elasticsearch + Logstash + Kibana**: Logs centralizados
- **Docker**: Containerização
- **Portainer**: Gerenciamento de containers

### Observabilidade
- **HealthChecks**: Monitoramento de saúde dos serviços
- **Serilog → Elasticsearch**: Logs centralizados
- **Kibana**: Visualização de logs

---

## 🐳 Docker Compose - Estrutura

```yaml
services:
  # Infraestrutura
  sqlserver:
    image: mcr.microsoft.com/mssql/server
    
  redis:
    image: redis
    
  rabbitmq:
    image: rabbitmq:3-management
    
  elasticsearch:
    image: docker.elastic.co/elasticsearch/elasticsearch:7.9.2
    
  kibana:
    image: docker.elastic.co/kibana/kibana:7.9.2
    
  portainer:
    image: portainer/portainer-ce
  
  # Serviços da aplicação
  apigateway:
    build: ./src/ApiGateway/WowRandomizer.ApiGateway
    depends_on:
      - gamedata-service
      - generator-service
      - character-service
    
  gamedata-service:
    build: ./src/Services/GameData/WowRandomizer.GameData.Service
    depends_on:
      - sqlserver
      - redis
      - rabbitmq
      - elasticsearch
    
  generator-service:
    build: ./src/Services/Generator/WowRandomizer.Generator.Service
    depends_on:
      - redis
      - rabbitmq
      - elasticsearch
    
  character-service:
    build: ./src/Services/Character/WowRandomizer.Character.Service
    depends_on:
      - sqlserver
      - rabbitmq
      - elasticsearch
```

---

## 📝 Passo a Passo de Implementação

### **Fase 1: Setup Inicial e Infraestrutura** ✅
1. ✅ Criar estrutura de solução com múltiplos projetos
2. ✅ Configurar Docker Compose com todas as dependências
3. ✅ Criar projeto Shared com DTOs, Events e Enums
4. ✅ Configurar Serilog + Elasticsearch em todos os serviços

### **Fase 2: GameData Service** 📊
5. Criar entidades (Faction, Race, Class, Profession)
6. Configurar Entity Framework + SQL Server
7. Implementar Features (Vertical Slices):
   - CreateFaction / GetFactions / GetFactionById
   - CreateRace / GetRaces / GetRaceById
   - CreateClass / GetClasses / GetClassById
   - CreateProfession / GetProfessions
8. Configurar Carter endpoints
9. Implementar caching com Redis
10. Adicionar FluentValidation
11. Criar seed data inicial com dados do WoW
12. Adicionar HealthChecks

### **Fase 3: Generator Service** 🎲
13. Implementar lógica de randomização
14. Criar Feature: GenerateRandomCharacter
15. Criar Feature: GenerateCustomCharacter
16. Integrar com GameData Service
17. Implementar validações de compatibilidade
18. Configurar MassTransit + RabbitMQ para publicar eventos
19. Adicionar Polly para resiliência
20. Implementar cache de dados do GameData

### **Fase 4: Character Service** 💾
21. Criar entidade Character
22. Configurar Entity Framework
23. Implementar Features:
    - CreateCharacter
    - GetCharacters
    - GetCharacterById
    - DeleteCharacter
24. Configurar MassTransit consumer para evento CharacterGenerated
25. Adicionar HealthChecks

### **Fase 5: API Gateway** 🚪
26. Configurar YARP
27. Configurar rotas para todos os serviços
28. Implementar HealthChecks agregados
29. Configurar rate limiting

### **Fase 6: Testes e Refinamentos** 🧪
30. Criar testes unitários
31. Criar testes de integração
32. Testar fluxo completo end-to-end
33. Otimizar performance
34. Documentar APIs (Swagger/OpenAPI)

### **Fase 7: Frontend (Futuro)** 🎨
35. Criar interface web
36. Implementar checkboxes e dropdowns
37. Conectar com API Gateway

---

## 🎯 Regras de Negócio

### Restrições de Raça-Classe
- Cada raça tem um conjunto específico de classes disponíveis
- Configurável via tabela `RaceClasses`

### Restrições de Profissões
- **Máximo 2 profissões principais**
- **Máximo 2 sub-profissões**
- Combinações permitidas:
  - 2 profissões + 0 sub-profissões
  - 2 profissões + 1 sub-profissão
  - 2 profissões + 2 sub-profissões
  - 1 profissão + 0-2 sub-profissões
  - 0 profissões + 0-2 sub-profissões

### Lógica de Geração Customizada
```
SE usuário escolhe Facção:
  - Filtrar raças da facção
  
SE usuário escolhe Classe:
  - Filtrar raças que podem ser essa classe
  
SE usuário escolhe ambos (Facção + Classe):
  - Filtrar raças que pertencem à facção E podem ser a classe
  - Se nenhuma raça compatível → erro de validação

Profissões SEMPRE randomizadas (sem restrição por raça/classe/facção)
```

---

## 🔒 Resiliência e Observabilidade

### Polly Policies
- **Retry**: 3 tentativas com exponential backoff
- **Circuit Breaker**: Abre após 5 falhas consecutivas
- **Timeout**: 30 segundos para operações HTTP

### HealthChecks
- SQL Server connection
- Redis connection
- RabbitMQ connection
- Elasticsearch connection
- Serviços dependentes (HTTP)

### Logging com Serilog
```csharp
Log.Information("Generating random character");
Log.Warning("No compatible races found for {Faction} and {Class}", faction, class);
Log.Error(ex, "Failed to save character");
```

---

## 📊 Dados Iniciais do WoW (Seed Data)

### Facções
- Aliança (Alliance)
- Horda (Horde)

### Raças da Aliança
- Humano (Human)
- Anão (Dwarf)
- Elfo Noturno (Night Elf)
- Gnomo (Gnome)
- Draenei
- Worgen
- Elfo do Vazio (Void Elf) - Allied Race
- Etc.

### Raças da Horda
- Orc
- Morto-Vivo (Undead)
- Tauren
- Troll
- Elfo Sangrento (Blood Elf)
- Goblin
- Etc.

### Classes
- Guerreiro (Warrior)
- Paladino (Paladin)
- Caçador (Hunter)
- Ladino (Rogue)
- Sacerdote (Priest)
- Cavaleiro da Morte (Death Knight)
- Xamã (Shaman)
- Mago (Mage)
- Bruxo (Warlock)
- Monge (Monk)
- Druida (Druid)
- Caçador de Demônios (Demon Hunter)
- Evocador (Evoker)

### Profissões
- Alquimia (Alchemy)
- Ferraria (Blacksmithing)
- Encantamento (Enchanting)
- Engenharia (Engineering)
- Herbalismo (Herbalism)
- Mineração (Mining)
- Couraria (Leatherworking)
- Joalheria (Jewelcrafting)
- Alfaiataria (Tailoring)
- Esfolamento (Skinning)
- Escrivania (Inscription)

### Sub-Profissões
- Pesca (Fishing)
- Culinária (Cooking)
- Primeiros Socorros (First Aid) - removido em expansões recentes
- Arqueologia (Archaeology)

---

## 🚀 Próximos Passos

1. **Você confirma este planejamento?**
2. **Implementamos a Fase 1 (Setup Inicial)?**
3. **Você tem a lista completa de raças e suas classes compatíveis?**

Aguardo sua confirmação para começarmos a implementação! 🎮

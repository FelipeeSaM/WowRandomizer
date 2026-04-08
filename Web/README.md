# Frontend - WowRandomizer

Frontend Vue 3 + TypeScript + Vite + TailwindCSS para o WowRandomizer.

## 🚀 Tecnologias

- **Vue 3** - Framework progressivo
- **TypeScript** - Type safety
- **Vite** - Build tool ultrarrápido
- **TailwindCSS** - Utility-first CSS
- **Vue Router** - Navegação SPA
- **Pinia** - State management
- **Axios** - Cliente HTTP

## 📦 Instalação

```bash
cd Web
npm install
```

## 🔧 Configuração

1. Copie o arquivo `.env.example` para `.env`:
```bash
cp .env.example .env
```

2. (Opcional) Configure a URL da API no `.env`:
```env
VITE_API_URL=http://localhost:6002/api
```

**Nota:** Se deixar vazio, o Vite usará o proxy configurado em `vite.config.ts` que redireciona `/api/*` para `http://localhost:6002`.

## 🏃 Executar

### Modo Desenvolvimento
```bash
npm run dev
```

O frontend estará disponível em: `http://localhost:3000`

### Build para Produção
```bash
npm run build
```

### Preview da Build
```bash
npm run preview
```

## 📁 Estrutura

```
src/
├── assets/              # Arquivos estáticos (imagens, etc)
├── components/          # Componentes reutilizáveis
│   ├── CharacterCard.vue
│   ├── CharacterDisplay.vue
│   └── CharacterGenerator.vue
├── router/              # Configuração das rotas
│   └── index.ts
├── services/            # Camada de serviços (API)
│   ├── api.ts
│   └── index.ts
├── stores/              # Pinia stores
│   ├── character.ts
│   ├── gameData.ts
│   └── index.ts
├── types/               # Tipos TypeScript
│   ├── character.ts
│   └── index.ts
├── views/               # Páginas/Views
│   ├── HomeView.vue
│   ├── GenerateView.vue
│   ├── CharactersView.vue
│   └── NotFoundView.vue
├── App.vue              # Componente raiz
├── main.ts              # Entry point
└── style.css            # Estilos globais (TailwindCSS)
```

## 🎨 Páginas

- **/** - Home com hero section
- **/generate** - Página de geração de personagens
- **/characters** - Lista de todos os personagens gerados
- **404** - Página não encontrada

## 🔌 API Endpoints

O frontend consome os seguintes endpoints do backend:

- `POST /api/character/generate/random` - Gera personagem aleatório
- `POST /api/character/generate/custom` - Gera personagem customizado
- `GET /api/characters` - Lista todos os personagens
- `GET /api/factions` - Lista facções
- `GET /api/races` - Lista raças
- `GET /api/classes` - Lista classes
- `GET /api/professions` - Lista profissões

## 🎯 Features

- ✅ Geração de personagens aleatórios
- ✅ Visualização detalhada do personagem
- ✅ Lista de personagens gerados
- ✅ Dark mode (automático via prefers-color-scheme)
- ✅ Design responsivo
- ✅ Animações suaves
- ✅ Loading states
- ✅ Tratamento de erros

## 📝 Desenvolver

### Adicionar nova página
1. Crie o arquivo em `src/views/MinhaView.vue`
2. Adicione a rota em `src/router/index.ts`

### Adicionar novo componente
1. Crie o arquivo em `src/components/MeuComponente.vue`
2. Importe onde necessário

### Adicionar nova API call
1. Adicione o método em `src/services/api.ts`
2. Use no store ou diretamente no componente

## 🚧 TODO

- [ ] Implementar geração customizada (com filtros)
- [ ] Adicionar filtros na página de personagens
- [ ] Implementar paginação
- [ ] Adicionar animações de transição entre rotas
- [ ] Melhorar acessibilidade (a11y)
- [ ] Adicionar testes unitários
- [ ] Dockerização

---

**Desenvolvido por:** FelipeeSaM  
**Repositório:** https://github.com/FelipeeSaM/WowRandomizer

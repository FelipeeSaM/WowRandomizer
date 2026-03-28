import axios, { type AxiosInstance } from 'axios'
import type {
  Character,
  CharactersListResponse,
  GenerateCustomRequest,
  FactionsResponse,
  RacesResponse,
  ClassesResponse,
  ProfessionsResponse,
} from '@/types'

// Configuração base do Axios
const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Interceptor para logging (desenvolvimento)
api.interceptors.request.use(
  (config) => {
    console.log(`[API] ${config.method?.toUpperCase()} ${config.url}`)
    return config
  },
  (error) => {
    console.error('[API Request Error]', error)
    return Promise.reject(error)
  }
)

// Interceptor para tratamento de erros
api.interceptors.response.use(
  (response) => {
    console.log(`[API] ✅ ${response.config.method?.toUpperCase()} ${response.config.url} - ${response.status}`)
    return response
  },
  (error) => {
    console.error('[API Response Error]', {
      url: error.config?.url,
      status: error.response?.status,
      message: error.message,
      data: error.response?.data,
    })
    return Promise.reject(error)
  }
)

// Character API
export const characterApi = {
  /**
   * Gera um personagem completamente aleatório
   */
  generateRandom: async (): Promise<Character> => {
    const response = await api.post<Character>('/character/generate/random')
    return response.data
  },

  /**
   * Gera um personagem customizado (pode fixar facção, raça ou classe)
   */
  generateCustom: async (params: GenerateCustomRequest): Promise<Character> => {
    const response = await api.post<Character>('/character/generate/custom', params)
    return response.data
  },

  /**
   * Lista todos os personagens gerados
   */
  getAll: async (): Promise<Character[]> => {
    const response = await api.get<CharactersListResponse>('/characters')
    return response.data.characters
  },
}

// Game Data API
export const gameDataApi = {
  /**
   * Lista todas as facções (Alliance, Horde)
   */
  getFactions: async (): Promise<FactionsResponse> => {
    const response = await api.get<FactionsResponse>('/factions')
    return response.data
  },

  /**
   * Lista todas as raças (pode filtrar por facção)
   */
  getRaces: async (factionName?: string): Promise<RacesResponse> => {
    const response = await api.get<RacesResponse>('/races', {
      params: { factionName },
    })
    return response.data
  },

  /**
   * Lista todas as classes (pode filtrar por raça)
   */
  getClasses: async (raceName?: string): Promise<ClassesResponse> => {
    const response = await api.get<ClassesResponse>('/classes', {
      params: { raceName },
    })
    return response.data
  },

  /**
   * Lista todas as profissões (primárias e secundárias)
   */
  getProfessions: async (): Promise<ProfessionsResponse> => {
    const response = await api.get<ProfessionsResponse>('/professions')
    return response.data
  },
}

// Export da instância do Axios (caso necessário para uso customizado)
export default api

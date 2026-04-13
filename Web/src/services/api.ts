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

// Base Axios configuration
const api: AxiosInstance = axios.create({
  baseURL: import.meta.env.VITE_API_URL || '/api',
  timeout: 10000,
  headers: {
    'Content-Type': 'application/json',
  },
})

// Interceptor for logging (development)
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

// Interceptor for error handling
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
   * Generates a completely random character
   */
  generateRandom: async (): Promise<Character> => {
    const response = await api.post<Character>('/character/generate/random')
    return response.data
  },

  /**
   * Generates a custom character (can fix faction, race, or class)
   */
  generateCustom: async (params: GenerateCustomRequest): Promise<Character> => {
    const response = await api.post<Character>('/character/generate/custom', params)
    return response.data
  },

  /**
   * Lists all generated characters
   */
  getAll: async (): Promise<Character[]> => {
    const response = await api.get<CharactersListResponse>('/characters')
    return response.data.characters
  },
}

// Game Data API
export const gameDataApi = {
  /**
   * Lists all factions (Alliance, Horde)
   */
  getFactions: async (): Promise<FactionsResponse> => {
    const response = await api.get<FactionsResponse>('/factions')
    return response.data
  },

  /**
   * Lists all races (can filter by faction)
   */
  getRaces: async (factionName?: string): Promise<RacesResponse> => {
    const response = await api.get<RacesResponse>('/races', {
      params: { factionName },
    })
    return response.data
  },

  /**
   * Lists all classes (can filter by race)
   */
  getClasses: async (raceName?: string): Promise<ClassesResponse> => {
    const response = await api.get<ClassesResponse>('/classes', {
      params: { raceName },
    })
    return response.data
  },

  /**
   * Lists all professions (primary and secondary)
   */
  getProfessions: async (): Promise<ProfessionsResponse> => {
    const response = await api.get<ProfessionsResponse>('/professions')
    return response.data
  },
}

// Export of the Axios instance (if needed for custom use)
export default api

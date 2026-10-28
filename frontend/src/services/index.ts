// Composition root: the one place where service instances are created and wired.
import { TokenStorage } from '../lib/storage/token-storage'
import { ApiClient } from './api-client'
import { AuthService } from './auth.service'

const apiBaseUrl = import.meta.env.VITE_API_URL ?? '/api'

const tokenStorage = new TokenStorage()
export const apiClient = new ApiClient(apiBaseUrl, tokenStorage)
export const authService = new AuthService(apiClient, tokenStorage)

export { ApiError } from './api-client'

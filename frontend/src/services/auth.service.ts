import type { TokenStorage } from '../lib/storage/token-storage'
import type { AuthResponse, LoginInput, RegisterInput, User } from '../types/auth'
import type { ApiClient } from './api-client'

export class AuthService {
  private readonly api: ApiClient
  private readonly tokens: TokenStorage

  constructor(api: ApiClient, tokens: TokenStorage) {
    this.api = api
    this.tokens = tokens
  }

  isAuthenticated(): boolean {
    return this.tokens.get() !== null
  }

  async login(input: LoginInput): Promise<User> {
    return this.storeSession(await this.api.post<AuthResponse>('/auth/login', input))
  }

  async register(input: RegisterInput): Promise<User> {
    return this.storeSession(await this.api.post<AuthResponse>('/auth/register', input))
  }

  me(): Promise<User> {
    return this.api.get<User>('/auth/me')
  }

  logout(): void {
    this.tokens.clear()
  }

  private storeSession(response: AuthResponse): User {
    this.tokens.set({ accessToken: response.accessToken, expiresAt: response.expiresAt })
    return response.user
  }
}

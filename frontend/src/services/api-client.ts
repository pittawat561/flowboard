import type { TokenStorage } from '../lib/storage/token-storage'

/** RFC 7807 body returned by the backend for errors. */
interface ProblemDetails {
  title?: string
  detail?: string
  status?: number
  errors?: Record<string, string[]>
}

export class ApiError extends Error {
  readonly status: number
  readonly fieldErrors: Record<string, string[]>

  constructor(status: number, problem: ProblemDetails) {
    super(problem.detail ?? problem.title ?? `Request failed (${status})`)
    this.name = 'ApiError'
    this.status = status
    this.fieldErrors = problem.errors ?? {}
  }
}

/** Thin fetch wrapper: JSON in/out, bearer token, ProblemDetails -> ApiError. */
export class ApiClient {
  private readonly baseUrl: string
  private readonly tokens: TokenStorage
  private onUnauthorized: (() => void) | null = null

  constructor(baseUrl: string, tokens: TokenStorage) {
    this.baseUrl = baseUrl
    this.tokens = tokens
  }

  /** Called when an authenticated request comes back 401 (expired or revoked token). */
  setUnauthorizedHandler(handler: () => void): void {
    this.onUnauthorized = handler
  }

  get<T>(path: string): Promise<T> {
    return this.request<T>('GET', path)
  }

  post<T>(path: string, body?: unknown): Promise<T> {
    return this.request<T>('POST', path, body)
  }

  put<T>(path: string, body?: unknown): Promise<T> {
    return this.request<T>('PUT', path, body)
  }

  delete<T = void>(path: string): Promise<T> {
    return this.request<T>('DELETE', path)
  }

  private async request<T>(method: string, path: string, body?: unknown): Promise<T> {
    const headers: Record<string, string> = { Accept: 'application/json' }
    if (body !== undefined) headers['Content-Type'] = 'application/json'

    const token = this.tokens.get()
    if (token) headers.Authorization = `Bearer ${token.accessToken}`

    const response = await fetch(`${this.baseUrl}${path}`, {
      method,
      headers,
      body: body === undefined ? undefined : JSON.stringify(body),
    })

    if (!response.ok) {
      if (response.status === 401 && token) this.onUnauthorized?.()
      throw new ApiError(response.status, await this.readProblem(response))
    }

    if (response.status === 204) return undefined as T
    return (await response.json()) as T
  }

  private async readProblem(response: Response): Promise<ProblemDetails> {
    try {
      return (await response.json()) as ProblemDetails
    } catch {
      return { status: response.status, title: response.statusText }
    }
  }
}

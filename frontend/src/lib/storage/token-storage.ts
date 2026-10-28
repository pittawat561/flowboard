export interface StoredToken {
  accessToken: string
  expiresAt: string
}

/** Persists the access token in localStorage and drops it once expired. */
export class TokenStorage {
  private readonly key: string

  constructor(key = 'flowboard.auth') {
    this.key = key
  }

  get(): StoredToken | null {
    try {
      const raw = localStorage.getItem(this.key)
      if (!raw) return null

      const token = JSON.parse(raw) as StoredToken
      if (new Date(token.expiresAt).getTime() <= Date.now()) {
        this.clear()
        return null
      }
      return token
    } catch {
      return null
    }
  }

  set(token: StoredToken): void {
    try {
      localStorage.setItem(this.key, JSON.stringify(token))
    } catch {
      // Storage may be unavailable (private mode); the session then lasts until reload.
    }
  }

  clear(): void {
    try {
      localStorage.removeItem(this.key)
    } catch {
      // Ignore: nothing to clear.
    }
  }
}

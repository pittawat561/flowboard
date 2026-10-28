import { useQuery, useQueryClient } from '@tanstack/react-query'
import { useCallback, useEffect, useMemo, type ReactNode } from 'react'
import { apiClient, authService } from '../../services'
import type { LoginInput, RegisterInput } from '../../types/auth'
import { AuthContext } from './auth-context'

const meQueryKey = ['auth', 'me'] as const

export function AuthProvider({ children }: { children: ReactNode }) {
  const queryClient = useQueryClient()

  const { data: user = null, isLoading } = useQuery({
    queryKey: meQueryKey,
    queryFn: () => authService.me(),
    enabled: authService.isAuthenticated(),
    retry: false,
    staleTime: Infinity,
  })

  const logout = useCallback(() => {
    authService.logout()
    queryClient.setQueryData(meQueryKey, null)
    queryClient.removeQueries({ predicate: (q) => q.queryKey[0] !== 'auth' })
  }, [queryClient])

  useEffect(() => apiClient.setUnauthorizedHandler(logout), [logout])

  const login = useCallback(
    async (input: LoginInput) => {
      const signedIn = await authService.login(input)
      queryClient.setQueryData(meQueryKey, signedIn)
      return signedIn
    },
    [queryClient],
  )

  const register = useCallback(
    async (input: RegisterInput) => {
      const created = await authService.register(input)
      queryClient.setQueryData(meQueryKey, created)
      return created
    },
    [queryClient],
  )

  const value = useMemo(
    () => ({ user, isLoading, login, register, logout }),
    [user, isLoading, login, register, logout],
  )

  return <AuthContext value={value}>{children}</AuthContext>
}


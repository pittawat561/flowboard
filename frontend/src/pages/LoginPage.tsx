import { useMutation } from '@tanstack/react-query'
import type { FormEvent } from 'react'
import { Link, Navigate, useLocation, useNavigate } from 'react-router'
import { Alert } from '../components/ui/Alert'
import { Button } from '../components/ui/Button'
import { TextField } from '../components/ui/TextField'
import { AuthLayout } from '../features/auth/AuthLayout'
import { useAuth } from '../features/auth/auth-context'
import { fieldError, formError } from '../lib/utils/errors'
import type { LoginInput } from '../types/auth'

export function LoginPage() {
  const { user, login } = useAuth()
  const navigate = useNavigate()
  const from = (useLocation().state as { from?: string } | null)?.from ?? '/'

  const mutation = useMutation({
    mutationFn: (input: LoginInput) => login(input),
    onSuccess: () => navigate(from, { replace: true }),
  })

  if (user) return <Navigate to={from} replace />

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = new FormData(event.currentTarget)
    mutation.mutate({ email: String(form.get('email')), password: String(form.get('password')) })
  }

  const message = formError(mutation.error)

  return (
    <AuthLayout
      title="Welcome back"
      subtitle="Sign in to continue to your boards."
      footer={
        <>
          No account yet?{' '}
          <Link to="/register" className="font-semibold text-brand-600 hover:underline">
            Create one
          </Link>
        </>
      }
    >
      <form className="space-y-4" onSubmit={handleSubmit} noValidate>
        {message && <Alert>{message}</Alert>}
        <TextField label="Email" name="email" type="email" autoComplete="email" required error={fieldError(mutation.error, 'email')} />
        <TextField label="Password" name="password" type="password" autoComplete="current-password" required error={fieldError(mutation.error, 'password')} />
        <Button type="submit" className="w-full" loading={mutation.isPending}>
          Sign in
        </Button>
      </form>
    </AuthLayout>
  )
}

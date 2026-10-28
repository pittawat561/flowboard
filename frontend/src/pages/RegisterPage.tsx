import { useMutation } from '@tanstack/react-query'
import type { FormEvent } from 'react'
import { Link, Navigate, useNavigate } from 'react-router'
import { Alert } from '../components/ui/Alert'
import { Button } from '../components/ui/Button'
import { TextField } from '../components/ui/TextField'
import { AuthLayout } from '../features/auth/AuthLayout'
import { useAuth } from '../features/auth/auth-context'
import { fieldError, formError } from '../lib/utils/errors'
import type { RegisterInput } from '../types/auth'

export function RegisterPage() {
  const { user, register } = useAuth()
  const navigate = useNavigate()

  const mutation = useMutation({
    mutationFn: (input: RegisterInput) => register(input),
    onSuccess: () => navigate('/', { replace: true }),
  })

  if (user) return <Navigate to="/" replace />

  function handleSubmit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = new FormData(event.currentTarget)
    mutation.mutate({
      displayName: String(form.get('displayName')),
      email: String(form.get('email')),
      password: String(form.get('password')),
    })
  }

  const message = formError(mutation.error)

  return (
    <AuthLayout
      title="Create your account"
      subtitle="Start organising work with your team."
      footer={
        <>
          Already have an account?{' '}
          <Link to="/login" className="font-semibold text-brand-600 hover:underline">
            Sign in
          </Link>
        </>
      }
    >
      <form className="space-y-4" onSubmit={handleSubmit} noValidate>
        {message && <Alert>{message}</Alert>}
        <TextField label="Display name" name="displayName" autoComplete="name" required error={fieldError(mutation.error, 'displayName')} />
        <TextField label="Email" name="email" type="email" autoComplete="email" required error={fieldError(mutation.error, 'email')} />
        <TextField
          label="Password"
          name="password"
          type="password"
          autoComplete="new-password"
          minLength={8}
          required
          placeholder="At least 8 characters"
          error={fieldError(mutation.error, 'password')}
        />
        <Button type="submit" className="w-full" loading={mutation.isPending}>
          Create account
        </Button>
      </form>
    </AuthLayout>
  )
}

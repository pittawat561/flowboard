import { ApiError } from '../../services/api-client'

/** First validation message for a field, matching ASP.NET's PascalCase keys case-insensitively. */
export function fieldError(error: unknown, field: string): string | undefined {
  if (!(error instanceof ApiError)) return undefined
  const key = Object.keys(error.fieldErrors).find((k) => k.toLowerCase() === field.toLowerCase())
  return key ? error.fieldErrors[key][0] : undefined
}

/** Top-level message to show in an alert, or undefined when only field errors exist. */
export function formError(error: unknown): string | undefined {
  if (!error) return undefined
  if (error instanceof ApiError) {
    return Object.keys(error.fieldErrors).length > 0 ? undefined : error.message
  }
  return 'Cannot reach the server. Please try again.'
}

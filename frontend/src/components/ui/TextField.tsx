import { useId, type InputHTMLAttributes } from 'react'
import { cn } from '../../lib/utils/cn'

interface TextFieldProps extends InputHTMLAttributes<HTMLInputElement> {
  label: string
  error?: string
}

export function TextField({ label, error, className, id, ...rest }: TextFieldProps) {
  const generatedId = useId()
  const inputId = id ?? generatedId
  const errorId = `${inputId}-error`

  return (
    <div className="space-y-1.5">
      <label htmlFor={inputId} className="block text-sm font-medium text-slate-700">
        {label}
      </label>
      <input
        id={inputId}
        aria-invalid={error ? true : undefined}
        aria-describedby={error ? errorId : undefined}
        className={cn(
          'block w-full rounded-lg border bg-white px-3 py-2 text-sm shadow-xs outline-none',
          'focus:border-brand-500 focus:ring-2 focus:ring-brand-500/20',
          error ? 'border-red-400' : 'border-slate-300',
          className,
        )}
        {...rest}
      />
      {error && (
        <p id={errorId} className="text-xs text-red-600">
          {error}
        </p>
      )}
    </div>
  )
}

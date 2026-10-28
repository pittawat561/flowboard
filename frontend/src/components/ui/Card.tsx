import type { HTMLAttributes } from 'react'
import { cn } from '../../lib/utils/cn'

export function Card({ className, ...rest }: HTMLAttributes<HTMLDivElement>) {
  return <div className={cn('rounded-2xl bg-white p-6 shadow-sm ring-1 ring-slate-200', className)} {...rest} />
}

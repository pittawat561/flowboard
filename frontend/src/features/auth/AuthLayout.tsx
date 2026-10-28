import type { ReactNode } from 'react'
import { Card } from '../../components/ui/Card'
import { Logo } from '../../components/ui/Logo'

interface AuthLayoutProps {
  title: string
  subtitle: string
  children: ReactNode
  footer: ReactNode
}

export function AuthLayout({ title, subtitle, children, footer }: AuthLayoutProps) {
  return (
    <div className="grid min-h-screen place-items-center bg-gradient-to-br from-brand-50 to-slate-100 px-4">
      <div className="w-full max-w-sm space-y-6">
        <div className="flex justify-center">
          <Logo />
        </div>
        <Card className="space-y-5">
          <div>
            <h1 className="text-xl font-semibold">{title}</h1>
            <p className="mt-1 text-sm text-slate-500">{subtitle}</p>
          </div>
          {children}
        </Card>
        <p className="text-center text-sm text-slate-600">{footer}</p>
      </div>
    </div>
  )
}

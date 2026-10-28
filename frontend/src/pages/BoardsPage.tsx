import { Card } from '../components/ui/Card'
import { useAuth } from '../features/auth/auth-context'

// Placeholder until Phase 2 adds board CRUD.
export function BoardsPage() {
  const { user } = useAuth()

  return (
    <div className="space-y-6">
      <div>
        <h1 className="text-2xl font-bold">Your boards</h1>
        <p className="mt-1 text-slate-500">Hi {user?.displayName}, boards are coming in the next phase.</p>
      </div>
      <Card className="grid place-items-center border-2 border-dashed border-slate-200 py-16 text-slate-400 shadow-none ring-0">
        No boards yet
      </Card>
    </div>
  )
}

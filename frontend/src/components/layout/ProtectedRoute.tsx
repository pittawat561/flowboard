import { Navigate, Outlet, useLocation } from 'react-router'
import { useAuth } from '../../features/auth/auth-context'

/** Renders child routes only for signed-in users; otherwise redirects to /login. */
export function ProtectedRoute() {
  const { user, isLoading } = useAuth()
  const location = useLocation()

  if (isLoading) {
    return <div className="grid min-h-screen place-items-center text-sm text-slate-500">Loading…</div>
  }

  if (!user) {
    return <Navigate to="/login" replace state={{ from: location.pathname }} />
  }

  return <Outlet />
}

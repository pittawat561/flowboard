import { clsx, type ClassValue } from 'clsx'

/** Joins conditional class names into one string. */
export function cn(...values: ClassValue[]): string {
  return clsx(values)
}

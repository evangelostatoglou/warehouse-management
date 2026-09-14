import { useState } from 'react'
import './App.css'

type Page =
  | 'Dashboard'
  | 'Products'
  | 'Warehouses'
  | 'Inventory'
  | 'Customers'
  | 'Suppliers'
  | 'Orders'
  | 'Purchase orders'
  | 'Users'
  | 'Stock movements'
  | 'Audit log'

type ApiResource = {
  page: Exclude<Page, 'Dashboard' | 'Stock movements' | 'Audit log'>
  endpoint: string
  description: string
}

const resources: ApiResource[] = [
  { page: 'Products', endpoint: '/api/Products', description: 'Catalogue and reorder levels.' },
  { page: 'Warehouses', endpoint: '/api/Warehouses', description: 'Physical warehouse locations.' },
  { page: 'Inventory', endpoint: '/api/Inventory', description: 'Stock by product and warehouse.' },
  { page: 'Customers', endpoint: '/api/Customers', description: 'Customer records.' },
  { page: 'Suppliers', endpoint: '/api/Suppliers', description: 'Supplier records.' },
  { page: 'Orders', endpoint: '/api/Orders', description: 'Customer orders and their current status.' },
  { page: 'Purchase orders', endpoint: '/api/PurchaseOrders', description: 'Incoming stock orders.' },
  { page: 'Users', endpoint: '/api/Users', description: 'Employee user accounts.' },
]

const plannedPages = {
  'Stock movements': 'This page will show purchases, sales, transfers, returns, and adjustments once its API is available.',
  'Audit log': 'This page will show important employee actions once its API is available.',
}

const navigation: Page[] = [
  'Dashboard',
  ...resources.map((resource) => resource.page),
  'Stock movements',
  'Audit log',
]

function titleFromKey(key: string) {
  return key.replace(/([A-Z])/g, ' $1').replace(/^./, (letter) => letter.toUpperCase())
}

function formatCell(value: unknown) {
  if (typeof value === 'boolean') return value ? 'Active' : 'Inactive'
  if (typeof value === 'number') return value.toLocaleString()
  if (typeof value === 'string' && value.includes('T')) return new Date(value).toLocaleString()
  if (Array.isArray(value)) return `${value.length} item(s)`
  if (value === null || value === undefined || value === '') return '—'
  return String(value)
}

function App() {
  const [page, setPage] = useState<Page>('Dashboard')
  const [rows, setRows] = useState<Record<string, unknown>[]>([])
  const [loading, setLoading] = useState(false)
  const [error, setError] = useState('')
  const [lastUpdated, setLastUpdated] = useState('')
  const [dashboard, setDashboard] = useState({ products: 0, inventory: 0, orders: 0, lowStock: 0 })

  const selectedResource = resources.find((resource) => resource.page === page)

  async function getJson<T>(endpoint: string): Promise<T> {
    const response = await fetch(endpoint)

    if (!response.ok) {
      throw new Error(`The server returned ${response.status} ${response.statusText}.`)
    }

    return response.json() as Promise<T>
  }

  async function loadDashboard() {
    setLoading(true)
    setError('')

    try {
      const [products, inventory, orders, lowStock] = await Promise.all([
        getJson<unknown[]>('/api/Products'),
        getJson<unknown[]>('/api/Inventory'),
        getJson<unknown[]>('/api/Orders'),
        getJson<unknown[]>('/api/Inventory/low-stock'),
      ])

      setDashboard({
        products: products.length,
        inventory: inventory.length,
        orders: orders.length,
        lowStock: lowStock.length,
      })
      setLastUpdated(new Date().toLocaleTimeString())
    } catch (requestError) {
      setError(requestError instanceof Error ? requestError.message : 'Could not reach the server.')
    } finally {
      setLoading(false)
    }
  }

  async function loadResource(resource: ApiResource) {
    setLoading(true)
    setError('')

    try {
      const data = await getJson<Record<string, unknown>[]>(resource.endpoint)
      setRows(data)
      setLastUpdated(new Date().toLocaleTimeString())
    } catch (requestError) {
      setRows([])
      setError(requestError instanceof Error ? requestError.message : 'Could not reach the server.')
    } finally {
      setLoading(false)
    }
  }

  const columns = rows.length > 0 ? Object.keys(rows[0]) : []

  function selectPage(nextPage: Page) {
    setPage(nextPage)

    if (nextPage === 'Dashboard') {
      void loadDashboard()
      return
    }

    const nextResource = resources.find((resource) => resource.page === nextPage)
    if (nextResource) void loadResource(nextResource)
  }

  function refresh() {
    if (page === 'Dashboard') void loadDashboard()
    if (selectedResource) void loadResource(selectedResource)
  }

  const pageIsPlanned = page === 'Stock movements' || page === 'Audit log'

  return (
    <main className="app-shell">
      <aside className="sidebar">
        <div className="brand">
          <span className="brand-mark">W</span>
          <div>
            <strong>Warehouse</strong>
            <small>Management</small>
          </div>
        </div>

        <nav aria-label="Main navigation">
          {navigation.map((item) => (
            <button
              className={page === item ? 'nav-item active' : 'nav-item'}
              key={item}
              onClick={() => selectPage(item)}
            >
              {item}
              {(item === 'Stock movements' || item === 'Audit log') && <span className="soon">Soon</span>}
            </button>
          ))}
        </nav>

        <p className="sidebar-note">A simple API test frontend while the full platform is being built.</p>
      </aside>

      <section className="content">
        <header className="topbar">
          <div>
            <p className="eyebrow">Warehouse Management System</p>
            <h1>{page}</h1>
          </div>
          <button className="refresh-button" onClick={refresh} disabled={loading || pageIsPlanned}>
            {loading ? 'Loading…' : 'Refresh data'}
          </button>
        </header>

        <div className="connection-status">
          <span className={error ? 'status-dot error' : 'status-dot'} />
          <span>{error ? 'Connection needs attention' : 'API route: http://localhost:5175'}</span>
          {lastUpdated && <span className="last-updated">Updated {lastUpdated}</span>}
        </div>

        {page === 'Dashboard' && (
          <>
            <section className="intro-card">
              <p className="eyebrow">Live API check</p>
              <h2>See your warehouse data in one place.</h2>
              <p>These totals are loaded from the ASP.NET API. Start the server, then refresh this page to confirm the connection.</p>
            </section>

            <section className="metric-grid" aria-label="Dashboard totals">
              <MetricCard label="Products" value={dashboard.products} note="/api/Products" />
              <MetricCard label="Inventory records" value={dashboard.inventory} note="/api/Inventory" />
              <MetricCard label="Orders" value={dashboard.orders} note="/api/Orders" />
              <MetricCard label="Low stock alerts" value={dashboard.lowStock} note="/api/Inventory/low-stock" attention />
            </section>
          </>
        )}

        {selectedResource && (
          <section className="data-card">
            <div className="card-heading">
              <div>
                <p className="eyebrow">Live endpoint</p>
                <h2>{selectedResource.description}</h2>
              </div>
              <code>{selectedResource.endpoint}</code>
            </div>

            {error ? (
              <ConnectionHelp message={error} />
            ) : loading ? (
              <p className="empty-state">Loading data from the API…</p>
            ) : rows.length === 0 ? (
              <p className="empty-state">The server responded successfully, but there are no records yet.</p>
            ) : (
              <div className="table-scroll">
                <table>
                  <thead>
                    <tr>{columns.map((column) => <th key={column}>{titleFromKey(column)}</th>)}</tr>
                  </thead>
                  <tbody>
                    {rows.map((row, index) => (
                      <tr key={String(row.id ?? index)}>
                        {columns.map((column) => <td key={column}>{formatCell(row[column])}</td>)}
                      </tr>
                    ))}
                  </tbody>
                </table>
              </div>
            )}
          </section>
        )}

        {page === 'Dashboard' && error && <ConnectionHelp message={error} />}

        {pageIsPlanned && (
          <section className="planned-card">
            <span className="planned-icon">+</span>
            <div>
              <p className="eyebrow">Planned feature</p>
              <h2>{page}</h2>
              <p>{plannedPages[page]}</p>
            </div>
          </section>
        )}
      </section>
    </main>
  )
}

function MetricCard({ label, value, note, attention = false }: { label: string; value: number; note: string; attention?: boolean }) {
  return (
    <article className={attention ? 'metric-card attention' : 'metric-card'}>
      <p>{label}</p>
      <strong>{value}</strong>
      <small>{note}</small>
    </article>
  )
}

function ConnectionHelp({ message }: { message: string }) {
  return (
    <div className="connection-help">
      <strong>Could not load data.</strong>
      <p>{message}</p>
      <p>Make sure the ASP.NET server is running on <code>http://localhost:5175</code>, then refresh this page.</p>
    </div>
  )
}

export default App

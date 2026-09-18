import { useState, type FormEvent, type ReactNode } from 'react'
import { NavLink, Route, Routes } from 'react-router-dom'
import { getList, patch, post } from './api'
import type { Inventory, Order, Product, PurchaseOrder, RecordItem, Warehouse } from './types'
import './App.css'

type Field = {
  name: string
  label: string
  type?: 'number' | 'email' | 'password'
}

type EntityConfig = {
  title: string
  endpoint: string
  description: string
  fields: Field[]
  supportsStatus?: boolean
}

const entityPages: EntityConfig[] = [
  {
    title: 'Products',
    endpoint: '/api/Products',
    description: 'Create products and keep the product catalogue active or inactive.',
    supportsStatus: true,
    fields: [
      { name: 'sku', label: 'SKU' },
      { name: 'name', label: 'Name' },
      { name: 'description', label: 'Description' },
      { name: 'price', label: 'Price', type: 'number' },
      { name: 'reorderLevel', label: 'Reorder level', type: 'number' },
    ],
  },
  {
    title: 'Warehouses',
    endpoint: '/api/Warehouses',
    description: 'Create and manage physical warehouse locations.',
    supportsStatus: true,
    fields: [
      { name: 'name', label: 'Name' },
      { name: 'location', label: 'Location' },
    ],
  },
  {
    title: 'Customers',
    endpoint: '/api/Customers',
    description: 'Create customers used when making sales orders.',
    supportsStatus: true,
    fields: [
      { name: 'name', label: 'Name' },
      { name: 'email', label: 'Email', type: 'email' },
      { name: 'phone', label: 'Phone' },
    ],
  },
  {
    title: 'Suppliers',
    endpoint: '/api/Suppliers',
    description: 'Create suppliers used for purchase orders.',
    supportsStatus: true,
    fields: [
      { name: 'name', label: 'Name' },
      { name: 'email', label: 'Email', type: 'email' },
    ],
  },
  {
    title: 'Users',
    endpoint: '/api/Users',
    description: 'Employee users. Authentication will be connected when the backend exposes login.',
    supportsStatus: true,
    fields: [
      { name: 'name', label: 'Name' },
      { name: 'email', label: 'Email', type: 'email' },
      { name: 'passwordHash', label: 'Password hash', type: 'password' },
      { name: 'role', label: 'Role (A, W, S, or V)' },
    ],
  },
]

const menu = [
  ['/', 'Dashboard'],
  ['/products', 'Products'],
  ['/warehouses', 'Warehouses'],
  ['/inventory', 'Inventory'],
  ['/customers', 'Customers'],
  ['/suppliers', 'Suppliers'],
  ['/orders', 'Orders'],
  ['/purchase-orders', 'Purchase orders'],
  ['/users', 'Users'],
  ['/stock-movements', 'Stock movements'],
  ['/audit-log', 'Audit log'],
  ['/login', 'Login'],
]

function App() {
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
          {menu.map(([to, label]) => (
            <NavLink className="nav-item" end={to === '/'} key={to} to={to}>
              {label}
            </NavLink>
          ))}
        </nav>

        <p className="sidebar-note">A small React client designed to demonstrate the ASP.NET API and its inventory workflows.</p>
      </aside>

      <section className="content">
        <Routes>
          <Route path="/" element={<Dashboard />} />
          {entityPages.map((config) => (
            <Route key={config.endpoint} path={`/${config.title.toLowerCase().replace(' ', '-')}`} element={<EntityPage config={config} />} />
          ))}
          <Route path="/inventory" element={<InventoryPage />} />
          <Route path="/orders" element={<OrdersPage />} />
          <Route path="/purchase-orders" element={<PurchaseOrdersPage />} />
          <Route path="/stock-movements" element={<PlannedPage title="Stock movements" text="The API endpoint for stock-movement history has not been added yet." />} />
          <Route path="/audit-log" element={<PlannedPage title="Audit log" text="The API endpoint for audit history has not been added yet." />} />
          <Route path="/login" element={<PlannedPage title="Login" text="The backend has no authentication endpoint yet, so this client does not pretend to log users in." />} />
        </Routes>
      </section>
    </main>
  )
}

function PageHeader({ title, description, onRefresh, loading }: { title: string; description: string; onRefresh?: () => void; loading?: boolean }) {
  return (
    <header className="page-header">
      <div>
        <p className="eyebrow">Warehouse Management System</p>
        <h1>{title}</h1>
        <p className="page-description">{description}</p>
      </div>
      {onRefresh && <button className="button" disabled={loading} onClick={onRefresh}>{loading ? 'Loading…' : 'Load / refresh'}</button>}
    </header>
  )
}

function Dashboard() {
  const [numbers, setNumbers] = useState({ products: 0, lowStock: 0, ordersToday: 0, openPurchaseOrders: 0 })
  const [message, setMessage] = useState('Press Load / refresh to check the API.')
  const [loading, setLoading] = useState(false)

  async function loadDashboard() {
    setLoading(true)
    setMessage('')

    try {
      const [products, lowStock, orders, purchaseOrders] = await Promise.all([
        getList<RecordItem>('/api/Products'),
        getList<RecordItem>('/api/Inventory/low-stock'),
        getList<Order>('/api/Orders'),
        getList<PurchaseOrder>('/api/PurchaseOrders'),
      ])
      const today = new Date().toDateString()

      setNumbers({
        products: products.length,
        lowStock: lowStock.length,
        ordersToday: orders.filter((order) => new Date(order.createdAt).toDateString() === today).length,
        openPurchaseOrders: purchaseOrders.filter((purchaseOrder) => purchaseOrder.status.toLowerCase() !== 'received').length,
      })
      setMessage('Live data loaded successfully.')
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not reach the API.')
    } finally {
      setLoading(false)
    }
  }

  return (
    <>
      <PageHeader title="Dashboard" description="Small live totals from the existing backend endpoints." loading={loading} onRefresh={loadDashboard} />
      <p className="message">{message}</p>
      <section className="metric-grid">
        <Metric label="Products" value={numbers.products} />
        <Metric label="Low-stock items" value={numbers.lowStock} />
        <Metric label="Orders today" value={numbers.ordersToday} />
        <Metric label="Open purchase orders" value={numbers.openPurchaseOrders} />
      </section>
    </>
  )
}

function EntityPage({ config }: { config: EntityConfig }) {
  const [rows, setRows] = useState<RecordItem[]>([])
  const [message, setMessage] = useState('Load records to view the current API data.')
  const [loading, setLoading] = useState(false)

  async function loadRows() {
    setLoading(true)
    setMessage('')
    try {
      setRows(await getList<RecordItem>(config.endpoint))
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not load records.')
    } finally {
      setLoading(false)
    }
  }

  async function submit(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = new FormData(event.currentTarget)
    const body = Object.fromEntries(config.fields.map((field) => {
      const value = String(form.get(field.name) ?? '')
      return [field.name, field.type === 'number' ? Number(value) : value]
    }))

    try {
      await post(config.endpoint, body)
      event.currentTarget.reset()
      setMessage(`${config.title.slice(0, -1)} created.`)
      await loadRows()
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not create the record.')
    }
  }

  async function toggleStatus(row: RecordItem) {
    try {
      await patch(`${config.endpoint}/${row.id}/status`, { isActive: !row.isActive })
      await loadRows()
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not update the status.')
    }
  }

  return (
    <>
      <PageHeader title={config.title} description={config.description} loading={loading} onRefresh={loadRows} />
      <section className="two-column">
        <form className="card form-card" onSubmit={submit}>
          <h2>Add {config.title.slice(0, -1)}</h2>
          {config.fields.map((field) => <FieldInput field={field} key={field.name} />)}
          <button className="button" type="submit">Create</button>
          <p className="message">{message}</p>
        </form>
        <DataTable rows={rows} onStatusChange={config.supportsStatus ? toggleStatus : undefined} />
      </section>
    </>
  )
}

function InventoryPage() {
  const [rows, setRows] = useState<Inventory[]>([])
  const [products, setProducts] = useState<Product[]>([])
  const [warehouses, setWarehouses] = useState<Warehouse[]>([])
  const [message, setMessage] = useState('Load inventory to see available stock by warehouse.')
  const [loading, setLoading] = useState(false)

  async function loadInventory() {
    setLoading(true)
    try {
      const [inventory, productList, warehouseList] = await Promise.all([
        getList<Inventory>('/api/Inventory'),
        getList<Product>('/api/Products'),
        getList<Warehouse>('/api/Warehouses'),
      ])
      setRows(inventory)
      setProducts(productList)
      setWarehouses(warehouseList)
      setMessage('')
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not load inventory.')
    } finally {
      setLoading(false)
    }
  }

  async function submitInventory(event: FormEvent<HTMLFormElement>, endpoint: string) {
    event.preventDefault()
    const form = new FormData(event.currentTarget)
    const body = Object.fromEntries([...form.entries()].map(([key, value]) => [key, Number(value)]))

    try {
      await post(endpoint, body)
      event.currentTarget.reset()
      setMessage('Inventory updated.')
      await loadInventory()
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not update inventory.')
    }
  }

  const productName = (id: number) => products.find((product) => product.id === id)?.name ?? `Product #${id}`
  const warehouseName = (id: number) => warehouses.find((warehouse) => warehouse.id === id)?.name ?? `Warehouse #${id}`

  return (
    <>
      <PageHeader title="Inventory" description="View stock by location, then use the existing adjustment and transfer API endpoints." loading={loading} onRefresh={loadInventory} />
      <p className="message">{message}</p>
      <section className="two-column">
        <div className="form-stack">
          <form className="card form-card" onSubmit={(event) => submitInventory(event, '/api/Inventory/adjust')}>
            <h2>Adjust stock</h2>
            <NumberInput name="productId" label="Product ID" />
            <NumberInput name="warehouseId" label="Warehouse ID" />
            <NumberInput name="quantityChange" label="Change (+ or -)" />
            <button className="button" type="submit">Adjust</button>
          </form>
          <form className="card form-card" onSubmit={(event) => submitInventory(event, '/api/Inventory/transfer')}>
            <h2>Transfer stock</h2>
            <NumberInput name="productId" label="Product ID" />
            <NumberInput name="sourceWarehouseId" label="From warehouse ID" />
            <NumberInput name="destinationWarehouseId" label="To warehouse ID" />
            <NumberInput name="quantity" label="Quantity" />
            <button className="button" type="submit">Transfer</button>
          </form>
        </div>
        <div className="card table-card">
          <h2>Inventory records</h2>
          <div className="table-scroll">
            <table>
              <thead><tr><th>Product</th><th>Warehouse</th><th>On hand</th><th>Reserved</th><th>Available</th></tr></thead>
              <tbody>{rows.map((row) => <tr key={row.id}><td>{productName(row.productId)}</td><td>{warehouseName(row.warehouseId)}</td><td>{row.quantityOnHand}</td><td>{row.quantityReserved}</td><td>{row.availableQuantity}</td></tr>)}</tbody>
            </table>
          </div>
        </div>
      </section>
    </>
  )
}

function OrdersPage() {
  const [rows, setRows] = useState<Order[]>([])
  const [message, setMessage] = useState('Load orders to view their status.')
  const [loading, setLoading] = useState(false)

  async function loadOrders() {
    setLoading(true)
    try {
      setRows(await getList<Order>('/api/Orders'))
      setMessage('')
    } catch (error) {
      setMessage(error instanceof Error ? error.message : 'Could not load orders.')
    } finally { setLoading(false) }
  }

  async function createOrder(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = new FormData(event.currentTarget)
    const value = (name: string) => Number(form.get(name))
    try {
      await post('/api/Orders', { customerId: value('customerId'), warehouseId: value('warehouseId'), createdBy: value('createdBy'), items: [{ productId: value('productId'), quantity: value('quantity') }] })
      event.currentTarget.reset()
      setMessage('Order created.')
      await loadOrders()
    } catch (error) { setMessage(error instanceof Error ? error.message : 'Could not create the order.') }
  }

  async function changeOrder(order: Order, action: 'confirm' | 'ship' | 'cancel') {
    try {
      await post(`/api/Orders/${order.id}/${action}`)
      await loadOrders()
    } catch (error) { setMessage(error instanceof Error ? error.message : 'Order action failed.') }
  }

  return <WorkflowPage title="Orders" description="Create a simple one-item order, then call the backend workflow actions." loading={loading} onRefresh={loadOrders} message={message} form={<form className="card form-card" onSubmit={createOrder}><h2>Create order</h2><NumberInput name="customerId" label="Customer ID" /><NumberInput name="warehouseId" label="Warehouse ID" /><NumberInput name="createdBy" label="Created by user ID" /><NumberInput name="productId" label="Product ID" /><NumberInput name="quantity" label="Quantity" /><button className="button" type="submit">Create order</button></form>} table={<OrderTable rows={rows} onAction={changeOrder} />} />
}

function PurchaseOrdersPage() {
  const [rows, setRows] = useState<PurchaseOrder[]>([])
  const [message, setMessage] = useState('Load purchase orders to view their status.')
  const [loading, setLoading] = useState(false)

  async function loadPurchaseOrders() {
    setLoading(true)
    try { setRows(await getList<PurchaseOrder>('/api/PurchaseOrders')); setMessage('') }
    catch (error) { setMessage(error instanceof Error ? error.message : 'Could not load purchase orders.') }
    finally { setLoading(false) }
  }

  async function createPurchaseOrder(event: FormEvent<HTMLFormElement>) {
    event.preventDefault()
    const form = new FormData(event.currentTarget)
    const value = (name: string) => Number(form.get(name))
    try {
      await post('/api/PurchaseOrders', { supplierId: value('supplierId'), warehouseId: value('warehouseId'), createdBy: value('createdBy'), items: [{ productId: value('productId'), quantity: value('quantity'), unitCost: value('unitCost') }] })
      event.currentTarget.reset()
      setMessage('Purchase order created.')
      await loadPurchaseOrders()
    } catch (error) { setMessage(error instanceof Error ? error.message : 'Could not create the purchase order.') }
  }

  async function receive(purchaseOrder: PurchaseOrder) {
    try { await post(`/api/PurchaseOrders/${purchaseOrder.id}/receive`); await loadPurchaseOrders() }
    catch (error) { setMessage(error instanceof Error ? error.message : 'Could not receive the purchase order.') }
  }

  return <WorkflowPage title="Purchase orders" description="Create a one-item purchase order, then receive it to update stock." loading={loading} onRefresh={loadPurchaseOrders} message={message} form={<form className="card form-card" onSubmit={createPurchaseOrder}><h2>Create purchase order</h2><NumberInput name="supplierId" label="Supplier ID" /><NumberInput name="warehouseId" label="Warehouse ID" /><NumberInput name="createdBy" label="Created by user ID" /><NumberInput name="productId" label="Product ID" /><NumberInput name="quantity" label="Quantity" /><NumberInput name="unitCost" label="Unit cost" /><button className="button" type="submit">Create purchase order</button></form>} table={<PurchaseOrderTable rows={rows} onReceive={receive} />} />
}

function WorkflowPage({ title, description, loading, onRefresh, message, form, table }: { title: string; description: string; loading: boolean; onRefresh: () => void; message: string; form: ReactNode; table: ReactNode }) {
  return <><PageHeader title={title} description={description} loading={loading} onRefresh={onRefresh} /><p className="message">{message}</p><section className="two-column">{form}{table}</section></>
}

function DataTable({ rows, onStatusChange }: { rows: RecordItem[]; onStatusChange?: (row: RecordItem) => void }) {
  const columns = rows.length > 0 ? Object.keys(rows[0]) : []
  return <div className="card table-card"><h2>Records</h2>{rows.length === 0 ? <p className="empty-state">No records loaded yet.</p> : <div className="table-scroll"><table><thead><tr>{columns.map((column) => <th key={column}>{readable(column)}</th>)}{onStatusChange && <th>Action</th>}</tr></thead><tbody>{rows.map((row) => <tr key={row.id}>{columns.map((column) => <td key={column}>{format(row[column])}</td>)}{onStatusChange && <td><button className="text-button" onClick={() => onStatusChange(row)}>{row.isActive ? 'Deactivate' : 'Activate'}</button></td>}</tr>)}</tbody></table></div>}</div>
}

function OrderTable({ rows, onAction }: { rows: Order[]; onAction: (order: Order, action: 'confirm' | 'ship' | 'cancel') => void }) {
  return <div className="card table-card"><h2>Orders</h2><div className="table-scroll"><table><thead><tr><th>ID</th><th>Status</th><th>Customer</th><th>Warehouse</th><th>Total</th><th>Actions</th></tr></thead><tbody>{rows.map((row) => <tr key={row.id}><td>{row.id}</td><td><Status value={row.status} /></td><td>{row.customerId}</td><td>{row.warehouseId}</td><td>{row.totalAmount}</td><td className="action-cell"><button onClick={() => onAction(row, 'confirm')}>Confirm</button><button onClick={() => onAction(row, 'ship')}>Ship</button><button onClick={() => onAction(row, 'cancel')}>Cancel</button></td></tr>)}</tbody></table></div></div>
}

function PurchaseOrderTable({ rows, onReceive }: { rows: PurchaseOrder[]; onReceive: (purchaseOrder: PurchaseOrder) => void }) {
  return <div className="card table-card"><h2>Purchase orders</h2><div className="table-scroll"><table><thead><tr><th>ID</th><th>Status</th><th>Supplier</th><th>Warehouse</th><th>Action</th></tr></thead><tbody>{rows.map((row) => <tr key={row.id}><td>{row.id}</td><td><Status value={row.status} /></td><td>{row.supplierId}</td><td>{row.warehouseId}</td><td><button className="text-button" onClick={() => onReceive(row)}>Receive</button></td></tr>)}</tbody></table></div></div>
}

function PlannedPage({ title, text }: { title: string; text: string }) {
  return <><PageHeader title={title} description="Planned frontend area." /><section className="card planned-card"><h2>Not connected yet</h2><p>{text}</p><p>This keeps the frontend honest: the page is ready in the navigation, but no request is made until the backend feature exists.</p></section></>
}

function FieldInput({ field }: { field: Field }) { return <label>{field.label}<input name={field.name} required type={field.type ?? 'text'} step={field.type === 'number' ? 'any' : undefined} /></label> }
function NumberInput({ name, label }: { name: string; label: string }) { return <label>{label}<input name={name} required type="number" step="any" /></label> }
function Metric({ label, value }: { label: string; value: number }) { return <article className="metric-card"><p>{label}</p><strong>{value}</strong></article> }
function Status({ value }: { value: string }) { return <span className="status">{value}</span> }
function readable(value: string) { return value.replace(/([A-Z])/g, ' $1').replace(/^./, (letter) => letter.toUpperCase()) }
function format(value: unknown) { if (typeof value === 'boolean') return value ? 'Active' : 'Inactive'; if (Array.isArray(value)) return `${value.length} item(s)`; if (value === null || value === undefined || value === '') return '—'; return String(value) }

export default App

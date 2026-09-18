export type RecordItem = Record<string, unknown> & {
  id: number
  isActive?: boolean
}

export type Product = RecordItem & {
  name: string
  sku: string
}

export type Warehouse = RecordItem & {
  name: string
  location: string
}

export type Inventory = RecordItem & {
  productId: number
  warehouseId: number
  quantityOnHand: number
  quantityReserved: number
  availableQuantity: number
}

export type Order = RecordItem & {
  customerId: number
  warehouseId: number
  status: string
  createdAt: string
  totalAmount: number
}

export type PurchaseOrder = RecordItem & {
  supplierId: number
  warehouseId: number
  status: string
  createdAt: string
}

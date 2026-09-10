export interface AuthUser { userId: string; name: string; email: string; role: string; }
export interface AuthResponse extends AuthUser { token: string; }
export interface DashboardData {
  totalContracts: number;
  activeContracts: number;
  expiredContracts: number;
  pendingApprovalContracts: number;
  totalValue: number;
  expiringIn30Days: number;
}
export interface Contract {
  id: string;
  number: string;
  title: string;
  description: string;
  supplierId?: string | null;
  supplierName: string;
  value: number;
  startDate: string;
  endDate: string;
  status: string;
  automaticRenewal: boolean;
  createdAt: string;
}
export interface Supplier {
  id: string;
  name: string;
  cnpj: string;
  email: string;
  phone: string;
  createdAt: string;
}
export interface AuditLog {
  id: string;
  userId?: string | null;
  action: string;
  entityType: string;
  entityId: string;
  details: string;
  createdAt: string;
}

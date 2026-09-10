import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable, shareReplay } from 'rxjs';
import { AuditLog, Contract, DashboardData, Supplier } from '../models/models';

@Injectable({ providedIn: 'root' })
export class DataService {
  private readonly api = 'http://localhost:5149/api';
  private contractsCache?: Observable<Contract[]>;
  private suppliersCache?: Observable<Supplier[]>;
  private dashboardCache?: Observable<DashboardData>;
  private auditCache?: Observable<AuditLog[]>;

  constructor(private http: HttpClient) {}

  contracts(force = false): Observable<Contract[]> {
    if (force || !this.contractsCache) {
      this.contractsCache = this.http.get<Contract[]>(`${this.api}/contracts`).pipe(shareReplay(1));
    }
    return this.contractsCache;
  }

  suppliers(force = false): Observable<Supplier[]> {
    if (force || !this.suppliersCache) {
      this.suppliersCache = this.http.get<Supplier[]>(`${this.api}/suppliers`).pipe(shareReplay(1));
    }
    return this.suppliersCache;
  }

  dashboard(force = false): Observable<DashboardData> {
    if (force || !this.dashboardCache) {
      this.dashboardCache = this.http.get<DashboardData>(`${this.api}/dashboard`).pipe(shareReplay(1));
    }
    return this.dashboardCache;
  }

  audit(force = false): Observable<AuditLog[]> {
    if (force || !this.auditCache) {
      this.auditCache = this.http.get<AuditLog[]>(`${this.api}/audit`).pipe(shareReplay(1));
    }
    return this.auditCache;
  }

  invalidateContracts(): void { this.contractsCache = undefined; this.dashboardCache = undefined; this.auditCache = undefined; }
  invalidateSuppliers(): void { this.suppliersCache = undefined; this.contractsCache = undefined; this.dashboardCache = undefined; }
  invalidateAudit(): void { this.auditCache = undefined; }
}

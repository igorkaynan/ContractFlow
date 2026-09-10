import { Routes } from '@angular/router';
import { Login } from './pages/login/login';
import { Shell } from './shared/shell/shell';
import { Dashboard } from './pages/dashboard/dashboard';
import { Contracts } from './pages/contracts/contracts';
import { ContractForm } from './pages/contract-form/contract-form';
import { Suppliers } from './pages/suppliers/suppliers';
import { Approvals } from './pages/approvals/approvals';
import { Audit } from './pages/audit/audit';
import { authGuard } from './guards/auth.guard';

export const routes: Routes = [
  { path: '', component: Login },
  {
    path: '', component: Shell, canActivate: [authGuard], children: [
      { path: 'dashboard', component: Dashboard },
      { path: 'contracts', component: Contracts },
      { path: 'contracts/new', component: ContractForm },
      { path: 'contracts/:id/edit', component: ContractForm },
      { path: 'suppliers', component: Suppliers },
      { path: 'approvals', component: Approvals },
      { path: 'audit', component: Audit }
    ]
  },
  { path: '**', redirectTo: '' }
];

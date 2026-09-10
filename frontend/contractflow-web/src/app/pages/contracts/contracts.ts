import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, RouterLink } from '@angular/router';
import { Contract } from '../../models/models';
import { Auth } from '../../services/auth';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-contracts',
  imports: [CommonModule, RouterLink],
  templateUrl: './contracts.html',
  styleUrl: './contracts.css'
})
export class Contracts implements OnInit {
  contracts: Contract[] = [];
  allContracts: Contract[] = [];
  filterExpiring30 = false;
  loading = true;
  errorMessage = '';
  successMessage = '';
  deletingId = '';

  constructor(private http: HttpClient, private auth: Auth, private data: DataService, private route: ActivatedRoute, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.filterExpiring30 = this.route.snapshot.queryParamMap.get('filter') === 'expiring30';
    this.loadContracts();
  }

  get canDelete(): boolean {
    const role = this.auth.getUser()?.role;
    return role === 'Admin' || role === 'Manager';
  }

  loadContracts(force = false): void {
    this.loading = true;
    this.errorMessage = '';
    this.data.contracts(force).subscribe({
      next: d => {
        this.allContracts = d;
        if (this.filterExpiring30) {
          const today = new Date(); today.setHours(0,0,0,0);
          const limit = new Date(today); limit.setDate(limit.getDate() + 30);
          this.contracts = d.filter(c => {
            const end = new Date(c.endDate); end.setHours(0,0,0,0);
            return c.status === 'Ativo' && end >= today && end <= limit;
          });
        } else {
          this.contracts = d;
        }
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => { this.errorMessage = 'Não foi possível carregar os contratos.'; this.loading = false; this.cdr.detectChanges(); }
    });
  }

  deleteContract(contract: Contract): void {
    if (!confirm(`Excluir o contrato ${contract.number}?`)) return;
    this.deletingId = contract.id;
    this.http.delete(`http://localhost:5149/api/contracts/${contract.id}`).subscribe({
      next: () => {
        this.deletingId = '';
        this.successMessage = 'Contrato excluído com sucesso.';
        this.data.invalidateContracts();
        this.loadContracts();
      },
      error: e => {
        this.deletingId = '';
        this.errorMessage = e.status === 403 ? 'Somente administradores podem excluir contratos.' : 'Não foi possível excluir o contrato.';
        this.cdr.detectChanges();
      }
    });
  }
}

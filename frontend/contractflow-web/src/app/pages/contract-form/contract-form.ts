import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { ActivatedRoute, Router, RouterLink } from '@angular/router';
import { Contract, Supplier } from '../../models/models';

@Component({
  selector: 'app-contract-form',
  imports: [CommonModule, FormsModule, RouterLink],
  templateUrl: './contract-form.html',
  styleUrl: './contract-form.css'
})
export class ContractForm implements OnInit {
  suppliers: Supplier[] = [];
  saving = false;
  loading = false;
  errorMessage = '';
  contractId: string | null = null;
  model = {
    number: '',
    title: '',
    description: '',
    supplierId: '',
    value: null as number | null,
    startDate: '',
    endDate: '',
    status: 'Rascunho',
    automaticRenewal: false
  };

  constructor(private http: HttpClient, private router: Router, private route: ActivatedRoute, private cdr: ChangeDetectorRef) {}

  get editing(): boolean { return !!this.contractId; }

  ngOnInit(): void {
    this.contractId = this.route.snapshot.paramMap.get('id');
    this.loadSuppliers();
    if (this.contractId) this.loadContract(this.contractId);
  }

  private loadSuppliers(): void {
    this.http.get<Supplier[]>('http://localhost:5149/api/suppliers').subscribe({
      next: s => { this.suppliers = s; this.cdr.detectChanges(); },
      error: () => { this.errorMessage = 'Não foi possível carregar os fornecedores.'; this.cdr.detectChanges(); }
    });
  }

  private loadContract(id: string): void {
    this.loading = true;
    this.http.get<Contract>(`http://localhost:5149/api/contracts/${id}`).subscribe({
      next: c => {
        this.model = {
          number: c.number,
          title: c.title,
          description: c.description ?? '',
          supplierId: c.supplierId ?? '',
          value: c.value,
          startDate: c.startDate.substring(0, 10),
          endDate: c.endDate.substring(0, 10),
          status: c.status === 'Vencido' ? 'Ativo' : c.status,
          automaticRenewal: c.automaticRenewal
        };
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Não foi possível carregar o contrato.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });
  }

  save(): void {
    this.errorMessage = '';
    if (!this.model.number || !this.model.title || !this.model.supplierId || !this.model.value || !this.model.startDate || !this.model.endDate) {
      this.errorMessage = 'Preencha os campos obrigatórios.';
      return;
    }
    if (Number(this.model.value) <= 0) {
      this.errorMessage = 'Informe um valor válido.';
      return;
    }
    if (new Date(this.model.endDate) < new Date(this.model.startDate)) {
      this.errorMessage = 'A data final não pode ser anterior à data inicial.';
      return;
    }

    this.saving = true;
    const payload = {
      ...this.model,
      value: Number(this.model.value),
      startDate: new Date(`${this.model.startDate}T12:00:00`).toISOString(),
      endDate: new Date(`${this.model.endDate}T12:00:00`).toISOString()
    };

    const request = this.editing
      ? this.http.put(`http://localhost:5149/api/contracts/${this.contractId}`, payload)
      : this.http.post('http://localhost:5149/api/contracts', payload);

    request.subscribe({
      next: () => this.router.navigate(['/contracts']),
      error: e => {
        this.saving = false;
        this.errorMessage = e?.error?.message || e?.error?.title || 'Não foi possível salvar o contrato.';
        this.cdr.detectChanges();
      }
    });
  }
}

import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { HttpClient } from '@angular/common/http';
import { Supplier } from '../../models/models';
import { Auth } from '../../services/auth';
import { DataService } from '../../services/data.service';

@Component({selector:'app-suppliers',imports:[CommonModule,FormsModule],templateUrl:'./suppliers.html',styleUrl:'./suppliers.css'})
export class Suppliers implements OnInit {
  suppliers: Supplier[] = [];
  showForm = false;
  saving = false;
  loading = true;
  errorMessage = '';
  successMessage = '';
  editingId: string | null = null;
  model = { name:'', cnpj:'', email:'', phone:'' };

  constructor(private http:HttpClient, private auth: Auth, private data: DataService, private cdr: ChangeDetectorRef) {}
  ngOnInit():void{this.load();}
  get canDelete(): boolean { const role = this.auth.getUser()?.role; return role === 'Admin' || role === 'Manager'; }

  load():void{
    this.loading=true; this.errorMessage='';
    this.http.get<Supplier[]>('http://localhost:5149/api/suppliers').subscribe({next:s=>{this.suppliers=s;this.loading=false;this.cdr.detectChanges();},error:()=>{this.errorMessage='Não foi possível carregar os fornecedores.';this.loading=false;this.cdr.detectChanges();}})
  }

  openNew(): void {
    this.editingId = null;
    this.model = {name:'',cnpj:'',email:'',phone:''};
    this.showForm = true;
    this.successMessage='';
  }

  edit(s: Supplier): void {
    this.editingId = s.id;
    this.model = { name:s.name, cnpj:s.cnpj, email:s.email, phone:s.phone };
    this.showForm = true;
    this.successMessage='';
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  cancel(): void { this.showForm=false; this.editingId=null; this.errorMessage=''; }

  save():void{
    this.errorMessage='';this.successMessage='';
    if(!this.model.name||!this.model.cnpj||!this.model.email){this.errorMessage='Preencha nome, CNPJ e e-mail.';return;}
    this.saving=true;
    const request = this.editingId
      ? this.http.put(`http://localhost:5149/api/suppliers/${this.editingId}`, this.model)
      : this.http.post('http://localhost:5149/api/suppliers',this.model);
    request.subscribe({next:()=>{this.saving=false;this.showForm=false;this.editingId=null;this.model={name:'',cnpj:'',email:'',phone:''};this.successMessage='Fornecedor salvo com sucesso.';this.data.invalidateSuppliers();this.load();},error:e=>{this.saving=false;this.errorMessage=typeof e?.error === 'string' ? e.error : (e?.error?.title || 'Não foi possível salvar o fornecedor.');this.cdr.detectChanges();}})
  }

  remove(s: Supplier): void {
    if (!confirm(`Excluir o fornecedor ${s.name}?`)) return;
    this.http.delete(`http://localhost:5149/api/suppliers/${s.id}`).subscribe({
      next:()=>{this.successMessage='Fornecedor excluído com sucesso.';this.data.invalidateSuppliers();this.load();},
      error:e=>{this.errorMessage=e.status===403?'Seu perfil não tem permissão para excluir fornecedores.':(typeof e?.error === 'string' ? e.error : 'Não foi possível excluir o fornecedor.');this.cdr.detectChanges();}
    });
  }
}

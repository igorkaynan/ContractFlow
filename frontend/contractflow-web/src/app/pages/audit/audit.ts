import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AuditLog } from '../../models/models';
import { DataService } from '../../services/data.service';
@Component({selector:'app-audit',imports:[CommonModule],templateUrl:'./audit.html',styleUrl:'./audit.css'})
export class Audit implements OnInit{
 logs:AuditLog[]=[];loading=true;errorMessage='';
 constructor(private data:DataService, private cdr:ChangeDetectorRef){}
 ngOnInit():void{this.data.audit().subscribe({next:l=>{this.logs=l;this.loading=false;this.cdr.detectChanges();},error:e=>{this.errorMessage=e.status===403?'Seu usuário não possui permissão para visualizar a auditoria.':'Não foi possível carregar os registros de auditoria.';this.loading=false;this.cdr.detectChanges();}})}
 label(action:string):string{const map:Record<string,string>={Create:'Criação',Update:'Atualização',Approve:'Aprovação',Reject:'Rejeição',Delete:'Exclusão'};return map[action]||action;}
}

import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { HttpClient } from '@angular/common/http';
import { Contract } from '../../models/models';
import { DataService } from '../../services/data.service';
@Component({selector:'app-approvals',imports:[CommonModule],templateUrl:'./approvals.html',styleUrl:'./approvals.css'})
export class Approvals implements OnInit{
 contracts:Contract[]=[];loading=true;message='';errorMessage='';processingId='';
 constructor(private http:HttpClient, private data:DataService, private cdr:ChangeDetectorRef){}
 ngOnInit():void{this.load();}
 load():void{this.loading=true;this.data.contracts(true).subscribe({next:c=>{this.contracts=c.filter(x=>x.status==='AguardandoAprovacao');this.loading=false;this.cdr.detectChanges();},error:e=>{this.errorMessage=e.status===403?'Seu usuário não possui permissão para aprovações.':'Não foi possível carregar as aprovações.';this.loading=false;this.cdr.detectChanges();}})}
 action(id:string,type:'approve'|'reject'):void{this.processingId=id;this.message='';this.errorMessage='';this.http.put(`http://localhost:5149/api/contracts/${id}/${type}`,{}).subscribe({next:()=>{this.processingId='';this.message=type==='approve'?'Contrato aprovado com sucesso.':'Contrato rejeitado.';this.data.invalidateContracts();this.load();},error:e=>{this.processingId='';this.errorMessage=e.status===403?'Você não possui permissão para esta ação.':'Não foi possível concluir a ação.';this.cdr.detectChanges();}})}
}

import { ChangeDetectorRef, Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Router } from '@angular/router';
import { Auth } from '../../services/auth';

@Component({ selector:'app-login', imports:[CommonModule,FormsModule], templateUrl:'./login.html', styleUrl:'./login.css' })
export class Login {
  email=''; password=''; loading=false; errorMessage='';
  constructor(private auth:Auth, private router:Router, private cdr:ChangeDetectorRef) {}
  login():void{
    this.errorMessage='';
    if(!this.email || !this.password){this.errorMessage='Informe o e-mail e a senha.';return;}
    this.loading=true;
    this.auth.login({email:this.email,password:this.password}).subscribe({
      next:()=>{this.loading=false;this.router.navigate(['/dashboard']);},
      error:e=>{this.loading=false;this.errorMessage=e.status===401?'E-mail ou senha inválidos.':'Não foi possível conectar ao servidor.';this.cdr.detectChanges();}
    });
  }
}

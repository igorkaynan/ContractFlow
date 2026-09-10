import { Component } from '@angular/core';
import { Router, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { Auth } from '../../services/auth';
import { AuthUser } from '../../models/models';

@Component({
  selector: 'app-shell',
  imports: [RouterOutlet, RouterLink, RouterLinkActive],
  templateUrl: './shell.html',
  styleUrl: './shell.css'
})
export class Shell {
  user: AuthUser | null;

  constructor(private auth: Auth, private router: Router) {
    this.user = this.auth.getUser();
  }

  get initials(): string {
    const name = this.user?.name?.trim() || 'Usuário';
    return name.split(/\s+/).slice(0, 2).map(part => part[0]).join('').toUpperCase();
  }

  get canManage(): boolean {
    return ['Admin', 'Manager'].includes(this.user?.role ?? '');
  }

  logout(): void {
    this.auth.logout();
    this.router.navigate(['/']);
  }
}

import { ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RouterLink } from '@angular/router';
import { Contract, DashboardData } from '../../models/models';
import { DataService } from '../../services/data.service';

@Component({
  selector: 'app-dashboard',
  imports: [CommonModule, RouterLink],
  templateUrl: './dashboard.html',
  styleUrl: './dashboard.css'
})
export class Dashboard implements OnInit {
  dashboard: DashboardData = {
    totalContracts: 0,
    activeContracts: 0,
    expiredContracts: 0,
    pendingApprovalContracts: 0,
    totalValue: 0,
    expiringIn30Days: 0
  };

  recentContracts: Contract[] = [];
  loading = true;
  errorMessage = '';

  constructor(private data: DataService, private cdr: ChangeDetectorRef) {}

  ngOnInit(): void {
    this.load();
  }

  load(): void {
    this.loading = true;
    this.errorMessage = '';

    this.data.dashboard().subscribe({
      next: data => {
        this.dashboard = data;
        this.loading = false;
        this.cdr.detectChanges();
      },
      error: () => {
        this.errorMessage = 'Não foi possível carregar o dashboard.';
        this.loading = false;
        this.cdr.detectChanges();
      }
    });

    this.data.contracts().subscribe({
      next: contracts => {
        this.recentContracts = contracts.slice(0, 6);
        this.cdr.detectChanges();
      },
      error: () => {}
    });
  }
}

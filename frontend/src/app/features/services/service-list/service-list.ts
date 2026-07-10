import { Component, inject, signal, OnInit } from '@angular/core';
import { DecimalPipe } from '@angular/common';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ServiceService } from '../../../core/services/service.service';
import { AuthService } from '../../../core/services/auth.service';
import { Service } from '../../../core/models/service.model';

@Component({
  selector: 'app-service-list',
  standalone: true,
  imports: [DecimalPipe, MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './service-list.html',
  styleUrl: './service-list.css'
})
export class ServiceListComponent implements OnInit {
  private serviceService = inject(ServiceService);
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  protected readonly services = signal<Service[]>([]);
  protected readonly displayedColumns = ['name', 'costPerHour', 'isActive', 'actions'];

  ngOnInit() {
    this.loadServices();
  }

  loadServices() {
    const providerId = this.authService.providerId() ?? '';

    this.serviceService.getAll(providerId).subscribe({
      next: (data) => this.services.set(data.items),
      error: () => this.snackBar.open('Error al cargar los servicios', 'Cerrar', { duration: 3000 })
    });
  }

  onCreate() {
    this.router.navigate(['/services/new']);
  }

  onEdit(service: Service) {
    this.router.navigate(['/services', service.id, 'edit']);
  }

  onDelete(service: Service) {
    if (!confirm(`¿Eliminar "${service.name}"?`)) return;

    this.serviceService.delete(service.id).subscribe({
      next: () => {
        this.snackBar.open('Servicio eliminado', 'Cerrar', { duration: 2000 });
        this.loadServices();
      },
      error: () => this.snackBar.open('Error al eliminar', 'Cerrar', { duration: 3000 })
    });
  }
}

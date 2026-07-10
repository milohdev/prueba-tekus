import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ServiceService } from '../../../core/services/service.service';

@Component({
  selector: 'app-service-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './service-form.html',
  styleUrl: './service-form.css'
})
export class ServiceFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private serviceService = inject(ServiceService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);

  protected readonly isEditMode = signal(false);
  protected readonly serviceId = signal<string | null>(null);

  protected form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(100)]],
    costPerHour: [0, [Validators.required, Validators.min(0.01)]]
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode.set(true);
      this.serviceId.set(id);
      this.loadService(id);
    }
  }

  loadService(id: string) {
    this.serviceService.getById(id).subscribe({
      next: (service) => {
        this.form.patchValue({
          name: service.name,
          costPerHour: service.costPerHour
        });
      },
      error: () => this.snackBar.open('Error al cargar el servicio', 'Cerrar', { duration: 3000 })
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    if (this.isEditMode()) {
      this.serviceService.update(this.serviceId()!, {
        id: this.serviceId()!,
        name: value.name!,
        costPerHour: value.costPerHour!
      }).subscribe({
        next: () => {
          this.snackBar.open('Servicio actualizado', 'Cerrar', { duration: 2000 });
          this.router.navigate(['/services']);
        },
        error: () => this.snackBar.open('Error al actualizar', 'Cerrar', { duration: 3000 })
      });
    } else {
      this.serviceService.create({
        name: value.name!,
        costPerHour: value.costPerHour!
      }).subscribe({
        next: () => {
          this.snackBar.open('Servicio creado', 'Cerrar', { duration: 2000 });
          this.router.navigate(['/services']);
        },
        error: () => this.snackBar.open('Error al crear', 'Cerrar', { duration: 3000 })
      });
    }
  }

  onCancel() {
    this.router.navigate(['/services']);
  }
}

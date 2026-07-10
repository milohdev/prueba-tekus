import { Component, inject } from '@angular/core';
import { Router, RouterLink } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { AuthService } from '../../../core/services/auth.service';

@Component({
  selector: 'app-provider-register',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    RouterLink,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule
  ],
  templateUrl: './provider-register.html',
  styleUrl: './provider-register.css'
})
export class ProviderRegisterComponent {
  private fb = inject(FormBuilder);
  private authService = inject(AuthService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  protected form = this.fb.group({
    name: ['', [Validators.required, Validators.maxLength(200)]],
    nit: ['', [Validators.required, Validators.maxLength(20)]],
    pageUrl: ['', [Validators.required, Validators.maxLength(500)]],
    email: ['', [Validators.required, Validators.email]],
    password: ['', [Validators.required, Validators.minLength(8)]]
  });

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    this.authService.registerProvider({
      name: value.name!,
      nit: value.nit!,
      pageUrl: value.pageUrl!,
      email: value.email!,
      password: value.password!
    }).subscribe({
      next: () => this.router.navigate(['/services']),
      error: (err) => {
        const message = err.error?.title || 'Error al registrar el proveedor';
        this.snackBar.open(message, 'Cerrar', { duration: 3000 });
      }
    });
  }
}

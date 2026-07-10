import { Component, inject, signal, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { FormBuilder, ReactiveFormsModule, Validators } from '@angular/forms';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatSelectModule } from '@angular/material/select';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ContentService } from '../../../core/services/content.service';

@Component({
  selector: 'app-content-form',
  standalone: true,
  imports: [
    ReactiveFormsModule,
    MatFormFieldModule,
    MatInputModule,
    MatSelectModule,
    MatButtonModule
  ],
  templateUrl: './content-form.html',
  styleUrl: './content-form.css'
})
export class ContentFormComponent implements OnInit {
  private fb = inject(FormBuilder);
  private contentService = inject(ContentService);
  private router = inject(Router);
  private route = inject(ActivatedRoute);
  private snackBar = inject(MatSnackBar);

  protected readonly isEditMode = signal(false);
  protected readonly contentId = signal<string | null>(null);

  protected readonly contentTypes = ['Image', 'Video', 'Text'];

  protected form = this.fb.group({
    title: ['', [Validators.required, Validators.maxLength(200)]],
    description: ['', [Validators.required, Validators.maxLength(1000)]],
    type: ['Text', Validators.required]
  });

  ngOnInit() {
    const id = this.route.snapshot.paramMap.get('id');
    if (id) {
      this.isEditMode.set(true);
      this.contentId.set(id);
      this.loadContent(id);
    }
  }

  loadContent(id: string) {
    this.contentService.getById(id).subscribe({
      next: (content) => {
        this.form.patchValue({
          title: content.title,
          description: content.description,
          type: content.type
        });
      },
      error: () => this.snackBar.open('Error al cargar el contenido', 'Cerrar', { duration: 3000 })
    });
  }

  onSubmit() {
    if (this.form.invalid) {
      this.form.markAllAsTouched();
      return;
    }

    const value = this.form.getRawValue();

    if (this.isEditMode()) {
      this.contentService.update(this.contentId()!, {
        id: this.contentId()!,
        title: value.title!,
        description: value.description!,
        type: value.type as 'Image' | 'Video' | 'Text'
      }).subscribe({
        next: () => {
          this.snackBar.open('Contenido actualizado', 'Cerrar', { duration: 2000 });
          this.router.navigate(['/contents']);
        },
        error: () => this.snackBar.open('Error al actualizar', 'Cerrar', { duration: 3000 })
      });
    } else {
      this.contentService.create({
        title: value.title!,
        description: value.description!,
        type: value.type as 'Image' | 'Video' | 'Text'
      }).subscribe({
        next: () => {
          this.snackBar.open('Contenido creado', 'Cerrar', { duration: 2000 });
          this.router.navigate(['/contents']);
        },
        error: () => this.snackBar.open('Error al crear', 'Cerrar', { duration: 3000 })
      });
    }
  }

  onCancel() {
    this.router.navigate(['/contents']);
  }
}

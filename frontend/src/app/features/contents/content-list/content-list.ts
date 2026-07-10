import { Component, inject, signal, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { MatTableModule } from '@angular/material/table';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';
import { ContentService } from '../../../core/services/content.service';
import { Content } from '../../../core/models/content.model';

@Component({
  selector: 'app-content-list',
  standalone: true,
  imports: [MatTableModule, MatButtonModule, MatIconModule],
  templateUrl: './content-list.html',
  styleUrl: './content-list.css'
})
export class ContentListComponent implements OnInit {
  private contentService = inject(ContentService);
  private router = inject(Router);
  private snackBar = inject(MatSnackBar);

  protected readonly contents = signal<Content[]>([]);
  protected readonly displayedColumns = ['title', 'description', 'type', 'isActive', 'actions'];

  ngOnInit() {
    this.loadContents();
  }

  loadContents() {
    this.contentService.getAll().subscribe({
      next: (data) => this.contents.set(data),
      error: () => this.snackBar.open('Error al cargar los contenidos', 'Cerrar', { duration: 3000 })
    });
  }

  onCreate() {
    this.router.navigate(['/contents/new']);
  }

  onEdit(content: Content) {
    this.router.navigate(['/contents', content.id, 'edit']);
  }

  onDelete(content: Content) {
    if (!confirm(`¿Eliminar "${content.title}"?`)) return;

    this.contentService.delete(content.id).subscribe({
      next: () => {
        this.snackBar.open('Contenido eliminado', 'Cerrar', { duration: 2000 });
        this.loadContents();
      },
      error: () => this.snackBar.open('Error al eliminar', 'Cerrar', { duration: 3000 })
    });
  }
}

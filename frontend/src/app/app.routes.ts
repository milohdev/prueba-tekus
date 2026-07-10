import { Routes } from '@angular/router';
import { ContentListComponent } from './features/contents/content-list/content-list';
import { ContentFormComponent } from './features/contents/content-form/content-form';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';

export const routes: Routes = [
  { path: '', redirectTo: 'contents', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'contents', component: ContentListComponent },
  { path: 'contents/new', component: ContentFormComponent },
  { path: 'contents/:id/edit', component: ContentFormComponent }
];

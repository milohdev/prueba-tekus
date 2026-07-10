import { Routes } from '@angular/router';
import { ContentListComponent } from './features/contents/content-list/content-list';
import { ContentFormComponent } from './features/contents/content-form/content-form';
import { LoginComponent } from './features/auth/login/login';
import { RegisterComponent } from './features/auth/register/register';
import { ProviderRegisterComponent } from './features/providers/provider-register/provider-register';
import { ProviderLoginComponent } from './features/providers/provider-login/provider-login';
import { ServiceListComponent } from './features/services/service-list/service-list';
import { ServiceFormComponent } from './features/services/service-form/service-form';
import { providerGuard } from './core/guards/provider.guard';

export const routes: Routes = [
  { path: '', redirectTo: 'contents', pathMatch: 'full' },
  { path: 'login', component: LoginComponent },
  { path: 'register', component: RegisterComponent },
  { path: 'contents', component: ContentListComponent },
  { path: 'contents/new', component: ContentFormComponent },
  { path: 'contents/:id/edit', component: ContentFormComponent },
  { path: 'provider/register', component: ProviderRegisterComponent },
  { path: 'provider/login', component: ProviderLoginComponent },
  { path: 'services', component: ServiceListComponent, canActivate: [providerGuard] },
  { path: 'services/new', component: ServiceFormComponent, canActivate: [providerGuard] },
  { path: 'services/:id/edit', component: ServiceFormComponent, canActivate: [providerGuard] }
];

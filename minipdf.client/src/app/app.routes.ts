import { Routes } from '@angular/router';
import { NotFound } from './common/not-found/not-found';

export const routes: Routes = [
  {
    path: '',
    component: NotFound,
  },
  //   {
  //     path: 'clients',
  //     component: ClientComponent,
  //     canActivate: [authGuard],
  //   },
  //   {
  //     path: 'services',
  //     component: JobComponent,
  //   },
  //   {
  //     path: 'tasks',
  //     component: TaskComponent,
  //   },
  //   {
  //     path: 'calendar',
  //     component: CalendarViewComponent,
  //   },
  //   {
  //     path: 'settings',
  //     component: SettingsComponent,
  //   },
  //   {
  //     path: 'cart',
  //     component: CartComponent,
  //   },
  //   {
  //     path: 'tasks/:id/details',
  //     component: TaskDetailComponent,
  //   },
  //   {
  //     path: 'checkout/success',
  //     loadComponent: () =>
  //       import(
  //         './features/checkout/checkout-success/checkout-success.component'
  //       ).then((cs) => cs.CheckoutSuccessComponent),
  //   },
  //   {
  //     path: 'account',
  //     loadChildren: () =>
  //       import('./core/routes/account-routes').then((r) => r.accountRoutes),
  //   },
  {
    path: '**',
    component: NotFound,
  },
];

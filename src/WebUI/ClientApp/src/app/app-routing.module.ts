import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { AuthorizeGuard } from 'src/api-authorization/authorize.guard';
import { CounterComponent } from './counter/counter.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { HomeComponent } from './home/home.component';
import { TodoComponent } from './todo/todo.component';
import { TokenComponent } from './token/token.component';
import { InflationTrackerComponent } from './tracker/inflation-tracker/inflation-tracker.component';

const routes: Routes = [
  { path: '', component: HomeComponent, pathMatch: 'full' },
  { path: 'counter', component: CounterComponent },
  { path: 'fetch-data', component: FetchDataComponent },
  { path: 'inflation-tracker', component: InflationTrackerComponent },
  { path: 'todo', component: TodoComponent, canActivate: [AuthorizeGuard] },
  { path: 'token', component: TokenComponent, canActivate: [AuthorizeGuard] },
  { path: 'welcome', loadChildren: () => import('./pages/welcome/welcome.module').then(m => m.WelcomeModule) },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

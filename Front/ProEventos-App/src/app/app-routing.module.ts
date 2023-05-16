import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { EventosComponent } from './componentes/eventos/eventos.component';
import { PalestrantesComponent } from './componentes/palestrantes/palestrantes.component';
import { ContatosComponent } from './componentes/contatos/contatos.component';
import { PerfilComponent } from './componentes/perfil/perfil.component';
import { DashboardComponent } from './componentes/dashboard/dashboard.component';
import { EventoDetalheComponent } from './componentes/eventos/evento-detalhe/evento-detalhe.component';
import { EventoListaComponent } from './componentes/eventos/evento-lista/evento-lista.component';

const routes: Routes = [
  { path: 'eventos', redirectTo: 'eventos/lista'},
  {
    path: 'eventos', component: EventosComponent,
    children: [
      {path: 'detalhe/:id', component: EventoDetalheComponent},
      {path: 'detalhe', component: EventoDetalheComponent},
      {path: 'lista', component: EventoListaComponent}
    ]
  },
  { path: 'palestrantes', component: PalestrantesComponent},
  { path: 'contatos', component: ContatosComponent},
  { path: 'perfil', component: PerfilComponent},
  { path: 'dashboard', component: DashboardComponent},
  { path: '', redirectTo: 'dashboard', pathMatch: 'full'},
  { path: '**', redirectTo: 'dashboard', pathMatch: 'full'}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

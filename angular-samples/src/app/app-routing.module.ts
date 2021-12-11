import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { DynamicHtmlRenderingComponent } from './components/dynamic-html-rendering/dynamic-html-rendering.component';
import { ProjectionRenderingComponent } from './components/projection-rendering/projection-rendering.component';


const routes: Routes = [
    { path: 'dynamic-rendering', component: DynamicHtmlRenderingComponent },
    { path: 'projection-rendering', component: ProjectionRenderingComponent },
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }

import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { DynamicHtmlRenderingComponent } from './components/dynamic-html-rendering/dynamic-html-rendering.component';
import { NotificationService } from './services/notification.service';
import { ProjectionRenderingComponent } from './components/projection-rendering/projection-rendering.component';
import { ProjectorComponent } from './components/projection-rendering/projector/projector.component';

@NgModule({
    declarations: [
        AppComponent,
        DynamicHtmlRenderingComponent,
        ProjectionRenderingComponent,
        ProjectorComponent,
    ],
    imports: [
        BrowserModule,
        AppRoutingModule
    ],
    providers: [NotificationService],
    bootstrap: [AppComponent]
})
export class AppModule { }

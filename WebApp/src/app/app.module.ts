import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { CalendarComponent } from './calendar/calendar.component';
import { FullCalendarModule } from '@fullcalendar/angular';

@NgModule({
  declarations: [AppComponent, CalendarComponent],
  imports: [BrowserModule, AppRoutingModule, FullCalendarModule],
  providers: [],
  bootstrap: [AppComponent],
})
export class AppModule {}

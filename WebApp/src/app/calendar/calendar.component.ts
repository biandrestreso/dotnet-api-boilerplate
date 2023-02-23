import { Component, ChangeDetectorRef } from '@angular/core';
import { CalendarOptions, EventApi } from '@fullcalendar/core';
import dayGridPlugin from '@fullcalendar/daygrid';
import timeGridPlugin from '@fullcalendar/timegrid';
import interactionPlugin from '@fullcalendar/interaction';
import listPlugin from '@fullcalendar/list';
import { INITIAL_EVENTS, createEventId } from './event-utils';
import tippy from 'tippy.js';

let events = [
  {
    id: '0',
    title: 'Consultation',
    doctor: 'Dr. Jack',
    type: 'Covid-19',
  },
  {
    id: '1',
    title: 'Timed event',
    doctor: 'Dr. Smith',
    type: 'Consultation',
  },
];

@Component({
  selector: 'app-calendar',
  templateUrl: './calendar.component.html',
  styleUrls: ['./calendar.component.scss'],
})
export class CalendarComponent {
  calendarOptions: CalendarOptions = {
    initialView: 'dayGridMonth',
    headerToolbar: {
      left: 'prev,next today',
      center: 'title',
      right: 'dayGridMonth,timeGridWeek,timeGridDay',
    },
    initialEvents: INITIAL_EVENTS,
    eventsSet: this.handleEvents.bind(this),
    plugins: [dayGridPlugin, timeGridPlugin, interactionPlugin, listPlugin],
    height: 'auto',
    displayEventTime: true,
    eventTimeFormat: {
      hour: '2-digit',
      minute: '2-digit',
      meridiem: 'short',
    },
    eventDidMount: (info) => {
      events.forEach((e) => {
        if (e.id == info.event.id) {
          tippy(info.el, {
            content: `<div class="text-center">
            <div class="font-bold">${e.title}</div>
            <div class="font-semibold">${info.event.start?.toLocaleString(
              'en-US',
              {
                hour: 'numeric',
                minute: 'numeric',
                hour12: true,
              }
            )} - ${info.event.end?.toLocaleString('en-US', {
              hour: 'numeric',
              minute: 'numeric',
              hour12: true,
            })}</div>
            <div>Doctor: ${e.doctor}</div>
            <div>Type: ${e.type}</div>
          </div>`,
            allowHTML: true,
            placement: 'top',
            theme: 'light',
            hideOnClick: false,
            arrow: true,
            animation: 'scale',
            delay: [0, 0],
            popperOptions: {
              modifiers: [
                {
                  name: 'offset',
                  options: {
                    offset: [0, 8],
                  },
                },
              ],
            },
          });
        }
      });
    },
  };

  currentEvents: EventApi[] = [];

  constructor(private changeDetector: ChangeDetectorRef) {}

  handleEvents(events: EventApi[]) {
    this.currentEvents = events;
    this.changeDetector.detectChanges();
  }
}

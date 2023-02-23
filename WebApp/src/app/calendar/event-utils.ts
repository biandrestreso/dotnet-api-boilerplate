import { EventInput } from '@fullcalendar/core';

let eventGuid = 0;
const TODAY_STR = new Date().toISOString().replace(/T.*$/, ''); // YYYY-MM-DD of today

export const INITIAL_EVENTS: EventInput[] = [
  {
    id: createEventId(),
    title: 'Consultation',
    start: TODAY_STR + 'T00:30:00',
    end: TODAY_STR + 'T03:00:00',
  },
  {
    id: createEventId(),
    title: 'Timed event',
    start: TODAY_STR + 'T12:00:00',
    end: TODAY_STR + 'T15:00:00',
    backgroundColor: '#f5d56f',
    borderColor: '#f5d56f',
  },
];

export function createEventId() {
  return String(eventGuid++);
}

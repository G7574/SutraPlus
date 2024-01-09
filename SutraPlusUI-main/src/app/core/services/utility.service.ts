import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class UtilityService {
  constructor() {}

  range(start: number, end: number): number[] {
    // return [...Array(end - start).keys()].map(i => i + start);
    const range = [];
    for (let i = start; i < end; i += 1) {
      range.push(i);
    }
    return range;
  }
}

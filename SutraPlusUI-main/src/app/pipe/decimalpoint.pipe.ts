import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'decimalpoint'
})
export class DecimalpointPipe implements PipeTransform {

  transform(value: any): any {
    return `${Math.round(value)}.00`;
  }

}

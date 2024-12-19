import {Pipe, PipeTransform} from '@angular/core';

@Pipe({
  standalone: true,
  name: 'durationFormat'
})
export class DurationFormatPipe implements PipeTransform {
  transform(value: string): string {
    if (!value) return value;

    const [hours, minutes, seconds] = value.split(':').map(Number);

    let result = '';
    if (hours > 0) {
      result += `${hours} hour${hours > 1 ? 's' : ''}`;
    }
    if (minutes > 0) {
      if (result) result += ', ';
      result += `${minutes} minute${minutes > 1 ? 's' : ''}`;
    }

    return result || '0 seconds';
  }
}

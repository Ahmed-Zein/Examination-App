export class Utils {

  public static leftPad(n: number | string, padWith: number): string {
    let s = n.toString();
    while (s.length < padWith) {
      s = `0${s}`
    }
    return s;
  }

  public static toLocalDate(date: string) {
    const utcDate = new Date(date);
    return new Date(utcDate.getTime() - utcDate.getTimezoneOffset() * 60000);
  }

  public static severity(number: number) {
    switch (number) {
      case 0:
        return 'info';
      case 1:
        return 'success';
      case 2:
        return 'warn';
      default:
        return 'error';
    }
  }
}

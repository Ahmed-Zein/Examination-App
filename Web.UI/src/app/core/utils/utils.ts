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
}

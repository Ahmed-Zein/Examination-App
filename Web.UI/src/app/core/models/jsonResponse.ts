export class JsonResponse<T> {
  data: T | undefined;
  success = false;
  message = '';
  errors: string[] = [];
}

export interface Pagination<T> {
  page: number;
  pageSize: number;
  totalCount: number;
  data: T;
  totalPages: number;
  hasPreviousPage: boolean;
  hasNextPage: boolean;
}

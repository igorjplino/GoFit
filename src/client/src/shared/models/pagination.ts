export type Pagination<T> = {
    pageNumber: number;
    pageSize: number;
    total: number;
    items: T[];
}
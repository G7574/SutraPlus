export class Pagination {
  totalItems: any;
  currentPage: number;
  pageSize: number;
  totalPages: any;
  startPage: number;
  endPage: any;
  startIndex: any;
  endIndex: any;
  pages: number[];

  constructor() {
    this.currentPage = 1;
    this.pageSize = 10;
    this.totalPages = '';
    this.startPage = 1;
    this.endPage = '';
    this.startIndex = '';
    this.endIndex = '';
    this.pages = new Array<number>();
  }
}

// production-master.model.ts
export class ProductionMaster {
  Companyid: number = 0;
  TranctDate: Date = new Date();
  Refno: number | null = null;
  QtySent: number = 0;
  QtyRcd: number = 0;
  SentItemid: number = 0;
  ReceivedItId: number = 0;
  GoodsValue: number = 0;
  SrNo: number = 0;
  ReceivedItemid: number = 0;
  ReceivedItemName: string = '';
  SentItemName: string = '';
}

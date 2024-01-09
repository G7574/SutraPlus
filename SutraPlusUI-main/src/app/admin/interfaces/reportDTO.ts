export interface ReportDTO {
  [key: string]: any; // Add this line

  companyId?: number;
  companyName?: string;
  addressLine1?: string;
  addressLine2?: string;
  addressLine3?: string;
  place?: string;
  gstin?: string;

  vochType?: number;
  vochNo?: number;
  tranctDate?: Date;
  noOfBags?: number;
  totalWeight?: number;
  amount?: number;
  partyInvoiceNumber?: string;
  sgst?: number;
  cgst?: number;
  igst?: number;
  taxable?: number;
  toPrint?: number;

  commodityId?: number;
  commodityName?: string;
  hsn?: string;

  ledgerName?: string;
  ledgerPlace?: string;
  ledgerGstin?: string;

  voucherId?: number;
  voucherName?: string;

  monthNo?: string;
  basicValue?: number;
  others?: number;
  billAmount?: number;

  accountingGroupId?: number;
  openingBalance?: number;
  credit?: number;
  debit?: number;
  groupName?: string;
}

export const MOCK_DATA: ReportDTO[] = [
  // Your mock data here
];

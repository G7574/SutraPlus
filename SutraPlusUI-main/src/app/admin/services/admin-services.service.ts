import { Injectable } from '@angular/core';
import { Observable, of } from 'rxjs';
import { DataService } from '../../core/services/data.service';

@Injectable({
  providedIn: 'root',
})
export class AdminServicesService {
  constructor(private dataService: DataService) { }

  getStates(): Observable<any> {
    return this.dataService.getAnonymous('Common/Get');
  }

  getCountries(): Observable<any> {
    return this.dataService.getAnonymous('Common/GetCountry');
  }

  getAutocomplete(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'TenantDBCommon/GetAutocompleteList',
      data
    );
  }

  getAddPartyAutoComplete(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetAddPartyAutoComplete', data);
  }

  getLorryDetailAutoComplete(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetLorryDetailAutoComplete', data);
  }

  getLedger(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetLedger', data);
  }

  getDealer(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetDealerType', data);
  }

  getVoucherType(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'TenantDBCommon/GetVoucherType',
      data
    );
  }

  AddBankJournalEntries(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'VoucherEntries/AddBankJournalEntries',
      data
    );
  }

  AddCashEntries(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'VoucherEntries/AddCashEntries',
      data
    );
  }

  getAccGrp(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetAccGroup', data);
  }
  getOtherAccGrp(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetOtherAccGroup', data);
  }

  getUnitsList(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetUnit', data);
  }

  getProductList(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'TenantDBCommon/GetProductList',
      data
    );
  }

  getEinvoiceKey(data:any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetEinvoiceKey',data);
  }

  getInvType(data:any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetInvtype',data);
  }

  createProduct(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/AddProduct', data);
  }

  getProductById(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetProduct', data);
  }

  updateProduct(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/UpdateProduct', data);
  }

  getAllPartyList(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/UpdateProduct', data);
  }

  createParty(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/AddLedger', data);
  }

  getLedgerList(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetLedgerList', data);
  }

  getLedgerListForOtherAccounts(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetLedgerListForOtherAccounts', data);
  }

  getLedgerListForPartyMaster(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetLedgerListForPartyMaster', data);
  }

  updateData(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/updateData', data);
  }

  getInvoiceList(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetAll', data);
  }

  getItemList(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/Getsingle', data);
  }

  getPartyById(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/Get_Ledger', data);
  }

  setParty(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/SetLedger', data);
  }

  createLedger(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'TenantDBCommon/AddOtherLedger',
      data
    );
  }

  editLedger(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'TenantDBCommon/SetOtherLedger',
      data
    );
  }

  getUserFormList(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetFormList', data);
  }

  getUserMenuList(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'TenantDBCommon/GetUserFormList',
      data
    );
  }

  createUser(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/AddUser', data);
  }

  updateUser(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/UpdateUser', data);
  }

  getUserList(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetUserList', data);
  }

  deleteUser(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/DeleteUser', data);
  }

  getUserById(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/GetUser', data);
  }

  getVoucherTypeAndInvoiceNo(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/Get', data);
  }

  getNewProductList(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetList', data);
  }

  addGoodsInvoice(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/Add', data);
  }

  getDispatcherDetails(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetDispatcherDetails', data);
  }

  updateLedgerDetails(data: any): Observable<any> {
    return this.dataService.postAnonymous('TenantDBCommon/UpdateLedger', data);
  }

  getSingleCompany(data: any): Observable<any> {
    return this.dataService.postAnonymous(`Company/GetInvoiceKey/${data}`, '');
  }

  getReport(data: any): Observable<any> {
    return this.dataService.postAnonymous(`Sales/GetReport`, data);
  }

  getBanksName(data: number): Observable<any> {
    return this.dataService.postAnonymous(`Sales/GetBanks`, data);
  }

  getAccountGroup(data: number): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetAccountGroups',data);
  }

  saveAndGetPaymentList(data: any): Observable<any> {
    return this.dataService.postAnonymous(`Sales/SavePayemnts`, data);
  }

  // saveEInvoice(data: any): Observable<any
  saveAkadaEntry(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/SaveAkadaEntry', data);
  }

  GetLotNoData(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetLotNoData', data);
  }

  GetLotNO(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetLotNO', data);
  }

  UpdateAkadaEntry(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/UpdateAkadaEntry', data);
  }

  UpdateAkadaTransDate(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/UpdateAkadaTransDate', data);
  }

  UpdateAkadaParty(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/UpdateAkadaParty', data);
  }

  DeleteAkadaEntry(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/DeleteAkadaEntry', data);
  }

  DeleteAllAkadaEntry(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/DeleteAllAkadaEntry', data);
  }

  TransferTo(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/TransferTo', data);
  }

  GetLastlyAddedRecord(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetLastlyAddedRecord', data);
  }

  GetBagWeight(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetBagWeight', data);
  }

  GetBills(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetBills', data);
  }

  UpdatePartyInvoiceNumber(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/UpdatePartyInvoiceNumber', data);
  }

  UpdateInvoice(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/UpdateInvoice', data);
  }

  GetDatabaseNameFromConnectionString(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetDatabaseNameFromConnectionString', data);
  }

  GetEditVerifyBillData(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetEditVerifyBillData', data);
  }

  GetOrderByMark(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetOrderByMark', data);
  }

  getAllCommodities(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetAllCommodities', data);
  }
  getCRDRDetails(data: any): Observable<any> {
    return this.dataService.postAnonymous(`Sales/CrDrdetails`, data);
  }

  GetInvoiceResponse(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetInvoiceResponse', data);
  }

  getAllProductionEntries(data: any): Observable<any> {
    return this.dataService.postAnonymous(
      'Sales/GetAllProductionEntries',
      data
    );
  }

  addProductionEntry(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/AddProductionEntry', data);
  }

  saveOptionSettings(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/SaveOptionSettings', data);
  }
  getOptionSettings(data: any): Observable<any> {
    return this.dataService.postAnonymous('Company/GetOptionSettings', data);
  }

  saveDC(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/SaveDC', data);
  }

  GetBNo(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/GetBNo', data);
  }

  SaveExcelData(data: any): Observable<any> {
    return this.dataService.postAnonymous('Sales/SaveExcelData', data);
  }
}

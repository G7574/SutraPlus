import { Injectable } from '@angular/core';
import { isNil, sum, values } from 'lodash-es';

@Injectable({
  providedIn: 'root',
})
export class CalculateService {
  constructor() {}
  /**
   * calculate Total
   * @parameter array
   *
   */
  calculate(totalCal: any) {
    let value: any;
    if (totalCal.isEinVoice) {
      // if E invoice Enabled
      value = this.getEinvoiceEnabledCalculation(totalCal);
    } else {
      // if E invoice not Enabled
      value = this.getEinvoiceDisabledCalculation(totalCal);
    }
    return value;
  }
  /**
   * if E invoice enabled
   */
  getEinvoiceEnabledCalculation(totalCal: any) {
    let result: any;
    if (!isNil(totalCal.lorryAdvance) && Number(totalCal.lorryAdvance) > 0) {
      const amount = this.calculateTotalAmount(totalCal);
      result = {
        amount: amount, //get total Amount
        gst: this.getGstDetails({ ...totalCal, ...{ amount: amount } }), // get GST details
      };
    }
    return result;
  }
  /**
   * if E invoice not enabled
   */
  getEinvoiceDisabledCalculation(totalCal: any) {}

  /**
   * get GST details
   */
  getGstDetails(totalCal: any) {
    let gst: any;
    if (totalCal.ledgerstate === totalCal.companyState) {
      gst = {
        sgstAmt: Number((totalCal.amount * (totalCal.newRate / 100)) / 2),
        cgstAmt: Number((totalCal.amount * (totalCal.newRate / 100)) / 2),
        igstAmt: 0,
      };
    } else {
      gst = {
        sgstAmt: 0,
        cgstAmt: 0,
        igstAmt: Number(totalCal.amount * totalCal.newRate),
      };
    }
    return { ...gst, ...{ totalGST: sum(values(gst)) } };
  }

  /**
   * Total Taxable amount calculate
   */
  calculateTaxable() {}

  /**
   * total GST
   */
  calculateTotalGST() {}

  /**
   * calculate total Amount
   */
  calculateTotalAmount(totalCal: any) {
    console.log(totalCal.itemDetails);
    const initialValue = 0;
    return totalCal.itemDetails.reduce(
      (accumulator: any, currentValue: any) =>
        accumulator + Number(currentValue.Amount),
      initialValue
    );
  }
   /**
   *  total Sum of array of objects
   */
   calculateTotal(array: any,property:string) {
    
    const initialValue = 0;
    return array.reduce(
      (accumulator: any, currentValue: any) =>
        accumulator + Number(currentValue[property]),
      initialValue
    );
  }
}

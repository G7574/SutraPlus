import { Injectable } from '@angular/core';
// import * as CryptoJS from 'crypto-js';

@Injectable({
  providedIn: 'root',
})
export class CommonService {
  constructor() { }
  ValidateNumeric(keyCode: number) {
    if ((keyCode >= 48 && keyCode <= 57) || keyCode === 46) {
      return true;
    } else {
      return false;
    }
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    // let btns = Array.from(document.getElementsByClassName('btn-primary'));
    // for (let x of btns) {
    //   const y = <HTMLElement>x;
    //   y.style.background = y.style.borderColor = themeCode;
    // }

    // let links = Array.from(document.getElementsByClassName('btn-link'));
    // for (let a of links) {
    //   const b = <HTMLElement>a;
    //   b.style.color = themeCode;
    // }

    let nav = Array.from(document.getElementsByClassName('navbar'));
    for (let n of nav) {
      const b = <HTMLElement>n;
      b.style.background = themeCode;
    }

    let sidebar = Array.from(document.getElementsByClassName('sidebar'));
    for (let s of sidebar) {
      const b = <HTMLElement>s;
      b.style.background = themeCode;
    }
  }

  gettotalPages(count: number) {
    return Math.ceil(count / 10);
  }

  getLastIndex(pages: any) {
    return pages.length - 1;
  }

  // encrypt(value: string, secretKey: string): string {
  //   const encryptedValue = CryptoJS.AES.encrypt(value, secretKey).toString();
  //   return encryptedValue;
  // }

  // decrypt(encryptedValue: string, secretKey: string): string {
  //   const decryptedValue = CryptoJS.AES.decrypt(
  //     encryptedValue,
  //     secretKey
  //   ).toString(CryptoJS.enc.Utf8);
  //   return decryptedValue;
  // }
}

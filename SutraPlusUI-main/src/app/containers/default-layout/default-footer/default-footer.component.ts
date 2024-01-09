import { Component } from '@angular/core';
import { FooterComponent } from '@coreui/angular';

@Component({
  selector: 'app-default-footer',
  templateUrl: './default-footer.component.html',
  styleUrls: ['./default-footer.component.scss'],
})
export class DefaultFooterComponent extends FooterComponent {
  currentYear!: number
  constructor() {
    super();
  }

  ngOnInit(): void {
    this.currentYear = new Date().getFullYear();

    this.setTheme();
  }

  setTheme(): void {
    let themeCode: any = sessionStorage.getItem('themeCode');
    let links = Array.from(document.getElementsByClassName('btn-link'));
    for (let a of links) {
      const b = <HTMLElement>a;
      b.style.color = themeCode;
    }
  }
}

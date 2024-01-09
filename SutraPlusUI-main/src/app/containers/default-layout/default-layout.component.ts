import { Component, OnInit } from '@angular/core';
import { navItems } from './_nav';
import { CommonService } from 'src/app/share/services/common.service';
import { DomSanitizer } from '@angular/platform-browser';
import { AdminServicesService } from '../../admin/services/admin-services.service';
import { get, has, isNil } from 'lodash-es';

@Component({
  selector: 'app-dashboard',
  templateUrl: './default-layout.component.html',
})
export class DefaultLayoutComponent implements OnInit {
  newnavArry = navItems;
  public navItems: any[] = [];

  public perfectScrollbarConfig = {
    suppressScrollX: true,
  };

  userDetails: any;
  logo?: any;
  formList: any;
  groupNames: any;

  constructor(
    public commonService: CommonService,
    public sanitizer: DomSanitizer, private adminService: AdminServicesService) { }

  ngOnInit(): void {
    this.userDetails = sessionStorage.getItem('userDetails');
    this.userDetails = JSON.parse(this.userDetails);
    this.logo = this.sanitizer.bypassSecurityTrustResourceUrl(
      `data:image;base64, ${sessionStorage.getItem('logo')}`);
    if (get(this.userDetails, 'result.UserType') !== "Administrator") {
      this.getUserMenuList();
    } else {
      this.navItems = this.newnavArry
    }
  }

  ngAfterViewInit() {
    this.commonService.setTheme();
  }

  getUserMenuList() {
    this.adminService.getUserMenuList('')?.subscribe({
      next: (res: any) => {
        this.formList = res.FormList.filter((i: any) => i.UserId === get(this.userDetails, 'result.UserId'));
        //  this.formList = res.FormList.filter((i:any) => i.UserId === 3);
        this.updtaedNav();

      },
      error: (error: any) => {

      }
    })
  }

  updtaedNav() {
    let newV: any = [];
    this.newnavArry.forEach((item: any) => {
      const child = get(item, 'children');
      if (isNil(child)) {
        newV.push(item)
      }
      else {
        let newChild: any = []

        child.forEach((val: any) => {
          if (this.isChild(val)) {
            newChild.push(val)
          }
        })

        if (newChild.length > 0) {
          newV.push({ ...item, ...{ children: newChild } })
        }
      }
    })
    this.navItems = newV;
  }

  isChild(g: any) {
    return this.formList.find((k: any) => k.FormName === g.name)
  }
}

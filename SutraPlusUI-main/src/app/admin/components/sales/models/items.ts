import { AnyObject } from "chart.js/types/basic";

export class Items {
    Sno!:number;
    CommodityName!: string;
    CommodityId!: number;
    WeightPerBag!: number;
    NoOfBags!: number;
    TotalWeight!: number;
    MOU!: string;
    Rate!: number;
    Taxable!: any;
    Amount!: any;
    Remark!: number;
    SgstAmount: number = 0;
    CgstAmount: number = 0;
    IgstAmount: number = 0;
    NoOfDocra: number = 0;
    SgstRate: number = 0;
    CgstRate: number = 0;
    IgstRate: number = 0;
}

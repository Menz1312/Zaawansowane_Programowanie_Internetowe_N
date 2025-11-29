import IProduct from "./IProduct";

export interface IProductListState{
    products: Array<IProduct>;
    name: string;
}
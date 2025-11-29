import React from "react";
import { IProductListState } from "../models/IProductListState";
import './ProductListItem.css'
import IProduct from "../models/IProduct";
import { ProductListItem } from "./ProductListItem";
import { AddProductToList } from "./AddProductToList";

export class ProductList extends React.Component<{ products: Array<IProduct> }, IProductListState> {
    constructor(props: { products: Array<IProduct> }) {
        super(props);
        this.state = {
            products: this.props.products != null ? this.props.products : [],
            name: ""
        }
    }
    onAddNewProductSubmit = (product: IProduct) => {
        let newState = { ...this.state }
        newState.products.push(product);
        this.setState(newState);
    }
    onInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let newState = { ...this.state }
        newState.name = e.target.value;
        this.setState(newState);
    }
    render() {
        return (
            <div className="product-list">
                <div className="product-list-name-change-container">
                    <input type="text" onChange={this.onInputChange}></input>
                </div>
                <div className="product-list-name-container">
                    <span className="product-list-name-text">
                        {this.state.name}
                    </span>
                </div>
                <div className="list-container">
                    {
                        this.state.name !== "" &&
                        this.state.products.map(product =>
                            <ProductListItem id={product.id}
                                key={product.id}
                                name={product.name}
                                type={product.type}
                                price={product.price}>
                            </ProductListItem>
                        )
                    }
                </div>
                <AddProductToList submit={this.onAddNewProductSubmit}></AddProductToList>
            </div>
        );
    }
}
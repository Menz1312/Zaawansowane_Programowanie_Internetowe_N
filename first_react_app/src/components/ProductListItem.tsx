import React from 'react'
import IProduct from '../models/IProduct';
import './ProductListItem.css'
export class ProductListItem extends React.Component<IProduct>{
    render(){
        return (
            <div className="product-item">
                <div className="product-item-element">
                    <label className="product-item-label">Id</label>
                    <span className="product-item-span">{this.props.id}</span>
                </div>
                <div className="product-item-element">
                    <label className="product-item-label">Name</label>
                    <span className="product-item-span">{this.props.name}</span>
                </div>
                <div className="product-item-element">
                    <label className="product-item-label">Price</label>
                    <span className="product-item-span">{this.props.price}</span>
                </div>
                <div className="product-item-element">
                    <label className="product-item-label">Type</label>
                    <span className="product-item-span">{this.props.type}</span>
                </div>
            </div>
        );
    }
}
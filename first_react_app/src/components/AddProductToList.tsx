import React from "react";
import IProduct from "../models/IProduct";
import './AddProductToList.css'
export class AddProductToList extends React.Component<{ submit: (product: IProduct) => void; }, IProduct> {
    constructor(props: { submit: (product: IProduct) => void; }) {
        super(props);
        this.state = {
            id: 0,
            name: "",
            type: "",
            price: 0.0
        }
    }
    onInputChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        let newState = { ...this.state } as any
        newState[e.target.name] = e.target.value;
        this.setState(newState);
    }
    onSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        this.props.submit(this.state);
        this.setState(state => ({ ...state, newProduct: undefined }));
    }
    render() {
        return (
            <div className="form-container">
                <form onSubmit={this.onSubmit}>
                    <div className="product-item-element">
                        <input className="product-item-input"
                            name="id"
                            placeholder="id"
                            onChange={this.onInputChange}
                            value={this.state.id}
                        >
                        </input>
                    </div>
                    <div className="product-item-element">
                        <input className="product-item-input"
                            name="name"
                            placeholder="name"
                            onChange={this.onInputChange}
                            value={this.state.name}
                        >
                        </input>
                    </div>
                    <div className="product-item-element">
                        <input className="product-item-input"
                            name="type"
                            placeholder="type"
                            onChange={this.onInputChange}
                            value={this.state.type}
                        >
                        </input>
                    </div>
                    <div className="product-item-element">
                        <input className="product-item-input"
                            name="price"
                            placeholder="price"
                            onChange={this.onInputChange}
                            value={this.state.price}
                        >
                        </input>
                    </div>
                    <button type="submit">Add</button>
                </form>
            </div>
        );
    }
}
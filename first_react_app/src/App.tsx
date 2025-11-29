import React from 'react';
import './App.css';
import { ProductList } from './components/ProductList';
import IProduct from './models/IProduct';

function App() {
  let staticProducts: Array<IProduct> = [
    {
      id: 1,
      name :"DELL",
      type : "Computers",
      price: 4000
    },
    {
      id: 2,
      name :"Logitech 2000",
      type : "Mouses",
      price: 30
    },
  ]
  return (
    <div className="App">
      <ProductList products={staticProducts}></ProductList>
    </div>
  );
}
export default App;
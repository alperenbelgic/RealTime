import { Component, Injectable, Inject, OnInit, OnDestroy } from '@angular/core';
import { ChannelService } from '../../services/ChannelService'
import { StockpriceComponent, Stock } from '../stockprice/stockprice.component'

export class Transaction {
    constructor(
        private seller: String,
        private buyer: String,
        private count: String,
        private price: String
    ) {
    }
}


@Component({
    selector: 'product-list',
    templateUrl: './productlist.component.html'
})
export class ProductlistComponent implements OnInit, OnDestroy {

    public streamingStocks: Array<Stock>;

    constructor(
        private ChannelService: ChannelService
    ) {
        console.log('PricelistComponent constructed');
    }

    ngOnInit(): void {

        console.log('pricelist init');

        this.streamingStocks = new Array<Stock>();

        this.streamingStocks.push(new Stock('LLOY', 'Lloyds Banking Group plc'));
        this.streamingStocks.push(new Stock('BARC', 'Barclays plc'));
        this.streamingStocks.push(new Stock('VOD', 'Vodafone Group plc'));
        this.streamingStocks.push(new Stock('GLEN', 'Glencore plc'));
        this.streamingStocks.push(new Stock('ITV', 'ITV plc'));
        this.streamingStocks.push(new Stock('BP.', 'BP Plc'));
        this.streamingStocks.push(new Stock('HSBA', 'HSBC Holdings plc'));
        this.streamingStocks.push(new Stock('CNA', 'Centrica plc'));
        this.streamingStocks.push(new Stock('TSCO', 'Tesco plc'));
        this.streamingStocks.push(new Stock('WPG', 'Worldpay Group plc'));
        this.streamingStocks.push(new Stock('BT.A', 'BT Group plc'));
        this.streamingStocks.push(new Stock('SKY', 'Sky plc'));
        this.streamingStocks.push(new Stock('TW.', 'Taylor Wimpey plc'));
        this.streamingStocks.push(new Stock('GKN', 'GKN plc'));
        this.streamingStocks.push(new Stock('NG.', 'National Grid'));
        this.streamingStocks.push(new Stock('KGF', 'Kingfisher'));
        this.streamingStocks.push(new Stock('LGEN', 'Legal & General Group plc'));
        this.streamingStocks.push(new Stock('MKS', 'Marks & Spencer Group plc'));
        this.streamingStocks.push(new Stock('BA.', 'BAE Systems plc'));
        this.streamingStocks.push(new Stock('OML', 'Old Mutual Plc'));
    }

    ngOnDestroy(): void {
        console.log('pricelist destroyed');
    }

}


import { Component, Injectable, Inject, OnInit, OnDestroy, Input, Output } from '@angular/core';
import { ChannelService } from '../../services/ChannelService'

export class Stock {

    constructor(
        public stockCode: string,
        public stockName: string
    ) {
        console.log('stockprice constructed');
    }
}

@Component({
    selector: 'stock-price',
    templateUrl: './stockprice.component.html',
    styleUrls: ['./stockprice.component.css']

})
export class StockpriceComponent implements OnInit, OnDestroy {

    @Input() public stock: Stock;

    @Output() public stockPrice: number;
    @Output() public stockBuyPrice: number;
    @Output() public stockSellPrice: number;
    @Output() public change: number;

    streamHandle: string;
    streamHandle2: string;

    constructor(
        private ChannelService: ChannelService
    ) {
        console.log('stockprice constructed');
    }

    public startStream() {

        //var getStreamResponse = this.ChannelService.getStream("StreamPrices", this.stock.stockCode);
        //this.streamHandle = getStreamResponse.streamHandle;

        //getStreamResponse.streamPromise.then((stream:any) => {
        //        stream.subscribe({
        //            next: (p: any) => {
        //                console.log(p);
        //                this.stockPrice = p.price;
        //                console.log(this.stockPrice);
        //            },
        //            error: (e: any) => {
        //                console.log(e);
        //            }
        //        });
        //});

        var getStreamPriceIntervalResponse = this.ChannelService.getStream("StreamPriceInformation", this.stock.stockCode);
        this.streamHandle2 = getStreamPriceIntervalResponse.streamHandle;
        getStreamPriceIntervalResponse.streamPromise.then((stream: any) => {
            stream.subscribe({
                next: (p: any) => {

                    this.stockBuyPrice = p.lowestOffer;
                    this.stockSellPrice = p.highestBid;
                    this.stockPrice = p.latestPrice;
                    this.change = p.change;


                    console.log(p);

                },
                error: (e: any) => {
                    console.log(e);
                }
            });
        });
    }

    public stopStream() {
        this.ChannelService.stopStream(this.streamHandle);
        this.ChannelService.stopStream(this.streamHandle2);
    }

    ngOnInit(): void {
        this.startStream();
    }

    ngOnDestroy(): void {
        this.stopStream();
    }

}


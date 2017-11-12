import { Component, Injectable, Inject } from '@angular/core';
import { ChannelService } from '../../services/ChannelService'

export  class Transaction {
    constructor(
        private seller: String,
        private buyer: String,
        private count: String,
        private price: String
    ) {

    }
}


@Component({
    selector: 'pricelist',
    templateUrl: './pricelist.component.html'
})
export class PricelistComponent {
    public currentCount = 0;

    public transactions = new Array<Transaction>();

    constructor(
        private ChannelService: ChannelService
    ) {
        console.log('PricelistComponent constructed');
    }

    public startStream() {

        this.ChannelService.getStream("StreamTransactions")
            .then(stream => {
                stream.subscribe({
                    next: (t: any) => {
                        console.log(t);
                        this.transactions.unshift(new Transaction(t.seller, t.buyer, t.count, t.price));
                    }
                });

            });

        //this.currentCount++;
    }

    public stopStream() {

        this.ChannelService.stopStream();
        

        //this.currentCount++;
    }


}

import { Component } from '@angular/core';

@Component({
    selector: 'pricelist',
    templateUrl: './pricelist.component.html'
})
export class PricelistComponent {
    public currentCount = 0;

    public incrementCounter() {
        this.currentCount++;
    }
}

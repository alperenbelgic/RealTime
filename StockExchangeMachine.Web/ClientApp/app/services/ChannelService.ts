import { Injectable, Inject } from "@angular/core";
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";
import { Router } from '@angular/router';

declare var signalR: any;

@Injectable()
export class ChannelService {

    private hubCon: any;
    private resultPromise: Promise<any>


    constructor() {


    }

    getStream(streamName: string): Promise<any> {
        // check if connection closed

        var a = true;

        if (this.hubCon == null) {

            this.hubCon = new signalR.HubConnection('/TheHub');

            console.log("before connect " + this.hubCon.connection.connectionState);

            this.resultPromise = new Promise<any>((resolve) => {
                this.hubCon.start()
                    .then(() => {
                        console.log('connected');
                        console.log("after connected " + this.hubCon.connection.connectionState);
                        console.log(this.hubCon);

                        resolve(this.hubCon.stream(streamName, "param"));
                    });
            });

            console.log("after connect promise created " + this.hubCon.connection.connectionState);

            return this.resultPromise;
        }
        else {
            throw new Error("Unexpected getstream call");
        }
    }

    stopStream() {

        if (this.hubCon != null) {
            
            console.log("before con. stoped " + this.hubCon.connection.connectionState);

            this.hubCon.stop();
            console.log("after con. stoped " + this.hubCon.connection.connectionState);

            this.hubCon = null;
        }
        else {
            console.log("hubCon is null");
        }



    }

}
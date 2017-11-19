import { Injectable, Inject } from "@angular/core";
import { Subject } from "rxjs/Subject";
import { Observable } from "rxjs/Observable";
import { Router } from '@angular/router';
import 'rxjs/add/operator/toPromise';
import "rxjs";


declare var signalR: any;

@Injectable()
export class ChannelService {
    
    private resultPromise: Promise<any>
    private streamConnections: Map<string, any>;
    private streams: Map<string, any>;


    constructor() {
        this.streamConnections = new Map<string, any>();
        this.streams = new Map<string, any>();
    }

    getStream(streamName: string, parameter: string): any {
        // service and hub lifecycle and states might be refactored.

        var streamHandle = Math.random().toString();
        var hubConnection = new signalR.HubConnection('/TheHub');

        this.streamConnections.set(streamHandle, hubConnection);

        var thePromise = new Promise<any>((resolve) => {
            hubConnection.start()
                .then(() => {
                    console.log('connected');

                    var stream = hubConnection.stream(streamName, parameter);
                    resolve(stream);
                });
        });

        return {
            "streamPromise": thePromise,
            streamHandle: streamHandle
        };
    }

    stopStream(streamHandle: string) {

        var hubCon = this.streamConnections.get(streamHandle);

        if (hubCon != null) {

            console.log("before con. stoped " + hubCon.connection.connectionState);

            hubCon.stop();
            console.log("after con. stoped " + hubCon.connection.connectionState);

            hubCon = null;
        }
        else {
            console.log("hubCon is null");
        }



    }

}
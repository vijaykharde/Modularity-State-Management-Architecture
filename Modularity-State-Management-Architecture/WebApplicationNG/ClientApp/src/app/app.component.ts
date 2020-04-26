import { Component } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { SampleWorkItem } from '../Samples/SampleWorkItem';
import { TestViewController } from '../Samples/TestViewController';
import { SampleViewController } from '../Samples/SampleViewController';
@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'app';
  private _hubConnection: HubConnection;
  msgs: Message[] = [];

  ngOnInit(): void {
    this._hubConnection = new HubConnectionBuilder().withUrl('http://localhost:1206/notify').build();
    this._hubConnection
      .start()
      .then(() => console.log('Connection started!'))
      .catch(err => console.log('Error while establishing connection :('));
    //this._hubConnection.serverTimeoutInMilliseconds = 30000 * 30000 * 30000;
    this._hubConnection.on('BroadcastMessage', (workItem: any) => {
      //this.msgs.push({ severity: type, summary: payload });
      /*if (workItem) */{
        console.log(workItem);
      }
    });
    
  }

  executeStateWorkItems(): void {
    var ctlr = new SampleViewController(new SampleWorkItem());
    ctlr.Run();
    ctlr.CWorkItem.WIState["test"] = { name: 'vijay' };
    ctlr.CWorkItem.WIState["test"] = { name: 'ajay' };
    ctlr.CWorkItem.WIState["test"] = { name: 'abhijit' };
    var ctlr = new TestViewController();
    ctlr.Run();
    ctlr.CWorkItem.WIState["test"] = { name: 'vijay' };
    ctlr.CWorkItem.WIState["test"] = { name: 'ajay' };
  }
}
class Message {
  severity: any;
  summary: any;
}

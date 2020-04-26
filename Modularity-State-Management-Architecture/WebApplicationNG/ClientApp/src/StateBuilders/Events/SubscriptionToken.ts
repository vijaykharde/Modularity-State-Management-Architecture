import { v4 as uuid } from 'uuid';
export class SubscriptionToken{
	private _token:uuid;
	private _unsubscribeAction:(_unsubscribeAction:SubscriptionToken)=>void;
	constructor(unsubscribeAction:(_unsubscribeAction:SubscriptionToken)=>void) {
            this._unsubscribeAction = unsubscribeAction;
            this._token =new uuid();		
	}
}
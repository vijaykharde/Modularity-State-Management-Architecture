import { EventEmitter } from '@angular/core'; 
import { SubscriptionToken } from '../../Events/SubscriptionToken';
import { v4 as uuid } from 'uuid';
import { State } from './State';
import { StateChangedEventArgs } from './StateChangedEventArgs';
import { WorkItemStatus } from './WorkItemStatus';
export interface IWorkItem{
	Id:uuid;
	Parent:IWorkItem;
	RootWorkItem:IWorkItem;
	//Container:IUnityContainer;
	WIState:State;
	Status:WorkItemStatus;
	WorkItemStateChanged : EventEmitter<StateChangedEventArgs>; 
	SubscribeStateChangedEvent(stateName:string, action:(_currentObject:any)=>void,currentObj:any):SubscriptionToken;
	SubscribeStateChangedEventWithObject(stateName:string, action:(_object:object,_currentObject:any)=>void,currentObj:any):SubscriptionToken;
	SubscribeStateChangedEventWithArgs(stateName:string, action:(_object:StateChangedEventArgs,_currentObject:any)=>any,currentObj:any):SubscriptionToken;
	//Activate(): void;
	//Run(continueWith : ()=>null):void;
	Run():void;
}
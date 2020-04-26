import { EventEmitter } from '@angular/core'; 
import { SubscriptionToken } from '../../Events/SubscriptionToken';
import { v4 as uuid } from 'uuid';
import { State } from './State';
import { StateChangedEventArgs } from './StateChangedEventArgs';
import { IWorkItem } from './IWorkItem';
import { WorkItemCollection } from './WorkItemCollection';
import { WorkItemStatus } from './WorkItemStatus';
export class WorkItem implements IWorkItem
{
	Id:uuid;
	Parent:IWorkItem;
	RootWorkItem:IWorkItem;
	WIState:State;
	Status:WorkItemStatus;
	WorkItemStateChanged : EventEmitter<StateChangedEventArgs>;
	WorkItems:WorkItemCollection;
	
	public constructor()
	{
		this.Id=new uuid();
		this.WIState=State.createDerived();
		this.WorkItems=new WorkItemCollection(<IWorkItem>this);
	}
	
	public Run(): void
	{
		if(this.Status!=WorkItemStatus.NotRunning)
		{
			throw new Error('Run can only be called when a WorkItem is not running!');
    }
		this.Status = WorkItemStatus.Running;
		this.OnRunStarted();
	}
	
	protected OnRunStarted():void
	{
    //console.log('protected OnRunStarted():void');
		var eventHandler = new EventEmitter<StateChangedEventArgs>(true);
		eventHandler.subscribe
		(
		  eventArgs=>
		  {
			if(this.WorkItemStateChanged)
			{
			  //console.log(eventArgs);
			  this.WorkItemStateChanged.emit(eventArgs);
			}
		  },
		  error=>
		  {
			  console.log(error);
		  },
		  complete=>
		  {
			  console.log(complete);
		  }
		);
		this.WIState.StateChanged = eventHandler;
	}
	
	SubscribeStateChangedEvent(stateName:string, action:(_currentObject:any)=>void,currentObj:any):SubscriptionToken
	{
		//console.log("SubscribeStateChangedEvent(stateName:string, action:()=>void):SubscriptionToken");
		return this.SubscribeStateChangedEventWithObject(stateName, (x,p) => action(currentObj),currentObj);
	}
	
	SubscribeStateChangedEventWithObject(stateName:string, action:(_object:object,_currentObject:any)=>void,currentObj:any):SubscriptionToken
	{
		//console.log("SubscribeStateChangedEventWithObject(stateName:string, action:(_object:object)=>void):SubscriptionToken");
		return this.SubscribeStateChangedEventWithArgs(stateName, x => action(x.NewValue,currentObj),currentObj);
	}
	
	SubscribeStateChangedEventWithArgs(stateName:string, action:(_object:StateChangedEventArgs,_currentObject:any)=>any,currentObj:any):SubscriptionToken
	{
		//console.log("SubscribeStateChangedEventWithArgs(stateName:string, action:(_object:StateChangedEventArgs)=>any):SubscriptionToken");    
		var stateObject = this.WIState[stateName];
		if (stateObject != null)
		{
			action(new StateChangedEventArgs(stateName, stateObject,null),currentObj);
		}
		//console.log("stateObject is null");
		var eventHandler= new EventEmitter<StateChangedEventArgs>(true);
		eventHandler.subscribe
		(
		  eventArgs=>
		  {
			console.log(eventArgs);
			if (eventArgs.Key == stateName)
			{
			  action(eventArgs,currentObj);
			}      
		  },
		  error=>
		  {
			console.log(error);
		  },
		  complete=>
		  {
			console.log(complete);
		  }
		);
		this.WorkItemStateChanged=eventHandler;
		//console.log(this.WorkItemStateChanged);
		return new SubscriptionToken(()=>
		{
			eventHandler.unsubscribe();
			this.WorkItemStateChanged =null;
		});
	}
}
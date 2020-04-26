import { EventEmitter, Input, Output} from '@angular/core'; 
import { IWorkItem } from './IWorkItem';
export class WorkItemCollection extends Array<IWorkItem>{
	private _workItems = new Array<IWorkItem>();
	private _parent:IWorkItem;
	public constructor(parent:IWorkItem)
	{
		super();
		this._parent=parent;
	}
	
	@Output() WorkItemAdded = new EventEmitter<IWorkItem>(); 
	@Output() WorkItemRemoved = new EventEmitter<IWorkItem>(); 
	public Add(workItem:IWorkItem):void
    {
		this._workItems.push(workItem);
		this.WorkItemAdded.emit(workItem);
	}
	
	public Get<TWorkItem extends IWorkItem>(): IWorkItem
	{
		this._workItems.forEach(function (_wi) {
			if (_wi as TWorkItem)
			{
				return _wi as IWorkItem;
			}
		});
		return null;
	}
	
	public Remove(workItem:IWorkItem):void{
		for( var i = 0; i < this._workItems.length; i++){ 
		   if ( this._workItems[i] === workItem) {
			 this._workItems.splice(i, 1); 
			 this.WorkItemRemoved.emit(workItem);
		   }
		}
	}
}
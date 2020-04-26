import { EventEmitter } from '@angular/core'; 
import { StateChangedEventArgs } from './StateChangedEventArgs';
import { v4 as uuid } from 'uuid';
/*****************************/
//https://stackoverflow.com/questions/47509326/indexers-in-typescript
/***********************************/
export class StateBase
{
	public _values: { [index: string]: object; } = {};	
	public StateChanged:EventEmitter<StateChangedEventArgs>;
	Id:string;	
  protected constructor (){
    this.Id=new uuid();
  }
  
  getValue(name: string) : any {
    if(name=='StateChanged')
    {
      return this.StateChanged;
    }
    else
    {
      return this._values[name] || null;
    }
  }
  setValue(name: string, value: any) {
    if(name=='StateChanged')
    {
      this.StateChanged=value;
    }
    else
    {
      this._values[name] = value;
    }
  }
	public RaiseStateChanged( key:string,  newValue:object,  oldValue:object):void{
		if(this.StateChanged){
			this.StateChanged.emit(new StateChangedEventArgs(key, newValue, oldValue));		
		}
	}
  public static wrap<T extends StateBase>(value: T): T {
    return new Proxy(value, { 
      get: (target, name)=>
      { 
        return target.getValue(<string>name);
      }
      ,
      set: (target, name, pValue)=> 
      {
        var key=name;
        var oldValue=target.getValue(<string>name);
        var newValue=pValue;
        target.setValue(<string>name, pValue);
        target.RaiseStateChanged(<string>key,newValue,oldValue);
        return true;
      }
    });
  }
}
export class State extends StateBase {
  protected constructor (){
      super();
  }
  public static createDerived(): State {
      return StateBase.wrap(new State());
  }
}
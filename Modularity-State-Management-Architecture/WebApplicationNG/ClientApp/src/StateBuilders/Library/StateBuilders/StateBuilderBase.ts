import { IWorkItem } from '../Infrastructure/IWorkItem';
import { Dictionary } from '../../../Abstractions/ViewControllerBase';
import { StateBuilderContext } from './StateBuilderContext';
import { StateChangedEventArgs } from '../Infrastructure/StateChangedEventArgs';

export abstract class StateBuilderBase<TStateObject extends any>{
  public _workItem:IWorkItem;
  public _stateName:string;
  public _stateBuilderContext:StateBuilderContext<any>;

  protected constructor(workItem:IWorkItem,stateName:string){
    this._workItem=workItem;
    this._stateName=stateName;
  }

  public Run():void{
    this.OnRunStarted();
  }

  public OnStateChanged(eventArgs:StateChangedEventArgs,_currentObject:any):void
  {
    if (eventArgs.OldValue != null && eventArgs.NewValue == null)
    {
      _currentObject.OnDependencyChanged(_currentObject);
    }
  }

  protected OnRunStarted():void{
    var selfDependencyFound=0;
    var Dependencies=this.GetDependencies();
    var OptionalDependencies=this.GetOptionalDependencies();
    for (var i = 0, len = Dependencies.length; i < len; i++) {
      if(Dependencies[i]==this._stateName)
      {
        selfDependencyFound=selfDependencyFound+1;
      }
      if(selfDependencyFound>0){ throw new Error('StateBuilder cannot have itself as a dependency.'); }
    }

    var selfOptionalDependencyFound=0;
    for (var j = 0, len = OptionalDependencies.length; j < len; j++) {
      if(OptionalDependencies[j]==this._stateName)
      {
        selfOptionalDependencyFound=selfOptionalDependencyFound+1;
      }
      if(selfOptionalDependencyFound>0){ throw new Error('StateBuilder cannot have itself as a dependency.'); }
    }

    this._workItem.SubscribeStateChangedEventWithArgs(this._stateName, this.OnStateChanged,this);//Need to check parameter here
    var anyDepedencyFound=0;
    for (var k = 0, len = Dependencies.length; k < len; k++) {
       this._workItem.SubscribeStateChangedEvent(Dependencies[k], this.OnDependencyChanged,this);
       anyDepedencyFound=anyDepedencyFound+1;
    }

    for (var l = 0, len = OptionalDependencies.length; l < len; l++) {
       this._workItem.SubscribeStateChangedEvent(OptionalDependencies[l], this.OnDependencyChanged,this);
       anyDepedencyFound=anyDepedencyFound+1;
    }
    if(anyDepedencyFound>0){
      this.OnDependencyChanged(this);
    }
  }

  public CreateStateBuilderContext(dependencies:Dictionary,optionalDependencies:Dictionary,_currentObject:any){
    return new StateBuilderCtx(_currentObject._workItem,_currentObject._stateName,_currentObject.GetDependencies(),_currentObject.GetOptionalDependencies(),null);
  }

  public OnDependencyChanged(_currentObject:any):void{
    var dependencies=_currentObject.GetDependencies();
    var optionalDependencies=_currentObject.GetOptionalDependencies();
    _currentObject._stateBuilderContext = _currentObject.CreateStateBuilderContext(dependencies, optionalDependencies,_currentObject);
    if(_currentObject._stateBuilderContext.Dependencies.length>0){
      _currentObject.OnDependenciesMet(_currentObject._stateBuilderContext,_currentObject);
    }
  }

  public OnDependenciesMet(stateBuilderContext:StateBuilderContext<any>,_currentObject:any):void{
    /**
      demo({ name: 'vijay' });
      function demo(pa: any): void {
          (async () => { await console.log('In async function'); console.log(pa); })();
      }
     */
    (async (_stateHandlerContext,_currentObj) => { 
        await _currentObj.DoWork(_stateHandlerContext,_currentObj);
      })(stateBuilderContext,_currentObject);
  }

  public async DoWork(stateBuilderContext:StateBuilderContext<any>,_currentObj:any){
    console.log(_currentObj);
    await _currentObj.GetStateObject(stateBuilderContext);
  }

  public abstract GetDependencies():Array<string>;
  public abstract GetOptionalDependencies():Array<string>;
  public abstract GetAdditionalData():Array<string>;

  public abstract GetStateObject(context:StateBuilderContext<any>):void;
}

class StateBuilderCtx extends StateBuilderContext<any>{
  public constructor(workItem:IWorkItem, stateName:string, dependencies:Dictionary, optionalDependencies:Dictionary, additionalData:Dictionary){
    super(workItem,stateName,dependencies,optionalDependencies,additionalData);
  }
}
import { IWorkItem } from '../Infrastructure/IWorkItem';
import { Dictionary } from '../../../Abstractions/ViewControllerBase';
export class StateBuilderContext<TStateObject extends any>{
    private _workItem:IWorkItem;
    private _stateName:string;
    public Dependencies:Dictionary;
    public OptionalDependencies:Dictionary;
    public AdditionalData:Dictionary;

    constructor(workItem: IWorkItem, stateName: string,dependencies:Dictionary,optionalDependencies:Dictionary,additionalData:Dictionary) {
      this._workItem=workItem;
      this._stateName=stateName;
      this.Dependencies=dependencies;
      this.OptionalDependencies=optionalDependencies;
      this.AdditionalData=additionalData;
    }

    public SetResult(result:TStateObject):void{
      this._workItem.WIState[this._stateName]=result;
    }

    public GetValue<TStateValue extends any>(key:string):TStateValue{
      if(this.Dependencies[key]!=null) return this.Dependencies[key];
      if(this.OptionalDependencies[key]!=null) return this.OptionalDependencies[key];
      if(this.AdditionalData[key]!=null) return this.AdditionalData[key];
      return null;
    }
}
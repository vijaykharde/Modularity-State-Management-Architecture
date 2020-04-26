import { IWorkItem } from '../StateBuilders/Library/Infrastructure/IWorkItem';
import { ViewControllerBase,Dictionary } from '../Abstractions/ViewControllerBase';
import { SampleWorkItem } from '../Samples/SampleWorkItem';
import { WorkItemStatus } from '../StateBuilders/Library/Infrastructure/WorkItemStatus';
export class TestViewController extends ViewControllerBase{
  public constructor(){
    super(new SampleWorkItem());
  }  
  public GetDependencies():Array<string>{
   var arr:string[]=['test'];
   return arr;
  }
  public OnDependencyChanged(dictionary:Dictionary):void{
    console.log(dictionary);
  }
}
import { WorkItemStatus } from '../StateBuilders/Library/Infrastructure/WorkItemStatus';
import { IWorkItem } from '../StateBuilders/Library/Infrastructure/IWorkItem';
import { Dictionary } from '../Abstractions/ViewControllerBase';
export class SampleViewController
{
  
  public constructor(workItem:IWorkItem)
  {
    workItem.Status=WorkItemStatus.NotRunning;
    this.CWorkItem=workItem;
    //console.log(this.CWorkItem);
  }
  public CWorkItem:IWorkItem=null;
  public Run():void{
    if(this.CWorkItem!=undefined && this.CWorkItem)
    {
      //console.log(this);
      if(this.CWorkItem.Status == WorkItemStatus.NotRunning)
      {
        //console.log(this.CWorkItem);
        this.CWorkItem.Run();
        this.OnRunStarted();
      }
    }
  }

  public OnRunStarted():void{
    var dependencies=this.GetDependencies();
	var currentWorkItem=this.CWorkItem;
	var localOnDependencyChanged=this.OnDependencyChanged;
    if(dependencies.length>0 && currentWorkItem!=undefined && currentWorkItem)
    {
		let OnDependencyChange=function(){
			var dictionary:Dictionary={};
			if(dependencies.length>0 && currentWorkItem!=undefined && currentWorkItem)
			{
				for (var i = 0, len = dependencies.length; i < len; i++) {
					dictionary[dependencies[i]]=currentWorkItem.WIState[dependencies[i]];
				}
			}
			localOnDependencyChanged(dictionary);			
		}
		for (var i = 0, len = dependencies.length; i < len; i++) {
		  this.CWorkItem.SubscribeStateChangedEvent(dependencies[i],this.OnDependencyChange,this);
		}
    }
  }

  public OnDependencyChange(currentObj:any):void{
	  console.log("In OnDependencyChange "+currentObj);
	var dependencies=currentObj.GetDependencies();
    var dictionary:Dictionary={};
    if(dependencies.length>0 && currentObj.CWorkItem!=undefined && currentObj.CWorkItem)
    {
		for (var i = 0, len = dependencies.length; i < len; i++) {
			dictionary[dependencies[i]]=currentObj.CWorkItem.WIState[dependencies[i]];
		}
	}
    currentObj.OnDependencyChanged(dictionary);
  }
  public GetDependencies():Array<string>{
   var arr:string[]=['test'];
   return arr;
  }
  public OnDependencyChanged(dictionary:Dictionary):void{
    console.log(dictionary);
  }
}
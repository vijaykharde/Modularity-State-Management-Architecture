import { WorkItem } from '../StateBuilders/Library/Infrastructure/WorkItem';
export class SampleWorkItem extends WorkItem{
  public constructor()
  {
    super();
  }
  public OnRunStarted():void{
    super.OnRunStarted();
    //Initialize StateBuilders here
  }
}
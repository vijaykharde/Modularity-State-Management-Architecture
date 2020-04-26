export class StateChangedEventArgs 
{
	public  Key : string;
	public  NewValue : object;
	public  OldValue : object;
	//public constructor( key:string,  newValue: object)
	//{
	//	this.Key=key;
	//	this.NewValue=newValue;
	//}
	
	public constructor( key:string,  newValue: object, oldValue:object)
	{
		this.Key=key;
		this.NewValue=newValue;
		this.OldValue=oldValue;
	}
}